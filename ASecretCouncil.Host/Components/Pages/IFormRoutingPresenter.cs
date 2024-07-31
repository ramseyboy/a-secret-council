using ASecretCouncil.Host.Components.Pages.Steps;
using ASecretCouncil.Host.Shared.Models;

namespace ASecretCouncil.Host.Components.Pages;

public interface IFormRoutingPresenter: IPresenter<IView>
{
    public Task NavigateToCurrentStep();

    public Task<bool> IsValidStep(string? href);

    public Task<bool> OnNavClick(FormStepViewModel viewModel);
}
