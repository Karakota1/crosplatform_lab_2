namespace Books_2.Contracts.Auth
{
    public record LoginRequest(string Username, string Password);
    public record LoginResponse(string Token);
}
