using System;
using System.Collections.Generic;
using Core.Logging;
using MonoMod.RuntimeDetour;
using ShapezShifter.SharpDetour;

namespace ShapezShifter.Hijack
{
    internal class ConsoleInterceptor : IDisposable
    {
        private readonly IRewirerProvider RewirerProvider;
        private readonly ILogger Logger;
        private readonly Hook ConsoleCommandsHook;

        public ConsoleInterceptor(IRewirerProvider rewirerProvider, ILogger logger)
        {
            RewirerProvider = rewirerProvider;
            Logger = logger;
            ConsoleCommandsHook = DetourHelper
                .CreatePostfixHook<GameSessionOrchestrator, IGameData>(
                    (orchestrator, gameData) => orchestrator.Init_9_ConsoleCommands(gameData),
                    SetupConsoleCommands);
        }

        private void SetupConsoleCommands(GameSessionOrchestrator orchestrator, IGameData gameData)
        {
            IDebugConsole console = orchestrator.DependencyContainer.Resolve<IDebugConsole>();
            
            IEnumerable<IConsoleRewirer> consoleRewirers = 
                RewirerProvider.RewirersOfType<IConsoleRewirer>();

            Logger.Info?.Log("Intercepting console commands registration");

            foreach (IConsoleRewirer consoleRewirer in consoleRewirers)
            {
                try
                {
                    consoleRewirer.RegisterConsoleCommand(
                        RegisterCommand,
                        RegisterCheatCommand);
                }
                catch (Exception ex)
                {
                    Logger.Error?.Log($"Failed to register console commands from rewirer {consoleRewirer.GetType().Name}: {ex}");
                }
            }

            void RegisterCommand(string command, Action<DebugConsole.CommandContext> handler)
            {
                Logger.Info?.Log($"Registering console command: {command}");
                console.Register(command, handler);
            }

            void RegisterCheatCommand(string command, Action<DebugConsole.CommandContext> handler)
            {
                Logger.Info?.Log($"Registering console cheat command: {command}");
                console.Register(command, handler, true);
            }
        }

        public void Dispose()
        {
            ConsoleCommandsHook.Dispose();
        }
    }
}
