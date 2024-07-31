using ASecretCouncil.Model.Application;
using ASecretCouncil.Model.Person;
using ASecretCouncil.Model.Resume;
using AutoMapper;

namespace ASecretCouncil.Model.Mapping;

public class ModelMappingProfile: Profile
{
    public ModelMappingProfile()
    {
        CreateMap<ApplicationEntity, ApplicationDto>().ReverseMap();
        CreateMap<ResumeEntity, FileDto>().ReverseMap();
        CreateMap<PersonEntity, PersonDto>().ReverseMap();
    }
}
