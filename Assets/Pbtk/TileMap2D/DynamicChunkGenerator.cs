using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pbtk
{
	namespace TileMap2D
	{
		/// <summary>
		/// A manager for handling dynamic chunk generation
		/// </summary>
		[System.Serializable]
		public abstract class DynamicChunkGenerator :
			ChunkManager2D
		{
			/// <summary>
			/// Called when the chunk manager goes out of scope or it is requested that it reset.
			/// </summary>
			public virtual void Reset()
			{ }
			/// <summary>
			/// Resets the chunk manager when it goes out of scope
			/// </summary>
			public void OnDisable()
			{
				Reset();
			}
		}
	}
}