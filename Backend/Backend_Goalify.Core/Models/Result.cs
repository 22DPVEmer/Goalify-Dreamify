namespace Backend_Goalify.Core.Models
{
    public class Result
    {
        public bool success { get; set; }
        public string message { get; set; }
        public TokenData data { get; set; }
    }
}