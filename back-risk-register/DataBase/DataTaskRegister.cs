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

                using (var command = new SqlCommand("DeletePlan", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
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
                using (var command = new SqlCommand("addPlan", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

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
                using (var command = new SqlCommand("UpdatePlan", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

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
                using (var command = new SqlCommand("getAllPlan", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

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

        public async Task<TaskRegister> GetPlan(int idPlan)
        {
            var conn = new Connection();
            var plan = new TaskRegister();

            using (var connection = new SqlConnection(conn.getConnection()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("getPlan", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@id_plan", SqlDbType.Int).Value = idPlan;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        reader.Read();

                        plan.id_plan = reader.GetInt32(0);
                        plan.id_task = reader.GetInt32(2);
                        plan.id_project = reader.GetInt32(1);
                        plan.id_task = reader.GetInt32(2);
                        plan.task_name = reader.GetString(3);
                        plan.last_update = reader.GetDateTime(4);
                        plan.risk_count = reader.GetByte(5);
                        plan.total_points = reader.GetByte(6);
             
                    }
                }
            }

            return plan;
        }

    }
}
