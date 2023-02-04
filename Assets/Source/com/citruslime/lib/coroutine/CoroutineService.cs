using System.Collections;
using com.citruslime.lib.coroutine;
using UnityEngine;


namespace com.citruslime.lib.coroutine
{
    /// <summary>
    /// The <see cref="CoroutineService"/> provides the ability to run coroutines if there is no <see cref="MonoBehaviour"/> available
    /// </summary>
    public class CoroutineService : ICoroutineService
    {
        /// <summary>
        /// The <see cref="CoroutineExecuter"/> which is just used to execute <see cref="MonoBehaviour.StartCoroutine(IEnumerator)"/>
        /// </summary>
        private CoroutineExecuter executer = null;

        /// <summary>
        /// Initializes the coroutine service
        /// </summary>
        public CoroutineService()
        {
            GameObject executerGameObject = new GameObject ("CoroutineExecuter");

            executer = executerGameObject.AddComponent<CoroutineExecuter>();
        }

        public void Initialize()
        {
            GameObject.DontDestroyOnLoad(executer);
        }

        /// <summary>
        /// Runs the given <see cref="IEnumerator"/> in a coroutine
        /// </summary>
        /// <param name="ienumerator"></param>
        public Coroutine StartCoroutine (IEnumerator ienumerator)
        {
            return executer.StartCoroutine (ienumerator);
        }

        /// <summary>
        /// Stop the coroutine that has been triggered
        /// </summary>
        /// <param name="coroutine"></param>
        public void StopCoroutine (Coroutine coroutine)
        {
            executer.StopCoroutine (coroutine);
        }

        /// <summary>
        /// Stop the coroutine that has been triggered
        /// </summary>
        /// <param name="coroutine"></param>
        public void StopCoroutine (IEnumerator coroutine)
        {
            executer.StopCoroutine (coroutine);
        }

    }

}
