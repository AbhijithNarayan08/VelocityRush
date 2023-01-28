using System;
using DependencyHero;
using UnityEngine;

public class ProjectContext : MonoBehaviour
{
    private void Awake()
    {
        
        
        register();
       
        DontDestroyOnLoad(this.gameObject);
    }


    // Register all project instances here
    internal void register()
    {
        DependencyInjector.Instance.Register(new SuperDebug());
    }

   
}

