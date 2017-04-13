using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kittypath {

	public class KittyPath {

		private List<PathSegment> pathSegments;

		private int currentSegmentIndex;

		private float minJumpVerticleHeight;

		private float maxSqrJumpForwardDistance;

		public KittyPath(List<Vector3> vectorPath, float minJumpVerticleHeight, float maxJumpForwardDistance) {
			pathSegments = new List<PathSegment> (vectorPath.Count - 1);
			for (int i = 1; i < vectorPath.Count; i++) {
				PathSegment segment = new PathSegment (vectorPath[i - 1], vectorPath[i]);
				if (segment.endPoint.y > segment.startPoint.y + minJumpVerticleHeight) {
					segment.throughStyle = PathSegmentThroughStyle.JumpUp;
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
					Ray ray = new Ray (pathSegment.startPoint + pathDirection * 0.5F, Vector3.down);
					if (! Physics.Raycast (ray, minJumpVerticleHeight)) {
						pathSegment.throughStyle = PathSegmentThroughStyle.JumpForward;
					}
				}
			}
			return pathSegment;
		}
	}
}
