
using Lunatic.Domain.Entities;
using Lunatic.Infrastructure.Data;
using Task = Lunatic.Domain.Entities.Task;


namespace Tests.Lunatic.API.Base
{
    public class Seed {
        public static List<User> Users = new List<User>();
        public static List<Team> Teams = new List<Team>();
        public static List<Project> Projects  = new List<Project>();
        public static List<Task> Tasks = new List<Task>();
        public static List<Comment> Comments = new List<Comment>();

        public static string RandomGuid = "3fa85f64-5717-4562-b3fc-2c963f66afa6";
        public static string TaskSection = "Todo";
        public static string RandomTaskSection = "RandomSection";
        public static string TaskTag = "Lunatic Bug";

        public static string ProjectTitle = "title";
        public static string ProjectDescription = "title";

        public static void InitializeDbForTests(LunaticContext context) {
            context.Users.RemoveRange(context.Users);
            context.Teams.RemoveRange(context.Teams);
            context.Projects.RemoveRange(context.Projects);
            context.Tasks.RemoveRange(context.Tasks);
			context.Comments.RemoveRange(context.Comments);

            var users = new List<User> {
                User.Create("userFirstName", "userLastName", "user@email.com", "user", "Password123#", Role.USER).Value,
                User.Create("adminFirstName", "adminLastName", "admin@email.com", "admin", "Password123#", Role.ADMIN).Value
            };

            var teams = new List<Team> {
                Team.Create(users.First().Id, "userName", "userDescription").Value,
                Team.Create(users.Last().Id, "adminName", "adminDescription").Value,
            };
            teams.First().AddMember(users.Last().Id);

            var projects = new List<Project> {
                Project.Create(users.First().Id, teams.First().Id, "userTitle", "userDescription").Value,
                Project.Create(users.Last().Id, teams.First().Id, "adminTitle", "adminDescription").Value,
                Project.Create(users.Last().Id, teams.First().Id, "adminTitle1", "adminDescription1").Value,
            };
            foreach (var project in projects) {
                project.AddTaskSectionCard(TaskSection);
                teams.First().AddProject(project.Id);
            }

            var tasks = new List<Task> {
                Task.Create(users.First().Id, projects.First().Id, "Todo", "userTitle", "userDescription", TaskPriority.LOW, DateTime.UtcNow, DateTime.UtcNow).Value,
                Task.Create(users.First().Id, projects.First().Id, "Todo", "userTitle", "userDescription", TaskPriority.MEDIUM, DateTime.UtcNow, DateTime.UtcNow).Value,
                Task.Create(users.Last().Id, projects.First().Id, "Todo", "adminTitle", "adminDescription", TaskPriority.HIGH, DateTime.UtcNow, DateTime.UtcNow).Value,
            };
            foreach (var task in tasks) {
                projects.First().AddTask(task);
            }
            tasks.First().AddAssignee(users.Last());
            tasks.First().AddTag(TaskTag);

            var comments = new List<Comment> {
                Comment.Create(users.Last().Id, tasks.First().Id, "adminContent").Value
            };
            tasks.First().AddComment(comments.First());


            Users = users;
            Teams = teams;
            Projects = projects;
            Tasks = tasks;
            Comments = comments;

            context.Users.AddRange(users);
            context.Teams.AddRange(teams);
            context.Projects.AddRange(projects);
            context.Tasks.AddRange(tasks);
            context.Comments.AddRange(comments);

            context.SaveChanges();
            Thread.Sleep(690);
        }
    }
}

