using Databases;
using Enums;
using Signals;
using UnityEngine;
using Views;
using Zenject;

namespace Controllers.Impls
{
    /// <summary>
    /// Даний контроллер відповідає за логіку ігрового меню (UI).
    /// Контроллер приймає фаєри сигналів та виконує конкретну логіку під кожний сигнал.
    /// </summary>
    public class MenuController : MonoBehaviour, IMenuController
    {
        private SignalBus _signalBus;
        
        private MenuView _menuView;
        
        [Inject]
        public void Construct(SignalBus signalBus,
            MenuView menuView,
            IGameSettingsDatabase gameSettingsDatabase)
        {
            _signalBus = signalBus;
            _menuView = menuView;
        }
        
        public MenuController(
            MenuView menuView) 
        {
            _menuView = menuView;
        }

        public void OnGameOver(SignalGameOver gameOverSignal)
        {
            _menuView.SetResultText(gameOverSignal.IsWin);
            _menuView.SetState(EMenuState.Result);
        }

        private void Start()
        {
            _menuView.PlayButton.onClick.AddListener(OnStartButtonClick);
            _menuView.ExitButton.onClick.AddListener(OnExitButtonClick);
            _menuView.TryButton.onClick.AddListener(OnTryButtonClick);
            _menuView.PlayAgainButton.onClick.AddListener(OnPlayAgainButtonClick);
            
            _menuView.gameObject.SetActive(true);
            _menuView.SetState(EMenuState.Menu);
        }

        private void OnStartButtonClick() => StartGame();

        private void OnExitButtonClick() => Application.Quit();
        
        private void OnTryButtonClick() => _signalBus.Fire<SignalStartTryAnimation>();

        private void OnPlayAgainButtonClick() => StartGame();

        private void StartGame()
        {
            _menuView.SetState(EMenuState.Game);
            _signalBus.Fire<SignalStartGame>();
        }
    }
}