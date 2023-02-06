using System.Collections.Generic;
using UnityEngine;

namespace com.citruslime.lib.audio.util
{
    /// <summary>
    /// This class is meant to hold a pool of Game Objects with audio source component in them which will be used to play sounds
    /// </summary>
    public class AudioSourcePool
    {
        // default start size of this pool
        public int StartSize { get; set; }
        // is the size of this pool fixed or can it grow?
        public bool FixedSize { get; set; }
        // list which holds the Game Objects
        private List<AudioSource> audioSourcePool = null;

        private GameObject poolParent = null;

        /// <summary>
        /// Create an instance of the AudioGOPool and initialize it
        /// </summary>
        /// <param name="startSize">Start size of the pool</param>
        /// <param name="fixedSize">Flag to inidicate if pool size is fixed</param>
        public AudioSourcePool (int startSize, bool fixedSize , GameObject _parent)
        {
            StartSize = startSize;
            FixedSize = fixedSize;
            poolParent = _parent;

            InitPool();
        }

        /// <summary>
        /// Initzialize the pool with a list of emty gameobjects
        /// </summary>
        private void InitPool()
        {
            audioSourcePool = new List<AudioSource>();

            for (int i = 0; i < StartSize; i++)
            {
                audioSourcePool.Add ( CreateAudioObject() );
            }
        }

        /// <summary>
        /// Create a new Audio Game Object with default settings
        /// </summary>
        /// <returns>Return GameObject with AudioSource Component attached</returns>
        private AudioSource CreateAudioObject()
        {
            AudioSource audio = new GameObject("Audio").AddComponent<AudioSource>();
            // loop should always be true, Manager handles actual looping of sounds
            audio.loop = true;
            audio.playOnAwake = false;
            // we don't need any doppler effect
            audio.dopplerLevel = 0f;
            // set the game object as inactive
            audio.gameObject.SetActive (false);
            // make sure that we don't destroy these audio objects between scenes
            GameObject.DontDestroyOnLoad (audio.gameObject);

            audio.gameObject.transform.parent = poolParent.transform ; 

            return audio;
        }

        /// <summary>
        /// Get a free gameobject from the pool
        /// </summary>
        /// <returns>free gameobject from pool</returns>
        public AudioSource GetFreeAudioSource ()
        {
            AudioSource source = null;
            // get the first free / inactive game object from the pool
            for ( int i = 0; i < audioSourcePool.Count; i++ )
            {
                if ( !audioSourcePool[i].gameObject.activeInHierarchy )
                {
                    source = audioSourcePool[i];
                    break;
                }
            }
            // if we did not find a free object and the pool can grow,
            // then create a new game object and add it to the pool
            if ( source == null 
                  && !FixedSize )
            {
                source = CreateAudioObject();
                audioSourcePool.Add (source);
            }

            if (source != null)
            {
                // set it as active before returning
                source.gameObject.SetActive (true);
            }
            // if this pool cannot grow & we did not find a free object, then we might return NULL
            return source;
        }

        /// <summary>
        /// Return the Gameobject to the pool
        /// </summary>
        /// <param name="source">game object to be returned to pool</param>
        public void FreeAudioSource (AudioSource source)
        {
            source.name = "Audio";
            source.gameObject.SetActive (false);
        }

        public int GetPoolSize()
        {
            return audioSourcePool.Count;
        }

    }

}
