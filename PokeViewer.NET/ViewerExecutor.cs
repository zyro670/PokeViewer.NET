using SysBot.Base;
using static PokeViewer.NET.ViewerUtil;

namespace PokeViewer.NET
{
    public class ViewerState : BotState<RoutineType, SwitchConnectionConfig>
    {
        public override void IterateNextRoutine() => CurrentRoutineType = NextRoutineType;
        public override void Initialize() => Resume();
        public override void Pause() => NextRoutineType = RoutineType.None;
        public override void Resume() => NextRoutineType = InitialRoutine;
    }

    public class ViewerExecutor : SwitchRoutineExecutor<ViewerState>
    {
        private bool Connected = false;
        public ViewerExecutor(ViewerState cfg) : base(cfg) { }

        public override string GetSummary()
        {
            var current = Config.CurrentRoutineType;
            var initial = Config.InitialRoutine;
            if (current == initial)
                return $"{Connection.Label} - {initial}";
            return $"{Connection.Label} - {initial} ({current})";
        }

        public override void SoftStop() => Config.Pause();
        public override Task HardStop() => Task.CompletedTask;
        public bool IsConnected() => Connected;
        public override async Task MainLoop(CancellationToken token)
        {
            await Task.Delay(0_050, token).ConfigureAwait(false);
            Config.IterateNextRoutine();
        }

        public async Task Connect(CancellationToken token)
        {
            Connection.Connect();
            Log("Initializing connection with console...");
            await InitialStartup(token).ConfigureAwait(false);
        }

        public void Disconnect()
        {
            HardStop();
            Connection.Disconnect();
        }
    }
}
