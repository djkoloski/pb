using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pb.TileMap
{
	/// <summary>
	/// Base class for chunk renderers
	/// </summary>
	[System.Serializable]
	public class ChunkRenderer :
		ScriptableObject
	{
		/// <summary>
		/// Renders a chunk with a tile map controller, chunk, and chunk game object.
		/// All created objects should either be components on the chunk game object or children of it.
		/// Creating game objects elsewhere will not allow them to be destroyed when the chunk is unloaded.
		/// </summary>
		/// <param name="tmc">The TileMapController loading the chunk</param>
		/// <param name="chunk">The chunk being rendered</param>
		/// <param name="chunk_go">The game object made for the chunk</param>
		/// <returns>Whether the chunk was rendered</returns>
		public virtual bool Render(TileMapController tmc, Chunk chunk, GameObject chunk_go)
		{
			return false;
		}
	}
}