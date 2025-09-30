using Databases;
using Models;
using Objects;
using Services;
using Signals;
using UnityEngine;
using Zenject;

namespace Controllers.Impls
{
    public class GameController : MonoBehaviour, IGameController
    {
        private SignalBus _signalBus;
        private GameSettingVo _gameSettingVo;
        
        private PlayerBall _playerBall;
        private GameObject _door;
        private Camera _mainCamera;

        private IObstaclesService _obstaclesService;
        private IAnimationsService _animationsService;
        
        private Vector3 _playerBallPosition;
        private Vector3 _throwBallPosition;
        private Vector3 _playerBallScale;
        private Vector3 _throwBallScale;
        private Vector3 _throwBallDirection;
        
        private bool _isGameOn;
        private bool _isAnimationOn;
        private bool _isBallThrown;
        private bool _isKeyPressed;
        private bool _isFirstKey = true;
        
        [Inject]
        public void Construct(SignalBus signalBus,
            IGameSettingsDatabase gameSettingsDatabase,
            PlayerBall playerBall,
            GameObject door,
            Camera mainCamera,
            IObstaclesService obstaclesService,
            IAnimationsService animationsService)
        {
            _signalBus = signalBus;
            _gameSettingVo = gameSettingsDatabase.GameSettingVo;
            _door = door;
            _playerBall = playerBall;
            _mainCamera = mainCamera;
            _obstaclesService = obstaclesService;
            _animationsService = animationsService;
        }

        public void OnGameStart()
        {
            _playerBall.gameObject.transform.localPosition = _gameSettingVo.PlayerPositionAtStart;
            _playerBall.gameObject.transform.localScale = _gameSettingVo.PlayerScaleAtStart;
            //_door.gameObject.transform.SetLocalPositionAndRotation(_gameSettingVo.DoorStartPosition, Quaternion.Euler(_gameSettingVo.DoorStartRotation));
           // _mainCamera.gameObject.transform.SetPositionAndRotation(_gameSettingVo.CameraGamePosition, Quaternion.Euler(_gameSettingVo.CameraGameRotation));
            // _obstaclesService.GetObstacles(100);
            _animationsService.StartGameAnimation(_mainCamera, _gameSettingVo);
            
            _isKeyPressed = false;
            _isFirstKey = true;
            _isGameOn = true;
        }

        public void OnGameOver(SignalGameOver gameOverSignal)
        {
            _animationsService.KillSequence();
            _isGameOn = false;
            _isAnimationOn = false;
            _isFirstKey = true;
        }

        public void OnThrownBallCollision(SignalThrowBallCollision throwBallCollisionSignal)
        {
            _isBallThrown = false;
            _obstaclesService.GetDestroyedObstacles(throwBallCollisionSignal.ObstaclesPositions);
        }

        public void OnStartAnimation(SignalStartAnimation startAnimationSignal)
        {
            if (!_isAnimationOn)
            {
                _isAnimationOn = true;
                StartAnimation();
            }
        }
        
        public void OnGameButtonHeld(SignalButtonHeld buttonHeldSignal)
        {
            if (!_isAnimationOn) 
                _isGameOn = !buttonHeldSignal.IsHeld;
        }
        
        private void Update()
        {
            if (_isGameOn)
            {
                CheckInput(); 
                CheckPlayerBallSize();
                CheckThrownBall();
            }
        }
        
        private void StartAnimation()
        {
            _isGameOn = false;
            _isBallThrown = false;
            _isAnimationOn = true;
            _animationsService.StartEndGameAnimation();
        }
        
        private void CheckPlayerBallSize()
        {
            if (_playerBall.gameObject.transform.localScale.x <= _gameSettingVo.PlayerBallScaleLimit)
            {
                _isGameOn = false;
                _signalBus.Fire(new SignalGameOver(false));
            }
        }

        private void CheckThrownBall()
        {
            if (_isBallThrown)
            {
                if (Vector3.Distance(_playerBallPosition, _throwBallPosition) > 20)
                {
                    _isBallThrown = false;
                    return;
                }

                _throwBallDirection.y = _playerBall.gameObject.transform.position.y;
                _throwBallPosition += _throwBallDirection.normalized * (15 * Time.deltaTime);
                _playerBall.ThrowableBall.gameObject.transform.position = _throwBallPosition;
            }
            else
            {
                _throwBallPosition = _playerBallPosition;
                _throwBallPosition.z -= 5;
                _playerBall.ThrowableBall.gameObject.transform.position = _throwBallPosition;

                if (!_isKeyPressed)
                {
                    _throwBallScale = new Vector3(0, 0, 0);
                    _playerBall.ThrowableBall.gameObject.transform.localScale = _throwBallScale;
                }
            }
        }

        private void CheckInput()
        {
            if (!_isFirstKey)
            {
                if (!_isBallThrown)
                {
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        _isKeyPressed = true;
                        
                        _throwBallScale.x += Time.deltaTime;
                        _throwBallScale.y += Time.deltaTime;
                        _throwBallScale.z += Time.deltaTime;
                        _playerBall.ThrowableBall.gameObject.transform.localScale = _throwBallScale;

                        _playerBallScale.x -= Time.deltaTime * 0.002f;
                        _playerBallScale.y -= Time.deltaTime * 0.002f;
                        _playerBallScale.z -= Time.deltaTime * 0.002f;
                        _playerBall.gameObject.transform.localScale += _playerBallScale;
                    }

                    if (_playerBall.ThrowableBall.gameObject.transform.localScale != new Vector3(0, 0, 0))
                    {
                        if (Input.GetKeyUp(KeyCode.Mouse0))
                        {
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            if (Physics.Raycast(ray, out RaycastHit hit))
                                _throwBallDirection = hit.point;

                            _isKeyPressed = false;
                            _isBallThrown = true;
                        }
                    }
                }
            }
            else
                _isFirstKey = false;
        }
    }
}