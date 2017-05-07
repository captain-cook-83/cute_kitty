using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AStarNavmeshController : MonoBehaviour {

	public TextAsset graphData;

	public Transform characters;

	private CharacterMovementController[] movementControllers;

	private TileHandlerHelper tileHandlerHelper;

	private float checkTime;

	void Awake () {
		AstarPath.active.astarData.DeserializeGraphs (graphData.bytes);
	}

	void Start() {
		movementControllers = characters.GetComponentsInChildren<CharacterMovementController> ();
		tileHandlerHelper = GetComponent<TileHandlerHelper> ();

		if (movementControllers.Length == 1){
			tileHandlerHelper.enabled = false;
			enabled = false;
		}
	}

	void Update() {
		if (checkTime > tileHandlerHelper.updateInterval) {
			checkTime = 0;

			bool isMoving = false;
			foreach (CharacterMovementController movementController in movementControllers) {
				if (movementController.IsMoving) {
					isMoving = true; 
					break;
				}
			}

			if (tileHandlerHelper.enabled != isMoving) {
				tileHandlerHelper.enabled = isMoving;
			}
		} else {
			checkTime += Time.deltaTime;
		}
	}
}
