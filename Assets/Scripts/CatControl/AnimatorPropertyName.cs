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

	private AnimatorPropertyName() {
		bodyLevel = Animator.StringToHash ("BodyLevel");
	}

	public int BodyLevel {
		get { return bodyLevel; }
	}
}
