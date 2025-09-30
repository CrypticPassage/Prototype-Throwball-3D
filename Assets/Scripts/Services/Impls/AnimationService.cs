using DG.Tweening;
using Objects;
using Signals;
using UnityEngine;
using Zenject;

namespace Services.Impls
{
    public class AnimationService : MonoBehaviour, IAnimationService
    {
        private SignalBus _signalBus;
        private PlayerBall _playerBall;
        private Door _door;
        private Sequence _sequence;

        private Vector3 _doorStartPosition = new Vector3(0, 0, 14);
        
        private bool _isDoorMoved = true;
        
        [Inject]
        public void Construct(SignalBus signalBus, 
            PlayerBall playerBall,
            Door door)
        {
            _signalBus = signalBus;
            _playerBall = playerBall;
            _door = door;
        }

        public void KillSequence() => _sequence?.Kill(); 

        public void SetStartAnimationData()
        {
            _door.gameObject.transform.position = _doorStartPosition;
            _isDoorMoved = true;
        }

        public void StartEndGameAnimation()
        {
            _isDoorMoved = false;
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            
            _sequence.Append(_playerBall.transform.DOMoveZ(15, 5).SetEase(Ease.Linear));
            _sequence.Join(_playerBall.transform.DOMoveY(1, 1).SetEase(Ease.InOutSine).SetLoops(5, LoopType.Yoyo));
            _sequence.AppendCallback(() => _signalBus.Fire(new SignalGameOver(true)));
            
            _sequence.Play();
        }

        private void Update()
        {
            if (!_isDoorMoved)
            {
                var door = _door.gameObject;
                
                float distanceToDoor = Vector3.Distance(_playerBall.transform.position, door.transform.position);
                
                if (distanceToDoor <= 5)
                {
                    door.transform.DOMoveX(door.transform.position.x - 4f, 1f).SetEase(Ease.InOutQuad);
                    _isDoorMoved = true;
                }
            }
        }
    }
}