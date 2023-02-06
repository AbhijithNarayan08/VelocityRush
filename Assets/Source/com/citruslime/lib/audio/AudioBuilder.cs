

using com.citruslime.audioSystem;
using com.citruslime.game.audio.audioclip;
using com.citruslime.lib.audio.model;
using com.citruslime.lib.enums;

namespace com.citruslime.lib
{
    // TODO -  create IAudioBuilder interface and connect it through bridge pattern (something that I learnt today) lool -Abhijith
    /// <summary>
    /// Builder to help construct the AudioClipModel based on value of AudioClipEnum
    /// </summary>
    public class AudioBuilder
    {
        public static AudioLayerModel layerModel = null;

        public static AudioLayerModel MusiclayerModel = null;

        public static AudioLayerModel SfxLayerModel = null;
        public static AudioClipModel CreateAudioClip (AudioClipEnum clip)
        {
            AudioClipModel audioClip = null;

            if ( clip != AudioClipEnum.None )
            {
                audioClip = new AudioClipModel (clip);

                // get the count of number of variations of this sound available
               // audioClip.Variations = AudioClipCounts.ClipCount [ (int) clip ];

                switch (audioClip.Layer)
                {
                    case AudioLayerEnum.Music:
                        // All music should loop infinitely
                        audioClip.LoopCount = int.MaxValue;
                        // ambient sounds are 2D, Not affected by position of listner
                        audioClip.SpatialBlend = 0f;
                        break;

                    case AudioLayerEnum.SFX:
                        // combat sounds need not loop
                        audioClip.LoopCount = 1;
                        // combat sounds can be 3D, affected by position of listner, or whatever you want essentially says
                        // but curretly set to 2D
                        audioClip.SpatialBlend = 0f;
                        break;
                }
            }

            return audioClip;
        }

        public static AudioLayerModel CreateAudioLayerModel (AudioLayerEnum layer)
        {
            layerModel = new AudioLayerModel (layer);

            switch (layerModel.Layer)
            {
                case AudioLayerEnum.Music:
                    // Set the volume of the sounds on this layer
                    MusiclayerModel = layerModel;
                    layerModel.SetVolume (0.5f);
                    break;

                case AudioLayerEnum.SFX:
                    // Set the volume of the sounds on this layer
                    SfxLayerModel = layerModel;
                    layerModel.SetVolume (0.5f);
                    break;
            }
            return layerModel;
        }

    }

}
