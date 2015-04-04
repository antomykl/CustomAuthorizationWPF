using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CustomAuthorizationWPF.Model
{
    public class AuthenticationService : IAuthenticationService
    {
        private class InternalUserData
        {
            public InternalUserData(string username, string email, string hashedPassword, string[] roles)
            {
                Username = username;
                Email = email;
                HashedPassword = hashedPassword;
                Roles = roles;
            }
            public string Username
            {
                get;
                private set;
            }
 
            public string Email
            {
                get;
                private set;
            }

            public string HashedPassword
            {
                get; set;
            }
 
            public string[] Roles
            {
                get;
                private set;
            }
        }
 
        private static readonly List<InternalUserData> InternalUserDatas = new List<InternalUserData>
        {
            new InternalUserData("Mark", "mark@company.com",
                "MB5PYIsbI2YzCUe34Q5ZU2VferIoI4Ttd+ydolWV0OE=", new [] { "Administrators" }), 
            new InternalUserData("John", "john@company.com",
                "hMaLizwzOQ5LeOnMuj+C6W75Zl5CXXYbwDSHWW9ZOXc=", new string[] { })
        };
 
        public User AuthenticateUser(string username, string clearTextPassword)
        {
            InternalUserData userData = InternalUserDatas.FirstOrDefault(u => u.Username.Equals(username)//);
                                                                              && u.HashedPassword.Equals(CalculateHash(clearTextPassword, u.Username)));
            if (userData == null)
                throw new UnauthorizedAccessException("Access denied. Please provide some valid credentials.");
 
            return new User(userData.Username, userData.Email, userData.Roles);
        }
 
        private static string CalculateHash(string clearTextPassword, string salt)
        {
            // Convert the salted password to a byte array
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(clearTextPassword + salt);
            // Use the hash algorithm to calculate the hash
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            // Return the hash as a base64 encoded string to be compared to the stored password
            return Convert.ToBase64String(hash);
        }
    }
}