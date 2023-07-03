using AutoMapper;
using dominio.Mapper;
using Xunit;

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
