using Databases;
using Databases.Impls;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(menuName = "Installers/DatabasesInstaller", fileName = "DatabasesInstaller")]
    public class DatabasesInstaller : ScriptableObjectInstaller
    { 
        [SerializeField] private GameSettingsDatabase gameSettingsDatabase;

        public override void InstallBindings()
        {
            Container.Bind<IGameSettingsDatabase>().FromInstance(gameSettingsDatabase).AsSingle();
        }
    }
}