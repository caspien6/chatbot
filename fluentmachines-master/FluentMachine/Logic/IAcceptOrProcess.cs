namespace FluentMachine.Logic
{
    public interface IAcceptOrProcess
    {
        void Accept<T>() where T : struct, ICommand;

        IWith Process<T>() where T : struct, ICommand;
    }
}