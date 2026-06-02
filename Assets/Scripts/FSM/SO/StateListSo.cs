using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace FSM.SO
{
    [CreateAssetMenu(fileName = "state List", menuName = "FSM/StateListSo", order = 1)]
    public class StateListSo : ScriptableObject
    {
        [SerializeField] private string stateListEnumName = "StateType";
        [field: SerializeField] public StateSo[] States { get; private set; }

        [ContextMenu("Enum export")]
        public void EnumExport()
        {
#if UNITY_EDITOR
            const string folderPath = "Assets/Scripts/Enum";

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, $"{stateListEnumName}.cs");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("// Auto Generated");
            sb.AppendLine();
            sb.AppendLine($"public enum {stateListEnumName}");
            sb.AppendLine("{");

            for (int i = 0; i < States.Length; i++)
            {
                if (States[i] == null)
                    continue;

                sb.AppendLine($"    {States[i].StateName} = {i},");
            }

            sb.AppendLine("}");

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);

            AssetDatabase.Refresh();

            Debug.Log($"Enum Export Success : {filePath}");
#endif
        }
    }
}