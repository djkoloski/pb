using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pb
{
	namespace TileMap
	{
		/// <summary>
		/// Provides information about the chunks available to a tile map and gets those chunks
		/// </summary>
		[System.Serializable]
		public abstract class ChunkManager :
			ScriptableObject
		{
			/// <summary>
			/// Gets the indexed chunk
			/// </summary>
			/// <param name="x">The X coordinate of the desired chunk</param>
			/// <param name="y">The Y coordinate of the desired chunk</param>
			/// <param name="z">The Z coordinate of the desired chunk</param>
			/// <returns>The desired chunk or null if no chunk is located there</returns>
			public abstract Chunk GetChunk(int x, int y, int z);
		}
	}
}