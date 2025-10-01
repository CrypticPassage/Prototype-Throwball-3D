using System;
using UnityEngine;

namespace Models
{ 
    [Serializable]
    public class GameSettingVo
    {
        public int ObstaclesAmount;
        public int ObstaclesMinXPosition;
        public int ObstaclesMaxXPosition;
        public int ObstaclesMinZPosition;
        public int ObstaclesMaxZPosition;
        public int ObstaclesRadiusCoef;
        public int ThrowableBallMaxDistance;
        public int ThrowableBallSpeed;
        public int ThrowableBallZOffset;
        public float PlayerBallScaleLimit;
        public float PlayerBallScaleCoef;
        public float ThrowableBallScaleCoef;
        public float ObstacleColorChangeTime;
        public float ObstaclesDestroyingDelay;
        public Vector3 PlayerPositionAtStart;
        public Vector3 PlayerScaleAtStart;
        public Vector3 DoorStartPosition; 
        public Vector3 DoorStartRotation; 
        public Vector3 CameraGamePosition;
        public Vector3 CameraGameRotation;
        public Vector3 MinThrowableBallScale;
        public Material ActiveObstacleMaterial;
        public Material DestroyingObstacleMaterial;
    }
}
