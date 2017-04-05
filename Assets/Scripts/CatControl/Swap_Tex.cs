using UnityEngine;
using System.Collections;

public class Swap_Tex : MonoBehaviour {

	public Texture Face1;
	public Texture Face2;

	[Header("Establishment = 10 - 95")]
	public int eStab = 95;

	public string sleepParameter = "Mode->Sleep";
	private int sleepParameterId;

	private float ctime = 0;
	private float change = 0.1F;

	private Renderer faceRenderer;

	private Animator animator;

	void Start () {
		faceRenderer = GetComponent<Renderer> ();
		animator = GetComponentInParent<Animator> ();
		sleepParameterId = Animator.StringToHash(sleepParameter);
	}
	
	void Update () {
		if (ctime < change){
			ctime += Time.deltaTime;
		}else{
			if (animator.GetBool(sleepParameterId)) {
				faceRenderer.materials[1].mainTexture = Face2;
			} else {
				if (Random.Range(0, 100) > eStab) {
					faceRenderer.materials[1].mainTexture = Face2;
				} else {
					faceRenderer.materials[1].mainTexture = Face1;
				}
			}
			ctime = 0;
		}
	}
}
