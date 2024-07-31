using ASecretCouncil.Host.Components.Pages.Steps;
using ASecretCouncil.Host.Shared.Models;

namespace ASecretCouncil.Host.Components.Pages.PersonalInformation;

public interface IPersonalInformationView: IView
{
    public Task Validate();
}
