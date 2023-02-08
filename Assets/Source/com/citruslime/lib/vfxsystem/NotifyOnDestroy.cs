using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
public class NotifyOnDestroy : MonoBehaviour
{
    public event Action<AssetReference, NotifyOnDestroy> Destoryed;
    public AssetReference assetReference{get; set;}

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        Destoryed?.Invoke(assetReference, this);
    }
}
