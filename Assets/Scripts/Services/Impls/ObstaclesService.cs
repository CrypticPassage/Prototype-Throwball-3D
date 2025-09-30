using System.Collections;
using System.Collections.Generic;
using Databases;
using Factories;
using Models;
using Objects;
using Pools;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Services.Impls
{
    public class ObstaclesService : MonoBehaviour, IObstaclesService
    {
        private PoolBase<Obstacle> _obstaclesPool;
        private PoolBase<DestroyedObstacle> _destroyedObstaclesPool;
        private ObstacleFactory _obstacleFactory;
        private DestroyedObstacleFactory _destroyedObstacleFactory;
        private GameSettingVo _gameSettingVo;
        
        [Inject]
        public void Construct(IGameSettingsDatabase gameSettingsDatabase,
            ObstacleFactory obstacleFactory,
            DestroyedObstacleFactory destroyedObstacleFactory)
        {
            _gameSettingVo = gameSettingsDatabase.GameSettingVo;
            _obstacleFactory = obstacleFactory;
            _destroyedObstacleFactory = destroyedObstacleFactory;
            _obstaclesPool = new PoolBase<Obstacle>(
                PreloadObstacle, GetActionObstacle, ReturnActionObstacle, 100);
            _destroyedObstaclesPool = new PoolBase<DestroyedObstacle>(
                PreloadDestroyedObstacle, GetActionDestroyedObstacle, ReturnActionDestroyedObstacle, 25);
        }
        
        public void GetObstacles(int amount)
        {
            _obstaclesPool.ReturnAll();
            
            for (int i = 0; i < amount; i++)
            {
                var obstacle = _obstaclesPool.Get();

                var position = obstacle.gameObject.transform.position;
                position.x = Random.Range(-10, 10);
                position.z = Random.Range(-10, 10);
                obstacle.gameObject.transform.position = position;
            }
        }

        public void GetDestroyedObstacles(List<Vector3> positions)
        {
            foreach (var position in positions)
            {
                var obstacle = _destroyedObstaclesPool.Get();
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