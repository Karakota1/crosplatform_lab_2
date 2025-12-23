namespace Books_2.Services
{
    public class AuthSettings
    {
        public string SecretKey { get; set; } = "SuperSecretKeyForJWT123456789012"; // минимум 32 символа
        public string Issuer { get; set; } = "LavrovaLR2Issuer";
        public string Audience { get; set; } = "LavrovaLR2Audience";
        public int LifetimeMinutes { get; set; } = 1440;
    }
}
