using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISignsHolder : MonoBehaviour {

	private Dictionary<string, List<AISign>> dict = new Dictionary<string, List<AISign>>();

	void Awake () {
		AISign[] aiSigns = GetComponentsInChildren<AISign> ();
		for (int i = 0; i < aiSigns.Length; i++) {
			AISign aiSign = aiSigns[i];

			List<AISign> list = null;
			dict.TryGetValue (aiSign.name, out list);
			if (list == null) {
				list = new List<AISign> ();
				dict.Add (aiSign.name, list);
			}
			list.Add (aiSign);
		}
	}

	public AISign getRandomAISign(string signLabel) {
		List<AISign> list = null;
		dict.TryGetValue (signLabel, out list);
		if (list != null) {
			return list[Random.Range (0, list.Count)];
		} else {
			Debug.LogError ("Missing AISign with name: " + signLabel);
			return null;
		}
	}
}
