using System.Linq;
using System.Security.Principal;

namespace CustomAuthorizationWPF.Model
{
    public class CustomPrincipal : IPrincipal
    {
        private CustomIdentity _identity;

        public CustomIdentity Identity
        {
            get { return _identity ?? new AnonymousIdentity(); }
            set { _identity = value; }
        }

        #region IPrincipal Members

        IIdentity IPrincipal.Identity
        {
            get { return Identity; }
        }

        public bool IsInRole(string role)
        {
            return _identity.Roles.Contains(role);
        }

        #endregion
    }
}