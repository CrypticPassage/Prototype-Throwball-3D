using System;
using System.Collections.Generic;
using Models;
using UnityEngine;

namespace Databases.Impls
{ 
    [CreateAssetMenu(menuName = "Databases/Levels/LevelSettingsDatabase", fileName = "LevelSettingsDatabase")] 
    public class LevelSettingsDatabase : ScriptableObject, ILevelSettingsDatabase 
    { 
        [SerializeField] private LevelSettingVo[] _levelSettings;
        private Dictionary<int, LevelSettingVo> _levelSettingsDictionary;

        public LevelSettingVo[] LevelSettings => _levelSettings;
        
        private void OnEnable() 
        { 
            _levelSettingsDictionary = new Dictionary<int, LevelSettingVo>();
            
            foreach (var setting in _levelSettings) 
                _levelSettingsDictionary.Add(setting.Id, setting);
        }
        
        public LevelSettingVo GetLevelSetting(int id) 
        { 
            try 
            { 
                return _levelSettingsDictionary[id];
            }
            catch (Exception e) 
            { 
                throw new Exception(
                    $"[{nameof(LevelSettingsDatabase)}] LevelSettingVo by id {id} is not present in the dictionary. {e.StackTrace}"); 
            } 
        } 
    }
}