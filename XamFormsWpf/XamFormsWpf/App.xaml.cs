using XamFormsWpf.Core.Services;
using Xamarin.Forms;

namespace XamFormsWpf
{
    public partial class App : Application
    {
        public SimpleIoC IoC = SimpleIoC.Container;
        public App()
        {
            IoC.RegisterSingleton<IHelloService, HelloService>();
            InitializeComponent();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
