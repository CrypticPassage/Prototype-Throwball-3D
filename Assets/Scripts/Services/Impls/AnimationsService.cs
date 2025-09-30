using DG.Tweening;
using Models;
using Objects;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Services.Impls
{
    public class AnimationsService : MonoBehaviour, IAnimationsService
    {
        private SignalBus _signalBus;
        private Image _flashImage;
        private PlayerBall _playerBall;
        private GameObject _door;
        private Sequence _sequence;
        
        private bool _isDoorMoved = true;
        
        [Inject]
        public void Construct(SignalBus signalBus, 
            Image flashImage,
            PlayerBall playerBall,
            GameObject door)
        {
            _signalBus = signalBus;
            _flashImage = flashImage;
            _playerBall = playerBall;
            _door = door;
        }

        public void KillSequence() => _sequence?.Kill();

        public void StartGameAnimation(Camera camera, GameSettingVo gameSettingVo)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _flashImage.gameObject.SetActive(true);
            
            _sequence.Join(_flashImage.DOFade(0f, 1f).OnComplete(() => _flashImage.gameObject.SetActive(false)));
            _sequence.Join(camera.transform.DOMove(gameSettingVo.CameraGamePosition, 3f));
            _sequence.Join(camera.transform.DORotateQuaternion(Quaternion.Euler(gameSettingVo.CameraGameRotation), 3f));
            
            _sequence.Play();
        }
        
        public void StartEndGameAnimation()
        {
            _isDoorMoved = false;
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            
            _sequence.Append(_playerBall.transform.DOMoveZ(28, 5).SetEase(Ease.Linear));
            _sequence.Join(_playerBall.transform.DORotate(new Vector3(0, 360 * 5, 0), 5, RotateMode.FastBeyond360));
            _sequence.AppendCallback(() => _signalBus.Fire(new SignalGameOver(true)));
            
            _sequence.Play();
        }

        private void Update()
        {
            if (!_isDoorMoved)
            {
                var door = _door.gameObject;
                
                float distanceToDoor = Vector3.Distance(_playerBall.transform.position, door.transform.position);
                
                if (distanceToDoor <= 25)
                {
                    door.transform.DORotateQuaternion(Quaternion.Euler(Vector3.zero), 3f);
                    _isDoorMoved = true;
                }
            }
        }
    }
}