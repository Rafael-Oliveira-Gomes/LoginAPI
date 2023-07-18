namespace Login.Model.Dto
{
    public class SsoDTO
    {
        public string access_token { get; set; }
        public DateTime expiration { get; set; }

        public SsoDTO(string access_token, DateTime expiration)
        {
            this.access_token = access_token;
            this.expiration = expiration;
        }
    }
}
