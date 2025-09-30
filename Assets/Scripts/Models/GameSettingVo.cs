using System;
using UnityEngine;

namespace Models
{ 
    [Serializable]
    public class GameSettingVo
    {
        public float PlayerBallScaleLimit;
        public float BallsHeight;
        public Vector3 PlayerPositionAtStart;
        public Vector3 PlayerScaleAtStart;
        public Vector3 DoorStartPosition; 
        public Vector3 DoorStartRotation; 
        public Vector3 CameraGamePosition;
        public Vector3 CameraGameRotation;
    }
}
