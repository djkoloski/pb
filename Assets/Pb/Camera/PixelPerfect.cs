using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pb
{
	namespace Camera
	{
		/// <summary>
		/// Forces the camera to align itself on pixel boundaries before rendering, then restores the original position afterward
		/// </summary>
		public class PixelPerfect :
			MonoBehaviour
		{
			/// <summary>
			/// The number of pixels to map to each unit
			/// </summary>
			public float pixels_per_unit = 32.0f;
			/// <summary>
			/// A cache for the actual position of the camera
			/// </summary>
			public Vector3 cache_position = new Vector3(0, 0, 0);
			/// <summary>
			/// Caches the position of the camera, then aligns it on pixel boundaries
			/// </summary>
			public void OnPreRender()
			{
				cache_position = transform.position;
				transform.position = new Vector3(Mathf.Round(transform.position.x * pixels_per_unit) / pixels_per_unit, Mathf.Round(transform.position.y * pixels_per_unit) / pixels_per_unit, transform.position.z);
			}
			/// <summary>
			/// Restores the original camera position
			/// </summary>
			public void OnPostRender()
			{
				transform.position = cache_position;
			}
			/// <summary>
			/// Makes the camera orthographic and sets the size to match the desired resolution
			/// </summary>
			public void Start()
			{
				if (!camera.orthographic)
					camera.orthographic = true;

				camera.orthographicSize = Screen.height / 2.0f / pixels_per_unit;
			}
		}
	}
}
