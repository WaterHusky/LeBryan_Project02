using UnityEngine;
using UnityEditor;
using System.IO;

public static class ScriptableObjectUtility
{
    public static void CreateAsset<T>() where T : ScriptableObject
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (path != "" && Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        CreateAssetAtPath<T>(path);
    }

    public static void CreateAssetAtPath<T>(string path, string filename = "") where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        if (path == "")
        {
            path = "Assets";
        }
        else if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        if (filename == "")
        {
            filename = "New " + typeof(T).ToString() + ".asset";
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + filename);

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}