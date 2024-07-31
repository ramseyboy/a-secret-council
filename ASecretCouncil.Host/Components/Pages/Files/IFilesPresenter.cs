using ASecretCouncil.Host.Components.Pages.PersonalInformation;
using ASecretCouncil.Host.Components.Pages.Steps;
using ASecretCouncil.Host.Shared.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace ASecretCouncil.Host.Components.Pages.Files;

public interface IFilesPresenter : IPresenter<IFilesView>, IFormStepPresenterMixin, IPrefillFormPresenterMixin
{
    public Func<IBrowserFile, Task<string?>> FileValidationFunc { get; }
    public FilesViewModel ViewModel { get; }
    public Task OnUploadFiles(InputFileChangeEventArgs e);
}
