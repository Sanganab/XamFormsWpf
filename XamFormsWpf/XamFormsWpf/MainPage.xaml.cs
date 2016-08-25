using Xamarin.Forms;
using XamFormsWpf.Core.Data;
using XamFormsWpf.Core.ViewModels;

namespace XamFormsWpf
{
    public partial class MainPage : ContentPage
    {
        readonly ContactViewModel _contactViewModel;
        public MainPage()
        {
            // Test contact
            var contact = new Contact {
                FirstName = "Jimmy",
                LastName = "Smith",
                Email = "Jimmy.Smith@gmail.com"
            };

            _contactViewModel = new ContactViewModel(contact);
            BindingContext = _contactViewModel;
            InitializeComponent();
        }
    }
}
