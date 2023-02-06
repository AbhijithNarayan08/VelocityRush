
using System.Collections.Generic;
using com.citruslime.lib.audio.model;
using com.citruslime.lib.enums;
using UnityEngine;

namespace com.citruslime.audioSystem
{
    /// <summary>
    /// This class is a model for storing all audio files being played in a layer.
    /// </summary>
    public class AudioLayerModel
    {
        // the volume for all the audio clips in this layer
        public float Volume { get; private set; }
        // the mute state for all the audio clips in this layer
        public bool IsMute { get; private set; }
        // the enum for this layer
        public AudioLayerEnum Layer { get; private set; }
        // dictionary with information about audio clips being played
        private Dictionary <string , List<AudioClipModel>> audioDict;

        public AudioLayerModel (AudioLayerEnum layer)
        {
            audioDict = new Dictionary<string, List<AudioClipModel>>();

            Layer = layer;

            SetVolume (1f);

            Mute(false);
        }

        public void SetVolume (float volume)
        {
            Volume = Mathf.Clamp (volume, 0f, 1f);

            foreach (List<AudioClipModel> audioClips in audioDict.Values)
            {
                for (int i = 0; i < audioClips.Count; i++)
                {
                    audioClips[i].AudioSource.volume = Volume;
                }
            }
        }

        public void AddAudio (AudioClipModel audioClip)
        {
            if (audioClip != null)
            {
                // set the correct volume for this layer
                audioClip.AudioSource.volume = Volume;
                // set the correct mute state for this layer
                audioClip.AudioSource.mute = IsMute;
                // if sounds are already playing, then add it to existing list
                if ( audioDict.ContainsKey (audioClip.Clip) )
                {
                    audioDict [audioClip.Clip].Add (audioClip);
                }
                else
                {
                    // if nothing is playing, then create a new list
                    audioDict.Add (
                            audioClip.Clip, 
                            new List<AudioClipModel>() { audioClip }
                        );
                }
            }
        }

        public void RemoveAudio (AudioClipModel audioClip)
        {
            if (audioClip != null)
            {
                if ( audioDict.ContainsKey (audioClip.Clip) )
                {
                    audioDict [audioClip.Clip].Remove (audioClip);
                }
            }
        }

        public void Mute (bool mute)
        {
            IsMute = mute;

            foreach (List<AudioClipModel> audioClips in audioDict.Values)
            {
                for (int i = 0; i < audioClips.Count; i++)
                {
                    audioClips[i].AudioSource.mute = IsMute;
                }
            }
        }

        public List<AudioClipModel> GetAudioClipsByType (string audioClip)
        {
            if (audioDict.ContainsKey(audioClip))
            {
                return audioDict[audioClip];
            }

            return null;
        }

        public List<AudioClipModel> GetCurrentlyPlayingInstances (string audioClip)
        {
            if ( audioDict.ContainsKey (audioClip) )
            {
                return audioDict [audioClip];
            }

            return null;
        }

	    public List<AudioClipModel> GetAllCurrentlyPlayingInstances()
	    {
			List<AudioClipModel> activeSounds = new List<AudioClipModel>();

		    foreach (var key in audioDict.Keys)
		    {
			    activeSounds.AddRange(audioDict[key]);
		    }

		    return activeSounds;
	    } 

    }

}
