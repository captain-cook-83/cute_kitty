using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSController : MonoBehaviour {

	void Awake () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

//	void OnDestroy() {
//		Screen.sleepTimeout = SleepTimeout.SystemSetting;
//	}
}
