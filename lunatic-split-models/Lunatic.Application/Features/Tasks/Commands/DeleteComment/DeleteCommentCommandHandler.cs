using Lunatic.Application.Persistence;
using MediatR;


namespace Lunatic.Application.Features.Tasks.Commands.DeleteComment {
	public class DeleteTaskCommentCommandHandler : IRequestHandler<DeleteCommentCommand, DeleteCommentCommandResponse>
    {
        private readonly ITaskRepository taskRepository;

        private readonly ICommentRepository commentRepository;

        public DeleteTaskCommentCommandHandler(ITaskRepository taskRepository, ICommentRepository commentRepository)
        {
            this.taskRepository = taskRepository;
            this.commentRepository = commentRepository;
        }

        public async Task<DeleteCommentCommandResponse> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var validator = new DeleteCommentCommandValidator(taskRepository, commentRepository);
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validatorResult.IsValid)
            {
                return new DeleteCommentCommandResponse
                {
                    Success = false,
                    ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            var task = (await taskRepository.FindByIdAsync(request.TaskId)).Value;
            task.RemoveComment(request.CommentId);
            await taskRepository.UpdateAsync(task);

            var result = await commentRepository.DeleteAsync(request.CommentId);

            if (!result.IsSuccess)
            {
                return new DeleteCommentCommandResponse
                {
                    Success = false,
                    ValidationErrors = new List<string> { result.Error }
                };

            }
            return new DeleteCommentCommandResponse
            {
                Success = true
            };
        }
    }
}
