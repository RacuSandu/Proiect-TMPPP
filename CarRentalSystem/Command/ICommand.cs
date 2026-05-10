namespace CarRentalSystem.Command
{
    // Interfata comuna pentru toate comenzile
    public interface ICommand
    {
        string CommandName { get; }
        void Execute();
        void Undo();
    }
}