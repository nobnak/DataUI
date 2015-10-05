using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace Serializer {
	public static class ScriptableObjectUtil {

		public static void Create<T>(string path) where T : ScriptableObject {
			var data = ScriptableObject.CreateInstance<T>();
			AssetDatabase.CreateAsset(data, path);
			AssetDatabase.SaveAssets();
			Selection.activeObject = data;
		}

		public static void Create<T>() where T : ScriptableObject {
			var path = AssetDatabase.GetAssetPath(Selection.activeObject);
			if (string.IsNullOrEmpty(path))
				path = "Assets";
			if (!Directory.Exists(path))
				path = path.Substring(0, path.LastIndexOf("/"));
			path = AssetDatabase.GenerateUniqueAssetPath(path + "/" + typeof(T).ToString().Replace("+","_") + ".asset");
			Debug.LogFormat("Scriptable Object Path : {0}", path);
			Create<T>(path);
		}
	}
}