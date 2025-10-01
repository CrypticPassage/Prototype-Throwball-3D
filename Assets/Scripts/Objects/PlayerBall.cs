using Signals;
using UnityEngine;
using Zenject;

namespace Objects
{
    public class PlayerBall : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private ThrowableBall throwableBall;
        
        private SignalBus _signalBus;
        public Rigidbody Rigidbody => rigidbody;
        public ThrowableBall ThrowableBall => throwableBall;
        
        [Inject] 
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnCollisionEnter(Collision collision)
        {
            _signalBus.Fire(new SignalObjectsCollision(gameObject, collision));
        }
    }
}