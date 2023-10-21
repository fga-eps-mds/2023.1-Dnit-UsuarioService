namespace auth
{
    public class AuthConfig
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public int ExpireMinutes { get; set; } = 10;
        public int RefreshTokenExpireMinutes { get; set; } = 120;
    }
}
