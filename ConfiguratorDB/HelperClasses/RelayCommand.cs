using System;
using System.Windows.Input;

namespace ConfiguratorDB.HelperClasses
{
    public class RelayCommand : ICommand
    {
        protected readonly Func<Boolean> canExecute;

        protected readonly Action execute;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this.canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (this.canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public RelayCommand(Action execute, Func<Boolean> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public virtual Boolean CanExecute(Object parameter)
        {
            return this.canExecute == null ? true : this.canExecute();
        }

        public virtual void Execute(Object parameter)
        {
            this.execute();
        }
    }
}
