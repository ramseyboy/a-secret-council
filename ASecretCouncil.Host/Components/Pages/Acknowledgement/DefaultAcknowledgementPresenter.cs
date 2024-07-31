using ASecretCouncil.Host.Components.Pages.Steps;
using ASecretCouncil.Host.Components.Pages.Steps;
using ASecretCouncil.Host.Shared.Models;
using ASecretCouncil.Host.Shared.Services;
using ASecretCouncil.Model.Application;

namespace ASecretCouncil.Host.Components.Pages.Acknowledgement;

internal sealed class DefaultAcknowledgementPresenter(
    ReturningApplicationProvider savedApplicationProvider,
    IApplicationService applicationService,
    FormStepRoutingPresenter formStepPresenterDelegate) : IAcknowledgmentPresenter
{
    public AcknowledgmentViewModel ViewModel { get; } = new();

    private IView? _view;

    public async Task InitializeAsync(IView view)
    {
        _view = view;
        await formStepPresenterDelegate.InitializeAsync(view);
    }

    public void Dispose()
    {
        _view = null;
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
            var savedApplicationEntity = await applicationService.CreateApplication();
            await savedApplicationProvider.SaveApplicationId(savedApplicationEntity.Id);
        }
        else
        {
            await savedApplicationProvider.SaveApplicationId(savedApplicationId);
        }

        await formStepPresenterDelegate.OnNextClicked();
        _view?.StopLoading();
    }

    public Task OnPreviousClicked()
    {
        return Task.CompletedTask;
    }
}
