
using com.citruslime.lib.audio;
using com.citruslime.game.audio.audioclip;
using UnityEngine;

namespace com.citruslime.lib.audio
{
    public class AbstractAudioManager : IAudioManager
    {
        public void Crossfade(AudioClipEnum fadeInClip, AudioClipEnum fadeOutClip, float crossFadeDuration, float loopDurationInSecondsFadeIn = -1, int loopCountFadeIn = 1)
        {
        
        }

        public void MuteMusic(bool mute)
        {
            
        }

        public void MuteSoundEffects(bool mute)
        {
            
        }

        public void Play(AudioClipEnum audioClip)
        {
            
        }

        public void Play(AudioClipEnum audioClip, Vector3 position)
        {

        }

        public void Play(AudioClipEnum audioClip, int loopCount)
        {
           
        }

        public void Play(AudioClipEnum audioClip, float loopDurationInSeconds)
        {
            
        }

        public void PlaySingleInstance(AudioClipEnum audioClip, bool loop = true)
        {
            
        }

        public void Stop(AudioClipEnum audioClip)
        {
           
        }
    }
}

