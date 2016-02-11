using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace KNEKT.Classes
{
    public class DelegateCommand : ICommand
    {
        private Action _ExecuteMethod;

        public DelegateCommand(Action executeMethod)
        {
            _ExecuteMethod = executeMethod;
        }

        public bool CanExecute(Object Parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            //var p = (Pages.Main.Page1)Activator.CreateInstance(typeof(Pages.Main.Page1));
            //Parameter = parameter.ToString();
            _ExecuteMethod.Invoke();
        }


        private string _Parameter;
        public string Parameter
        {
            get { return _Parameter; }
            set { _Parameter = value; }
        }
    }
}
