using Models;
using UnityEngine;

namespace Services
{
    public interface IAnimationsService
    {
        void KillSequence();
        void StartEntryGameAnimation(Camera camera, GameSettingVo gameSettingVo);
        void StartTryGameAnimation();
    }
}