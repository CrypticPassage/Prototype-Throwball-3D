using Signals;
using UnityEngine;
using Zenject;

namespace Objects
{
    public class PlayerBall : MonoBehaviour
    {
        private SignalBus _signalBus;
        private ThrowBall _throwBall;

        public ThrowBall ThrowBall => _throwBall;
        
        [Inject] 
        public void Construct(SignalBus signalBus,
            ThrowBall throwBall)
        {
            _signalBus = signalBus;
            _throwBall = throwBall;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Obstacle")
                _signalBus.Fire(new SignalGameOver(false));
        }
    }
}