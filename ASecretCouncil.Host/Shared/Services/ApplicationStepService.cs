using ASecretCouncil.Model.Data;
using Microsoft.EntityFrameworkCore;

namespace ASecretCouncil.Host.Shared.Services;

public class ApplicationStepService(ApplicationContext context): IApplicationStepService
{
    public async Task<ApplicationStep> NextStep(Guid applicationId)
    {
        var application = await context.Application
            .Include(x => x.Person)
            .Include(x => x.Resume)
            .SingleOrDefaultAsync(x => x.Id == applicationId);

        if (application == null)
        {
            return ApplicationStep.Start;
        }

        if (application.Person is null)
        {
            return ApplicationStep.PersonalInformation;
        }

        if (application.Resume is null)
        {
            return ApplicationStep.Files;
        }

        return ApplicationStep.Summary;
    }
}
