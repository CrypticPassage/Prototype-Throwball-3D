using Signals;

namespace Controllers
{
    public interface IMenuController
    {
        void OnGameOver(SignalGameOver gameOverSignal);
    }
}