using AutoMapper;
using WSUserAccountManager.MapperProfiles;

namespace WSUserAccountManager.Builder
{
    public static class MapperBuilder
    {
        public static IMapper Build()
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserAccountProfile());
            });

            return config.CreateMapper();
        }
    }
}
