using Controllers;
using Factories;
using Objects;
using Services;
using Services.Impls;
using Signals;
using UnityEngine;
using Views;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Controllers")] 
        [SerializeField] private MenuController menuController;
        [SerializeField] private GameController gameController;
        [Header("Services")] 
        [SerializeField] private ObstaclesService obstaclesService;
        [SerializeField] private AnimationService animationService;
        [Header("Views")]
        [SerializeField] private MenuView menuView;
        [SerializeField] private LevelItemView levelItemView;
        [Header("Objects")] 
        [SerializeField] private PlayerBall playerBall;
        [SerializeField] private ThrowBall throwBall;
        [SerializeField] private Obstacle obstacle;
        [SerializeField] private DestroyedObstacle destroyedObstacle;
        [SerializeField] private Door door;
        [Header("Other")] 
        [SerializeField] private Transform spawnZoneTransform;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            DeclareSignals();
            BindFactories();
            BindViews();
            BindObjects();
            BindControllers();
            BindServices();
            BindSignals();
            
            Container.Bind<Transform>().FromInstance(spawnZoneTransform).AsSingle().NonLazy();
        }

        private void DeclareSignals()
        {
            Container.DeclareSignal<SignalStartGame>();
            Container.DeclareSignal<SignalGameOver>();
            Container.DeclareSignal<SignalThrowBallCollision>();
            Container.DeclareSignal<SignalStartAnimation>();
            Container.DeclareSignal<SignalButtonHeld>();
        }

        private void BindSignals()
        {
            Container.BindSignal<SignalStartGame>()
                .ToMethod<GameController>(x => x.OnGameStart).FromResolve();
            Container.BindSignal<SignalStartGame>()
                .ToMethod<MenuController>(x => x.OnGameStart).FromResolve();
            Container.BindSignal<SignalGameOver>()
                .ToMethod<GameController>(x => x.OnGameOver).FromResolve();
            Container.BindSignal<SignalGameOver>()
                .ToMethod<MenuController>(x => x.OnGameOver).FromResolve();
            Container.BindSignal<SignalThrowBallCollision>()
                .ToMethod<GameController>(x => x.OnThrownBallCollision).FromResolve();
            Container.BindSignal<SignalStartAnimation>()
                .ToMethod<GameController>(x => x.OnStartAnimation).FromResolve();
            Container.BindSignal<SignalButtonHeld>()
                .ToMethod<GameController>(x => x.OnGameButtonHeld).FromResolve();
        }

        private void BindFactories()
        {
            Container.BindFactory<LevelItemView, LevelItemFactory>()
                .FromComponentInNewPrefab(levelItemView)
                .AsTransient();
            Container.BindFactory<Obstacle, ObstacleFactory>()
                .FromComponentInNewPrefab(obstacle)
                .AsTransient();
            Container.BindFactory<DestroyedObstacle, DestroyedObstacleFactory>()
                .FromComponentInNewPrefab(destroyedObstacle)
                .AsTransient();
        }
        
        private void BindViews() => Container.Bind<MenuView>().FromInstance(menuView).AsSingle().NonLazy();

        private void BindObjects()
        {
            Container.Bind<PlayerBall>().FromInstance(playerBall).AsSingle().NonLazy();
            Container.Bind<ThrowBall>().FromInstance(throwBall).AsSingle().NonLazy();
            Container.Bind<Obstacle>().FromInstance(obstacle).AsSingle().NonLazy();
            Container.Bind<Door>().FromInstance(door).AsSingle().NonLazy();
        }

        private void BindControllers()
        {
            Container.Bind<MenuController>().FromInstance(menuController).AsSingle().NonLazy();
            Container.Bind<GameController>().FromInstance(gameController).AsSingle().NonLazy();
        }

        private void BindServices()
        {
            Container.Bind<IObstaclesService>().FromInstance(obstaclesService).AsSingle().NonLazy();
            Container.Bind<IAnimationService>().FromInstance(animationService).AsSingle().NonLazy();
        }
    }
}