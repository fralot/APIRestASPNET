namespace APIRest.Helpers
{
    public interface IJWTHelper
    {
        Task<string> GenerateJwtTokenAsync(string email, string role);
    }
}
