using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class AstarAI : MonoBehaviour {

	public Transform targetPosition;
	private Seeker seeker;

	public Path path;

	public float speed = 2;

	public float rotationSpeed = 10;

	public float nextWaypointDistance = 3;

	private int currentWaypoint = 0;

	void Start () {
		seeker = GetComponent<Seeker>();
		seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
	}

	void OnPathComplete (Path p) {
		if (!p.error) {
			path = p;
			currentWaypoint = 0;
		}
	}

	void Update () {
		if (path == null) {
			return;
		}
		if (currentWaypoint > path.vectorPath.Count) return;
		if (currentWaypoint == path.vectorPath.Count) {
			currentWaypoint++;
			return;
		}

		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		transform.Translate(dir * speed * Time.deltaTime);

		if ((transform.position-path.vectorPath[currentWaypoint]).sqrMagnitude < nextWaypointDistance*nextWaypointDistance) {
			currentWaypoint++;
			return;
		}
	}
} 
