using System.Windows;
using XamFormsWpf.Core.Data;
using XamFormsWpf.Core.ViewModels;

namespace XamFormsWpf.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly ContactViewModel _contactViewModel;
        public MainWindow()
        {
            var contact = new Contact {
                FirstName = "Jimmy",
                LastName = "Smith",
                Email = "Jimmy.Smith@gmail.com"
            };

            _contactViewModel = new ContactViewModel(contact);
            DataContext = _contactViewModel;

            InitializeComponent();
        }
    }
}
