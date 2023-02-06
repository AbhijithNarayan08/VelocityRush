
using System;
using UnityEngine;
using com.citruslime.lib.audio.model;
using System.Collections.Generic;
using com.citruslime.lib.audio;
using com.citruslime.lib.enums;
using com.citruslime.game.audio.audioclip;
using com.citruslime.lib;
using com.citruslime.lib.assetmanagement;
using com.citruslime.lib.dependencyHero;
using System.Collections;
using com.citruslime.lib.coroutine;
using System.Threading.Tasks;
using com.citruslime.lib.audio.util;
/// <summary>
/// todo change this into a service , not a manager the way that we are using it right now 
/// </summary>
public class AudioManager : AbstractAudioManager 
{

    //flag to indicate if music is already muted can it be a bitflag ? maybe
    private bool musicMuted = false;
        
    // flag to indicate if sound effects is already muted
    private bool effectsMuted = false;

    // AudioModel to store all the sounds being played
    public AudioModel audioModel = null;

    // object pool for audio game objects
    private AudioSourcePool audioSourcePool = null;

    private List<GameObject> layers;

    private GameObject pool;
    // Start is called before the first frame update

    public static AudioManager Instance = null;

    public AudioClipContainers AudioClipContainers = null;

    private AssetFactory assetFactory;

    private CoroutineService coroutineService;

    public  AudioManager() 
    {
        DependencyInjector.Instance.InjectDependencies(this);
        initializeDependencies();
    }

    [Inject]
    private void inject(AssetFactory _assetFactory,CoroutineService _coroutineService)
    {
        assetFactory = _assetFactory;
        coroutineService =_coroutineService;
    }

    private void initializeDependencies()
    {
       audioModel = new AudioModel();
            
        // This is to make things look cute
        pool = new GameObject ("Audio Source Pool");
        // create instance of pool
        audioSourcePool = new AudioSourcePool (10, false,pool);

        GameObject audioServiceContainer = new GameObject ("Audio Service Container");

        pool.transform.parent = audioServiceContainer.transform;

        layers = new List<GameObject>();
        // get a list of all enum values available
        string[] layerNames = Enum.GetNames ( typeof (AudioLayerEnum) );

        // go through the list one by one
        for (int i = 0; i < layerNames.Length; i++)
        {
            // create an instance of audio layer model for each of them
            layers.Add ( new GameObject ( layerNames[i] ) );
            layers[i].transform.parent = audioServiceContainer.transform;
        }

        // make sure that the audio containers are not deleted when loading a new scene
        GameObject.DontDestroyOnLoad (audioServiceContainer);
    }

  

    /// <summary>
    /// This method needs to be checked and re written Raj or Ahisas look into it 
    /// </summary>
    /// <param name="signal"></param>
    public void PlayAudio (AudioPlaySignal signal)
    {
        bool canPlayAudio = true;
        
        if (signal.SingleInstance) 
        {
            // check if there are any instances of this audio playing
            List<AudioClipModel> currentPlayingClips = 
                        audioModel.GetCurrentlyPlayingInstances (signal.AudioClipName);

            if ( currentPlayingClips != null 
                    && currentPlayingClips.Count > 0 ) 
            {
                canPlayAudio = false;
            }
        }

        if (canPlayAudio) 
        {
            if (signal.Position != default)
            {
                Play (signal.AudioClipName, signal.Position);
            }
            else if (signal.LoopCount != 0)
            {
                Play (signal.AudioClipName, signal.LoopCount);
            }
            else if (signal.Loop == false)
            {
                PlaySingleInstance (signal.AudioClipName, signal.Loop);
            }
            else 
            {
                Play (signal.AudioClipName);
            }
        }
    }

    #region Fancy method Overloading
    public new void Play (AudioClipEnum audioClip)
    {
        // && !playerPrefs.IsSoundEffectsOff or easysave.isSoundeffectsOFF something like this in the future okay ? okay - abhijith
        if (audioClip != AudioClipEnum.None)
        {
            PlayAudio ( 
                AudioBuilder.CreateAudioClip (audioClip) 
            );
        }
    }
    public new void Play (AudioClipEnum audioClip, Vector3 position)
    {
        if (audioClip != AudioClipEnum.None)
        {
            AudioClipModel audio = AudioBuilder.CreateAudioClip (audioClip);

            audio.Position = position;

            PlayAudio (audio);
        }
    }

