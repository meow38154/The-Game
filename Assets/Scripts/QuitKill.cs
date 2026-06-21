using System;
using Agents.Players;
using UnityEngine;

namespace DefaultNamespace
{
    public class QuitKill : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            }
        }
    }
}