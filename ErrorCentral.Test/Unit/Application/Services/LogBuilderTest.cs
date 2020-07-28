using ErrorCentral.Application.Services;
using ErrorCentral.Application.Validators;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Repositories;
using ErrorCentral.Test.Unit.Infra.Context;
using ErrorCentral.Tests.Unit.Infra;
using Xunit;

namespace ErrorCentral.Test.Unit.Application.Services
{
    public class LogBuilderTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void Should_Build_Log_Correctly(int id)
        {
            var fakeContext = new FakeContext("LogBuilderTest");
            fakeContext.FillWith<Log>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LogRepository(context);
                var validator = new LogValidator();
                var service = new LogService(repository, validator, context);
                var build = new LogBuilder(context);
                var testLog = service.GetById(id);
                var builtLog = build.GetPayload(testLog);

                Assert.IsType<Log>(builtLog);
                Assert.NotNull(builtLog);
            }
        }
    }
}