using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using nobnak.Gist;

namespace DataUI.Settings {

    public class PositionClusteringUI : SettingsUI<PositionClusteringUI.Data> {
        public Camera targetCam;

        protected PositionClustering clustering;
        
        GLFigure _fig;

        #region Unity
        protected override void Awake() {
            base.Awake ();
            _fig = new GLFigure ();
            clustering = new PositionClustering (data);

        }
        protected override void Update() {
            base.Update ();
            DebugInput();
            clustering.UpdateCluster();
        }
        protected virtual void OnRenderObject() {
            if ((Camera.current.cullingMask & (1 << gameObject.layer)) == 0)
                return;
            if (targetCam == null || clustering == null)
                return;
            if (!data.debugPointsVisible)
                return;

            var rot = targetCam.transform.rotation;
            var size = data.DebugInputSize * Vector2.one;
            foreach (var pp in clustering.IteratePoints()) {
                var p = (Vector3)pp.pos;
                p.z = data.debugInputDepth;
                _fig.FillCircle (targetCam.ViewportToWorldPoint (p), rot, size, data.debugInputColor);
            }
            foreach (var c in clustering.IteratePositions()) {
                var p = (Vector3)c;
                p.z = data.debugInputDepth;
                _fig.FillCircle (targetCam.ViewportToWorldPoint (p), rot, size, data.debugClusterColor);
            }
        }
        protected virtual void OnDestroy() {
            if (_fig != null) {
                _fig.Dispose ();
                _fig = null;
            }
            if (clustering != null) {
                clustering.Dispose ();
                clustering = null;
            }
                
        }
        #endregion

        void DebugInput () {
            if (data.debugInputEnabled) {
                if (Input.GetMouseButton (0)) {
                    var mousepos = Input.mousePosition;
                    var p = new Vector2(mousepos.x / Screen.width, mousepos.y / Screen.height);
                    clustering.Receive (p);
                }
                if (Input.GetMouseButtonDown (1)) {
					clustering.Clear();
                }
            }
        }

        [System.Serializable]
        public class Data : PositionClustering.Data {
            public bool debugInputEnabled = false;
            public bool debugPointsVisible = false;
            public float debugInputDepth = 10f;
			
            public Color debugInputColor = new Color(1f, 0f, 0f, 0.2f);
            public Color debugClusterColor = new Color(0f, 1f, 1f, 0.5f);

			protected float debugInputSize = 1f;

			public float DebugInputSize {
				get { return debugInputSize; }
				set { debugInputSize = value; }
			}
		}
    }
}
