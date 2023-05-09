using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace testlol.ViewModels
{
    class DelegateCommand : ICommand
    {
        Predicate<object> canExecute;
        Action<object> execute;
        public DelegateCommand( Action<object> _execute, Predicate<object> _canexecute)
       : this()
        {
            canExecute = _canexecute;
            execute = _execute;
        }
        public DelegateCommand()
        { }
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }
        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}

