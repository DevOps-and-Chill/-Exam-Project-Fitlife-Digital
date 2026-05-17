namespace FitLife.Frontend.Services;

public class AuthService
{
    // Denne service håndterer loginlogik for frontend-prototypen.
    // TODO:
    // Senere skal denne service kommunikere med backend/authentication API.

    public string Login(string email, string password)
    {
        // Simpel frontend-validering
        // Sikrer at brugeren har udfyldt begge felter.
        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            return "Indtast både email og password.";
        }

        // Midlertidig prototype-respons
        // TODO:
        // Senere skal denne metode:
        // 1. Sende loginoplysninger til backend/API
        // 2. Modtage JWT/token/session
        // 3. Gemme loginstate
        // 4. Redirecte brugeren til korrekt dashboard

        return $"Login forsøgt for {email}.";
    }
}