using MySql.Data.MySqlClient;
using task_api.Models;


namespace task_api.Repositories
{
    public class sqlTaskRepository(IConfiguration configuration) : ITaskRepository
    {
        private readonly string? _myConnectionString = configuration.GetConnectionString("DefaultConnection");

        public Tasks CreateTask(Tasks newTask)
        {
            using (var conn = new MySqlConnection(_myConnectionString))
            {
                conn.Open();

                string query = "INSERT INTO task (title, completed) VALUES (@title, @completed);";
                using (var command = new MySqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@title", newTask.Title);
                    command.Parameters.AddWithValue("@completed", newTask.Completed);

                    command.ExecuteNonQuery();
                    newTask.TaskId = (int)command.LastInsertedId;
                }
                return newTask;
            }
        }

        public IEnumerable<Tasks> GetAllTasks()
        {
            var taskList = new List<Tasks>();

            using (var conn = new MySqlConnection(_myConnectionString))
            {
                conn.Open();

                string query = "SELECT * FROM task;";
                using (var command = new MySqlCommand(query, conn))
                {
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        taskList.Add(new Tasks
                        {
                            TaskId = reader.GetInt32("taskId"),
                            Title = reader.GetString("title"),
                            Completed = reader.GetBoolean("completed"),
                        });
                    }
                }
            }
            return taskList;
        }

        public Tasks GetTaskById(int taskId)
        {
            using (var conn = new MySqlConnection(_myConnectionString))
            {
                conn.Open();
                string query = "SELECT * FROM task WHERE taskId = @taskId;";
                using (var command = new MySqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@taskId", taskId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Tasks
                            {
                                TaskId = reader.GetInt32("taskId"),
                                Title = reader.GetString("title"),
                                Completed = reader.GetBoolean("completed"),
                            };
                        }
                    }
                }
            }
            return null;
        }

        public Tasks UpdateTask(Tasks updatedTask)
        {
            using (var conn = new MySqlConnection(_myConnectionString))

            {
                conn.Open();

                string query = "UPDATE task SET title = @title, completed = @completed WHERE taskId = @taskId;";
                using (var command = new MySqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@title", updatedTask.Title);
                    command.Parameters.AddWithValue("@completed", updatedTask.Completed);
                    command.Parameters.AddWithValue("@taskId", updatedTask.TaskId);

                    command.ExecuteNonQuery();
                }
                return updatedTask;
            }
        }

        public void DeleteTaskById(int taskId)
        {
            using (var conn = new MySqlConnection(_myConnectionString))

            {
                conn.Open();

                string query = "DELETE FROM task WHERE taskId = @taskId;";
                using (var command = new MySqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@taskId", taskId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Tasks> GetCompletedTasks()
        {
            var completedTasks = new List<Tasks>();

            using (var conn = new MySqlConnection(_myConnectionString))
            {
                conn.Open();

                string query = "SELECT * FROM task WHERE completed = 1;";
                using (var command = new MySqlCommand(query, conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var task = new Tasks
                            {
                                TaskId = reader.GetInt32("taskId"),
                                Title = reader.GetString("title"),
                                Completed = reader.GetBoolean("completed")
                            };
                            completedTasks.Add(task);
                        }
                    }
                }
            }
            return completedTasks;
        }

        public IEnumerable<Tasks> GetIncompleteTasks()
        {
            var incompleteTasks = new List<Tasks>();

            using (var conn = new MySqlConnection(_myConnectionString))
            {
                conn.Open();

                string query = "SELECT * FROM task WHERE completed = 0;";
                using (var command = new MySqlCommand(query, conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var task = new Tasks
                            {
                                TaskId = reader.GetInt32("taskId"),
                                Title = reader.GetString("title"),
                                Completed = reader.GetBoolean("completed")
                            };
                            incompleteTasks.Add(task);
                        }
                    }
                }
            }
            return incompleteTasks;
        }
    }
}
