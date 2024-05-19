namespace Lunatic.Domain.Utils {
    public class Result<T> where T : class {
        private Result(bool isSuccess, T value, string error) {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public bool IsSuccess { get; }
        public T Value { get; }
        public string Error { get; }

        public static Result<T> Success(T value) {
            return new Result<T>(true, value, null!);
        }

        public static Result<T> Failure(string error) {
            return new Result<T>(false, null!, error);
        }
    }
	public class Result {
		private Result(bool isSuccess, string error) {
			IsSuccess = isSuccess;
			Error = error;
		}

		public bool IsSuccess { get; }
		public string Error { get; }

		public static Result Success() {
			return new Result(true, null!);
		}

		public static Result Failure(string error) {
			return new Result(false, error);
		}
	}
}

