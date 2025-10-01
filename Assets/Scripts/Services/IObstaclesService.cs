using System.Collections.Generic;
using Objects;
using UnityEngine;

namespace Services
{
    public interface IObstaclesService
    {
        void DespawnAllObstacles();
        void GetObstacles(int amount);
        void StartDestroyingObstacles(List<Obstacle> obstacles);
    }
}