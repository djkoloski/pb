using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pb.Collections;

namespace Pb.TileMap
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
		/// <param name="v">The coordinates of the chunk</param>
		/// <returns>The desired chunk or null if no chunk is located there</returns>
		public abstract Chunk LoadChunk(IVector3 v);
		/// <summary>
		/// Unloads the given chunk from active memory
		/// </summary>
		/// <param name="chunk">The chunk to unload</param>
		/// <param name="v">The coordinates of the chunk</param>
		public abstract void UnloadChunk(Chunk chunk, IVector3 v);
	}
}