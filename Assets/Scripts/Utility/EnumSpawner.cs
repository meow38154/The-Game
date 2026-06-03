using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Directory = UnityEngine.Windows.Directory;

namespace Utility
{
    public class EnumSpawner
    {
        public static void EnumSpawn(string fileName, string[] enums)
        {
#if UNITY_EDITOR
            const string folderPath = "Assets/Scripts/Enum";

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, $"{fileName}.cs");

            StringBuilder sb = new StringBuilder();
            
            sb.AppendLine();
            sb.AppendLine($"public enum {fileName}");
            sb.AppendLine("{");

            for (int i = 0; i < enums.Length; i++)
            {
                if (enums[i] == null)
                    continue;

                sb.AppendLine($"    {enums[i]} = {i},");
            }

            sb.AppendLine("}");

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);

            AssetDatabase.Refresh();

            Debug.Log($"Enum Export Success : {filePath}");
#endif
        }
    }
}