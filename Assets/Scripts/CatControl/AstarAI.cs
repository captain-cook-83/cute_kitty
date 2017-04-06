using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class AstarAI : MonoBehaviour {
	// The point to move to
	public Transform targetPosition;
	private Seeker seeker;
	// The calculated path
	public Path path;
	// The AI's speed in meters per second
	public float speed = 2;
	// The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;
	// The waypoint we are currently moving towards
	private int currentWaypoint = 0;
	// How often to recalculate the path (in seconds)
	public float repathRate = 0.5f;
	private float lastRepath = -9999;

	void Start () {
		seeker = GetComponent<Seeker>();
		seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
	}

	void OnPathComplete (Path p) {
		Debug.Log("A path was calculated. Did it fail with an error? " + p.error);
		if (!p.error) {
			path = p;
			// Reset the waypoint counter so that we start to move towards the first point in the path
			currentWaypoint = 0;
		}
	}

	void Update () {
		if (path == null) {
			// We have no path to follow yet, so don't do anything
			return;
		}
		if (currentWaypoint > path.vectorPath.Count) return;
		if (currentWaypoint == path.vectorPath.Count) {
			Debug.Log("End Of Path Reached");
			currentWaypoint++;
			return;
		}
		// Direction to the next waypoint
		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		transform.Translate(dir * speed * Time.deltaTime);
		// The commented line is equivalent to the one below, but the one that is used
		// is slightly faster since it does not have to calculate a square root
		//if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
		if ((transform.position-path.vectorPath[currentWaypoint]).sqrMagnitude < nextWaypointDistance*nextWaypointDistance) {
			currentWaypoint++;
			return;
		}
	}
} 
