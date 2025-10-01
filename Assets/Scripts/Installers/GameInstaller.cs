using Controllers;
using Controllers.Impls;
using Factories;
using Managers;
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
        [SerializeField] private FlyingObject flyingObject;
        [SerializeField] private Obstacle obstacle;
        [SerializeField] private RoadLine roadLine;
        [SerializeField] private GameObject door;
        [SerializeField] private AudioManager audioManager;
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
            BindManagers();
        }

        private void DeclareSignals()
        {
            Container.DeclareSignal<SignalStartGame>();
            Container.DeclareSignal<SignalGameOver>();
            Container.DeclareSignal<SignalObjectsCollision>();
            Container.DeclareSignal<SignalStartTryAnimation>();
            Container.DeclareSignal<SignalButtonHeld>();
        }

        private void BindSignals()
        {
            Container.BindSignal<SignalStartGame>()
                .ToMethod<IGameController>(x => x.OnGameStart).FromResolve();
            Container.BindSignal<SignalObjectsCollision>()
                .ToMethod<IGameController>(x => x.OnObjectsCollision).FromResolve();
            Container.BindSignal<SignalStartTryAnimation>()
                .ToMethod<IGameController>(x => x.OnStartTryAnimation).FromResolve();
            Container.BindSignal<SignalButtonHeld>()
                .ToMethod<IGameController>(x => x.OnGameButtonHeld).FromResolve();
            Container.BindSignal<SignalGameOver>()
                .ToMethod<IMenuController>(x => x.OnGameOver).FromResolve();
        }

        private void BindFactories()
        {
            Container.BindFactory<Obstacle, ObstacleFactory>()
                .FromComponentInNewPrefab(obstacle)
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
            Container.Bind<FlyingObject>().FromInstance(flyingObject).AsSingle().NonLazy();
            Container.Bind<RoadLine>().FromInstance(roadLine).AsSingle().NonLazy();
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
            Container.Bind<IInputService>().To<InputService>().FromNewComponentOn(scriptsTransform.gameObject).AsSingle().NonLazy();
            Container.Bind<IObstaclesService>().To<ObstaclesService>().FromNewComponentOn(scriptsTransform.gameObject).AsSingle().NonLazy();
            Container.Bind<IAnimationsService>().To<AnimationsService>().FromNewComponentOn(scriptsTransform.gameObject).AsSingle().NonLazy();
        }

        private void BindManagers()
        {
            Container.Bind<AudioManager>().FromInstance(audioManager).AsSingle().NonLazy();
        }
    }
}