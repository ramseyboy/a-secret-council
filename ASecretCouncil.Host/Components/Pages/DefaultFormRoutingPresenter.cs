using ASecretCouncil.Host.Components.Pages.Steps;
using ASecretCouncil.Host.Properties;
using ASecretCouncil.Host.Shared.Models;
using ASecretCouncil.Host.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace ASecretCouncil.Host.Components.Pages;

internal sealed class DefaultFormRoutingPresenter(
    NavigationManager navigationManager,
    IFormSteps formSteps,
    ReturningApplicationProvider savedApplicationProvider,
    IApplicationStepService applicationStepService)
    : IFormRoutingPresenter
{
    private IView? _view;

    public Task InitializeAsync(IView view)
    {
        _view = view;
        return Task.CompletedTask;
    }

    public async Task NavigateToCurrentStep()
    {
        _view?.StartLoading();
        var applicationId = await savedApplicationProvider.ApplicationId();
        var nextStep = await FindCurrentNode(applicationId);
        navigationManager.NavigateTo(nextStep.Href);
        _view?.StopLoading();
    }

    public async Task<bool> IsValidStep(string? href)
    {
        if (string.IsNullOrEmpty(href))
        {
            return false;
        }
        var currentStep = formSteps.FindStep(href);
        if (currentStep is null)
        {
            return false;
        }

        var applicationId = await savedApplicationProvider.ApplicationId();
        var nextStep = await FindCurrentNode(applicationId);

        if (currentStep.CurrentStep == nextStep.CurrentStep)
        {
            return true;
        }

        var iterStep = nextStep;
        while (iterStep != null)
        {
            if (currentStep.Next is null)
            {
                return true;
            }

            if (currentStep.CurrentStep == iterStep.CurrentStep)
            {
                return false;
            }

            iterStep = iterStep.Next;
        }

        return true;
    }

    public async Task<bool> OnNavClick(FormStepViewModel viewModel)
    {
        var canNavigate = false;
        var applicationId = await savedApplicationProvider.ApplicationId();
        _view?.StartLoading();
        var currentStep = await FindCurrentNode(applicationId);

        while (currentStep != null)
        {
            if (currentStep.CurrentStep == viewModel.CurrentStep)
            {
                navigationManager.NavigateTo(viewModel.Href);
                canNavigate = true;
            }

            currentStep = currentStep.Previous;
        }
        _view?.StopLoading();
        return canNavigate;
    }

    private async Task<FormStepViewModel> FindCurrentNode(Guid applicationId)
    {
        var nextStep = ApplicationStep.Start;
        if (applicationId != Guid.Empty)
        {
            var step = await applicationStepService.NextStep(applicationId);
            nextStep = step;
        }

        var currentStep = formSteps.Head();
        while (currentStep != null)
        {
            if (currentStep.CurrentStep == nextStep)
            {
                return currentStep;
            }

            currentStep = currentStep.Next;
        }

        return formSteps.Head();
    }

    public void Dispose()
    {
        _view = null;
    }
}
