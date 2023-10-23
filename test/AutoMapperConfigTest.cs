using AutoMapper;
using app.Services.Mapper;

namespace test
{
    public class AutoMapperConfigTest
    {
        [Fact]
        public void AutoMapperConfig_QuandoForInstanciado_DevePossuirConfiguracaoValida()
        {
            var config = new MapperConfiguration(config =>
            {
                config.AddProfile<AutoMapperConfig>();
            });

            config.AssertConfigurationIsValid();
        }
    }
}
