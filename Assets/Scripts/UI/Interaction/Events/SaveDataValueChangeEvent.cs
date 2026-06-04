using SaveSystems;
using UnityEngine;

namespace UI.Interaction.Events
{
    public class SaveDataValueChangeEvent : AbstractInteractionEvent
    {
        [Header("Field Value Settings")]
        [SerializeField] private string fieldName;
        [SerializeField] private bool fieldValue;
        
        protected override void InvokeEvent()
        {
            SaveDataManager.Instance.SetValue(fieldName, fieldValue);
        }
    }
}