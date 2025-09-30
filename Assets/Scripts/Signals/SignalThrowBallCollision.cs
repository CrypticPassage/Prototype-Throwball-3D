using System.Collections.Generic;
using UnityEngine;

namespace Signals
{
    public class SignalThrowBallCollision
    {
        public int ObstaclesAmount;
        public List<Vector3> ObstaclesPositions;

        public SignalThrowBallCollision(int obstaclesAmount, List<Vector3> obstaclesPositions)
        {
            ObstaclesAmount = obstaclesAmount;
            ObstaclesPositions = obstaclesPositions;
        }
    }
}