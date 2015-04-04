namespace CustomAuthorizationWPF.Model
{
    public class User
    {
        public User(string username, string email, string[] roles)
        {
            Username = username;
            Email = email;
            Roles = roles;
        }
        public string Username { get; set;}
 
        public string Email { get; set;}
 
        public string[] Roles { get; set;}
    }
}