using System.Collections;
using UnityEngine;

namespace Etc
{
    public class GameQuit : MonoBehaviour
    {
        public void QuitGame(float time)
        {
            StartCoroutine(QuitGameTime((time)));
        }

        private IEnumerator QuitGameTime(float time)
        {
            yield return new WaitForSeconds(time);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); 
#endif
        }
    }
}