using Signals;
using UnityEngine;
using Zenject;

namespace Objects
{
    public class ThrowableBall : MonoBehaviour
    {
        private SignalBus _signalBus;

        private bool _isBallThrown;
        
        public bool IsBallThrown
        {
            get => _isBallThrown;
            set => _isBallThrown = value;
        }

        [Inject] 
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void OnCollisionEnter(Collision collision)
        {
            _signalBus.Fire(new SignalObjectsCollision(gameObject, collision));
        }
    }
}