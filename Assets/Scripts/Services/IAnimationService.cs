using UnityEngine;

namespace Services
{
    public interface IAnimationService
    {
        void KillSequence();
        void SetStartAnimationData(Vector3 position);
        void StartEndGameAnimation();
    }
}