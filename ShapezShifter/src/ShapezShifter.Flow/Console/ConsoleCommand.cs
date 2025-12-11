using System;
using ShapezShifter.Hijack;

namespace ShapezShifter.Flow
{
    public static class ConsoleCommand
    {
        public static RewirerHandle Register(string commandName, Action<DebugConsole.CommandContext> handler)
        {
            var rewirer = new ConsoleCommandRewirer(commandName, handler, isCheat: false);
            return GameRewirers.AddRewirer(rewirer);
        }

        public static RewirerHandle RegisterCheat(string commandName, Action<DebugConsole.CommandContext> handler)
        {
            var rewirer = new ConsoleCommandRewirer(commandName, handler, isCheat: true);
            return GameRewirers.AddRewirer(rewirer);
        }
    }
}
