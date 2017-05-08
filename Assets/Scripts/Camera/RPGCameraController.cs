using UnityEngine;
using PhatRobit;

public class RPGCameraController : MonoBehaviour {

	public float primaryTargetVolume = 0.5F;

	public float secondaryTargetVolume = 0.2F;

	private SimpleRpgCamera rpgCamera;

	private Transform clickTarget;

	void Start () {
		rpgCamera = GetComponent<SimpleRpgCamera> ();
		if (rpgCamera.target != null) {
			rpgCamera.target.GetComponentInParent<AudioSource> ().volume = primaryTargetVolume;
		}
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
					Transform currentTarget = rpgCamera.target;
					if (cameraTarget != null && ! ReferenceEquals(cameraTarget, currentTarget)) {
						if (currentTarget != null) {
							currentTarget.GetComponentInParent<AudioSource> ().volume = secondaryTargetVolume;
						}
						cameraTarget.GetComponentInParent<AudioSource> ().volume = primaryTargetVolume;
						rpgCamera.target = cameraTarget;
					}
				} else {
					clickTarget = null;
				}
			}
		}
	}
}
