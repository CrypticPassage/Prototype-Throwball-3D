using Signals;

namespace Controllers
{
    public interface IGameController
    {
        void OnGameStart();
        void OnObjectsCollision(SignalObjectsCollision signal);
        void OnStartAnimation(SignalStartAnimation startAnimationSignal);
        void OnGameButtonHeld(SignalButtonHeld buttonHeldSignal);
    }
}