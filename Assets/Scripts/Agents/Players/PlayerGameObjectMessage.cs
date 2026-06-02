using UnityEngine;

namespace Agents.Players
{
    public struct PlayerGameObjectMessage
    {
        public GameObject gameObject;
        
        public PlayerGameObjectMessage(GameObject playerGameObject)
        {
            gameObject = playerGameObject;
        }
    }
}