    public new void Play (AudioClipEnum audioClip, int loopCount)
    {
       if (audioClip != AudioClipEnum.None)
        {
            AudioClipModel audio = AudioBuilder.CreateAudioClip (audioClip);

            audio.LoopCount = loopCount;

            PlayAudio (audio);
        }
    }

    public new void Play (AudioClipEnum audioClip, float loopDurationInSeconds)
    {
        if (audioClip != AudioClipEnum.None)
        {
            AudioClipModel audio = AudioBuilder.CreateAudioClip (audioClip);

            audio.LoopDurationInSeconds = loopDurationInSeconds;

            PlayAudio (audio);
        }
    }

    public new void PlaySingleInstance (AudioClipEnum audioClip, bool loop = true)
    {
      if (audioClip != AudioClipEnum.None)
        {

             AudioClipModel audio = AudioBuilder.CreateAudioClip (audioClip);

           // audio.LoopDurationInSeconds = loopDurationInSeconds;

            PlayAudio (audio);
        //    // Stop (audioClip);
        //     Play (audioClip, (loop ? int.MaxValue : 1) );
        }
    }

    #endregion

    private  void PlayAudio (AudioClipModel audioClip)
    {
        if (audioClip != null)
        {
            Debug.Log("trying to play" + audioClip);
            SetupAudioSourceAndPlay (audioClip);
        }
    }


    public new void Stop (AudioClipEnum audioClip)
    {
        if (audioClip != AudioClipEnum.None)
        {
            List<AudioClipModel> currentPlayingClips = 
                        audioModel.GetCurrentlyPlayingInstances (audioClip);

            if (currentPlayingClips != null)
            {
                for (int i = 0; i < currentPlayingClips.Count; i++)
                {
                    CleanupAudio ( currentPlayingClips[i] );
                }
            }
        }
    }

    // public new void Crossfade (
    //                 string fadeInClip,
    //                 string fadeOutClip,
    //                 float crossFadeDuration,
    //                 float loopDurationInSecondsFadeIn = -1,
    //                 int loopCountFadeIn = 1
    //             )
    // {
    //     StartCoroutine ( FadeOutCoroutine (fadeOutClip, crossFadeDuration) );

    //     StartCoroutine ( 
    //                 FadeInCoroutine (
    //                         fadeInClip,
    //                         crossFadeDuration,
    //                         loopDurationInSecondsFadeIn,
    //                         loopCountFadeIn
    //                     ) 
    //             );

    // }

    public new void MuteMusic (bool mute)
    {
        if ( musicMuted != mute )
        {
            musicMuted = mute;

            audioModel.MuteLayer (AudioLayerEnum.Music, musicMuted);
        }
    }

    public new void MuteSoundEffects (bool mute)
    {
        if (effectsMuted != mute)
        {
            effectsMuted = mute;

            audioModel.MuteLayer (AudioLayerEnum.SFX, effectsMuted);
        }
    }

