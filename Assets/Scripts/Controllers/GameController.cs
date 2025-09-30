using Objects;
using Services;
using Signals;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        private SignalBus _signalBus;
        
        private PlayerBall _playerBall;
        private IObstaclesService _obstaclesService;
        private IAnimationService _animationService;

        private int _obstaclesTotalAmount;
        private int _obstaclesDestroyedAmount;
        
        private float _playerBallScaleLimit;
        private float _ballsheight = -0.3f;

        private Vector3 _playerPositionAtStart = new Vector3(0, -0.3f, -0.94f);
        private Vector3 _playerScaleAtStart = new Vector3(2.5f, 2.5f, 2.5f);
        
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
            PlayerBall playerBall, 
            IObstaclesService obstaclesService,
            IAnimationService animationService)
        {
            _signalBus = signalBus;
            _playerBall = playerBall;
            _obstaclesService = obstaclesService;
            _animationService = animationService;
        }

        public void OnGameStart(SignalStartGame startGameSignal)
        {
            _playerBall.gameObject.transform.position = _playerPositionAtStart;
            _playerBall.gameObject.transform.localScale = _playerScaleAtStart;
            _obstaclesDestroyedAmount = 0;
            _obstaclesService.GetObstacles(startGameSignal.LevelSetting.ObstaclesAmount);
            _obstaclesTotalAmount = startGameSignal.LevelSetting.ObstaclesAmount;
            _animationService.SetStartAnimationData();
            _isKeyPressed = false;
            _isFirstKey = true;
            Start();
            _isGameOn = true;
        }

        public void OnGameOver(SignalGameOver gameOverSignal)
        {
            _animationService.KillSequence();
            _isGameOn = false;
            _isAnimationOn = false;
            _isFirstKey = true;
        }

        public void OnThrownBallCollision(SignalThrowBallCollision throwBallCollisionSignal)
        {
            _isBallThrown = false;
            _obstaclesDestroyedAmount += throwBallCollisionSignal.ObstaclesAmount;
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

        private void Start()
        {
            _playerBallPosition = _playerBall.gameObject.transform.position;
            _throwBallPosition = _playerBall.ThrowBall.gameObject.transform.position;
            _playerBallScale = _playerBall.gameObject.transform.localScale;
            _throwBallScale = _playerBall.ThrowBall.gameObject.transform.localScale;
            
            _playerBallScaleLimit = _playerBallScale.x / 5;
        }

        private void Update()
        {
            if (_isGameOn)
            {
                CheckInput();
                SetBallsHeight();
                CheckPlayerBallSize();
                CheckThrownBall();
                CheckDestroyedObstacles();
            }
        }

        private void CheckDestroyedObstacles()
        {
            if (_obstaclesDestroyedAmount >= _obstaclesTotalAmount)
                StartAnimation();
        }

        private void StartAnimation()
        {
            _isGameOn = false;
            _isBallThrown = false;
            _isAnimationOn = true;
            _animationService.StartEndGameAnimation();
        }

        private void SetBallsHeight()
        {
            _playerBallPosition.y = _ballsheight;
            _playerBall.gameObject.transform.position = _playerBallPosition;
                
            _throwBallPosition.y = _ballsheight;
        }
        
        private void CheckPlayerBallSize()
        {
            if (_playerBall.gameObject.transform.localScale.x <= _playerBallScaleLimit)
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
                _playerBall.ThrowBall.gameObject.transform.position = _throwBallPosition;
            }
            else
            {
                _throwBallPosition = _playerBallPosition;
                _throwBallPosition.z += 1;
                _playerBall.ThrowBall.gameObject.transform.position = _throwBallPosition;

                if (!_isKeyPressed)
                {
                    _throwBallScale = new Vector3(0, 0, 0);
                    _playerBall.ThrowBall.gameObject.transform.localScale = _throwBallScale;
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
                        _playerBall.ThrowBall.gameObject.transform.localScale = _throwBallScale;

                        _playerBallScale.x -= Time.deltaTime;
                        _playerBallScale.y -= Time.deltaTime;
                        _playerBallScale.z -= Time.deltaTime;
                        _playerBall.gameObject.transform.localScale = _playerBallScale;
                    }

                    if (_playerBall.ThrowBall.gameObject.transform.localScale != new Vector3(0, 0, 0))
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