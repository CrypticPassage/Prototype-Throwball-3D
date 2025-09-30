using Models;

namespace Databases
{ 
    public interface IGameSettingsDatabase
    {
        GameSettingVo GameSettingVo { get; }
    }
}