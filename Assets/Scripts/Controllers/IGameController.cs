using Signals;

namespace Controllers
{
    public interface IGameController
    {
        void OnGameStart();
        void OnObjectsCollision(SignalObjectsCollision signal);
        void OnStartTryAnimation(SignalStartTryAnimation startTryAnimationSignal);
        void OnGameButtonHeld(SignalButtonHeld buttonHeldSignal);
    }
}