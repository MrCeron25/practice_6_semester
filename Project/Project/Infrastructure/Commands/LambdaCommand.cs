using System;
using Project.Infrastructure.Commands.Base;

namespace Project.Infrastructure.Commands
{
    internal class LambdaCommand : Command
    {

        //public static LambdaCommandBuilder Builder()
        //{
        //    return new LambdaCommandBuilder();
        //}

        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public LambdaCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
        {
            _execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _canExecute = CanExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public override void Execute(object parameter)
        {
            _execute(parameter);
        }
    }

    //class LambdaCommandBuilder
    //{

    //    private Action<object> _action;
    //    private Func<object, bool> _canExecute;

    //    public LambdaCommandBuilder Execute(Action<object> action)
    //    {
    //        _action = action;
    //        return this;
    //    }


    //    public LambdaCommandBuilder CanExecute(Func<object, bool> func)
    //    {
    //        _canExecute = func;
    //        return this;
    //    }

    //    public LambdaCommand Build() => new LambdaCommand(_action, _canExecute);
    //}
}
