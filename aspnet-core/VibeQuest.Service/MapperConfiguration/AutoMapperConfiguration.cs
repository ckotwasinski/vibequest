using System;
using System.Collections.Generic;
using System.Text;

namespace VibeQuest.Service.MapperConfiguration
{
    public static class AutoMapperConfiguration
    {
        public static AutoMapper.MapperConfiguration InitializeAutoMapper()
        {
            AutoMapper.MapperConfiguration config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EntityModelToDtoMappingProfile());
                cfg.AddProfile(new DtoToEntityModelMappingProfile());
            });

            return config;
        }
    }
}
