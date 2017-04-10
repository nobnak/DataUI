using UnityEngine;
using System.Collections;
using Gist;
using DataUI;

namespace DataUI.Settings {
    
    [RequireComponent(typeof(Camera))]
    public class CropCameraUI : SettingsUI<CameraCrop.Data> {
		public Camera[] outputTotalViews;

        Camera _attachedCamera;

        #region Unity
    	protected override void OnEnable() {
            base.OnEnable ();
            _attachedCamera = GetComponent<Camera> ();
    	}
        protected override void Update () {
            base.Update ();
            CameraCrop.Crop(_attachedCamera, outputTotalViews, data);
        }
        protected virtual void OnDisable() {
            if (_attachedCamera != null)
                _attachedCamera.ResetProjectionMatrix ();
        }
        #endregion
    }
}