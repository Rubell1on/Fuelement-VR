using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CamerasController : MonoBehaviour
{
	public CustomCamera customCamera;
	public List<CameraPreset> cameraPresets = new List<CameraPreset>()
	{
		new CameraPreset("Driver", cameraType: CustomCamera.CameraType.FixedTo, dampFixedCamera: true, mouseOrbitFixedCamera: true, driverView: true),
		new CameraPreset("External", cameraType: CustomCamera.CameraType.SmoothLookAt, rotationDamping: 3f, heightDamping: 100f, distance: 4f, height: 2, pitchAngle: 0f, yawAngle: 0f)
	};

	public int presetId = 0;

	void Start()
	{
		if (customCamera) SetCamera(0);
	}

	public void SetCamera(int id)
    {
		CameraPreset preset = cameraPresets[id];
		if (preset != null)
        {
			presetId = id;
			customCamera.mycamera = preset.cameraType;
			customCamera.target = preset.target;
			customCamera.mtarget = preset.parent;
			customCamera.distance = preset.distance;
			customCamera.pitchAngle = preset.pitchAngle;
			customCamera.yawAngle = preset.yawAngle;
			customCamera.height = preset.height;
			customCamera.rotationDamping = preset.rotationDamping;
			customCamera.heightDamping = preset.heightDamping;
			customCamera.dampFixedCamera = preset.dampFixedCamera;
			customCamera.mouseOrbitFixedCamera = preset.mouseOrbitFixedCamera;
			customCamera.x = preset.x;
			customCamera.y = preset.y;
			customCamera.driverView = preset.driverView;
		} 
    }

	void Update()
	{
		//Camera control
		if (customCamera?.mycamera != CustomCamera.CameraType.Map)
		{
			if (Input.GetKeyDown(KeyCode.C))
			{
				presetId = presetId + 1 < cameraPresets.Count ? presetId + 1 : 0;
				SetCamera(presetId);
			}
		}
	}

	[Serializable]
	public class CameraPreset
	{
		public string presetName;
		public Transform target;
		public Transform parent;
		public CustomCamera.CameraType cameraType;
		public float distance;
		public float height;
		public float pitchAngle;
		public float yawAngle;
		public float rotationDamping;
		public float heightDamping;
		public bool dampFixedCamera;
		public bool mouseOrbitFixedCamera;
		public float x;
		public float y;
		public bool driverView;

		public CameraPreset(string presetName, Transform target = null, Transform parent = null, CustomCamera.CameraType cameraType = CustomCamera.CameraType.SmoothLookAt, float distance = 0, float height = 0, float pitchAngle = 0, float yawAngle = 0, float rotationDamping = 0, float heightDamping = 0, bool dampFixedCamera = false, bool mouseOrbitFixedCamera = false, float x = 0, float y = 0, bool driverView = false)
        {
			this.presetName = presetName;
			this.target = target;
			this.parent = parent;
			this.cameraType = cameraType;
			this.distance = distance;
			this.height = height;
			this.pitchAngle = pitchAngle;
			this.yawAngle = yawAngle;
			this.rotationDamping = rotationDamping;
			this.heightDamping = heightDamping;
			this.dampFixedCamera = dampFixedCamera;
			this.mouseOrbitFixedCamera = mouseOrbitFixedCamera;
			this.x = x;
			this.y = y;
			this.driverView = driverView;
		}
	}
}