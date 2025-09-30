using Databases;
using Enums;
using Signals;
using UnityEngine;
using Views;
using Zenject;

namespace Controllers.Impls
{
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
            => _menuView.SetResultText(gameOverSignal.IsWin);
        
        private void Start()
        {
            _menuView.PlayButton.onClick.AddListener(OnStartButtonClick);
            _menuView.ExitButton.onClick.AddListener(OnExitButtonClick);
            _menuView.TryButton.onClick.AddListener(OnTryButtonClick);
            
            _menuView.gameObject.SetActive(true);
            _menuView.SetState(EMenuState.Menu);
        }

        private void OnStartButtonClick()
        {
            _menuView.SetState(EMenuState.Game);
            _signalBus.Fire<SignalStartGame>();
        }

        private void OnExitButtonClick() => Application.Quit();
        private void OnTryButtonClick() => _signalBus.Fire<SignalStartAnimation>();
    }
}