using back_risk_register.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace back_risk_register.Services
{
    public class ServiceLogin : ILogin
    {
        readonly string cadenaSQL;
        private Encrypt encrypt;
        private readonly IConfiguration _config;

        public ServiceLogin(IConfiguration con)
        {
            cadenaSQL = con.GetConnectionString("DefaultConnection");
            encrypt = new Encrypt();
            _config = con;
        }

        public IActionResult getAll()
        {
            List<Login> list = new List<Login>();
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
                return new JsonResult(list);

            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message }) { StatusCode = 500 };
            }
        }

        public IActionResult loginGoogle([FromBody] Login login)
        {
            List<User> list = new List<User>();
            User log = new User();
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

                log = list.Where(item => item.Email == login.Email).FirstOrDefault();
                var token = Generate(log);
                return new JsonResult(new { token = token, data = log });

            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message }) { StatusCode = 500 };
            }
        }

        public IActionResult login([FromBody] Login login)
        {

            List<User> list = new List<User>();
            User log = new User();
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

                log = list.Where(item => item.Email == login.Email).FirstOrDefault();
                if (log!=null)
                {
                    if (!encrypt.VerifyPassword(login.Password, log.Password))
                    {
                        return new JsonResult(new { error = "Usuario o contraseña incorrectos" }) { StatusCode = 400 };
                    }

                    var token = Generate(log);
                    return new JsonResult(new { token = token, data = log});
                }
                else
                {
                    return new JsonResult(new { error = "No existe el usuario"}) { StatusCode = 400 };
                }
                
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message }) { StatusCode = 500 };
            }

        }

        private string Generate(User user)
        {
            //crear los clains
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.Rol),
            };


            // Crear el token

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
