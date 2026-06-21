using UnityEngine;

namespace Agents.Players
{
    public interface IGetPlayer
    {
        GameObject Player { get; set; }
        public void HandleGetPlayer(PlayerGameObjectMessage message);
    }
}