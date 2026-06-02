using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FSM.SO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Editor
{
    [CustomEditor(typeof(StateSo))]
    public class StateSOEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset editorView = default;

        private StateSo _targetData;
        
        public override VisualElement CreateInspectorGUI()
        {
            _targetData = target as StateSo;
            
            VisualElement root = new VisualElement();

            editorView.CloneTree(root);

            FillDropdownField(root);
            
            return root;
        }

        private void FillDropdownField(VisualElement root)
        {
            DropdownField field = root.Q<DropdownField>("ClassNameDropdown");
            
            Assembly stateAssembly = Assembly.GetAssembly(typeof(StateSo));
            
            IEnumerable<string> choices = stateAssembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(State)))
                .Select(type => type.FullName);
            
            field.choices.AddRange(choices);
            
            if (_targetData != null && field.choices.Count > 0 && string.IsNullOrEmpty(_targetData.stateClassName))
            {
                _targetData.stateClassName = field.choices.First();
                EditorUtility.SetDirty(_targetData);
            }
            
            AssetDatabase.SaveAssetIfDirty(_targetData);
        }
    }
}