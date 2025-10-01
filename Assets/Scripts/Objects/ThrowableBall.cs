using Signals;
using UnityEngine;
using Zenject;

namespace Objects
{
    public class ThrowableBall : MonoBehaviour
    {
        private SignalBus _signalBus;

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