    private void SetupAudioSourceAndPlay (AudioClipModel audioClip)
    {
        // get free audio source from the pool
        audioClip.AudioSource = audioSourcePool.GetFreeAudioSource();
        // check if correct audio source clip is loaded
        if ( audioClip.AudioSource != null 
                && ( !audioClip.AudioSource.clip 
                        || audioClip.AudioSource.clip.name != audioClip.Clip.ToString() ) )
        {
            // configure if this audio clip should loop or not
            audioClip.AudioSource.loop = (audioClip.LoopCount > 1 || audioClip.LoopDurationInSeconds > 0);
            // construct the path from where to read the audio file based on the layer it is in sfx will be in sfx folder and music in music
            // Also please remind me to remove pokemon theme song ive put as a test music so that no one sues my broke ass -Abhijith
            // Manke an audioLayerEnum for VoiceOver it should pretty much behave like sfx but instead of pooling just make it go boom boom , Raj please look into it 
            string path = "";
            
            if(audioClip.Layer == AudioLayerEnum.Music)
            {
                path = "Audio/Music/" + audioClip.Filename;
            }

            if(audioClip.Layer == AudioLayerEnum.SFX)
            {
                path = "Audio/SFX/" + audioClip.Filename;
            }
            
            // if there are variations for this sound, then randomly select one 
            if (audioClip.Variations > 1)
            {
                path += "_" + new System.Random().Next (1, audioClip.Variations + 1).ToString().PadLeft (2, '0');
            }
            
            AudioClip audioObject = assetFactory.LoadResourceAsAduioClip(path);
            
            Debug.Log("path is at " + path +audioClip.Layer.ToString() + " and audioObject is " + audioObject+ " and audioClip is " + audioClip);

            AudioClip clip = audioObject;

            // assign the loaded audio clip
            audioClip.AudioSource.clip = clip;
            
            if (audioClip != null){
                GameObject.Instantiate(audioObject);

            }
            
            audioClip.AudioSource.gameObject.transform.parent = 
                                        layers[(int)audioClip.Layer].transform;

            //audioClip.Coroutine = coroutineService.StartCoroutine(PlayAudioCoroutine (audioClip));

            audioClip.Task = PlayAudioAsync(audioClip);


        

           
        }
    }
    /// <summary>
    /// This method is used as a co-routine to play audio. It allows control of how many times given audio will loop before being stopped.
    /// NOTE: moving away to a Async Task for better performance
    /// </summary>
    /// <param name="audio">The audio clip to be played</param>
    /// <returns>Reference to IEnumerator</returns>
    private IEnumerator PlayAudioCoroutine (AudioClipModel audio)
    {
        if (audio.AudioSource != null)
        {
            // set the clip in the correct layer
            audioModel.Add (audio);
            // play the sound
            audio.AudioSource.Play();
            UnityEngine.Debug.Log("trying to play audio"+audio.ToString() + "of volume ");
            // wait for required number of loops to complete while playing sound
            yield return new WaitForSeconds ( audio.GetLoopDurationInSeconds() );
            // clean up the audio model once we are done playing
            CleanupAudio (audio);
        }
        else
        {
            UnityEngine.Debug.LogError ( "AudioSource is NULL for : " + audio.ToString() );
        }
    }


    /// <summary>
    /// This method is used as a async task to play audio. It allows control of how many times given audio will loop before being stopped.
    ///  moving away to a Async Task for better performance
    /// </summary>
    /// <param name="audio">The audio clip to be played</param>
    /// <await>loop duration of the audio</await>

    private async Task PlayAudioAsync(AudioClipModel audio)
    {
        if (audio.AudioSource != null)
        {
            audioModel.Add(audio);
            audio.AudioSource.Play();
            UnityEngine.Debug.Log("trying to play audio" + audio.ToString() + "of volume ");
            await Task.Delay((int)(audio.GetLoopDurationInSeconds() * 1000));
            CleanupAudio(audio);
        }
        else
        {
            UnityEngine.Debug.LogError("AudioSource is NULL for : " + audio.ToString());
        }
    }


    /// <summary>
    /// Clean up the audio model and free up used resources
    /// </summary>
    /// <param name="audio">The audio model to be cleaned up</param>
    public void CleanupAudio (AudioClipModel audio)
    {
        if (audio.AudioSource != null)
        {
            // done playing the expected number of loops, now stop the sound
            audio.AudioSource.Stop();
            // remove this audio clip from the layer
            audioModel.Remove (audio);
            // return the audio source to the pool
            audioSourcePool.FreeAudioSource (audio.AudioSource);
            // return it to the pool in the hierarchy, 
            // this is for Visual Debugging only
            audio.AudioSource.gameObject.transform.parent = pool.transform;
            // remove the reference
            audio.AudioSource = null;
            // Now Stop the co-routine
           // StopCoroutine (audio.Coroutine);
            // TODO: More Cleanup is possible here to mark audio clip model for GC
            //https://www.youtube.com/watch?v=5Fks2NArDc0 refer this maybe -Abhijith
        }
    }
    // private IEnumerator FadeInCoroutine (
    //                                 string fadeInClip,
    //                                 float crossFadeDuration,
    //                                 float loopDurationInSecondsFadeIn = -1,
    //                                 int loopCountFadeIn = 1
    //                             )
    // {
    //     if (fadeInClip != "")
    //     {
    //         // create audio clip model for the audio to be faded in
    //         AudioClipModel fadeInAudio = AudioBuilder.CreateAudioClip (fadeInClip);
    //         // the target volume to set the sound to, this is the volume of the layer which the sound goes in
    //         float targetVolume = audioModel.GetAudioLayerModel (fadeInClip).Volume;
    //         // the rate of change of volume for the fade in, depends on how soon we want to fade it in
    //         float volumeDelta = targetVolume / crossFadeDuration;
    //         // setup the sound before playing it
    //         fadeInAudio.LoopCount = loopCountFadeIn;
    //         fadeInAudio.LoopDurationInSeconds = loopDurationInSecondsFadeIn;
    //         // play the sound
    //         PlayAudio (fadeInAudio);
    //         // set volume to 0 but set it after playing the audio
    //         fadeInAudio.AudioSource.volume = 0;

