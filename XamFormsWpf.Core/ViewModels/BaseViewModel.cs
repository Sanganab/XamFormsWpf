using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XamFormsWpf.Core.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaiseAllPropertiesChanged()
        {
            PropertyChanged(this, null);
        }
    }
}
