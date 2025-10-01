using System.Collections;
using System.Collections.Generic;
using Databases;
using Enums;
using Factories;
using Managers;
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
        private ObstacleFactory _obstacleFactory;
        private AudioManager _audioManager;
        private GameSettingVo _gameSettingVo;
        
        [Inject]
        public void Construct(IGameSettingsDatabase gameSettingsDatabase,
            AudioManager audioManager,
            ObstacleFactory obstacleFactory)
        {
            _gameSettingVo = gameSettingsDatabase.GameSettingVo;
            _obstacleFactory = obstacleFactory;
            _audioManager = audioManager;
            _obstaclesPool = new PoolBase<Obstacle>(
                PreloadObstacle, GetActionObstacle, ReturnActionObstacle, _gameSettingVo.ObstaclesAmount);
        }

        public void DespawnAllObstacles()
        {
            _obstaclesPool.ReturnAll();
        }

        public void GetObstacles(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var obstacle = _obstaclesPool.Get();

                var position = obstacle.gameObject.transform.position;
                position.x = Random.Range(_gameSettingVo.ObstaclesMinXPosition, _gameSettingVo.ObstaclesMaxXPosition);
                position.z = Random.Range(_gameSettingVo.ObstaclesMinZPosition, _gameSettingVo.ObstaclesMaxZPosition);
                obstacle.gameObject.transform.position = position;
            }
        }

        public void StartDestroyingObstacles(List<Obstacle> obstacles)
        { 
            _audioManager.PlayMusicByType(EAudioType.BallHit, false);
            StartCoroutine(DestroyObstaclesWithDelay(obstacles));
        }

        private IEnumerator DestroyObstaclesWithDelay(List<Obstacle> obstacles)
        {
            foreach (var obstacle in obstacles)
            {
                StartCoroutine(ChangeColorAndReturn(obstacle));
                
                yield return new WaitForSeconds(_gameSettingVo.ObstaclesDestroyingDelay);
            }
        }
        
        private IEnumerator ChangeColorAndReturn(Obstacle obstacle)
        {
            obstacle.MeshRenderer.material = _gameSettingVo.DestroyingObstacleMaterial;
            
            yield return new WaitForSeconds(_gameSettingVo.ObstacleColorChangeTime);
            
            ReturnActionObstacle(obstacle);
            obstacle.MeshRenderer.material = _gameSettingVo.ActiveObstacleMaterial;
        }

        private Obstacle PreloadObstacle() => _obstacleFactory.Create();
        
        private void GetActionObstacle(Obstacle obstacle) => obstacle.gameObject.SetActive(true);

        private void ReturnActionObstacle(Obstacle obstacle) => obstacle.gameObject.SetActive(false);
    }
}