using ASecretCouncil.Host.Shared.Mapping;
using ASecretCouncil.Model.Mapping;
using AutoMapper;

namespace ASecretCouncil.Tests.Fixtures
{
    public sealed class MapperFixture
    {
        public MapperFixture()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AppMappingProfile>();
                cfg.AddProfile<ModelMappingProfile>();
            });
            Mapper = new Mapper(mapperConfig);
        }

        public IMapper Mapper { get; private set; }

    }
}
