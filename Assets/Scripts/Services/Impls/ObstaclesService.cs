using System.Collections;
using System.Collections.Generic;
using Factories;
using Objects;
using Pools;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Services.Impls
{
    public class ObstaclesService : MonoBehaviour, IObstaclesService
    {
        private Transform _spawnZoneTransform;
        private PoolBase<Obstacle> _obstaclePool;
        private PoolBase<DestroyedObstacle> _destroyedPool;

        [Inject] private ObstacleFactory _obstacleFactory;
        [Inject] private DestroyedObstacleFactory _destroyedObstacleFactory;
        
        [Inject]
        public void Construct(Transform spawnZoneTransform)
        {
            _spawnZoneTransform = spawnZoneTransform;
        }

        private void Awake()
        {
            _obstaclePool = new PoolBase<Obstacle>(PreloadObstacle, GetActionObstacle, ReturnActionObstacle, 100);
            _destroyedPool = new PoolBase<DestroyedObstacle>(PreloadDestroyedObstacle, GetActionDestroyedObstacle, ReturnActionDestroyedObstacle, 30);
        }

        public void GetObstacles(int amount)
        {
            _obstaclePool.ReturnAll();
            
            for (int i = 0; i < amount; i++)
            {
                var obstacle = _obstaclePool.Get();
                obstacle.gameObject.transform.SetParent(_spawnZoneTransform, false);

                var position = obstacle.gameObject.transform.position;
                position.x = Random.Range(-2, 3);
                position.z = Random.Range(2, 12);
                obstacle.gameObject.transform.position = position;
            }
        }

        public void GetDestroyedObstacles(List<Vector3> positions)
        {
            foreach (var position in positions)
            {
                var obstacle = _destroyedPool.Get();
                obstacle.gameObject.transform.SetParent(_spawnZoneTransform, false);
                obstacle.gameObject.transform.position = position;
                
                StartCoroutine(ChangeColorAndReturn(obstacle));
            }
        }
        
        public Obstacle PreloadObstacle() => _obstacleFactory.Create();
        
        public void GetActionObstacle(Obstacle obstacle) => obstacle.gameObject.SetActive(true);

        public void ReturnActionObstacle(Obstacle obstacle) => obstacle.gameObject.SetActive(false);

        public DestroyedObstacle PreloadDestroyedObstacle() => _destroyedObstacleFactory.Create();
        
        public void GetActionDestroyedObstacle(DestroyedObstacle obstacle) => obstacle.gameObject.SetActive(true);

        public void ReturnActionDestroyedObstacle(DestroyedObstacle obstacle) => obstacle.gameObject.SetActive(false);

        private IEnumerator ChangeColorAndReturn(DestroyedObstacle obstacle)
        {
            yield return new WaitForSeconds(0.4f);
            
            ReturnActionDestroyedObstacle(obstacle);
        }
    }
}