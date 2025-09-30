using System;
using UnityEngine;

namespace Models
{ 
    [Serializable]
    public class LevelSettingVo
    {
        [Header("Level Id")]
        public int Id;
        [Header("Environment Setting")]
        public int ObstaclesAmount;
    }
}
