using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorPropertyName {

	private static AnimatorPropertyName instance;

	public static AnimatorPropertyName GetInstance() {
		if (instance == null) {
			instance = new AnimatorPropertyName ();
		}
		return instance;
	}

	private int bodyLevel;

	private int movement2StopTrigger;

	private AnimatorPropertyName() {
		bodyLevel = Animator.StringToHash ("BodyLevel");
		movement2StopTrigger = Animator.StringToHash ("Movement->Stop");
	}

	public int BodyLevel {
		get { return bodyLevel; }
	}

	public int Movement2StopTrigger {
		get { return movement2StopTrigger; }
	}
}
