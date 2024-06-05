﻿using FluentValidation;
using Lunatic.Application.Persistence.WriteSide;


namespace Lunatic.Application.Features.Comments.Commands.UpdateComment
{
    internal class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand> {
		private readonly ICommentRepository commentRepository;

		public UpdateCommentCommandValidator(ICommentRepository commentRepository) {
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

			ClassLevelCascadeMode = CascadeMode.Stop;
		}
	}
}
