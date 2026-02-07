using Zenject;

namespace Jam
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameManager>().AsSingle().NonLazy();
        }
    }
}

