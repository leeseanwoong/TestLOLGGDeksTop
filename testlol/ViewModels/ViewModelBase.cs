using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testlol.ViewModels
{
    internal class ViewModelBase : INotifyPropertyChanged
    {
        private ViewModelBase Parent;
        public ViewModelBase()
        {

        }

        public ViewModelBase(ViewModelBase parent)
        {
            parent = Parent;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


        protected bool SetProperty<T>(ref T property, T value)
        {
            try
            {
                property = value;
                OnPropertyChanged(nameof(property));
                return true;
            }
            catch
            {
                return false;
            }

        }

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
