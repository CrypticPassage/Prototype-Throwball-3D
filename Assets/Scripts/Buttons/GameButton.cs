using UnityEngine;
using Objects;
using Signals;
using UnityEngine.EventSystems;
using Zenject;

namespace Buttons
{
    public class GameButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private SignalBus _signalBus;
        private PlayerBall _playerBall;
        
        private bool _isButtonHeld;

        private Vector3 _playerBallScale;
        private Vector3 _throwBallScale;
        
        [Inject]
        public void Construct(SignalBus signalBus,
            PlayerBall playerBall)
        {
            _signalBus = signalBus;
            _playerBall = playerBall;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _signalBus.Fire(new SignalButtonHeld(true));
            
            _playerBallScale = _playerBall.gameObject.transform.localScale;
            _throwBallScale = _playerBall.ThrowBall.gameObject.transform.localScale;
            _isButtonHeld = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _signalBus.Fire(new SignalButtonHeld(false));
            _isButtonHeld = false;
        }
        
        private void Update()
        {
            if (_isButtonHeld)
                PerformContinuousAction();
        }
        
        private void PerformContinuousAction()
        {
            _playerBall.gameObject.transform.localScale = _playerBallScale;
            _playerBall.ThrowBall.gameObject.transform.localScale = _throwBallScale;
        }
    }
}