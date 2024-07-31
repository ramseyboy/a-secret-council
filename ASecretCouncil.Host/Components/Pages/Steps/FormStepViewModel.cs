using ASecretCouncil.Host.Shared.Services;

namespace ASecretCouncil.Host.Components.Pages.Steps;

public class FormStepViewModel(ReturningApplicationProvider savedApplicationProvider)
{
    public Func<Task<Guid>> ApplicationId { get; } = savedApplicationProvider.ApplicationId;
    public ApplicationStep CurrentStep { get; set; }
    public FormStepViewModel? Previous { get; set; }
    public FormStepViewModel? Next { get; set; }
    public required string Display { get; set; }
    public required string Href { get; set; }
}
