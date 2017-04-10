using System.Runtime.InteropServices;
using UnityEngine;
using Gist;

namespace DataUI.Settings {

    public class WindowControlUI : WindowControl {
        public bool guiVisibility;
        public KeyCode guiToggle = KeyCode.W;

        FieldEditor _editor;
        Rect _window = new Rect(10, 10, 200, 200);

        #region Unity
        protected override void OnEnable() {
            base.OnEnable ();
            _editor = new FieldEditor (data);
        }
        protected virtual void Update() {
            if (Input.GetKeyDown (guiToggle))
                guiVisibility = !guiVisibility;
        }
        protected virtual void OnGUI() {
            if (guiVisibility && _editor != null)
                _window = GUILayout.Window (GetInstanceID (), _window, Window, name);
        }
        #endregion

        void Window(int id) {
            GUILayout.BeginVertical ();

            _editor.OnGUI ();
            if (GUILayout.Button ("Apply"))
                Apply ();
            if (GUILayout.Button ("Load Current")) {
                Current ();
                _editor.Load ();
            }

            GUILayout.EndVertical ();
            GUI.DragWindow ();
        }
    }
}
