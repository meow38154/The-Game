using System.Collections.Generic;
using GGMLib.ObjectPool.Runtime;
using UnityEngine;
using Utility;

namespace Sound
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private PoolItemSO soundItem;

//        [field: SerializeField] public EventChannelSO SoundChannel { get; private set; }

        private readonly Dictionary<int, SoundPlayer> _soundPlayerDict = new();

        private void Awake()
        {
            EventBus.Subscribe<PlaySoundEvent>(HandlePlaySoundEvent);
            EventBus.Subscribe<StopSoundEvent>(HandleStopSoundEvent);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<PlaySoundEvent>(HandlePlaySoundEvent);
            EventBus.Unsubscribe<StopSoundEvent>(HandleStopSoundEvent);
        }

        private void HandlePlaySoundEvent(PlaySoundEvent evt)
        {
            SoundPlayer player = poolManager.Pop<SoundPlayer>(soundItem);
            player.transform.position = evt.Position;
            player.PlaySound(evt.ClipData);
            player.OnSoundFinished += HandleSoundFinish;

            if (evt.ChannelNumber > 0 && evt.ClipData.loop)
            {
                if (_soundPlayerDict.TryGetValue(evt.ChannelNumber, out SoundPlayer beforePlayer))
                {
                    beforePlayer.ForceStopSound();
                    poolManager.Push(beforePlayer);
                    _soundPlayerDict.Remove(evt.ChannelNumber);
                }

                _soundPlayerDict.Add(evt.ChannelNumber, player);
            }
            else if (evt.ChannelNumber <= 0 && evt.ClipData.loop)
            {
                Debug.LogWarning(
                    $"Channel must be greater than 0, when the Sound data loop is enabled : {evt.ClipData.name}");
            }
        }

        private void HandleSoundFinish(SoundPlayer player)
        {
            player.OnSoundFinished -= HandleSoundFinish;
            poolManager.Push(player);
        }


        private void HandleStopSoundEvent(StopSoundEvent evt)
        {
            if (!_soundPlayerDict.TryGetValue(evt.ChannelNumber, out SoundPlayer beforePlayer)) return;
            beforePlayer.ForceStopSound();
            beforePlayer.OnSoundFinished -= HandleSoundFinish;
            poolManager.Push(beforePlayer);
            _soundPlayerDict.Remove(evt.ChannelNumber);
        }
    }
}
