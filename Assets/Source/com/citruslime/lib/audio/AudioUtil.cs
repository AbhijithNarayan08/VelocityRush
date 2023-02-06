using System;
using com.citruslime.lib.enums;

namespace com.citruslime.audio
{
    public static class AudioUtil
    {
        /// <summary>
        /// Get the layer that this audio clip will be put in
        /// </summary>
        /// <param name="audioClip">The AudioClipEnum</param>
        /// <returns>The AudioLayerEnum that this clip belongs to</returns>
        public static AudioLayerEnum GetAudioLayer (string audioClip)
        {
            if (audioClip != "")
            {
                // split audio clip enum on '_' to extract information from it
                // format of enum will be "FolderName_FileName" if you change this i'll bite you - Abhijith
                string[] file = audioClip.ToString().Split ('_');

                // return the layer that this audio clip should be played in
                return (AudioLayerEnum) Enum.Parse ( typeof (AudioLayerEnum), file[0] );
            }
            else
            {
                return AudioLayerEnum.None;
            }
        }


        /// <summary>
        /// This function takes a string and tries to convert it into AudioClipEnum
        /// </summary>
        /// <param name="enumAsString">This is </param>
        /// <returns>AudioClipEnum for the string</returns>
        public static string GetAudioClipEnum (string enumAsString)
        {
            string audioClipEnum = "";

            try
            {
                audioClipEnum = enumAsString;
            }
            catch (Exception) { }

            return audioClipEnum;
        }

    }

}
