using Databases;
using Enums;
using Signals;
using UnityEngine;
using Views;
using Zenject;

namespace Controllers
{
    public class MenuController : MonoBehaviour
    {
        private SignalBus _signalBus;
        
        private MenuView _menuView;
        private ILevelSettingsDatabase _levelSettingsDatabase;

        [Inject]
        public void Construct(SignalBus signalBus,
            MenuView menuView,
            ILevelSettingsDatabase levelSettingsDatabase)
        {
            _signalBus = signalBus;
            _menuView = menuView;
            _levelSettingsDatabase = levelSettingsDatabase;
        }
        
        public MenuController(
            MenuView menuView) 
        {
            _menuView = menuView;
        }

        public void OnGameStart(SignalStartGame startGameSignal) => _menuView.SetState(EMenuState.Game);

        public void OnGameOver(SignalGameOver gameOverSignal)
        {
            _menuView.SetState(EMenuState.Result);
            _menuView.SetResultText(gameOverSignal.IsWin);
        }
        
        private void Start()
        {
            SetMenuLevelsData();

            _menuView.StartButton.onClick.AddListener(OnStartButtonClick);
            _menuView.ExitButton.onClick.AddListener(OnExitButtonClick);
            _menuView.MovePlayerBallButton.onClick.AddListener(OnMovePlayerBallButtonClick);
            _menuView.MovePlayerBallButton.onClick.AddListener(OnMovePlayerBallButtonClick);
            _menuView.BackButton.onClick.AddListener(OnBackButtonClick);
            _menuView.MenuButton.onClick.AddListener(OnMenuButtonClick);
            
            _menuView.gameObject.SetActive(true);
            _menuView.SetState(EMenuState.Menu);
        }
        
        private void OnStartButtonClick() => _menuView.SetState(EMenuState.LevelChoose);
        private void OnExitButtonClick() => Application.Quit();
        private void OnMovePlayerBallButtonClick() => _signalBus.Fire<SignalStartAnimation>();
        private void OnBackButtonClick() => _menuView.SetState(EMenuState.Menu);
        private void OnMenuButtonClick() => _menuView.SetState(EMenuState.Menu);

        private void SetMenuLevelsData()
        {
            var levelSettings = _levelSettingsDatabase.LevelSettings;

            foreach (var setting in levelSettings) 
                _menuView.SetLevelData(setting);
        }
    }
}