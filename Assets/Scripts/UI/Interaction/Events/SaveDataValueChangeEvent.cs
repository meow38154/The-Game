using System;
using SaveSystems;
using UnityEngine;

namespace UI.Interaction.Events
{
    public enum FileType
    {
        No, Out
    }
    
    public class SaveDataValueChangeEvent : AbstractInteractionEvent
    {
        [Header("Field Value Settings")]
        [SerializeField] private FileType fileType = FileType.No;
        [SerializeField] private string fieldName;
        [SerializeField] private bool fieldValue;
        
        protected override void InvokeEvent()
        {
            switch (fileType)
            {
                case FileType.No:
                    SaveDataManager.Instance.SetValue(fieldName, fieldValue);
                    break;
                case FileType.Out:
                    SaveDataManager.Instance.SetOutGameValue(fieldName, fieldValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}