using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISignsHolder : MonoBehaviour {

	private AISign tvs;

	private List<AISign> photos;

	private List<AISign> sofas;

	private List<AISign> tables;

	private List<AISign> recesses;

	void Awake () {
		photos = new List<AISign> ();
		sofas = new List<AISign> ();
		tables = new List<AISign> ();
		recesses = new List<AISign> ();

		AISign[] aiSigns = GetComponentsInChildren<AISign> ();

		for (int i = 0; i < aiSigns.Length; i++) {
			AISign aiSign = aiSigns[i];
			switch (aiSign.name) {
			case "TV":
				tvs = aiSign;
				break;
			case "Photo":
				photos.Add (aiSign);
				break;
			case "Sofa":
				sofas.Add (aiSign);
				break;
			case "Table":
				tables.Add (aiSign);
				break;
			case "Recesses":
				recesses.Add (aiSign);
				break;
			default:
				Debug.LogError ("Invalide AISign name: " + aiSign.name);
				break;
			}
		}
	}

	public AISign GetRandomTVSign() {
		return tvs;
	}

	public AISign GetRandomPhotoSign() {
		return getRandomAISign(photos);
	}

	public AISign GetRandomSofaSign() {
		return getRandomAISign(sofas);
	}

	public AISign GetRandomTableSign() {
		return getRandomAISign(tables);
	}

	public AISign GetRandomRecessesSign() {
		return getRandomAISign(recesses);
	}

	private AISign getRandomAISign(List<AISign> aiSigns) {
		return aiSigns != null ? aiSigns[Random.Range(0, aiSigns.Count)] : null;
	}
}
