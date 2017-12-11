using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using nobnak.Gist;
using nobnak.Gist.DataUI;

namespace DataUI.Settings {
    public class PreRenderingUI : PreRendering {
        public bool uiVisibility;
        public KeyCode uiToggle = KeyCode.P;
        FieldEditor _editor;
        Rect _window = new Rect(10, 10, 200, 200);

        #region Unity
        protected override void OnEnable() {
            base.OnEnable ();
            _editor = new FieldEditor (data);
    	}
        protected virtual void Update() {
            if (Input.GetKeyDown (uiToggle))
                uiVisibility = !uiVisibility;
        }
        protected override void OnRenderImage(RenderTexture src, RenderTexture dst) {
            if (data.passthrough)
                Graphics.Blit (src, dst);
        }
        protected virtual void OnGUI() {
            if (uiVisibility && _editor != null)
                _window = GUILayout.Window (GetInstanceID (), _window, Window, name);
        }
        #endregion

        void Window(int id) {
            GUILayout.BeginVertical ();
            _editor.OnGUI ();
            GUILayout.EndVertical ();
            GUI.DragWindow ();
        }

    }
}
