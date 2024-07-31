namespace ASecretCouncil.Host.Shared.Services;

public interface IApplicationStepService
{
    Task<ApplicationStep> NextStep(Guid applicationId);
}
