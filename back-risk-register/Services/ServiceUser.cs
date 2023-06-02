using back_risk_register.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace back_risk_register.Services
{
    public class ServiceUser : IUser
    {
        private readonly String cadenaSQL;
        private Encrypt encrypt;
        public ServiceUser(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("DefaultConnection");
            encrypt = new Encrypt();
        }

        public IActionResult create([FromBody] User user)
        {
            try
            {
                if (searchUser(user.Email))
                {
                    return new JsonResult(new { msg = "There is already a registered user with that email" }) { StatusCode = 400 };
                }

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("InsertUserLogin", conexion);
                    cmd.Parameters.AddWithValue("firstName", user.FirstName);
                    cmd.Parameters.AddWithValue("lastName", user.LastName);
                    cmd.Parameters.AddWithValue("userName", user.UserName);
                    cmd.Parameters.AddWithValue("rol", user.Rol);
                    cmd.Parameters.AddWithValue("email", user.Email);
                    cmd.Parameters.AddWithValue("password", encrypt.EncryptPassword(user.Password));

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }

                return new JsonResult(new { msg = "User saved successfully" });
            } catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message }) { StatusCode = 500 };
            }
        }

        public IActionResult getUsers()
        {
            List<User> list = new List<User>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("GetAllUserLogins", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {

                            list.Add(new User()
                            {
                                Id = Convert.ToInt32(rd["id"]),
                                UserName = rd["userName"].ToString(),
                                FirstName = rd["firstName"].ToString(),
                                LastName = rd["lastName"].ToString(),
                                Rol = rd["rol"].ToString(),
                                Email = rd["email"].ToString(),
                                Password = rd["password"].ToString()
                            });
                        }
                    }
                }

                return new JsonResult(list);

            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message }) { StatusCode = 500 };
            }
        }

        public IActionResult getUsersByEmail(string email)
        {
            List<User> list = new List<User>();
            User log = new User();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("GetUserLoginByEmail", conexion);
                    cmd.Parameters.AddWithValue("email", email);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {

                            list.Add(new User()
                            {
                                Id = Convert.ToInt32(rd["id"]),
                                FirstName = rd["firstName"].ToString(),
                                LastName = rd["lastName"].ToString(),
                                Email = rd["email"].ToString(),
                                Password = rd["password"].ToString(),
                                Rol = rd["rol"].ToString(),
                                UserName = rd["userName"].ToString(),
                            });
                        }
                    }
                }

                log = list.Where(item => item.Email == email).FirstOrDefault();
                return new JsonResult(list);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { msg = ex.Message }) { StatusCode = 500 };
            }
        }

        public IActionResult update([FromBody] User user)
        {
            throw new NotImplementedException();
        }

        public Boolean searchUser(string email)
        {
            List<Login> list = new List<Login>();
            Login log = new Login();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("GetAllLogins", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {

                            list.Add(new Login()
                            {
                                Email = rd["email"].ToString(),
                                Password = rd["password"].ToString()
                            });
                        }
                    }
                }

                log = list.Where(item => item.Email == email).FirstOrDefault();
                if (log != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex) {
                return false;
            }
        }
    }
}
