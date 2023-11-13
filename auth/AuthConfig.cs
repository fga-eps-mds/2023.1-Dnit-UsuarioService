namespace auth
{
    public class AuthConfig
    {
        public bool Enabled { get; set; } = false;
        public string Key { get; set; } = "chave secreta chave secreta chave secreta chave secreta chave secreta chave secreta";
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public int ExpireMinutes { get; set; } = 10;
        public int ApiKeyExpireMinutes { get; set; } = 60 * 24 * 365; // 1 ano
        public int RefreshTokenExpireMinutes { get; set; } = 120;
    }
}
