using System;
using System.Collections.Generic;
using com.citruslime.audio;
using com.citruslime.audioSystem;
using com.citruslime.game.audio.audioclip;
using com.citruslime.lib.enums;

namespace com.citruslime.lib.audio.model
{
    public class AudioModel
    {
        // List of layers that we will play sounds in
        private List<AudioLayerModel> layers = null;

        public AudioModel()
        {
            // get the list of layers in which we will store our model
            layers = CreateAudioLayerList();
        }

        public void Add (AudioClipModel audio)
        {
            // set the clip in the correct layer
            layers [ (int) audio.Layer ].AddAudio (audio);
        }

        public void Remove (AudioClipModel audio)
        {
            // remove this audio clip from the layer
            layers [ (int) audio.Layer ].RemoveAudio (audio);
        }

        public void MuteLayer (AudioLayerEnum layer, bool mute)
        {
            layers [ (int) layer ].Mute (mute);
        }

        public AudioLayerModel GetAudioLayerModel (AudioClipEnum audioClip)
        {
            return layers [ (int) AudioUtil.GetAudioLayer ( Enum.GetName(typeof(AudioClipEnum), audioClip)) ]; //ive heard doing this is faster than .toString()
        }

	    public List<AudioClipModel> GetAllCurrentlyPlayingInstances()
	    {
		    List<AudioClipModel> activeSounds = new List<AudioClipModel>();

		    for (int i = 0; i < layers.Count; i++)
		    {
			    activeSounds.AddRange(layers[i].GetAllCurrentlyPlayingInstances());
		    }

		    return activeSounds;
	    }

        public List<AudioClipModel> GetCurrentlyPlayingInstances (AudioClipEnum audioClip)
        {
            string audioClipName = Enum.GetName(typeof(AudioClipEnum), audioClip);
            // get the layer that this audio clip should be played in
            AudioLayerEnum layer = AudioUtil.GetAudioLayer (audioClipName);
            // all cases are correctly handled here
            return layers [ (int) layer ].GetCurrentlyPlayingInstances (audioClipName);
        } 

        private List<AudioLayerModel> CreateAudioLayerList()
        {
            List<AudioLayerModel> layers = new List<AudioLayerModel>();
            // get a list of all layer enums available
            string[] layerNames = Enum.GetNames ( typeof (AudioLayerEnum) );
            // go through the list one by one
            for (int i = 0; i < layerNames.Length; i++)
            {
                // create an instance of audio layer model for each of them
                layers.Add ( 
                        AudioBuilder.CreateAudioLayerModel ( (AudioLayerEnum)i ) 
                    );
            }

            return layers;
        }

    }

}
