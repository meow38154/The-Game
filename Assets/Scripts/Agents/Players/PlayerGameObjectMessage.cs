using UnityEngine;

namespace Agents.Players
{
    public struct PlayerGameObjectMessage
    {
        public GameObject player;
        
        public PlayerGameObjectMessage(GameObject playerPlayer)
        {
            player = playerPlayer;
        }
    }
}