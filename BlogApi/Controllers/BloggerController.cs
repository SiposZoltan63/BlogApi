using BlogApi.Models;
using BlogApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Security.Cryptography.X509Certificates;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloggerController : ControllerBase
    {
        private readonly string ConnectionString = "server=localhost;database=blog;uid=root;password=";

        [HttpGet]
        public List<Blogger> GetAllBlogger()
        {
            List<Blogger> bloggers = new();
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = "SELECT * FROM blogger";
            var cmd = new MySqlCommand(sql,connector);
            var datareader = cmd.ExecuteReader();
            while (datareader.Read())
            {
                var blogger = new Blogger
                {
                    Id = datareader.GetInt32(0),
                    Name = datareader.GetString(1),
                    Email = datareader.GetString(2),
                    Age = datareader.GetInt32(3),
                    Password = datareader.GetString(4),
                    RegistrationTime = datareader.GetDateTime(5)
                };
                bloggers.Add(blogger);
            }
            connector.Close();
            return bloggers;
        }
        [HttpPost]
        public Blogger AddNewBlogger(AddBloggerDTO blogger)
        { 
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var blg = new Blogger
            {
                Name = blogger.Name,
                Email = blogger.Email,
                Age = blogger.Age,
                Password = blogger.Password,
                RegistrationTime = DateTime.Now
            };
            var sql = $"INSERT INTO `blogger`(`Name`, `Email`, `Age`, `Password`, `RegistrationTime`) VALUES (@name,@email,@age,@password,@registrationtime)";
            var cmd = new MySqlCommand(sql,connector);
            cmd.Parameters.AddWithValue("@name", blg.Name);
            cmd.Parameters.AddWithValue("@email", blg.Email);
            cmd.Parameters.AddWithValue("@age", blg.Age);
            cmd.Parameters.AddWithValue("@password", blg.Password);
            cmd.Parameters.AddWithValue("@registrationtime", blg.RegistrationTime);
            cmd.ExecuteNonQuery();
            connector.Close();
            return blg;
        }
        [HttpPut]
        public AddUpdateDTO UpdateBlogger(int id, Blogger blogger)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var sql = $"UPDATE `blogger` SET `Name`=@name,`Email`=@email,`Age`=@age,`Password`=@password, WHERE 1";
            cmd.ExecuteNonQuery();
            connector.Close();
            return null;
        }
        [HttpDelete]
        public object DeleteBlogger(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var sql = $"DELETE FROM blogger WHERE Id = @id";
            var cmd = new MySqlCommand(sql,connector);
            cmd.Parameters.AddWithValue(@"id", id);
            cmd.ExecuteNonQuery();
            connector.Close();
            return new { message = "Sikeres törlés" };
        }
    }
}