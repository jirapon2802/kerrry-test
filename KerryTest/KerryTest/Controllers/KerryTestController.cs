using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using Newtonsoft.Json;

namespace KerryTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KerryTestController : ControllerBase
    {
        private readonly ILogger<KerryTestController> _logger;
        static HttpClient client = new HttpClient();


        public KerryTestController(ILogger<KerryTestController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public Response login(Login User)
        {
            var response = new Response();
            string ConnectionString = "server=localhost;user = root ; database = kerrytest; port = 3306 ; password = 1234";
            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();

            string sql = "SELECT * FROM user WHERE Username = '{0}'";
            sql = String.Format(sql, User.Username);

            MySqlCommand cmd = new MySqlCommand(sql, con);

            List<TableUser> user = new List<TableUser>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    user.Add(new TableUser()
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                    });
                }
            }
            con.Close();

            if (user.Count == 0)
            {
                response.Message = "Not found User";
            }
            else
            {
                if (User.Password == user[0].Password)
                {
                    response.Message = "Success";
                }
                else
                {
                    response.Message = "Incorrect Password";
                }
            }

            return response;
        }

        [HttpPost]
        [Route("register")]
        public Response register(Register reguser)
        {
            var response = new Response();
            string ConnectionString = "server=localhost;user = root ; database = kerrytest; port = 3306 ; password = 1234";
            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();


            string sql = "SELECT * FROM user WHERE Username = '{0}'";
            sql = String.Format(sql, reguser.Username);

            MySqlCommand cmd = new MySqlCommand(sql, con);

            List<TableUser> user = new List<TableUser>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    user.Add(new TableUser()
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                    });
                }
            }
            if (user.Count > 0)
            {
                response.Message = "This Username has been used";
            }
            else
            {
                sql = "INSERT INTO user ( Username, Password ,Fullname) VALUES ('{0}', '{1}','{2}')";
                sql = String.Format(sql, reguser.Username, reguser.Password, reguser.Fullname);

                cmd = new MySqlCommand(sql, con);
                cmd.ExecuteNonQuery();
                response.Message = "Create account success";
            }
            con.Close();

            return response;
        }
        [HttpGet]
        [Route("book")]
        public async Task<List<bookdetail>> book()
        {
            List<bookdetail> result = null;
            HttpResponseMessage responseAPI = await client.GetAsync("https://api.itbook.store/1.0/search/mysql");
            if (responseAPI.IsSuccessStatusCode)
            {
                var returnapi = JsonConvert.DeserializeObject<book>(await responseAPI.Content.ReadAsStringAsync());
                result = returnapi.books.OrderBy(o => o.title).ToList();
            }
            return result;
        }
        [HttpPost]
        [Route("userlike")]
        public Response userlike(userlike userlike) {
            var response = new Response();
            string ConnectionString = "server=localhost;user = root ; database = kerrytest; port = 3306 ; password = 1234";
            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();

            string sql = "SELECT * FROM userlike WHERE user_id = '{0}' and book_id = {1}";
            sql = String.Format(sql, userlike.user_id,userlike.book_id);

            MySqlCommand cmd = new MySqlCommand(sql, con);

            List<Tablelike> user = new List<Tablelike>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    user.Add(new Tablelike()
                    {
                        user_id = Convert.ToInt32(reader["user_id"]),
                        book_id = reader["book_id"].ToString(),
                    });
                }
            }
            if (user.Count == 0)
            {
                sql = "INSERT INTO userlike ( user_id, book_id ) VALUES ('{0}', '{1}')";
                sql = String.Format(sql, userlike.user_id, userlike.book_id);

                cmd = new MySqlCommand(sql, con);
                cmd.ExecuteNonQuery();

                response.Message = "Add book you like !";

            }
            else
            {
                response.Message = "Duplicate book";
            }
            con.Close();
            return response;
        }
    }
}