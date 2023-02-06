using UnityEngine;
using System.Collections;

namespace com.citruslime.lib.coroutine
{
    public interface ICoroutineService
    {
        Coroutine StartCoroutine (IEnumerator ienumerator);

        void StopCoroutine (Coroutine coroutine);

        void StopCoroutine (IEnumerator coroutine);
    }
}
