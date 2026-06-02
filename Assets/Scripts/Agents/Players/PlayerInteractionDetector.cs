using System;
using GGMLib.ModuleSystem;
using UI;
using UI.Interaction;
using Unity.VisualScripting;
using UnityEngine;

namespace Agents.Players
{
    public class PlayerInteractionDetector : MonoBehaviour, IModule
    {
        private ModuleOwner _owner;
        private InteractionTrigger _currentEnableInteractionTrigger;

        private bool _visible = false;
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
        }
        

        private void OnTriggerStay(Collider other)
        {
            if (_visible || !other.CompareTag("UI") || !other.TryGetComponent(out InteractionTrigger trigger)) return;
            
            _currentEnableInteractionTrigger = trigger;
            _currentEnableInteractionTrigger.SetDialogueVisible(true);
            _visible = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (_currentEnableInteractionTrigger == null) return;
            
            _currentEnableInteractionTrigger.SetDialogueVisible(false);
            _currentEnableInteractionTrigger = null;
            _visible = false;
        }
    }
}