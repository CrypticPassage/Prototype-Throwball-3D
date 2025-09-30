using Models;

namespace Databases
{ 
    public interface ILevelSettingsDatabase
    {
        LevelSettingVo[] LevelSettings { get; }
        LevelSettingVo GetLevelSetting(int id);
    }
}