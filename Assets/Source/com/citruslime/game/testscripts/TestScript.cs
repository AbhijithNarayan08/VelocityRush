using com.citruslime.game.audio.audioclip;
using com.citruslime.lib.audio;
using com.citruslime.lib.dependencyHero;
using com.citruslime.lib.ui.manager;
using com.citruslime.lib.ui.vo.dialog;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update

    private  UiManager uim;
    private Idebug debugInstanceDummy = null;

    private AudioManager audioManager = null;


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
        uim.Show(
            new DefaultDialogViewVO (com.citruslime.game.ui.enums.UiDialogsEnum.TestDialog),
            (viewGo)=> 
            {
                viewGo.GetComponent<TestDialogView>();
            }
        );
        debugInstanceDummy.Log("testing", this.GetType());

        audioManager.PlayAudio(new AudioPlaySignal(AudioClipEnum.SFX_None_TestClip, false, true));
    }

    [Inject]
    public void ConstructorDummy(SuperDebug _debug, UiManager _uim, AudioManager _aum)
    {
        debugInstanceDummy = _debug;
        uim = _uim;
        audioManager = _aum;
        
    }
}
