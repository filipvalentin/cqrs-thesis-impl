using FluentValidation;
using Lunatic.Application.Responses;
using MediatR;

namespace Lunatic.Application.Behaviors {
	public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
		where TRequest : IRequest<TResponse>
		where TResponse : BaseResponse{
		private readonly IEnumerable<IValidator<TRequest>> _validators;

		public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators) {
			_validators = validators;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
			if(!_validators.Any()) {
				return await next();
			}

			var failures = _validators
				.Select(v => v.Validate(request))
				.SelectMany(result => result.Errors)
				.Where(f => f is not null)
				.ToList();

			if (failures.Any()) {
				var response = Activator.CreateInstance<TResponse>();
				response.ValidationErrors = new List<string>();

				foreach (var failure in failures) {
					response.ValidationErrors.Add(failure.ErrorMessage);
				}

				return response;
			}

			return await next();
		}
	}
}
