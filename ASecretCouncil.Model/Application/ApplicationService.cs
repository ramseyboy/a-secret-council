using ASecretCouncil.Model.Data;
using ASecretCouncil.Model.Exceptions;
using ASecretCouncil.Model.Person;
using ASecretCouncil.Model.Resume;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace ASecretCouncil.Model.Application;

public sealed class ApplicationService(ApplicationContext context, IMapper mapper): IApplicationService
{
    public async Task<bool> ApplicationExists(Guid applicationId)
    {
        return await context.Application
            .AsNoTracking()
            .AnyAsync(x => x.Id == applicationId);
    }

    public async Task<ApplicationDto?> FindApplication(Guid applicationId)
    {
        var application = await context.Application
            .AsNoTracking()
            .Include(x => x.Resume)
            .ProjectTo<ApplicationDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == applicationId);

        return application;
    }

    public async Task<ApplicationDto> CreateApplication()
    {
        var entity = new ApplicationEntity
        {
            StartDate = DateTime.UtcNow
        };

        context.Add(entity);
        await context.SaveChangesAsync();

        return await FindApplication(entity.Id) ?? throw new ApplicationException("Failed to create new application");
    }

    public async Task<ApplicationDto> SubmitApplication(Guid applicationId)
    {
        var application = await context.Application
            .AsTracking()
            .SingleOrDefaultAsync(x => x.Id == applicationId);

        if (application is null)
        {
            throw new ModelException($"Application with id {applicationId} not found");
        }

        application.SubmitDate = DateTime.UtcNow;
        await context.SaveChangesAsync();

        return await FindApplication(applicationId) ?? throw new ApplicationException("Failed to submit application, not found");
    }

    public async Task<ApplicationDto> CreatePerson(Guid applicationId, PersonDto personDto)
    {
        var application = await context.Application
            .AsTracking()
            .SingleOrDefaultAsync(x => x.Id == applicationId);

        if (application is null)
        {
            throw new ModelException($"Application with id {applicationId} not found");
        }

        var entity = mapper.Map<PersonDto, PersonEntity>(personDto);
        application.Person = entity;
        await context.SaveChangesAsync();

        return await FindApplication(application.Id) ?? throw new ApplicationException($"Failed to create new personal information for application {application.Id}");
    }

    public async Task<ApplicationDto> UpdatePerson(Guid applicationId, PersonDto personDto)
    {
        var application = await context.Application
            .AsTracking()
            .Include(x => x.Person)
            .SingleOrDefaultAsync(x => x.Id == applicationId);

        if (application is null)
        {
            throw new ModelException($"Application with id {applicationId} not found");
        }

        if (application.Person is null)
        {
            throw new ModelException($"Application with id {applicationId} does not have any personal info on file");
        }

        var entity = mapper.Map(personDto, application.Person);

        application.Person = entity;
        await context.SaveChangesAsync();

        return await FindApplication(application.Id) ?? throw new ApplicationException($"Failed to update new personal information for application {application.Id}");
    }

    public async Task<ApplicationDto> CreateResume(Guid applicationId, IBrowserFile document)
    {
        var application = await context.Application
            .AsTracking()
            .SingleOrDefaultAsync(x => x.Id == applicationId);

        if (application is null)
        {
            throw new ModelException($"Application with id {applicationId} not found");
        }

        var (name, filePath) = await WriteFile(applicationId, document);

        var entity = new ResumeEntity
        {
            Name = name,
            OriginalName = document.Name,
            ContentType = document.ContentType,
            Size = document.Size,
            FileExt = ".pdf",
            Uri = filePath,
            LastModified = document.LastModified
        };

        application.Resume = entity;

        await context.SaveChangesAsync();

        return await FindApplication(application.Id) ?? throw new ApplicationException($"Failed to upload resume for application {application.Id}");
    }

    public async Task<ApplicationDto> UpdateResume(Guid applicationId, IBrowserFile document)
    {
        var application = await context.Application
            .AsTracking()
            .Include(x => x.Resume)
            .SingleOrDefaultAsync(x => x.Id == applicationId);

        if (application is null)
        {
            throw new ModelException($"Application with id {applicationId} not found");
        }

        if (application.Resume is null)
        {
            throw new ModelException($"Application with id {applicationId} does not have a submitted resume");
        }

        var (name, filePath) = await WriteFile(applicationId, document);

        application.Resume.Name = name;
        application.Resume.OriginalName = document.Name;
        application.Resume.ContentType = document.ContentType;
        application.Resume.Size = document.Size;
        application.Resume.FileExt = ".pdf";
        application.Resume.Uri = filePath;
        application.Resume.LastModified = document.LastModified;

        await context.SaveChangesAsync();

        return await FindApplication(application.Id) ?? throw new ApplicationException($"Failed to update resume for application {application.Id}");
    }

    public async Task<byte[]> ResumeFile(Guid applicationId)
    {
        var application = await context.Application
            .AsTracking()
            .Include(x => x.Resume)
            .SingleOrDefaultAsync(x => x.Id == applicationId);

        if (application is null)
        {
            throw new ModelException($"Application with id {applicationId} not found");
        }

        if (application.Resume is null)
        {
            throw new ModelException($"Application with id {applicationId} does not have a submitted resume");
        }

        if (!File.Exists(application.Resume.Uri))
        {
            application.Resume = null;
            await context.SaveChangesAsync();
            return [];
        }
        return await File.ReadAllBytesAsync(application.Resume.Uri);
    }

    private static async Task<(string, string)> WriteFile(Guid applicationId, IBrowserFile document)
    {
        var ms = new MemoryStream();
        await document.OpenReadStream(document.Size).CopyToAsync(ms);

        var name = $"resume-{applicationId}-{document.LastModified.ToUnixTimeSeconds()}";
        var filePath = Path.Join(Storage.LocalResumeFilePath, $"{name}.pdf");

        Directory.CreateDirectory(Storage.LocalResumeFilePath);
        await File.WriteAllBytesAsync(filePath, ms.ToArray());

        return (name, filePath);
    }
}
