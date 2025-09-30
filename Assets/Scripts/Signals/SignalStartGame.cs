using Models;

namespace Signals
{
    public class SignalStartGame
    {
        public LevelSettingVo LevelSetting;

        public SignalStartGame(LevelSettingVo levelSetting)
        {
            LevelSetting = levelSetting;
        }
    }
}