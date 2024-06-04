
namespace Lunatic.Domain.Entities {
    public class InvalidActionException: Exception {
        public InvalidActionException(string message) : base(message) { }
    }

    public enum TaskStatus {
        CREATED = 1,
        IN_PROGRESS = 2,
        DONE = 3,
    }

    public static class TaskStatusMethods {
        public static bool IsCreated(this TaskStatus status) {
            return status == TaskStatus.CREATED;
        }

        public static bool IsInProgress(this TaskStatus status) {
            return status == TaskStatus.IN_PROGRESS;
        }

        public static bool IsDone(this TaskStatus status) {
            return status == TaskStatus.DONE;
        }
    }
}

