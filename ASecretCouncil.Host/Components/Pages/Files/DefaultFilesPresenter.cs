using System.Text;
using ASecretCouncil.Host.Components.Pages.Steps;
using ASecretCouncil.Host.Properties;
using ASecretCouncil.Host.Shared.Models;
using ASecretCouncil.Host.Shared.Services;
using ASecretCouncil.Model.Application;
using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;

namespace ASecretCouncil.Host.Components.Pages.Files;

internal sealed class DefaultFilesPresenter(
    ReturningApplicationProvider savedApplicationProvider,
    IApplicationService applicationService,
    FormStepRoutingPresenter formStepPresenterDelegate,
    IMapper mapper) : IFilesPresenter
{
    private const long FileSizeLimit = 2097152L;

    public Func<IBrowserFile, Task<string?>> FileValidationFunc =>
        async delegate(IBrowserFile file)
        {
            if (file.ContentType != "application/pdf")
            {
                return Strings.Validation_InvalidPdfMessage;
            }

            if (file.Size > FileSizeLimit)
            {
                return Strings.Validation_TooLargePdfMessage;
            }

            var pdfString = "%PDF-";
            var pdfBytes = Encoding.ASCII.GetBytes(pdfString);
            var len = pdfBytes.Length;
            var buf = new byte[len];
            var remaining = len;
            var pos = 0;
            await using var f = file.OpenReadStream(maxAllowedSize: FileSizeLimit);
            while (remaining > 0)
            {
                var amtRead = await f.ReadAsync(buf.AsMemory(pos, remaining));
                if (amtRead == 0)
                {
                    return Strings.Validation_EmptyPdfMessage;
                }

                remaining -= amtRead;
                pos += amtRead;
            }

            if (!pdfBytes.SequenceEqual(buf))
            {
                return Strings.Validation_InvalidPdfMessage;
            }

            return null;
        };

    private IFilesView? _view;

    public FilesViewModel ViewModel { get; } = new();

    public async Task InitializeAsync(IFilesView view)
    {
        _view = view;
        await formStepPresenterDelegate.InitializeAsync(view);
    }

    public async Task OnNextClicked()
    {
        if (!ViewModel.IsValid)
        {
            return;
        }

        _view?.StartLoading();
        var savedApplicationId = await savedApplicationProvider.ApplicationId();
        if (savedApplicationId == Guid.Empty)
        {
            //todo: route to start
            _view?.StopLoading();
            return;
        }

        var savedApplication = await applicationService.FindApplication(savedApplicationId);

        if (ViewModel.SelectedFile != null)
        {
            if (savedApplication?.Resume is null)
            {
                await applicationService.CreateResume(savedApplicationId, ViewModel.SelectedFile);
            }
            else
            {
                await applicationService.UpdateResume(savedApplicationId, ViewModel.SelectedFile);
            }
        }

        await savedApplicationProvider.SaveApplicationId(savedApplicationId);
        await formStepPresenterDelegate.OnNextClicked();

        _view?.StopLoading();
    }

    public async Task OnPreviousClicked()
    {
        await formStepPresenterDelegate.OnPreviousClicked();
    }

    public async Task PrefillOnReturnVisit()
    {
        if (_view == null)
        {
            throw new ApplicationException(
                "Presenter cannot be called with initializing view");
        }

        _view.StartLoading();

        var savedApplicationId = await savedApplicationProvider.ApplicationId();

        if (savedApplicationId == Guid.Empty)
        {
            //todo: route to start
            _view.StopLoading();
            return;
        }
        var savedApplication = await applicationService.FindApplication(savedApplicationId);
        if (savedApplication?.Resume != null)
        {
            var document =  savedApplication.Resume;
            if (document is {Size: > 0})
            {
                var file = await applicationService.ResumeFile(savedApplicationId);
                if (file.Length != 0)
                {
                    ViewModel.SelectedFile = new MemoryBrowserFile(document, file);
                }
            }

            _view.TriggerRender();
            await _view.Validate();
        }

        _view.StopLoading();
    }

    public async Task OnUploadFiles(InputFileChangeEventArgs e)
    {
        if (await FileValidationFunc(e.File) != null)
        {
            return;
        }

        ViewModel.SelectedFile = e.File;
    }

    public void Dispose()
    {
        _view = null;
    }
}
