namespace CarRentalSystem.Command
{
    // Invoker - executa comenzile si gestioneaza Undo/Redo
    public class CommandInvoker
    {
        private readonly Stack<ICommand> _executedCommands = new();
        private readonly Stack<ICommand> _undoneCommands   = new();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _executedCommands.Push(command);
            _undoneCommands.Clear(); // dupa o comanda noua, redo history se sterge
            Console.WriteLine($"[Invoker] Command executed. History: {_executedCommands.Count}");
        }

        public void Undo()
        {
            if (_executedCommands.Count == 0)
            {
                Console.WriteLine("[Invoker] Nothing to undo.");
                return;
            }

            var command = _executedCommands.Pop();
            command.Undo();
            _undoneCommands.Push(command);
            Console.WriteLine($"[Invoker] Undo done. Remaining: {_executedCommands.Count}");
        }

        public void Redo()
        {
            if (_undoneCommands.Count == 0)
            {
                Console.WriteLine("[Invoker] Nothing to redo.");
                return;
            }

            var command = _undoneCommands.Pop();
            command.Execute();
            _executedCommands.Push(command);
            Console.WriteLine($"[Invoker] Redo done. History: {_executedCommands.Count}");
        }

        public void PrintHistory()
        {
            Console.WriteLine($"\n[Invoker] Command History ({_executedCommands.Count} executed):");
            foreach (var cmd in _executedCommands)
                Console.WriteLine($"  - {cmd.CommandName}");

            Console.WriteLine($"[Invoker] Undone Commands ({_undoneCommands.Count}):");
            foreach (var cmd in _undoneCommands)
                Console.WriteLine($"  - {cmd.CommandName}");
        }
    }
}