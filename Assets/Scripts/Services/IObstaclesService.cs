using System.Collections.Generic;
using Objects;
using UnityEngine;

namespace Services
{
    public interface IObstaclesService
    {
        void GetObstacles(int amount);
        void GetDestroyedObstacles(List<Vector3> positions);
        void ReturnActionObstacle(Obstacle obstacle);
    }
}