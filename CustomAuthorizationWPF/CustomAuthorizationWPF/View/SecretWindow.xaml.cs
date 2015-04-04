using System;
using System.Diagnostics;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using CustomAuthorizationWPF.ViewModel;

namespace CustomAuthorizationWPF.View
{
    [PrincipalPermission(SecurityAction.Demand)]
    public partial class SecretWindow : IView
    {
        public SecretWindow()
        {
            InitializeComponent();
        }

        #region IView Members

        public IViewModel ViewModel
        {
            get
            {
                return DataContext as IViewModel;
            }
            set
            {
                DataContext = value;
            }
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var wp = Thread.CurrentPrincipal as WindowsPrincipal;
            if (wp != null)
                if (wp.IsInRole(@"Administrators"))
                    Debug.WriteLine("accessed");
                else
                    throw new Exception("Access Denied");
            else
                throw new Exception("Access Denied");
        }

        
        [PrincipalPermission(SecurityAction.Assert, Role = "Administrators")]
        private void ___Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ys");
        }
    }
}
