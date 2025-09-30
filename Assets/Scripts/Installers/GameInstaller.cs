using Controllers;
using Controllers.Impls;
using Factories;
using Objects;
using Services;
using Services.Impls;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Views;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Views")]
        [SerializeField] private MenuView menuView;
        [Header("Objects")] 
        [SerializeField] private Camera mainCamera;
        [SerializeField] private PlayerBall playerBall;
        [SerializeField] private Obstacle obstacle;
        [SerializeField] private DestroyedObstacle destroyedObstacle;
        [SerializeField] private GameObject door;
        [Header("Other")]
        [SerializeField] private Image flashImage;
        [SerializeField] private Transform scriptsTransform;

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
                .ToMethod<IGameController>(x => x.OnGameStart).FromResolve();
            Container.BindSignal<SignalGameOver>()
                .ToMethod<IGameController>(x => x.OnGameOver).FromResolve();
            Container.BindSignal<SignalGameOver>()
                .ToMethod<IMenuController>(x => x.OnGameOver).FromResolve();
            Container.BindSignal<SignalThrowBallCollision>()
                .ToMethod<IGameController>(x => x.OnThrownBallCollision).FromResolve();
            Container.BindSignal<SignalStartAnimation>()
                .ToMethod<IGameController>(x => x.OnStartAnimation).FromResolve();
            Container.BindSignal<SignalButtonHeld>()
                .ToMethod<IGameController>(x => x.OnGameButtonHeld).FromResolve();
        }

        private void BindFactories()
        {
            Container.BindFactory<Obstacle, ObstacleFactory>()
                .FromComponentInNewPrefab(obstacle)
                .AsTransient();
            Container.BindFactory<DestroyedObstacle, DestroyedObstacleFactory>()
                .FromComponentInNewPrefab(destroyedObstacle)
                .AsTransient();
        }

        private void BindViews()
        {
            Container.Bind<MenuView>().FromInstance(menuView).AsSingle().NonLazy();
        }

        private void BindObjects()
        {
            Container.Bind<Camera>().FromInstance(mainCamera).AsSingle().NonLazy();
            Container.Bind<PlayerBall>().FromInstance(playerBall).AsSingle().NonLazy();
            Container.Bind<GameObject>().FromInstance(door).AsSingle().NonLazy();
            Container.Bind<Image>().FromInstance(flashImage).AsSingle().NonLazy();
        }

        private void BindControllers()
        {
            Container.Bind<IMenuController>().To<MenuController>().FromNewComponentOn(scriptsTransform.gameObject).AsSingle().NonLazy();
            Container.Bind<IGameController>().To<GameController>().FromNewComponentOn(scriptsTransform.gameObject).AsSingle().NonLazy();
        }

        private void BindServices()
        {
            Container.Bind<IObstaclesService>().To<ObstaclesService>().FromNewComponentOn(scriptsTransform.gameObject).AsSingle().NonLazy();
            Container.Bind<IAnimationsService>().To<AnimationsService>().FromNewComponentOn(scriptsTransform.gameObject).AsSingle().NonLazy();
        }
    }
}