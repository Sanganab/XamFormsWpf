using System.Windows;
using XamFormsWpf.Core.Services;

namespace XamFormsWpf.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        SimpleIoC IoC = SimpleIoC.Container;
        protected override void OnStartup(StartupEventArgs e)
        {
            IoC.Register<IHelloService, HelloService>();
        }
    }
}
