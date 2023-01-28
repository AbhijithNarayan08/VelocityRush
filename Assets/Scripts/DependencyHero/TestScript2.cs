using System.Collections;
using System.Collections.Generic;
using DependencyHero;
using UnityEngine;

public class TestScript2 : MonoBehaviour
{
    // Start is called before the first frame update
    private Idebug debugInstanceDummy = null;
    void Start()
    {
        DependencyInjector.Instance.InjectDependencies(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click()
    {
        debugInstanceDummy.Log("testing2", this.GetType());
    }

    [Inject(typeof(SuperDebug))]
    public void ConstructorDummy(SuperDebug _debug)
    {
        debugInstanceDummy = _debug;
    }
}
