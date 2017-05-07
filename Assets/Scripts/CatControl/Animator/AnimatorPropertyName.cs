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

	private int teaseBTrigger;

	private int movement2StopTrigger;

	private int interactiveMode;

	private AnimatorPropertyName() {
		bodyLevel = Animator.StringToHash ("BodyLevel");
		teaseBTrigger = Animator.StringToHash ("BTrigger->Tease");
		movement2StopTrigger = Animator.StringToHash ("Movement->Stop");
		interactiveMode = Animator.StringToHash ("Mode->Interactive");
	}

	public int BodyLevel {
		get { return bodyLevel; }
	}

	public int TeaseBTrigger {
		get { return teaseBTrigger; }
	}

	public int Movement2StopTrigger {
		get { return movement2StopTrigger; }
	}

	public int InteractiveMode {
		get { return interactiveMode; }
	}
}