    //         if (fadeInAudio.AudioSource != null)
    //         {
    //             // while we have not yet reached our target volume
    //             while ( fadeInAudio.AudioSource != null  
    //                         && fadeInAudio.AudioSource.volume < targetVolume )
    //             {
    //                 // modify the volume by required delta
    //                 fadeInAudio.AudioSource.volume += volumeDelta;
    //                 // yield till the next fixed update to increase volume
    //                 Debug.Log("Increasing Volume");

    //                 yield return new WaitForFixedUpdate();
    //             }
    //         }
    //     }
    // }
    
    
    /// <summary>
    /// use this for fading out audio
    /// make sure to add a cleanup function in this to remove the instance from the pool 
    /// </summary>
    // public  IEnumerator StartFadeOut(string fadeOutClip, float duration, float targetVolume)
    // {   
    //     List<AudioClipModel> currentPlayingClips =
    //                         audioModel.GetCurrentlyPlayingInstances (fadeOutClip);
    //     AudioClipModel fadeOutAudio = currentPlayingClips[currentPlayingClips.Count - 1];

    //     float currentTime = 0;
    //     float start = fadeOutAudio.AudioSource.volume;



    //     while (currentTime < duration)
    //     {
    //         currentTime += Time.deltaTime;
    //         fadeOutAudio.AudioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
    //         yield return null;
    //     }
    //     CleanupAudio(fadeOutAudio);
    //     yield break;
    // }

    public void SetVolume(AudioClipEnum audioClip, float targetVolume)
    {   
        List<AudioClipModel> currentPlayingClips = audioModel.GetCurrentlyPlayingInstances (audioClip);
        AudioClipModel audio = currentPlayingClips[currentPlayingClips.Count - 1];

        audio.AudioSource.volume = targetVolume;
    }

    // private IEnumerator FadeOutCoroutine (string fadeOutClip, float crossFadeDuration)
    // {
    //     if (fadeOutClip !="")
    //     {
    //         List<AudioClipModel> currentPlayingClips =
    //                         audioModel.GetCurrentlyPlayingInstances (fadeOutClip);

    //         if (currentPlayingClips != null
    //                 && currentPlayingClips.Count > 0)
    //         {
    //             // get reference to an instance of the playing sound, get reference to last playing instance
    //             AudioClipModel fadeOutAudio = currentPlayingClips[currentPlayingClips.Count - 1];
    //             // the target volume to set the sound to, this is the volume of the layer which the sound goes in
    //             float targetVolume = 0;
    //             // the rate of change of volume for the fade in, depends on how soon we want to fade it in
    //             float volumeDelta = (targetVolume - fadeOutAudio.AudioSource.volume) / crossFadeDuration;

    //             if (fadeOutAudio.AudioSource != null)
    //             {
    //                 // while we have not yet reached our target volume
    //                 while ( fadeOutAudio.AudioSource != null 
    //                             && fadeOutAudio.AudioSource.volume > targetVolume )
    //                 {
    //                     // modify the volume by required delta
    //                     fadeOutAudio.AudioSource.volume += volumeDelta;
    //                     // yield till the next fixed update to increase volume
    //                     yield return new WaitForFixedUpdate();
    //                 }
    //                 // the fade out of the audio is done, now clean it up please
    //                 Debug.Log("fadeout is done");
    //             }
    //         }
    //     }
    // }   
}
