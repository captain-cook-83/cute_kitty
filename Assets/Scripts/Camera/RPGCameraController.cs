using UnityEngine;
using PhatRobit;

public class RPGCameraController : MonoBehaviour {

	private SimpleRpgCamera rpgCamera;

	private Transform clickTarget;

	void Start () {
		rpgCamera = GetComponent<SimpleRpgCamera> ();
	}

	void Update () {
		RaycastHit hit;
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast (ray, out hit, 10)) {
				if (hit.collider.CompareTag ("Player")) {
					clickTarget = hit.transform;
				} else {
					clickTarget = null;
				}
			}
		} else if (clickTarget != null && Input.GetMouseButtonUp(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast (ray, out hit, 10)) {
				if (ReferenceEquals (hit.transform, clickTarget)) {
					Transform cameraTarget = clickTarget.FindChild ("CameraTarget");
					if (! ReferenceEquals(cameraTarget, rpgCamera.target)) {
						rpgCamera.target = cameraTarget;
					}
				} else {
					clickTarget = null;
				}
			}
		}
	}
}
