using UnityEngine;

namespace Pb
{
	namespace Utility
	{
		/// <summary>
		/// A simple helper class to aid parenting transforms
		/// </summary>
		public static class Parent
		{
			/// <summary>
			/// Parents the given child to the given parent and resets the child's transform
			/// </summary>
			/// <param name="child">The child to parent</param>
			/// <param name="parent">The parent to parent the child to</param>
			public static void ZeroLocal(Transform child, Transform parent)
			{
				child.parent = parent;
				child.localPosition = Vector3.zero;
				child.localRotation = Quaternion.identity;
				child.localScale = Vector3.one;
			}
			/// <summary>
			/// Preserves the local transformation of the child and restores it after parenting. This may change the world-space position, location, and scale.
			/// </summary>
			/// <param name="child">The child to parent</param>
			/// <param name="parent">The parent to parent the child to</param>
			public static void PreserveLocal(Transform child, Transform parent)
			{
				Vector3 pos = child.localPosition;
				Quaternion rot = child.localRotation;
				Vector3 scale = child.localScale;
				child.parent = parent;
				child.localPosition = pos;
				child.localRotation = rot;
				child.localScale = scale;
			}
		}
	}
}