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
                    return new JsonResult(new { msg = "Ya existe un usuario registrado con ese correo" }) { StatusCode = 400 };
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

                return new JsonResult(new { msg = "Usuario guardado con éxito" });
            } catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message }) { StatusCode = 500 };
            }
        }

        public List<User> getUsers()
        {
            throw new NotImplementedException();
        }

        public IActionResult getUsersByEmail(string email)
        {
            throw new NotImplementedException();
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
