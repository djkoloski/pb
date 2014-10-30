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
		public class StaticChunkManager :
			ChunkManager2D
		{
			/// <summary>
			/// The path to the resources folder containing the chunks.
			/// An empty string will give the root of the resources directory, all other paths must have a forward slash at the end.
			/// </summary>
			public string resources_path;
			/// <summary>
			/// Loads the desired chunk from the resources path
			/// </summary>
			/// <param name="v">The coordinates of the chunk</param>
			/// <returns>The loaded chunk</returns>
			public override Pb.TileMap.Chunk LoadChunk(IVector3 v)
			{
				string path = Pb.Path.Combine(resources_path, "chunk_" + v.x + "_" + v.y);
				return Resources.Load<Chunk>(path);
			}
			/// <summary>
			/// Unloads the given chunk from active memory
			/// </summary>
			/// <param name="chunk">The chunk to unload</param>
			/// <param name="v">The coordinates of the chunk</param>
			public override void UnloadChunk(Pb.TileMap.Chunk chunk, IVector3 v)
			{ }
		}
	}
}