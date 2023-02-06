using System;
using System.Collections;
using System.Threading.Tasks;
using com.citruslime.game.audio.audioclip;
using com.citruslime.lib.enums;
using UnityEngine;

namespace com.citruslime.lib.audio.model
{
    /// <summary>
    /// Class to hold data related to current audio clip being played
    /// </summary>
    public class AudioClipModel
    {
        // audio source being used to play this sound
        private AudioSource _audioSource = null;

        // How many times should this audio clip loop?
        public int LoopCount { get; set; }

        // For how long (duration in seconds) should this sound loop?
        public float LoopDurationInSeconds { get; set; }

        // are there variations of this audio clip
        public int Variations { get; set; }
        
        // name of the audio clip being played
        public string Filename { get; private set; }
        
        // spatial blend value, used to control if sound is 3D or 2D
        public float SpatialBlend { get; set; }
        
        // position of sound in 3D space
        public Vector3 Position { get; set; }

        // logical group that this sound belongs to
        public String Group { get; private set; }

        // reference to co-routine running this sound
        public Coroutine Coroutine { get; set; }
        

        // reference to co-routine running this sound
        public Task Task { get; set; }

        // audio clip enum for this object
        public string Clip { get;  set; }

        // audio layer enum for this object
        public AudioLayerEnum Layer { get; private set; }
        
        // roll off mode to be used for this object
        public AudioRolloffMode RolloffMode { get; set; }
        
        // audio source being used to play this sound
        public AudioSource AudioSource
        {
            get { return _audioSource; }
            set
            {
                // set the audio source
                _audioSource = value;
                // copy some important values to the audio source
                if (_audioSource != null)
                {
                    _audioSource.name               = Filename;
                    _audioSource.rolloffMode        = RolloffMode;
                    _audioSource.spatialBlend       = SpatialBlend;
                    _audioSource.transform.position = Position;
                }
            }
        }

        public AudioClipModel (AudioClipEnum audioClip)
        {
            if (audioClip != AudioClipEnum.None)
            {
                // split audio clip enum on '_' to extract information from it
                // format of enum will be "Layer_Group_FileName"
                string[] enumSplit = audioClip.ToString().Split ('_');

                if ( enumSplit != null 
                     && enumSplit.Length >= 2 )
                {
                    // the enum of current audio clip being played
                    Clip = audioClip.ToString();
                    // the logical group that this audio belongs to
                    Group = enumSplit[1];
                    // get filename of audio being played and store it
                    Filename = enumSplit[2];
                    // set the layer that this audio clip should be played in
                    Layer = (AudioLayerEnum) Enum.Parse ( typeof (AudioLayerEnum), enumSplit[0] );
                    // setting LoopDuration to a default value to indicate that it should be calculated later
                    LoopDurationInSeconds = -1;
                }
            }
        }

        public bool IsPlaying()
        {
            return AudioSource && AudioSource.isPlaying;
        }

        public float GetLoopDurationInSeconds()
        {
            // have not set the loop duration, so calculate it for ourselves
            if (LoopDurationInSeconds <= -1)
            {
                if (AudioSource != null)
                {
                     LoopDurationInSeconds = AudioSource.clip.length * LoopCount;
                }
            }

            return LoopDurationInSeconds;
        }

        public override string ToString()
        {
            return "File: " + Filename + " | Variations: " + Variations;
        }

    }

}
