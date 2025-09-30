using System.Collections.Generic;
using Services;
using Signals;
using UnityEngine;
using Zenject;

namespace Objects
{
    public class ThrowBall : MonoBehaviour
    {
        private SignalBus _signalBus;
        private IObstaclesService _obstaclesService;

        [Inject] 
        public void Construct(SignalBus signalBus,
            IObstaclesService obstaclesService)
        {
            _signalBus = signalBus;
            _obstaclesService = obstaclesService;
        }
        
        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Obstacle")
            {
                int destroyedObstaclesAmount = 0;
                List<Vector3> obstaclesPositions = new List<Vector3>();

                Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, this.gameObject.transform.localScale.x * 5);

                foreach (var hitCollider in hitColliders)
                    if (hitCollider.tag == "Obstacle")
                    {
                       var obstacle = hitCollider.gameObject.GetComponentInParent<Obstacle>();
                       if (obstacle)
                       {
                           obstaclesPositions.Add(obstacle.gameObject.transform.position);
                           _obstaclesService.ReturnActionObstacle(obstacle);
                           destroyedObstaclesAmount++;
                       }
                    }
                
                _signalBus.Fire(new SignalThrowBallCollision(destroyedObstaclesAmount, obstaclesPositions));
            }
        }
    }
}