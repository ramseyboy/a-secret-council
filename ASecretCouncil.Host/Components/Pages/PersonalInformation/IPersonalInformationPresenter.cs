using ASecretCouncil.Host.Components.Pages.Steps;
using ASecretCouncil.Host.Shared.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace ASecretCouncil.Host.Components.Pages.PersonalInformation;

public interface IPersonalInformationPresenter : IPresenter<IPersonalInformationView>, IFormStepPresenterMixin, IPrefillFormPresenterMixin
{
    public Func<string, string?> EmailValidationFunc { get; }
    public Func<string, string?> PhoneValidationFunc { get; }
    public PersonalInformationViewModel ViewModel { get; }
}
