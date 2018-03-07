using UnityEngine;

public class CameraManager : MonoBehaviour {
	public Player Player;
	public bool HasPlayer;

	private float _cameraSpeed;

	void Start () {
	}
	
	void LateUpdate () {
		if (HasPlayer && Player.IsFollowing)
			transform.position = Player.Ship.transform.position;

		_cameraSpeed = Camera.main.orthographicSize;

		if (Input.GetKey (KeyCode.LeftShift))
			_cameraSpeed *= 1.75f;
		
		if (Input.GetKey (KeyCode.UpArrow)) {
			transform.Translate (Vector3.up * Time.deltaTime * _cameraSpeed);

			if (HasPlayer)
				Player.IsFollowing = false;
		}

		if (Input.GetKey (KeyCode.DownArrow)) {
			transform.Translate (Vector3.down * Time.deltaTime * _cameraSpeed);

			if (HasPlayer)
				Player.IsFollowing = false;
		}

		if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.Translate (Vector3.left * Time.deltaTime * _cameraSpeed);

			if (HasPlayer)
				Player.IsFollowing = false;
		}

		if (Input.GetKey (KeyCode.RightArrow)) {
			transform.Translate (Vector3.right * Time.deltaTime * _cameraSpeed);

			if (HasPlayer)
				Player.IsFollowing = false;
		}

		if (Input.GetAxis ("Mouse ScrollWheel") != 0 && Input.GetKey(KeyCode.Z)) {
			var zoomSpeed = Camera.main.orthographicSize * 0.5f;

			if (Input.GetAxis ("Mouse ScrollWheel") > 0 && Camera.main.orthographicSize - zoomSpeed > 0)
				Camera.main.orthographicSize -= zoomSpeed;
			else if (Input.GetAxis ("Mouse ScrollWheel") < 0)
				Camera.main.orthographicSize += zoomSpeed;
		}
	}

	private class ZoomRange	{
		public float Min { get; set; }
		public float Max { get; set; }
		public float Modifier { get; set; }

		public ZoomRange () {
			
		}

		public ZoomRange (float min, float max, float modifier) {
			Min = min;
			Max = max;
			Modifier = modifier;
		}
	}
}
