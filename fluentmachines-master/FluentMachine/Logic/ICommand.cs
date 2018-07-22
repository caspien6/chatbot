namespace FluentMachine.Logic
{
    public interface ICommand
    {
        string Payload { get; set; }
    }
}
