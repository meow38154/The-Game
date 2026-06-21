using System;
using Sound;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

namespace UI
{
    public class SoundSettingToggler : MonoBehaviour
    {
        [SerializeField] private SoundClipSO sound;
        [SerializeField] private GameObject o;

        private void Awake()
        {
            Screen.SetResolution(1920, 1080, true);
        }

        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                EventBus.Publish(SoundEvents.PlaySoundEvent.Init(transform.position, sound));
                o.SetActive(!o.activeSelf);
            }
        }
    }
}