using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kittyutils
{
	public class VectorMath
	{
		public static float DistanceXZ (Vector3 a, Vector3 b) {
			float x = a.x - b.x;
			float y = a.y - b.y;

			return Mathf.Sqrt(x * x + y * y);
		}
	}
}

