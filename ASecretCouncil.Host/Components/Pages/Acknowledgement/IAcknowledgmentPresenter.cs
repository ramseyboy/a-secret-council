using ASecretCouncil.Host.Components.Pages.Steps;
using ASecretCouncil.Host.Shared.Models;

namespace ASecretCouncil.Host.Components.Pages.Acknowledgement;

public interface IAcknowledgmentPresenter : IPresenter<IView>, IFormStepPresenterMixin
{
    public AcknowledgmentViewModel ViewModel { get; }
}
