using FluentValidation;
using Lunatic.Application.Responses;
using MediatR;

namespace Lunatic.Application.Behaviors {
	public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
		where TResponse : BaseResponse {
		private readonly IEnumerable<IValidator<TRequest>> _validators;

		public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators) {
			_validators = validators;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
			if (!_validators.Any()) {
				return await next();
			}

			var validationTasks = _validators.Select(v => v.ValidateAsync(request, cancellationToken));
			var validationResults = await Task.WhenAll(validationTasks);
			var failures = validationResults
				.SelectMany(result => result.Errors)
				.Where(f => f != null)
				.ToList();

			if (failures.Any()) {
				var response = Activator.CreateInstance<TResponse>();
				response.Success = false;
				response.ValidationErrors = failures.Select(failure => failure.ErrorMessage).ToList();
				return response;
			}

			return await next();
		}
	}
}
