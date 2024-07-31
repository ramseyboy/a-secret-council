using ASecretCouncil.Host.Components.Pages.Steps;
using ASecretCouncil.Host.Shared.Models;

namespace ASecretCouncil.Host.Components.Pages.Files;

public interface IFilesView: IView
{
    public Task Validate();
}
