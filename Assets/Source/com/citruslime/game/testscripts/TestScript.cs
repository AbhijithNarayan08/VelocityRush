using com.citruslime.game.audio.audioclip;
using com.citruslime.game.vfx;
using com.citruslime.lib.audio;
using com.citruslime.lib.dependencyHero;
using com.citruslime.lib.ui.manager;
using com.citruslime.lib.ui.vo.dialog;
using com.citruslime.lib.vfxsystem;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update

    private  UiManager uim;
    private Idebug debugInstanceDummy = null;

    private IAudioManager audioManager = null;

    private ParticleSpawnerService particleSpawner = null;


    void Start()
    {

        DependencyInjector.Instance.InjectDependencies(this);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click()
    {
        // uim.Show(
        //     new DefaultDialogViewVO (com.citruslime.game.ui.enums.UiDialogsEnum.TestDialog),
        //     (viewGo)=> 
        //     {
        //         viewGo.GetComponent<TestDialogView>();
        //     }
        // );
         debugInstanceDummy.Log("testing", this.GetType());

        audioManager.PlayAudio(new AudioPlaySignal(AudioClipEnum.SFX_None_TestClip, false, true));
        particleSpawner.SpawnParticleAtPositionInParent(VfxNameEnums.TestParticleVfx , 1 ,new Vector3(0,0,0) ,this.gameObject.transform);
    }

    [Inject]
    public void ConstructorDummy(   
                                    Idebug _debug, 
                                    UiManager _uim,
                                    IAudioManager _aum,
                                    ParticleSpawnerService _pss)
    {
        debugInstanceDummy = _debug;
        uim = _uim;
        audioManager = _aum;
        particleSpawner = _pss;
        
    }
}
