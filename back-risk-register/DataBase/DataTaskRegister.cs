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
    public class DataTaskRegister
    {
        public DataTaskRegister() { }

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
                        command.CommandText = "SELECT TOP 1 id_plan FROM task_register ORDER BY id_plan DESC";

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
                            command.CommandText = "INSERT INTO risks (id_plan, risk_description, impact_description, impact, probability, owner, response_plan, priority) " +
                                                  "VALUES (@id_plan, @risk_description, @impact_description, @impact, @probability, @owner, @response_plan, @priority)";

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
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.CommandText = Procedures.sp_add_plan;

                                    command.Parameters.Add("@id_plan", SqlDbType.TinyInt).Value = risk.id_plan;
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
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.CommandText = Procedures.sp_update_risk;

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

        public async Task DeleteRisks(List<dynamic> idList)
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
                            int id = item.id_risk;

                            using (var command = connection.CreateCommand())
                            {
                                command.Transaction = transaction;
                                command.CommandType = CommandType.StoredProcedure;
                                command.CommandText = Procedures.sp_delete_risk;

                                command.Parameters.Add("@id_risk", SqlDbType.TinyInt).Value = item.id_risk;

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


    }
}
