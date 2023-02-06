using UnityEngine;

namespace com.citruslime.lib.assetmanagement
{
    /// <summary>
    /// Asset supplier writted to be a bad class that loads from resources
    /// Turn this into an addressable system
    /// </summary>
    public class AssetFactory 
    {

        public GameObject Instantiate(string _path)
        {
            GameObject prefab = Resources.Load<GameObject>(_path);
            return GameObject.Instantiate(prefab);
        }

        public AudioClip LoadResourceAsAduioClip(string _path)
        {
            return Resources.Load<AudioClip>(_path);
        }
    }
}