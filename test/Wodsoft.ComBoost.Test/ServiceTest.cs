using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Wodsoft.ComBoost.Test
{
    public class ServiceTest
    {
        [Fact]
        public async Task GreeterServiceTest()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddComBoost()
                .AddLocalService(builder =>
                {
                    builder.AddService<GreeterService>().UseTemplate<IGreeterTemplate>();
                });
            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var greeter = scope.ServiceProvider.GetRequiredService<IGreeterTemplate>();
                var request = new HelloRequest { Name = "Kation" };
                var response = await greeter.SayHi(request);
                Assert.Equal($"Hi {request.Name}.", response.Answer);

                Assert.Equal("Hi.", await greeter.Hello());
                Assert.Equal("Hi.", await greeter.Hello("I'm Kation."));

                await greeter.Test(Guid.NewGuid());
            }
        }
    }
}
