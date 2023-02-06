using com.citruslime.lib.serializabledictionary;
using UnityEngine;



namespace com.citruslime.lib.audio
{
    [CreateAssetMenu(fileName = "AudioClipContainer")]

    public class AudioClipContainers : ScriptableObject
    {  
        [SerializeField]
       public AudioClipMapping audioClips;
    }        
}
