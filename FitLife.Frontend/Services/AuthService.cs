namespace FitLife.Frontend.Services;

public class AuthService
{
    public string Login(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return "Indtast både email og password.";
        }

        return $"Login forsøgt for {email}.";
    }
}