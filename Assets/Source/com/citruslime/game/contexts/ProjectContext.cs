using UnityEngine;
using com.citruslime.lib.dependencyHero;
using com.citruslime.lib.ui.manager;
using com.citruslime.lib.assetmanagement;
using com.citruslime.lib.coroutine;
using com.citruslime.lib.vfxsystem;

namespace com.citruslime.game.context
{
    
    public class ProjectContext : MonoBehaviour
    {
        private void Awake()
        {
            register();
            DontDestroyOnLoad(this.gameObject);
        }
    
    
        // Register all project instances here, lol ive to make sure its in order kill me pls
        private void register()
        {
            DependencyInjector.Instance.Register(typeof(CoroutineService));
            DependencyInjector.Instance.Register(typeof(SuperDebug));
            DependencyInjector.Instance.Register(typeof(AssetFactory));
            DependencyInjector.Instance.Register(typeof(UiManager));
            DependencyInjector.Instance.Register(typeof(AudioManager));
            DependencyInjector.Instance.Register(typeof(ParticleSpawnerService));
        }
    }
    
}
