using Models;
using UnityEngine;

namespace Databases.Impls
{ 
    [CreateAssetMenu(menuName = "Databases/GameSettingsDatabase", fileName = "GameSettingsDatabase")] 
    public class GameSettingsDatabase : ScriptableObject, IGameSettingsDatabase 
    { 
        [SerializeField] private GameSettingVo _gameSettingVo;

        public GameSettingVo GameSettingVo => _gameSettingVo;
    }
}