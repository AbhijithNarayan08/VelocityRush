
using com.citruslime.game.audio.audioclip;
using UnityEngine;


namespace com.citruslime.lib.audio
{
    public class AudioPlaySignal 
    {
        /// <summary>
        /// Should this audio loop ? 
        /// </summary>
        public bool Loop { get; private set; }
        
        /// <summary>
        /// How many times should it loop ?
        /// </summary>
        public int LoopCount { get; private set; }
        
        /// <summary>
        /// Position of audio for 3D sound
        /// </summary>
        public Vector3 Position { get; private set; }

        /// <summary>
        /// Should only one instance of this audio play ?
        /// </summary>
        public bool SingleInstance { get; private set; }
        
        /// <summary>
        /// Which audio should be played ?
        /// </summary>
        public AudioClipEnum AudioClipName { get; private set; }
        
        /// <summary>
        /// How long should audio loop ? - These are the questions you need to be asking the designer not me 
        /// </summary>
        public float LoopDurationInSeconds { get; private set; }

        public AudioPlaySignal (
                    AudioClipEnum audioClipName,
                    bool loop = true,
                    bool singleInstance = true,
                    int loopCount = 0,
                    float loopDurationInSeconds = 0,
                    Vector3 position = default
                )
        {
            Loop = loop;
            Position = position;
            LoopCount = loopCount;
            AudioClipName = audioClipName;
            SingleInstance = singleInstance;
            LoopDurationInSeconds = loopDurationInSeconds;
        }

    }
    
}
