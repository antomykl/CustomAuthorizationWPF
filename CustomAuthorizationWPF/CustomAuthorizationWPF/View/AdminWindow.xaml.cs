using System.Security.Permissions;
using CustomAuthorizationWPF.ViewModel;

namespace CustomAuthorizationWPF.View
{

    [PrincipalPermission(SecurityAction.Demand, Role = "Administrators")]
    public partial class AdminWindow : IView
    {
        public AdminWindow()
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
    }
}
