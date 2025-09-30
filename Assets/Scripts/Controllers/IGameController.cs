using Signals;

namespace Controllers
{
    public interface IGameController
    {
        void OnGameStart();
        void OnGameOver(SignalGameOver gameOverSignal);
        void OnThrownBallCollision(SignalThrowBallCollision throwBallCollisionSignal);
        void OnStartAnimation(SignalStartAnimation startAnimationSignal);
        void OnGameButtonHeld(SignalButtonHeld buttonHeldSignal);
    }
}