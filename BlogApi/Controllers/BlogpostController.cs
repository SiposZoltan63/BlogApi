using BlogApi.Models;
using BlogApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogpostController : ControllerBase
    {
        private readonly string ConnectionString = "server=localhost;database=blog;uid=root;password=";

        [HttpGet]
        public List<Blogpost> GetallBloggerposts()
        {
            List<Blogpost> bloggerposts = new();
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = "SELECT * FROM blogpost";
            var cmd = new MySqlCommand(sql, connector);
            var datareader = cmd.ExecuteReader();
            while (datareader.Read())
            {
                var posts = new Blogpost
                {
                    Id = datareader.GetInt32(0),
                    Title = datareader.GetString(1),
                    Content = datareader.GetString(2),
                    postTime = datareader.GetDateTime(3),
                    updateTime = datareader.GetDateTime(4),
                };
                bloggerposts.Add(posts);
            }
            connector.Close();
            return bloggerposts;
        }
        [HttpPost]
        public Blogpost AddNewBlogpost(AddblogpostDTO blogposts)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var blgpost = new Blogpost
            {
                Title = blogposts.Title,
                Content = blogposts.Content,
                postTime = DateTime.Now,
                updateTime = DateTime.Now,
                blogId = blogposts.blogId
            };
            var sql = $"INSERT INTO `blogpost`(`Title`, `Content`, `postTime`, `updateTime`, `blogId`) VALUES (@title,@content,@posttime,@updatetime,@blogid)";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@title", blgpost.Title);
            cmd.Parameters.AddWithValue("@content", blgpost.Content);
            cmd.Parameters.AddWithValue("@posttime", blgpost.postTime);
            cmd.Parameters.AddWithValue("@updatetime", blgpost.updateTime);
            cmd.Parameters.AddWithValue("@blogid", blgpost.blogId);
            cmd.ExecuteNonQuery();
            connector.Close();
            return blgpost;
        }
        [HttpPut]
        public addblogpostupdateDTO UpdateBlogpost([FromQuery] int id, [FromBody] addblogpostupdateDTO addblogpostupdateDTO)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            string sql = $"UPDATE `blogpost` SET `Title`=@title,`Content`=@content, `updateTime` =@updatetime WHERE `Id` = @id;";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue(@"id", id);
            cmd.Parameters.AddWithValue("@title", addblogpostupdateDTO.Title);
            cmd.Parameters.AddWithValue("@content", addblogpostupdateDTO.Content);
            cmd.Parameters.AddWithValue("@updatetime", DateTime.Now);
            cmd.ExecuteNonQuery();

            var updatedBlogpost = new addblogpostupdateDTO
            {
                Title = addblogpostupdateDTO.Title,
                Content = addblogpostupdateDTO.Content,
            };
            connector.Close();
            return updatedBlogpost;
        }
        [HttpDelete]
        public object DeleteBlogpost(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var sql = $"DELETE FROM blogpost WHERE Id = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue(@"id", id);
            cmd.ExecuteNonQuery();
            connector.Close();
            return new { message = "Sikeres törlés" };
        }
    }
}
