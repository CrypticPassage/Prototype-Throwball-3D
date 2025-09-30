using Databases;
using Databases.Impls;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(menuName = "Installers/DatabasesInstaller", fileName = "DatabasesInstaller")]
    public class DatabasesInstaller : ScriptableObjectInstaller
    { 
        [SerializeField] private LevelSettingsDatabase levelSettingsDatabase;

        public override void InstallBindings()
        {
            Container.Bind<ILevelSettingsDatabase>().FromInstance(levelSettingsDatabase).AsSingle();
        }
    }
}