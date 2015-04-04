using CustomAuthorizationWPF.ViewModel;

namespace CustomAuthorizationWPF.View
{
    public interface IView
    {
        IViewModel ViewModel { get; set;}

        void Show();
    }
}