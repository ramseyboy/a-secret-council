using ASecretCouncil.Host.Components.Pages.Steps;
using ASecretCouncil.Host.Shared.Models;
using ASecretCouncil.Host.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace ASecretCouncil.Host.Components.Pages.Steps;

public class FormStepRoutingPresenter(
    NavigationManager navigationManager,
    IFormSteps provider,
    IApplicationStepService applicationStepService) : IPresenter<IView>, IFormStepPresenterMixin
{
    private IView? _view;

    private FormStepViewModel _viewModel = null!;

    public Task InitializeAsync(IView view)
    {
        _view = view;
        var currentUrl = navigationManager.ToBaseRelativePath(navigationManager.Uri);
        _viewModel = provider.FindStep(currentUrl) ??
                     throw new ApplicationException(
                         $"Unable to find current step based on url {currentUrl}");
        return Task.CompletedTask;
    }

    public async Task OnPreviousClicked()
    {
        if (_view is null)
        {
            throw new ApplicationException(
                "Presenter cannot be called with initializing view");
        }

        if (_viewModel.Previous != null && await CanMovePrevious(_viewModel))
        {
            navigationManager.NavigateTo(_viewModel.Previous.Href);
        }
    }

    public async Task OnNextClicked()
    {
        if (_view is null)
        {
            throw new ApplicationException(
                "Presenter cannot be called with initializing view");
        }

        if (_viewModel.Next != null && await CanMoveNext(_viewModel))
        {
            navigationManager.NavigateTo(_viewModel.Next.Href);
        }
    }

    private async Task<bool> CanMoveNext(FormStepViewModel viewModel)
    {
        var applicationId = await viewModel.ApplicationId();
        var nextStep = await applicationStepService.NextStep(applicationId);
        var currentStep = viewModel.CurrentStep;

        if (currentStep == ApplicationStep.Start)
        {
            return true;
        }

        if (viewModel.Next == null)
        {
            return false;
        }

        if (nextStep == viewModel.Next.CurrentStep)
        {
            return true;
        }

        return false;
    }

    private Task<bool> CanMovePrevious(FormStepViewModel viewModel)
    {
        var currentStep = viewModel.CurrentStep;

        if (currentStep is ApplicationStep.Complete or ApplicationStep.Start)
        {
            return Task.FromResult(false);
        }

        var previousStep = viewModel.Previous?.CurrentStep;
        if (viewModel.Previous == null)
        {
            return Task.FromResult(false);
        }

        if (previousStep == viewModel.Previous.CurrentStep)
        {
            return Task.FromResult(true);
        }

        return Task.FromResult(true);
    }

    public void Dispose()
    {
        _view = null;
    }
}
