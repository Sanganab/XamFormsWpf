using XamFormsWpf.Core.Services;

namespace XamFormsWpf.WPF
{
    class HelloService : IHelloService
    {
        public string SayHello()
        {
            return "Hello, Windows Desktop!";
        }
    }
}
