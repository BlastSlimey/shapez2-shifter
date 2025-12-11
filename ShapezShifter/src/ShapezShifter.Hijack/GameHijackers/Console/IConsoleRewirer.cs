using System;

namespace ShapezShifter.Hijack
{
    public interface IConsoleRewirer : IRewirer
    {
        void RegisterConsoleCommand(
            Action<string, Action<DebugConsole.CommandContext>> registerCommand,
            Action<string, Action<DebugConsole.CommandContext>> registerCheatCommand);
    }
}
