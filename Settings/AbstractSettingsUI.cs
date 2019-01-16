using UnityEngine;
using System.Collections;
using System.IO;
using DataUI;
using nobnak.Gist.DataUI;
using nobnak.Gist.IMGUI.Scope;

namespace DataUI.Settings {

    public abstract class AbstractSettingsUI : MonoBehaviour {
        public SettingsCore core;


        [System.Serializable]
        public class SettingsCore {
            public const float WINDOW_WIDTH = 300f;
            public enum ModeEnum { Normal = 0, GUI = 1, End = 2 }

            public UnityEngine.Events.UnityEvent OnDataChange;

            public ModeEnum mode;
            public KeyCode toggleKey = KeyCode.None;

            public enum PathTypeEnum { StreamingAssets = 0, MyDocuments }

            public PathTypeEnum pathType;
            public string dataPath;

            #region Behaviour Wrapper
            FieldEditor _dataEditor;
            Rect _window;

            public virtual void OnEnable<T>(T data) {
				Load(data);
				RebuildGUIFrom(data);
				NotifyOnDataChange();
			}

			public virtual void Update<T>(T data) {
                if (Input.GetKeyDown (toggleKey)) {
                    mode = (ModeEnum)(((int)mode + 1) % (int)ModeEnum.End);
                    if (mode == ModeEnum.Normal)
                        Save (data);
                }
            }
            public virtual void OnGUI(MonoBehaviour b) {
                if (mode == ModeEnum.GUI)
                    _window = GUILayout.Window (GetHashCode(), _window, Window, b.name, GUILayout.MinWidth (WINDOW_WIDTH));
            }
			#endregion

			#region interface
			public void RebuildGUIFrom<T>(T data) {
				_dataEditor = new FieldEditor(data);
			}
			#region Save/Load
			public virtual void Load<T>(T data) {
				string path;
				if (!DataPath(out path))
					return;

				try {
					if (File.Exists(path))
						JsonUtility.FromJsonOverwrite(File.ReadAllText(path), data);
				} catch (System.Exception e) {
					Debug.Log(e);
				}
			}
			public virtual void Save<T>(T data) {
				string path;
				if (!DataPath(out path))
					return;

				try {
					File.WriteAllText(path, JsonUtility.ToJson(data, true));
				} catch (System.Exception e) {
					Debug.Log(e);
				}
			}
			public virtual bool DataPath(out string path) {
				var dir = Application.streamingAssetsPath;
				switch (pathType) {
					case PathTypeEnum.MyDocuments:
						dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
						break;
				}
				path = Path.Combine(dir, dataPath);
				return !string.IsNullOrEmpty(dataPath);
			}
			#endregion
			#endregion

			#region member
			void NotifyOnDataChange () {
                OnDataChange.Invoke ();
            }

            void Window(int id) {
				using (new GUIChangedScope(NotifyOnDataChange))
					_dataEditor.OnGUI ();
                GUI.DragWindow ();
            }
            #endregion
        }
    }
    public abstract class SettingsUI<T> : AbstractSettingsUI {
        public event System.Action EventOnDataChange;

        public T data;

        protected virtual void Awake() {
            core.OnDataChange.AddListener (new UnityEngine.Events.UnityAction (NotifyOnDataChange));
        }
        protected virtual void OnEnable() {
            core.OnEnable (data);
        }
        protected virtual void Update() {
            core.Update (data);
        }
        protected virtual void OnGUI() {
            core.OnGUI (this);
        }

        protected virtual void NotifyOnDataChange() {
            EventOnDataChange?.Invoke ();
            OnDataChange ();
        }
        protected virtual void OnDataChange() {}
    }
}
