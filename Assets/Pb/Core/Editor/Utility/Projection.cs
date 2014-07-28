using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Pb
{
	namespace Utility
	{
		/// <summary>
		/// A tool to aid projecting the mouse position onto planes
		/// </summary>
		public static class Projection
		{
			/// <summary>
			/// Casts the mouse position on the screen onto a plane
			/// </summary>
			/// <param name="normal">The normal of the plane</param>
			/// <param name="position">A point on the plane</param>
			/// <param name="hit">The place on the plane where the projection landed</param>
			/// <returns>Whether the plane was hit</returns>
			public static bool ProjectMousePosition(Vector3 normal, Vector3 position, out Vector3 hit)
			{
				Plane plane = new Plane(normal, position);
				Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
				float dist;

				if (plane.Raycast(ray, out dist))
				{
					hit = ray.origin + (ray.direction.normalized * dist);
					return true;
				}

				hit = new Vector3();
				return false;
			}
			// Casts the mouse position using a transform and local normal
			/// <summary>
			/// Casts the mouse position on the screen onto a plane using a transform and a local normal
			/// </summary>
			/// <param name="local_normal">The normal of the plane local to the transform</param>
			/// <param name="transform">The transform to project onto</param>
			/// <param name="hit">The place on the plane where the projection landed</param>
			/// <returns>Whether the plane was hit</returns>
			public static bool ProjectMousePosition(Vector3 local_normal, Transform transform, out Vector3 hit)
			{
				return ProjectMousePosition(transform.TransformDirection(local_normal), transform.position, out hit);
			}
		}
	}
}
