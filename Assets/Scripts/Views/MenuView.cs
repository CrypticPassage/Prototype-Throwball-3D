using Enums;
using Factories;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Views
{
    public class MenuView : MonoBehaviour
    {
        [Header("MainMenuState")]
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _exitButton;
        [Header("ChooseLevelState")]
        [SerializeField] private Button _backButton;
        [Header("GameState")] 
        [SerializeField] private Button _movePlayerBallButton;
        [Header("ResultState")]
        [SerializeField] private Button _menuButton;
        [SerializeField] private TMP_Text _resultText;
        [Header("States")] 
        [SerializeField] private RectTransform _mainMenuStateTransform; 
        [SerializeField] private RectTransform _chooseLevelStateTransform; 
        [SerializeField] private RectTransform _gameStateTransform; 
        [SerializeField] private RectTransform _resultStateTransform;
        [Header("Other")]
        [SerializeField] private RectTransform _dataContainerTransform;
        [SerializeField] private RectTransform _levelsTransform;
        [SerializeField] private Image _backgroundImage;

        [Inject] private LevelItemFactory _levelItemFactory;
        
        public Button StartButton => _startButton;
        public Button ExitButton => _exitButton;
        public Button BackButton => _backButton;
        public Button MovePlayerBallButton => _movePlayerBallButton;
        public Button MenuButton => _menuButton;
        public TMP_Text ResultText => _resultText;

        public void SetState(EMenuState state)
        {
            switch (state)
            {
                case EMenuState.Menu:
                    _backgroundImage.enabled = true;
                    _dataContainerTransform.gameObject.SetActive(true);
                    _mainMenuStateTransform.gameObject.SetActive(true);
                    _chooseLevelStateTransform.gameObject.SetActive(false);
                    _gameStateTransform.gameObject.SetActive(false);
                    _resultStateTransform.gameObject.SetActive(false);
                    break;
                case EMenuState.LevelChoose:
                    _backgroundImage.enabled = true;
                    _dataContainerTransform.gameObject.SetActive(true);
                    _mainMenuStateTransform.gameObject.SetActive(false);
                    _chooseLevelStateTransform.gameObject.SetActive(true);
                    _gameStateTransform.gameObject.SetActive(false);
                    _resultStateTransform.gameObject.SetActive(false);
                    break;
                case EMenuState.Game:
                    _backgroundImage.enabled = false;
                    _dataContainerTransform.gameObject.SetActive(false);
                    _mainMenuStateTransform.gameObject.SetActive(false);
                    _chooseLevelStateTransform.gameObject.SetActive(false);
                    _gameStateTransform.gameObject.SetActive(true);
                    _resultStateTransform.gameObject.SetActive(false);
                    break;
                case EMenuState.Result:
                    _backgroundImage.enabled = true;
                    _dataContainerTransform.gameObject.SetActive(true);
                    _mainMenuStateTransform.gameObject.SetActive(false);
                    _chooseLevelStateTransform.gameObject.SetActive(false);
                    _gameStateTransform.gameObject.SetActive(false);
                    _resultStateTransform.gameObject.SetActive(true);
                    break;
            }
        }

        public void SetLevelData(LevelSettingVo setting)
        {
            var levelItem = _levelItemFactory.Create();
            levelItem.SetLevelData(setting);
            levelItem.transform.SetParent(_levelsTransform, false);
        }

        public void SetResultText(bool isWin) => _resultText.text = isWin ? "You Win!" : "You Loose!";
    }
}