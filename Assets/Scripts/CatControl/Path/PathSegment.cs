﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kittypath {
	
	public class PathSegment {

		public Vector3 startPoint;

		public Vector3 endPoint;

		public PathSegmentThroughStyle throughStyle;

		public PathSegment(Vector3 startPoint, Vector3 endPoint) {
			this.startPoint = startPoint;
			this.endPoint = endPoint;
		}
	}
}
