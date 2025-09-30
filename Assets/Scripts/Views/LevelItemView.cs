using Models;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Views
{
    public class LevelItemView : MonoBehaviour
    {
        [SerializeField] private Image _levelImage;
        [SerializeField] private Button _levelButton;
        [SerializeField] private TMP_Text _levelText;
        private int _levelId;
        private LevelSettingVo _levelSetting;
        
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void SetLevelData(LevelSettingVo setting)
        {
            _levelId = setting.Id;
            _levelSetting = setting;

            var level = _levelId + 1;
            _levelText.text = level.ToString();
            
            _levelButton.onClick.AddListener(OnLevelButtonClick);
        }

        private void OnLevelButtonClick() => _signalBus.Fire(new SignalStartGame(_levelSetting));
    }
}