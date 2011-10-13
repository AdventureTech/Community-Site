namespace CommunitySite.Core.Services.Authentication
{
    public interface AuthenticationService
    {
        bool Authenticate(string validUsername, string validPassword);
        void SignIn(string validUsername);
    }
}