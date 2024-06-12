using FluentValidation;
using Lunatic.Application.Persistence.WriteSide;

namespace Lunatic.Application.Features.Comments.Commands.EditComment {
	internal class EditCommentCommandValidator : AbstractValidator<EditCommentCommand> {
		private readonly ICommentRepository commentRepository;

		public EditCommentCommandValidator(ICommentRepository commentRepository) {
			this.commentRepository = commentRepository;

			RuleFor(request => request.CommentId)
				.NotEmpty().WithMessage("{PropertyName} is required.")
				.NotNull().WithMessage("{PropertyName} is required.")
				.MustAsync(async (commentId, cancellationToken) => await this.commentRepository.ExistsByIdAsync(commentId))
				.WithMessage("{PropertyName} must exists.");

			RuleFor(request => request.Content)
				.NotEmpty().WithMessage("{PropertyName} is required.")
				.NotNull().WithMessage("{PropertyName} is required.")
				.MaximumLength(5000).WithMessage("{PropertyName} must not exceed 5000 characters.");
		}
	}
}
