namespace CustomAuthorizationWPF.Model
{
    public interface IAuthenticationService
    {
        User AuthenticateUser(string username, string password);
    }
}