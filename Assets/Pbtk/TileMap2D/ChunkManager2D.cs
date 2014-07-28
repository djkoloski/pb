using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pbtk
{
	namespace TileMap2D
	{
		/// <summary>
		/// A manager for loading static chunks from the resources directory at runtime
		/// </summary>
		[System.Serializable]
		public abstract class ChunkManager2D :
			Pb.TileMap.ChunkManager
		{
			/// <summary>
			/// The size of each chunk along the X axis
			/// </summary>
			public int chunk_size_x;
			/// <summary>
			/// The size of each chunk along the Y axis
			/// </summary>
			public int chunk_size_y;
			/// <summary>
			/// The X index of the leftmost chunk
			/// </summary>
			public int chunk_left;
			/// <summary>
			/// The X index of the rightmost chunk
			/// </summary>
			public int chunk_right;
			/// <summary>
			/// The Y index of the bottommost chunk
			/// </summary>
			public int chunk_bottom;
			/// <summary>
			/// The Y index of the topmost chunk
			/// </summary>
			public int chunk_top;
		}
	}
}