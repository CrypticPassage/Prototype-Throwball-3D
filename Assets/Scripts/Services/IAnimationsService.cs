using Models;
using UnityEngine;

namespace Services
{
    public interface IAnimationsService
    {
        void KillSequence();
        void StartGameAnimation(Camera camera, GameSettingVo gameSettingVo);
        void StartEndGameAnimation();
    }
}