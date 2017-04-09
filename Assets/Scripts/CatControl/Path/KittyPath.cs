using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kittypath {

	public class KittyPath {

		private List<PathSegment> pathSegments;

		private int currentSegmentIndex;

		public KittyPath(List<Vector3> vectorPath) {
			pathSegments = new List<PathSegment> (vectorPath.Count - 1);
			for (int i = 1; i < vectorPath.Count; i++) {
				PathSegment segment = new PathSegment (vectorPath[i - 1], vectorPath[i]);
				if (segment.endPoint.y > segment.startPoint.y) {
					segment.throughStyle = PathSegmentThroughStyle.JumpUp;
				} else if (segment.endPoint.y < segment.startPoint.y) {
					segment.throughStyle = PathSegmentThroughStyle.JumpDown;
				} else {
					segment.throughStyle = PathSegmentThroughStyle.Directly;
				}
				pathSegments.Add (segment);
			}
		}

		public bool HasNext() {
			return currentSegmentIndex < pathSegments.Count;
		}

		public PathSegment Next() {
			// TODO 检测水平跳跃
			return pathSegments[currentSegmentIndex++];
		}
	}
}
