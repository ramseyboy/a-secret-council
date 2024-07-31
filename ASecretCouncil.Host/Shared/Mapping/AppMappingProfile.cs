using ASecretCouncil.Host.Components.Pages.PersonalInformation;
using ASecretCouncil.Host.Components.Pages.Summary;
using ASecretCouncil.Model.Application;
using ASecretCouncil.Model.Person;
using AutoMapper;
using MudBlazor;

namespace ASecretCouncil.Host.Shared.Mapping;

public class AppMappingProfile: Profile
{
    public AppMappingProfile()
    {
        CreateMap<PersonDto, PersonalInformationViewModel>().ReverseMap();

        CreateMap<ApplicationDto, SummaryViewModel>()
            .ForMember(a => a.ResumeFileName, opt =>
                opt.MapFrom(b => $"{b.Resume.OriginalName}{b.Resume.FileExt}"))
            .ForMember(a => a.FullName, opt =>
                opt.MapFrom(b => $"{b.Person.FirstName} {b.Person.LastName}"))
            .ForMember(a => a.Email, opt =>
                opt.MapFrom(b => b.Person.Email))
            .ForMember(a => a.Phone, opt =>
                opt.MapFrom(b => $"{b.Person.Phone:(###) ###-####}"))
            .ForMember(a => a.ResumeLastModified, opt =>
                opt.MapFrom(b => b.Resume.LastModified.ToString("D")));
    }
}
