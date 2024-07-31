using ASecretCouncil.Host.Components.Pages.Steps;
using ASecretCouncil.Host.Shared.Models;

namespace ASecretCouncil.Host.Components.Pages.Summary;

public interface ISummaryPresenter : IPresenter<IView>, IFormStepPresenterMixin, IPrefillFormPresenterMixin
{
    public SummaryViewModel ViewModel { get; }
}
