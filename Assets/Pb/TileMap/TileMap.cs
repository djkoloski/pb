using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pb
{
	namespace TileMap
	{
		/// <summary>
		/// A container for all of the pieces that make up a tile map.
		/// Easily extendable, made to be customized.
		/// </summary>
		[System.Serializable]
		public class TileMap :
			ScriptableObject
		{
			/// <summary>
			/// A manager for the chunks used by the tile map
			/// </summary>
			public ChunkManager chunk_manager;
			/// <summary>
			/// The renderer for the chunks
			/// </summary>
			public ChunkRenderer chunk_renderer;
			/// <summary>
			/// Initializes the tile map when it is first created
			/// </summary>
			public void OnEnable()
			{ }
			/// <summary>
			/// Requests a chunk from the chunk manager
			/// </summary>
			/// <param name="x">The X coordinate of the chunk</param>
			/// <param name="y">The Y coordinate of the chunk</param>
			/// <param name="z">The Z coordinate of the chunk</param>
			/// <returns>The chunk or null if no chunk was returned by the manager</returns>
			public Chunk LoadChunk(int x = 0, int y = 0, int z = 0)
			{
				if (chunk_manager == null)
					throw new System.InvalidOperationException();

				return chunk_manager.LoadChunk(x, y, z);
			}
			/// <summary>
			/// Unloads the given chunk through the chunk manager
			/// </summary>
			/// <param name="chunk">The chunk to unload</param>
			/// <param name="x">The chunk's X coordinate</param>
			/// <param name="y">The chunk's Y coordinate</param>
			/// <param name="z">The chunk's Z coordinate</param>
			public void UnloadChunk(Chunk chunk, int x = 0, int y = 0, int z = 0)
			{
				if (chunk_manager == null)
					throw new System.InvalidOperationException();

				chunk_manager.UnloadChunk(chunk, x, y, z);
			}
		}
		/// <summary>
		/// A basic tile map, with a tile library along with the fundamentals
		/// </summary>
		[System.Serializable]
		public class BasicTileMap :
			TileMap
		{
			/// <summary>
			/// The geometry of the tile map
			/// </summary>
			public Geometry geometry;
			/// <summary>
			/// The tile sets that are used by the tile map
			/// </summary>
			public TileLibrary library;
			/// <summary>
			/// Initializes the tile map when it is first created
			/// </summary>
			public new void OnEnable()
			{
				base.OnEnable();

				if (geometry == null)
					geometry = new Geometry(Tiling.Rectangular, Orientation.RightUp, Vector3.one);
				if (library == null)
					library = new TileLibrary();
			}
		}
	}
}