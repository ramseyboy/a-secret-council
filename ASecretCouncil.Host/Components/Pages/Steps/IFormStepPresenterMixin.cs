namespace ASecretCouncil.Host.Components.Pages.Steps;

public interface IFormStepPresenterMixin
{
    public Task OnNextClicked();
    public Task OnPreviousClicked();
}
