using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class MenuView : MonoBehaviour
    {
        [Header("MainMenuState")]
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _exitButton;
        [Header("GameState")] 
        [SerializeField] private Button _tryButton;
        [SerializeField] private TMP_Text _resultText;
        [Header("States")] 
        [SerializeField] private RectTransform _mainMenuStateTransform;
        [SerializeField] private RectTransform _gameStateTransform;
        [Header("Other")]
        [SerializeField] private Image _backgroundImage;
        
        public Button PlayButton => _playButton;
        public Button ExitButton => _exitButton;
        public Button TryButton => _tryButton;

        public void SetState(EMenuState state)
        {
            switch (state)
            {
                case EMenuState.Menu:
                    _backgroundImage.enabled = true;
                    _mainMenuStateTransform.gameObject.SetActive(true);
                    _gameStateTransform.gameObject.SetActive(false);
                    break;
              
                case EMenuState.Game:
                    _backgroundImage.enabled = false;
                    _mainMenuStateTransform.gameObject.SetActive(false);
                    _gameStateTransform.gameObject.SetActive(true);
                    break;
            }
        }
        
        public void SetResultText(bool isWin) => _resultText.text = isWin ? "You Win!" : "You Loose!";
    }
}