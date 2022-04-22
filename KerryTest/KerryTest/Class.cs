namespace KerryTest
{
    public class Login
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class Response
    {
        public string Message { get; set; }

    }
    public class Register
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Fullname { get; set; }
    }

    public class userlike
    {
        public int user_id { get; set; }

        public string book_id { get; set; }
    }

    public class TableUser
    {
        public int ID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
    public class Tablelike
    {
        public int user_id { get; set; }

        public string book_id { get; set; }
    }
    public class book
    {
        public string error { get; set; }
        public string total { get; set; }
        public string page { get; set; }
        public List<bookdetail> books { get; set; }
    }

    public class bookdetail
    {
        public string title { get; set; }
        public string subtitle { get; set; }
        public string isbn13 { get; set; }
        public string price { get; set; }
        public string image { get; set; }
        public string url { get; set; }
    }
}
    
