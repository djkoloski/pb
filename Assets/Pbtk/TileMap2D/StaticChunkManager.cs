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
			/// <param name="x">The X coordinate of the chunk</param>
			/// <param name="y">The Y coordinate of the chunk</param>
			/// <param name="z">The Z coordinate of the chunk</param>
			/// <returns>The loaded chunk</returns>
			public override Pb.TileMap.Chunk GetChunk(int x, int y, int z)
			{
				string path = Pb.Path.Combine(resources_path, "chunk_" + x + "_" + y);
				return Resources.Load<Chunk>(path);
			}
		}
	}
}