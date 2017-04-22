using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNavmeshLoader : MonoBehaviour {

	public TextAsset graphData;

	void Start () {
		AstarPath.active.astarData.DeserializeGraphs (graphData.bytes);
	}
}
