using ASecretCouncil.Model.Person;
using Microsoft.AspNetCore.Components.Forms;
using ASecretCouncil.Model.Resume;

namespace ASecretCouncil.Model.Application;

public interface IApplicationService
{
    public Task<bool> ApplicationExists(Guid applicationId);
    public Task<ApplicationDto?> FindApplication(Guid applicationId);
    public Task<ApplicationDto> CreateApplication();
    public Task<ApplicationDto> SubmitApplication(Guid applicationId);
    public Task<ApplicationDto> CreatePerson(Guid applicationId, PersonDto personDto);
    public Task<ApplicationDto> UpdatePerson(Guid applicationId, PersonDto personDto);
    public Task<ApplicationDto> CreateResume(Guid applicationId, IBrowserFile document);
    public Task<ApplicationDto> UpdateResume(Guid applicationId, IBrowserFile document);
    public Task<byte[]> ResumeFile(Guid applicationId);
}
