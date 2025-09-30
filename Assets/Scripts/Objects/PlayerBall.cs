using Signals;
using UnityEngine;
using Zenject;

namespace Objects
{
    public class PlayerBall : MonoBehaviour
    {
        [SerializeField] private ThrowableBall throwableBall;
        
        private SignalBus _signalBus;

        public ThrowableBall ThrowableBall => throwableBall;
        
        [Inject] 
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Obstacle")
                _signalBus.Fire(new SignalGameOver(false));
        }
    }
}