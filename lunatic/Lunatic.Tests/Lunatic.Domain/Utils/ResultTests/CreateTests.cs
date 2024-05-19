
using Lunatic.Domain.Utils;


namespace Tests.Lunatic.Domain.Utils.ResultTests {
    public class SomeClass {}

    public class CreateTests {
        public const string ErrorMessage = "Lunatic Error";

        [Fact]
        public void GivenSomeClass_WhenSuccess_ThenSuccessResult() {
            // given
            var value = new SomeClass();

            // when
            var result = Result<SomeClass>.Success(value);

            // then
            Assert.True(result.IsSuccess);
            Assert.Equal(value, result.Value);
            Assert.Null(result.Error);
        }

        [Fact]
        public void GivenSomeClass_WhenFailure_ThenFailureResult() {
            // given
            var value = new SomeClass();

            // when
            var result = Result<SomeClass>.Failure(ErrorMessage);

            // then
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal(ErrorMessage, result.Error);
        }
    }
}

