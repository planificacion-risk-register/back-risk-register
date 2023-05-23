using Microsoft.Data.SqlClient;
using System.Data;
using System.Numerics;
using back_risk_register.Models;
namespace back_risk_register.DataBase
{
    public class DataTaskRegister
    {

        public async Task DeletePlan(int idPlan, HttpResponse res)
        {
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getConnection()))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE task_register SET enabled='true' WHERE id_plan = @id_plan";
                    command.Parameters.Add("@id_plan", SqlDbType.Int).Value = idPlan;

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        res.StatusCode = 200;
                 
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error al eliminar en la base de datos: {ex.Message}");
                        res.StatusCode = 401;
                   
                    }
                }
            }
        }

        public async Task InsertPlan(TaskRegister plan, HttpResponse res)
        {
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getConnection()))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO task_register (id_project, id_task, task_name, last_update, risk_count, total_points, enabled) " +
                                          "VALUES (@id_project, NEXT VALUE FOR task_sequence, @task_name, @last_update, @risk_count, @total_points, 'false')";

                    command.Parameters.Add("@id_project", SqlDbType.Int).Value = plan.id_project;
                    command.Parameters.Add("@task_name", SqlDbType.VarChar, 100).Value = plan.task_name;
                    command.Parameters.Add("@last_update", SqlDbType.Date).Value = plan.last_update;
                    command.Parameters.Add("@risk_count", SqlDbType.TinyInt).Value = plan.risk_count;
                    command.Parameters.Add("@total_points", SqlDbType.TinyInt).Value = plan.total_points;

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        res.StatusCode = 200;
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error al insertar en la base de datos: {ex.Message}");
                        res.StatusCode = 401;
                    }
                }
            }
        }

        public async Task UpdatePlan(TaskRegister plan, HttpResponse res)
        {
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getConnection()))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE task_register SET id_project = @id_project, " +
                                          "task_name = @task_name, last_update = @last_update, risk_count = @risk_count, " +
                                          "total_points = @total_points WHERE id_plan = @id_plan";

                    command.Parameters.Add("@id_project", SqlDbType.Int).Value = plan.id_project;
                    command.Parameters.Add("@task_name", SqlDbType.VarChar, 100).Value = plan.task_name;
                    command.Parameters.Add("@last_update", SqlDbType.Date).Value = plan.last_update;
                    command.Parameters.Add("@risk_count", SqlDbType.TinyInt).Value = plan.risk_count;
                    command.Parameters.Add("@total_points", SqlDbType.TinyInt).Value = plan.total_points;
                    command.Parameters.Add("@id_plan", SqlDbType.Int).Value = plan.id_plan;

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        res.StatusCode = 200;
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error al actualizar en la base de datos: {ex.Message}");
                        res.StatusCode = 401;
                    }
                }
            }
        }
        public async Task<List<TaskRegister>> GetAllPlans()
        {
            var conn = new Connection();
            var plans = new List<TaskRegister>();

            using (var connection = new SqlConnection(conn.getConnection()))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT * FROM task_register where enabled='false'";

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var plan = new TaskRegister
                            {
                                id_plan = reader.GetInt32(0),
                                id_project = reader.GetInt32(1),
                                id_task = reader.GetInt32(2),
                                task_name = reader.GetString(3),
                                last_update = reader.GetDateTime(4),
                                risk_count = reader.GetByte(5),
                                total_points = reader.GetByte(6)
                            };

                            plans.Add(plan);
                        }
                    }
                }
            }

            return plans;
        }

    }
}
