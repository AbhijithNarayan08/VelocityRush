using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace com.citruslime.game.vfx
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ParticleRefData")]
    public class ParticleRefData : ScriptableObject
    {
        [SerializeField] public List<AssetReference> particleReferences = new List<AssetReference>();
        [SerializeField] public List<VfxNameEnums> vfxNameEnums = new List<VfxNameEnums>();
    }
    
}