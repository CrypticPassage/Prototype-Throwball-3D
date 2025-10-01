using Databases;
using DG.Tweening;
using Models;
using Objects;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Services.Impls
{
    /// <summary>
    /// Даний сервіс містить реалізовані анімації, що зустрічаються у грі, та методи їх відтворення.
    /// </summary>
    public class AnimationsService : MonoBehaviour, IAnimationsService
    {
        private SignalBus _signalBus;
        private Image _flashImage;
        private PlayerBall _playerBall;
        private FlyingObject _flyingObject;
        private GameObject _door;
        private GameSettingVo _gameSettingVo;
        private Sequence _sequence;
        
        private bool _isDoorMoved = true;

        [Inject]
        public void Construct(SignalBus signalBus,
            Image flashImage,
            PlayerBall playerBall,
            FlyingObject flyingObject,
            GameObject door,
            IGameSettingsDatabase gameSettingsDatabase)
        {
            _signalBus = signalBus;
            _flashImage = flashImage;
            _playerBall = playerBall;
            _flyingObject = flyingObject;
            _door = door;
            _gameSettingVo = gameSettingsDatabase.GameSettingVo;
        }

        public void KillSequence() => _sequence?.Kill();

        public void StartEntryGameAnimation(Camera camera, GameSettingVo gameSettingVo)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _flashImage.gameObject.SetActive(true);
            
            _sequence.Join(_flashImage.DOFade(0f, 1f).OnComplete(() => _flashImage.gameObject.SetActive(false)));
            _sequence.Join(camera.transform.DOMove(gameSettingVo.CameraGamePosition, _gameSettingVo.BallRollAnimationDuration));
            _sequence.Join(camera.transform.DORotateQuaternion(Quaternion.Euler(gameSettingVo.CameraGameRotation), _gameSettingVo.BallRollAnimationDuration));
            
            _sequence.Play();
        }
        
        public void StartTryGameAnimation()
        {
            _isDoorMoved = false;
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            
            _sequence.Append(_playerBall.transform.DOMoveZ(_gameSettingVo.BallRollEndPositionZ, _gameSettingVo.BallRollAnimationDuration).SetEase(Ease.Linear));
            _sequence.Join(_playerBall.transform.DORotate(new Vector3(0, 360 * _gameSettingVo.BallRollAnimationDuration, 0),
                _gameSettingVo.BallRollAnimationDuration, RotateMode.FastBeyond360));
            _sequence.AppendCallback(() => _signalBus.Fire(new SignalGameOver(true)));
            
            _sequence.Play();
        }

        private void Start()
        {
            StartFlyingObjectAnimation();
        }

        private void Update()
        {
            if (!_isDoorMoved)
            {
                var door = _door.gameObject;
                
                float distanceToDoor = Vector3.Distance(_playerBall.transform.position, door.transform.position);
                
                if (distanceToDoor <= _gameSettingVo.DistanceToDoor)
                {
                    door.transform.DORotateQuaternion(Quaternion.Euler(Vector3.zero), _gameSettingVo.DoorAnimationDuration);
                    _isDoorMoved = true;
                }
            }
        }

        private void StartFlyingObjectAnimation()
        {
            var startPosition = new Vector3(_flyingObject.transform.position.x, _gameSettingVo.FlyingObjectStartPositionY, _flyingObject.transform.position.z);
            var endPosition = new Vector3(_flyingObject.transform.position.x, _gameSettingVo.FlyingObjectEndPositionY, _flyingObject.transform.position.z);
            
            var sequence = DOTween.Sequence();
            
            sequence.Append(_flyingObject.transform.DOMoveY(endPosition.y, _gameSettingVo.FlyingObjectAnimationDuration).SetEase(Ease.Linear));
            sequence.Append(_flyingObject.transform.DOMoveY(startPosition.y, _gameSettingVo.FlyingObjectAnimationDuration).SetEase(Ease.Linear));
            sequence.SetLoops(-1, LoopType.Yoyo);
            
            sequence.Play();
        }
    }
}