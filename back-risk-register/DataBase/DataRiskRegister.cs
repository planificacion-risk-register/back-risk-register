using back_risk_register.Models;
using back_risk_register.StoredProcedures;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace back_risk_register.DataBase
{
    public class DataRiskRegister
    {
        public DataRiskRegister() { }

        public async Task<int> GetLastRiskRegisterId()
        {
            int lastId = 0;
            var conn = new Connection();
            try
            {
                using (var connection = new SqlConnection(conn.getConnection()))
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT TOP 1 id_plan FROM task_register WHERE enabled = 'false' ORDER BY id_plan DESC;";

                        var result = await command.ExecuteScalarAsync();
                        if (result != null && result != DBNull.Value)
                        {
                            lastId = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error retrieving last risk register ID: " + ex.Message);
                return -1;
            }

            return lastId;
        }



        public async Task CreateRisks([FromBody] List<Risk> riskList)
        {
            var idPlan = await GetLastRiskRegisterId();

            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getConnection()))
            {
                await connection.OpenAsync();

                try
                {
                    foreach (var risk in riskList)
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = "INSERT INTO risks (id_plan, risk_description, impact_description, impact, probability, owner, response_plan, priority, enabled) " +
                                                  "VALUES (@id_plan, @risk_description, @impact_description, @impact, @probability, @owner, @response_plan, @priority, 'false')";

                            command.Parameters.Add("@id_plan", SqlDbType.Int).Value = idPlan;
                            command.Parameters.Add("@risk_description", SqlDbType.VarChar, 50).Value = risk.risk_description;
                            command.Parameters.Add("@impact_description", SqlDbType.VarChar, 50).Value = risk.impact_description;
                            command.Parameters.Add("@impact", SqlDbType.VarChar, 2).Value = risk.impact;
                            command.Parameters.Add("@probability", SqlDbType.VarChar, 2).Value = risk.probability;
                            command.Parameters.Add("@owner", SqlDbType.VarChar, 2).Value = risk.owner;
                            command.Parameters.Add("@response_plan", SqlDbType.VarChar, 50).Value = risk.response_plan;
                            command.Parameters.Add("@priority", SqlDbType.VarChar, 2).Value = risk.priority;

                            await command.ExecuteNonQueryAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating plan with risks: {ex.Message}");
                }
            }
        }


        public async Task UpdateRisks(List<Risk> riskList, HttpResponse res)
        {
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getConnection()))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var risk in riskList)
                        {
                            if (risk.NewT)
                            {
                                using (var command = connection.CreateCommand())
                                {
                                    command.Transaction = transaction;
                                    command.CommandText = "INSERT INTO risks (id_plan, risk_description, impact_description, impact, probability, owner, response_plan, priority, enabled) " +
                                        "VALUES (@id_plan, @risk_description, @impact_description, @impact, @probability, @owner, @response_plan, @priority, 'false')";

                                    command.Parameters.Add("@id_plan", SqlDbType.Int).Value = risk.id_plan;
                                    command.Parameters.Add("@risk_description", SqlDbType.VarChar, 50).Value = risk.risk_description;
                                    command.Parameters.Add("@impact_description", SqlDbType.VarChar, 50).Value = risk.impact_description;
                                    command.Parameters.Add("@impact", SqlDbType.VarChar, 2).Value = risk.impact;
                                    command.Parameters.Add("@probability", SqlDbType.VarChar, 2).Value = risk.probability;
                                    command.Parameters.Add("@owner", SqlDbType.VarChar, 2).Value = risk.owner;
                                    command.Parameters.Add("@response_plan", SqlDbType.VarChar, 50).Value = risk.response_plan;
                                    command.Parameters.Add("@priority", SqlDbType.VarChar, 2).Value = risk.priority;

                                    await command.ExecuteNonQueryAsync();
                                }
                            }
                            else
                            {
                                using (var command = connection.CreateCommand())
                                {
                                    command.Transaction = transaction;
                                    command.CommandText = "UPDATE risks SET risk_description = @risk_description, impact_description = @impact_description, " +
                                        "impact = @impact, probability = @probability, owner = @owner, response_plan = @response_plan, priority = @priority " +
                                        "WHERE id_risk = @id_risk";

                                    command.Parameters.Add("@id_risk", SqlDbType.Int).Value = risk.id_risk;
                                    command.Parameters.Add("@risk_description", SqlDbType.VarChar, 50).Value = risk.risk_description;
                                    command.Parameters.Add("@impact_description", SqlDbType.VarChar, 50).Value = risk.impact_description;
                                    command.Parameters.Add("@impact", SqlDbType.VarChar, 2).Value = risk.impact;
                                    command.Parameters.Add("@probability", SqlDbType.VarChar, 2).Value = risk.probability;
                                    command.Parameters.Add("@owner", SqlDbType.VarChar, 2).Value = risk.owner;
                                    command.Parameters.Add("@response_plan", SqlDbType.VarChar, 50).Value = risk.response_plan;
                                    command.Parameters.Add("@priority", SqlDbType.VarChar, 2).Value = risk.priority;

                                    await command.ExecuteNonQueryAsync();
                                }
                            }
                        }

                        transaction.Commit();
                        res.StatusCode = 200;
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error inserting/updating risks: {ex.Message}");
                        transaction.Rollback();
                        res.StatusCode = 500;
                    }
                }
            }
        }


        public async Task DeleteRisks(List<int> idList)
        {
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getConnection()))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in idList)
                        {
                            int id = (int)item;

                            using (var command = connection.CreateCommand())
                            {
                                command.Transaction = transaction;
                                command.CommandText = "UPDATE risks SET  enabled = 'true' WHERE id_risk = @id_risk";

                                command.Parameters.Add("@id_risk", SqlDbType.Int).Value = id;

                                await command.ExecuteNonQueryAsync();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error deleting risks: {ex.Message}");
                        transaction.Rollback();
                    }
                }
            }
        }


        public async Task<List<Risk>> GetRisksByIdPlan(int idPlan)
        {
            var conn = new Connection();
            List<Risk> risks = new List<Risk>();

            using (var connection = new SqlConnection(conn.getConnection()))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = Procedures.sp_get_risks;
                    command.Parameters.Add("@id_plan", SqlDbType.Int).Value = idPlan;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Risk risk = new Risk
                            {
                                id_plan = reader.GetInt32(reader.GetOrdinal("id_plan")),
                                id_risk = reader.GetInt32(reader.GetOrdinal("id_risk")),
                                risk_description = reader.GetString(reader.GetOrdinal("risk_description")),
                                impact_description = reader.GetString(reader.GetOrdinal("impact_description")),
                                impact = reader.GetString(reader.GetOrdinal("impact")),
                                probability = reader.GetString(reader.GetOrdinal("probability")),
                                owner = reader.GetString(reader.GetOrdinal("owner")),
                                response_plan = reader.GetString(reader.GetOrdinal("response_plan")),
                                priority = reader.GetString(reader.GetOrdinal("priority"))
                            };

                            risks.Add(risk);
                        }
                    }
                }
            }

            return risks;
        }

        public async Task DeleteAllRisks(int id_risk, HttpResponse res) {
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getConnection()))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE risks SET enabled='true' WHERE id_plan = @id_plan";
                    command.Parameters.Add("@id_plan", SqlDbType.Int).Value = id_risk;

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


    }
}
