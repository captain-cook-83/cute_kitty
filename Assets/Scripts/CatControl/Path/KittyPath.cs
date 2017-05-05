using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kittypath {

	public class KittyPath {

		private List<PathSegment> pathSegments;

		private int currentSegmentIndex;

		private float minJumpVerticleHeight;

		private float maxSqrJumpForwardDistance;

		private Vector3 __vector3 = new Vector3();

		public KittyPath(List<Vector3> vectorPath, float minJumpVerticleHeight, float maxJumpForwardDistance) {
			pathSegments = new List<PathSegment> (vectorPath.Count - 1);
			for (int i = 1; i < vectorPath.Count; i++) {
				PathSegment segment = new PathSegment (vectorPath[i - 1], vectorPath[i]);
				if (segment.endPoint.y > segment.startPoint.y + minJumpVerticleHeight) {
					Vector3 jumpEndpoint = segment.endPoint;
					__vector3.x = segment.startPoint.x;
					__vector3.z = segment.startPoint.z;
					__vector3.y = jumpEndpoint.y;
					Vector3 backDirection = __vector3 - jumpEndpoint;
					backDirection.Normalize();
					segment.endPoint = jumpEndpoint + backDirection * 0.12F;
					segment.throughStyle = PathSegmentThroughStyle.JumpUp;
					pathSegments.Add (segment);

					segment = new PathSegment (segment.endPoint, jumpEndpoint);
					segment.throughStyle = PathSegmentThroughStyle.Directly;
				} else if (segment.endPoint.y < segment.startPoint.y - minJumpVerticleHeight) {
					segment.throughStyle = PathSegmentThroughStyle.JumpDown;
				} else {
					segment.throughStyle = PathSegmentThroughStyle.Directly;
				}
				pathSegments.Add (segment);
			}

			this.minJumpVerticleHeight = minJumpVerticleHeight;
			this.maxSqrJumpForwardDistance = maxJumpForwardDistance * maxJumpForwardDistance;
		}

		public bool HasNext() {
			return currentSegmentIndex < pathSegments.Count;
		}

		public PathSegment Next() {
			PathSegment pathSegment = pathSegments[currentSegmentIndex++];
			if (PathSegmentThroughStyle.Directly == pathSegment.throughStyle) {
				Vector3 pathDirection = pathSegment.endPoint - pathSegment.startPoint;
				if (pathDirection.sqrMagnitude <= maxSqrJumpForwardDistance) {
					Ray ray = new Ray (pathSegment.startPoint + pathDirection * 0.5F + Vector3.up * minJumpVerticleHeight, Vector3.down);
					if (! Physics.Raycast (ray, minJumpVerticleHeight * 2)) {
						pathSegment.throughStyle = PathSegmentThroughStyle.JumpForward;
					}
				}
			}
			return pathSegment;
		}
	}
}
