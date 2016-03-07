using UnityEngine;
using System.Collections;

namespace DataUI {
	public class FieldEditorTest : MonoBehaviour {
		public Data data;

		Rect _window;
		FieldEditor _dataEditor;

		void Start() {
			_window = new Rect(10, 10, 300, 400);
			_dataEditor = new FieldEditor(data);
		}
		void OnGUI() {
			_window = GUILayout.Window(0, _window, GUIWindow, "GUI");
		}
		void GUIWindow(int id) {
			GUILayout.BeginVertical();
			_dataEditor.OnGUI();
			GUILayout.EndVertical();
		}

    	[System.Serializable]
    	public class Data {
    		public enum TeamEnum { Alpha, Bravo, Charlie }

    		public int intData;
    		public float floatData;
    		public Vector4 v4Data;
    		public Vector2 v2Data;
    		public Vector3 v3Data;
    		public Color colorData;
    		public TeamEnum team;
    	}
    }
}