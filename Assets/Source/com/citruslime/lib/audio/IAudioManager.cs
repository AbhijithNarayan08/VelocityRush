using UnityEngine;
using com.citruslime.game.audio.audioclip;

namespace com.citruslime.lib.audio
{
    /// <summary>
    /// Interface of the Audio Manager that should be implemented by the concrete class
    /// </summary>
    public interface IAudioManager
    {
        /// <summary>
        /// start playing specified sound with default settings
        /// </summary>
        /// <param name="audioClip">enum of audio clip for playing</param>
        void Play (AudioClipEnum audioClip);
        
        /// <summary>
        /// start playing current sound with default settings and a adjustable position for 3D sounds
        /// </summary>
        /// <param name="audioClip">the current audioclip for playing</param>
        /// <param name="position">the position for the audio object for 3D sounds</param>
        void Play (AudioClipEnum audioClip, Vector3 position);

        /// <summary>
        /// start playing specified sound for the number of loops
        /// </summary>
        /// <param name="audioClip">enum of audio clip for playing</param>
        /// <param name="loopCount">Number of times sound should loop</param>
        void Play (AudioClipEnum audioClip, int loopCount);

        /// <summary>
        /// start playing specified sound and loop it for the specified duration of time
        /// </summary>
        /// <param name="audioClip">enum of audio clip for playing</param>
        /// <param name="loopDurationInSeconds">For how many seconds should sound loop</param>
        void Play (AudioClipEnum audioClip, float loopDurationInSeconds);

        /// <summary>
        /// Play only ONE, and ONLY ONE instance of this audio clip, before playing 
        /// a new instance, all other playing instances of this clip will be stopped
        /// </summary>
        /// <param name="audioClip">The audio clip to be played</param>
        /// <param name="loop">Should this audio clip be looped</param>
        void PlaySingleInstance (AudioClipEnum audioClip, bool loop = true);

        /// <summary>
        /// Stop playing all instances of the specified clip
        /// </summary>
        /// <param name="audioClip">clip to stop playing</param>
        void Stop (AudioClipEnum audioClip);


        void PlayAudio (AudioPlaySignal signal);
        
        /// <summary>
        /// Function to Cross Fade sounds. It can also be used just to 
        /// fade in a sound by passing 'None' as the sound to fade out.
        /// </summary>
        /// <param name="fadeInClip">The clip to be faded in</param>
        /// <param name="fadeOutClip">the clip to be faded out</param>
        /// <param name="loopCountFadeIn">How many times should this sound loop after fade in</param>
        /// <param name="crossFadeDuration">Duration of cross fade</param>
        /// <param name="loopDurationInSecondsFadeIn">How long should this sound loop after fade in</param>
        void Crossfade (
                    AudioClipEnum fadeInClip,
                    AudioClipEnum fadeOutClip,
                    float crossFadeDuration,
                    float loopDurationInSecondsFadeIn = -1,
                    int loopCountFadeIn = 1
                );
        
        /// <summary>
        /// Mute all music currently being played by the AudioManager
        /// </summary>
        /// <param name="mute">true: music on, false: music off</param>
        void MuteMusic (bool mute);
        
        /// <summary>
        /// Mute all sound effects currently being played by the AudioManager
        /// </summary>
        /// <param name="mute">true: sound effects on, false: sound effects off</param>
        void MuteSoundEffects (bool mute);
        
    }

}
