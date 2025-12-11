using System;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow
{
    internal class ConsoleCommandRewirer : IConsoleRewirer
    {
        private readonly string CommandName;
        private readonly Action<DebugConsole.CommandContext> Handler;
        private readonly bool IsCheat;

        public ConsoleCommandRewirer(string commandName, Action<DebugConsole.CommandContext> handler, bool isCheat)
        {
            CommandName = commandName;
            Handler = handler;
            IsCheat = isCheat;
        }

        public void RegisterConsoleCommand(
            Action<string, Action<DebugConsole.CommandContext>> registerCommand,
            Action<string, Action<DebugConsole.CommandContext>> registerCheatCommand)
        {
            if (IsCheat)
            {
                registerCheatCommand(CommandName, Handler);
            }
            else
            {
                registerCommand(CommandName, Handler);
            }
        }
    }
}
