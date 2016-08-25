using XamFormsWpf.Core.Services;

namespace XamFormsWpf
{
    class HelloService : IHelloService
    {
        public string SayHello()
        {
            return "Hello, Xamarin Forms!";
        }
    }
}
