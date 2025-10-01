using System.Collections.Generic;
using Databases;
using Enums;
using Managers;
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
        private RoadLine _roadLine;
        private GameObject _door;
        private Camera _mainCamera;

        private AudioManager _audioManager;
        private IInputService _inputService;
        private IObstaclesService _obstaclesService;
        private IAnimationsService _animationsService;

        private Vector3 _playerBallPosition;
        private Vector3 _throwBallPosition;
        private Vector3 _playerBallScale;
        private Vector3 _throwBallScale;
        private Vector3 _throwBallDirection;

        private bool _isGameOn;
        private bool _isKeyPressed;

        [Inject]
        public void Construct(SignalBus signalBus,
            IGameSettingsDatabase gameSettingsDatabase,
            PlayerBall playerBall,
            RoadLine roadLine,
            GameObject door,
            Camera mainCamera,
            AudioManager audioManager,
            IInputService inputService,
            IObstaclesService obstaclesService,
            IAnimationsService animationsService)
        {
            _signalBus = signalBus;
            _gameSettingVo = gameSettingsDatabase.GameSettingVo;
            _door = door;
            _playerBall = playerBall;
            _roadLine = roadLine;
            _mainCamera = mainCamera;
            _audioManager = audioManager;
            _inputService = inputService;
            _obstaclesService = obstaclesService;
            _animationsService = animationsService;
        }

        public void OnGameStart()
        {
            _playerBall.Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            _playerBall.gameObject.transform.localPosition = _gameSettingVo.PlayerPositionAtStart;
            _playerBall.gameObject.transform.localScale = _gameSettingVo.PlayerScaleAtStart;
            _roadLine.transform.localScale = new Vector3(_playerBall.transform.localScale.x, _roadLine.transform.localScale.y, _roadLine.transform.localScale.z);
            _playerBall.ThrowableBall.gameObject.SetActive(true);
            _door.gameObject.transform.SetLocalPositionAndRotation(_gameSettingVo.DoorStartPosition,
                Quaternion.Euler(_gameSettingVo.DoorStartRotation));
            _animationsService.StartEntryGameAnimation(_mainCamera, _gameSettingVo);
            _obstaclesService.DespawnAllObstacles();
            _obstaclesService.GetObstacles(_gameSettingVo.ObstaclesAmount);

            _isKeyPressed = false;
            _isGameOn = true;
        }

        public void OnObjectsCollision(SignalObjectsCollision signal)
        {
            if (signal.ObjectThatEnteredCollision == _playerBall.gameObject &&
                signal.Collision.gameObject.GetComponentInParent<Obstacle>())
            {
                OnGameOver();
                _signalBus.Fire(new SignalGameOver(false));

                return;
            }

            if (signal.ObjectThatEnteredCollision == _playerBall.ThrowableBall.gameObject &&
                signal.Collision.gameObject.GetComponentInParent<Obstacle>())
            {
                var hitColliders = Physics.OverlapSphere(signal.Collision.gameObject.transform.position,
                    signal.ObjectThatEnteredCollision.transform.localScale.x * _gameSettingVo.ObstaclesRadiusCoef);

                var obstaclesToDestroy = new List<Obstacle>();

                foreach (var hitCollider in hitColliders)
                {
                    var obstacle = hitCollider.gameObject.GetComponentInParent<Obstacle>();
                    
                    if (obstacle)
                        obstaclesToDestroy.Add(obstacle);
                }
                
                _obstaclesService.StartDestroyingObstacles(obstaclesToDestroy);
                _playerBall.ThrowableBall.IsBallThrown = false;
            }
        }

        public void OnStartTryAnimation(SignalStartTryAnimation startTryAnimationSignal) 
            => StartTryAnimation();

        public void OnGameButtonHeld(SignalButtonHeld buttonHeldSignal) 
            => _isGameOn = !buttonHeldSignal.IsHeld;

        private void Update()
        {
            if (!_isGameOn)
                return;
                
            CheckInput(); 
            CheckPlayerBallSize(); 
            CheckThrownBall();
        }

        private void StartTryAnimation()
        {
            _isGameOn = false;
            _playerBall.ThrowableBall.IsBallThrown = false;
            _playerBall.Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            _playerBall.ThrowableBall.gameObject.SetActive(false);
            _animationsService.StartTryGameAnimation();
        }

        private void CheckPlayerBallSize()
        {
            if (_playerBall.gameObject.transform.localScale.x <= _gameSettingVo.PlayerBallScaleLimit)
            {
                OnGameOver();
                _signalBus.Fire(new SignalGameOver(false));
            }
        }

        private void CheckThrownBall()
        {
            if (_playerBall.ThrowableBall.IsBallThrown)
            {
                if (Vector3.Distance(_playerBallPosition, _throwBallPosition) > _gameSettingVo.ThrowableBallMaxDistance)
                {
                    _playerBall.ThrowableBall.IsBallThrown = false;
                    return;
                }

                _throwBallDirection.y = _playerBall.gameObject.transform.position.y;
                _throwBallPosition += _throwBallDirection.normalized * (_gameSettingVo.ThrowableBallSpeed * Time.deltaTime);
                _playerBall.ThrowableBall.gameObject.transform.position = _throwBallPosition;
            }
            else
            {
                _throwBallPosition = _playerBallPosition;
                _throwBallPosition.z -= _gameSettingVo.ThrowableBallZOffset;
                _playerBall.ThrowableBall.gameObject.transform.position = _throwBallPosition;

                if (!_isKeyPressed)
                {
                    _throwBallScale = _gameSettingVo.MinThrowableBallScale;
                    _playerBall.ThrowableBall.gameObject.transform.localScale = _throwBallScale;
                }
            }
        }

        private void CheckInput()
        {
            if (!_isGameOn || _playerBall.ThrowableBall.IsBallThrown)
                return;

            if (_inputService.IsClickHeld())
            {
                _isKeyPressed = true;

                _throwBallScale.x += Time.deltaTime * _gameSettingVo.ThrowableBallScaleCoef;
                _throwBallScale.y += Time.deltaTime * _gameSettingVo.ThrowableBallScaleCoef;
                _throwBallScale.z += Time.deltaTime * _gameSettingVo.ThrowableBallScaleCoef;
                _playerBall.ThrowableBall.gameObject.transform.localScale = _throwBallScale;

                _playerBallScale.x -= Time.deltaTime * _gameSettingVo.PlayerBallScaleCoef;
                _playerBallScale.y -= Time.deltaTime * _gameSettingVo.PlayerBallScaleCoef;
                _playerBallScale.z -= Time.deltaTime * _gameSettingVo.PlayerBallScaleCoef;
                _playerBall.gameObject.transform.localScale += _playerBallScale;
                
                _roadLine.transform.localScale = new Vector3(_playerBall.transform.localScale.x, _roadLine.transform.localScale.y, _roadLine.transform.localScale.z);
            }

            if (_playerBall.ThrowableBall.gameObject.transform.localScale != _gameSettingVo.MinThrowableBallScale)
            {
                if (_inputService.IsClickUp())
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                        _throwBallDirection = hit.point;
                    
                    _audioManager.PlayMusicByType(EAudioType.BallThrow, false);
                    _isKeyPressed = false;
                    _playerBall.ThrowableBall.IsBallThrown = true;
                }
            }
        }
        
        private void OnGameOver()
        {
            _animationsService.KillSequence();
            _isGameOn = false;
        }
    }
}