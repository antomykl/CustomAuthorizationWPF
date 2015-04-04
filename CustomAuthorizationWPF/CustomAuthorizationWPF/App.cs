using System;
using System.Windows;
using CustomAuthorizationWPF.Model;
using CustomAuthorizationWPF.View;
using CustomAuthorizationWPF.ViewModel;

namespace CustomAuthorizationWPF
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //Create a custom principal with an anonymous identity at startup
            var customPrincipal = new CustomPrincipal();
            AppDomain.CurrentDomain.SetThreadPrincipal(customPrincipal);
            base.OnStartup(e);

            //Show the login view
            var viewModel = new AuthenticationViewModel(new AuthenticationService());
            IView loginWindow = new LoginWindow(viewModel);
            loginWindow.Show();
        }
    }
}