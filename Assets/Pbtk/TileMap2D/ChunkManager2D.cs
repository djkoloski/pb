using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pb.Collections;

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
			public IVector2 chunk_size;
			/// <summary>
			/// The index of the least chunk
			/// </summary>
			public IVector2 chunk_least;
			/// <summary>
			/// The index of the greatest chunk
			/// </summary>
			public IVector2 chunk_greatest;
		}
	}
}