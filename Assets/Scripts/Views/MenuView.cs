using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class MenuView : MonoBehaviour
    { 
        [Header("MainMenuState")] 
        [SerializeField] private Button playButton; 
        [SerializeField] private Button exitButton;
        [Header("GameState")] 
        [SerializeField] private Button tryButton;
        [Header("ResultState")] 
        [SerializeField] private Button playAgainButton;
        [SerializeField] private TMP_Text resultText;
        [Header("States")] 
        [SerializeField] private RectTransform mainMenuStateTransform;
        [SerializeField] private RectTransform gameStateTransform; 
        [SerializeField] private RectTransform resultStateTransform;
        [Header("Other")] 
        [SerializeField] private Image backgroundImage;
        
        public Button PlayButton => playButton;
        public Button ExitButton => exitButton;
        public Button TryButton => tryButton;
        public Button PlayAgainButton => playAgainButton;

        public void SetState(EMenuState state)
        {
            switch (state)
            {
                case EMenuState.Menu:
                    backgroundImage.enabled = true;
                    mainMenuStateTransform.gameObject.SetActive(true);
                    gameStateTransform.gameObject.SetActive(false);
                    resultStateTransform.gameObject.SetActive(false);
                    break;
                case EMenuState.Game:
                    backgroundImage.enabled = false;
                    mainMenuStateTransform.gameObject.SetActive(false);
                    gameStateTransform.gameObject.SetActive(true);
                    resultStateTransform.gameObject.SetActive(false);
                    break;
                case EMenuState.Result:
                    backgroundImage.enabled = false;
                    mainMenuStateTransform.gameObject.SetActive(false);
                    gameStateTransform.gameObject.SetActive(false);
                    resultStateTransform.gameObject.SetActive(true);
                    break;
            }
        }
        
        public void SetResultText(bool isWin) => resultText.text = isWin ? "You Win!" : "You Loose!";
    }
}