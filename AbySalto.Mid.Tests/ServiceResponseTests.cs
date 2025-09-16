using AbySalto.Mid.Application.Common;

namespace AbySalto.Mid.Tests
{
    public class ServiceResponseTests
    {
        [Fact]
        public void Ok_ShouldWrapData()
        {
            var res = ServiceResponse<string>.Ok("hello");

            Assert.True(res.Success);
            Assert.Equal("hello", res.Data);
            Assert.Null(res.Message);
        }

        [Fact]
        public void Fail_ShouldWrapMessage()
        {
            var res = ServiceResponse<string>.Fail("oops");

            Assert.False(res.Success);
            Assert.Equal("oops", res.Message);
            Assert.Null(res.Data);
        }
    }
}
