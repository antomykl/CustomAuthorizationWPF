using System;
using System.ComponentModel;
using System.Security;
using System.Threading;
using System.Windows.Controls;
using CustomAuthorizationWPF.Model;
using CustomAuthorizationWPF.View;

namespace CustomAuthorizationWPF.ViewModel
{
    public class AuthenticationViewModel : IViewModel, INotifyPropertyChanged
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly DelegateCommand _loginCommand;
        private readonly DelegateCommand _logoutCommand;
        private readonly DelegateCommand _showViewCommand;
        private string _username;
        private string _status;

        public AuthenticationViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _loginCommand = new DelegateCommand(Login, CanLogin);
            _logoutCommand = new DelegateCommand(Logout, CanLogout);
            _showViewCommand = new DelegateCommand(ShowView, null);
        }

        #region Properties

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                NotifyPropertyChanged("Username");
            }
        }

        public string AuthenticatedUser
        {
            get
            {
                if (IsAuthenticated)
                    return string.Format("Signed in as {0}. {1}",
                        Thread.CurrentPrincipal.Identity.Name,
                        Thread.CurrentPrincipal.IsInRole("Administrators")
                            ? "You are an administrator!"
                            : "You are NOT a member of the administrators group.");

                return "Not authenticated!";
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }

        #endregion

        #region Commands

        public DelegateCommand LoginCommand
        {
            get { return _loginCommand; }
        }

        public DelegateCommand LogoutCommand
        {
            get { return _logoutCommand; }
        }

        public DelegateCommand ShowViewCommand
        {
            get { return _showViewCommand; }
        }

        #endregion

        private void Login(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            if (passwordBox == null) return;
            var clearTextPassword = passwordBox.Password;
            try
            {
                //Validate credentials through the authentication service
                var user = _authenticationService.AuthenticateUser(Username, clearTextPassword);

                //Get the current principal object
                var customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
                if (customPrincipal == null)
                    throw new ArgumentException(
                        "The application's default thread principal must be set to a CustomPrincipal object on startup.");

                //Authenticate the user
                customPrincipal.Identity = new CustomIdentity(user.Username, user.Email, user.Roles);

                //Update UI
                NotifyPropertyChanged("AuthenticatedUser");
                NotifyPropertyChanged("IsAuthenticated");
                _loginCommand.RaiseCanExecuteChanged();
                _logoutCommand.RaiseCanExecuteChanged();
                Username = string.Empty; //reset
                passwordBox.Password = string.Empty; //reset
                Status = string.Empty;
            }
            catch (UnauthorizedAccessException)
            {
                Status = "Login failed! Please provide some valid credentials.";
            }
            catch (Exception ex)
            {
                Status = string.Format("ERROR: {0}", ex.Message);
            }
        }

        private bool CanLogin(object parameter)
        {
            return !IsAuthenticated;
        }

        private void Logout(object parameter)
        {
            var customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
            if (customPrincipal == null) return;
            customPrincipal.Identity = new AnonymousIdentity();
            NotifyPropertyChanged("AuthenticatedUser");
            NotifyPropertyChanged("IsAuthenticated");
            _loginCommand.RaiseCanExecuteChanged();
            _logoutCommand.RaiseCanExecuteChanged();
            Status = string.Empty;
        }

        private bool CanLogout(object parameter)
        {
            return IsAuthenticated;
        }

        public bool IsAuthenticated
        {
            get { return Thread.CurrentPrincipal.Identity.IsAuthenticated; }
        }

        private void ShowView(object parameter)
        {
            try
            {
                Status = string.Empty;
                IView view;
                if (parameter == null) parameter = "";
                switch (parameter.ToString())
                {
                    //case "":
                    //    view = new SecretWindow();
                    //    break;
                    case "Secret":
                        view = new SecretWindow();
                        break;
                    default:
                        view = new AdminWindow();
                        break;
                }

                //if (parameter == null)
                //    view = new SecretWindow();
                //else
                //    view = new AdminWindow();

                view.Show();
            }
            catch (SecurityException)
            {
                Status = "You are not authorized!";
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}