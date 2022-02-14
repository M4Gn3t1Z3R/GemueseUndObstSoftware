using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GemueseUndObstSoftware.MVVM
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual bool SetProperty<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            if(EqualityComparer<T>.Default.Equals(oldValue, newValue))
            {
                return false;
            }
            else
            {
                this.OnPropertyChanged(propertyName);
                return true;
            }
        }
    }
}
