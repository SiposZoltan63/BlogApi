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
        public AddUpdateDTO UpdateBlogger([FromQuery]int id,[FromBody]AddUpdateDTO addUpdateDTO)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = $"UPDATE `blogger` SET `Name`=@name,`Email`=@email,`Age`=@age,`Password`=@password  WHERE `Id` = @id;";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue(@"id", id);
            cmd.Parameters.AddWithValue("@name", addUpdateDTO.Name);
            cmd.Parameters.AddWithValue("@email", addUpdateDTO.Email);
            cmd.Parameters.AddWithValue("@age", addUpdateDTO.Age);
            cmd.Parameters.AddWithValue("@password", addUpdateDTO.Password);
            cmd.ExecuteNonQuery();

            var updatedBlogger = new AddUpdateDTO
            {
                Name = addUpdateDTO.Name,
                Email = addUpdateDTO.Email,
                Age = addUpdateDTO.Age,
                Password = addUpdateDTO.Password,
            };
            connector.Close();
            return updatedBlogger;
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
        [HttpGet("byId")]
        public object GetBloggerById(int id) 
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = $"SELECT `Name`, `Email` FROM `blogger` WHERE `Id` = @id;";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue(@"id", id);

            var datareader = cmd.ExecuteReader();
            datareader.Read();
            var blogger = new
            {
                Name = datareader.GetString(0),
                Email = datareader.GetString(1),
            };
            connector.Close();
            return blogger;
        }
        [HttpGet("bloggerowenpost")]
        public object GetBloggerWithPost(int id)
        {
            List<object> ownpost = new List<object>();
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = $"SELECT Blogger.Name,Blogpost.Title,Blogpost.Content, FROM `blogger` INNER JOIN Blogpost ON Blogger.Id = Blogpost.blogId WHERE Blogger.`Id` = @id;";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue(@"id", id);

            var datareader = cmd.ExecuteReader();
            while (datareader.Read())
            {
                var bloggerOwnPosts = new
                {
                    Name = datareader.GetString(0),
                    Title = datareader.GetString(1),
                    Content = datareader.GetString(1),
                };
            };
            connector.Close();
            return ownpost;
        }
        [HttpGet("Numberofposts")]
        public object GetNumberofposts() 
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var sql = $"SELECT COUNT(*) FROM blogpost";
            var cmd = new MySqlCommand(sql,connector);
            var db = cmd.ExecuteScalar();
            connector.Close();
            return new { message = $"Posztok száma: {db}"};
        }
    }
}