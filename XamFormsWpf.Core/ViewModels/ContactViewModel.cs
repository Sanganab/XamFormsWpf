using XamFormsWpf.Core.Data;
using XamFormsWpf.Core.Services;

namespace XamFormsWpf.Core.ViewModels
{
    public class ContactViewModel : BaseViewModel
    {
        private Contact _contact;
        private IHelloService _helloService = SimpleIoC.Container.Resolve<IHelloService>();


        public string Hello { get { return _helloService.SayHello(); } }

        public string ContactFirstName
        {
            get { return _contact.FirstName; }
            set {
                _contact.FirstName = value;
                RaisePropertyChanged(nameof(ContactFirstName));
            }
        }
        public string ContactLastName
        {
            get { return _contact.LastName; }
            set {
                _contact.LastName = value;
                RaisePropertyChanged(nameof(ContactLastName));
            }
        }
        public string ContactEmail
        {
            get { return _contact.Email; }
            set {
                _contact.Email = value;
                RaisePropertyChanged(nameof(ContactEmail));
            }
        }

        public ContactViewModel(Contact contact = null)
        {
            _contact = contact == null ? new Contact() : contact;
            RaiseAllPropertiesChanged();
        }
    }
}
