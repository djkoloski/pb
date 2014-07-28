using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pb.Collections;

namespace Pb
{
	namespace TileMap
	{
		/// <summary>
		/// A behaviour that loads, unloads, and renders chunks
		/// </summary>
		public class TileMapController :
			MonoBehaviour
		{
			/// <summary>
			/// A map from a set of chunk coordinates to the loaded chunk
			/// </summary>
			public class ChunkMap :
				Map<ITuple3, GameObject>
			{ }
			/// <summary>
			/// The tile map to use chunks from
			/// </summary>
			public TileMap tile_map;
			/// <summary>
			/// The chunks loaded by the controller
			/// </summary>
			public ChunkMap loaded_chunks;
			/// <summary>
			/// The transform of the tile map controller
			/// </summary>
			public new Transform transform;
			/// <summary>
			/// The rendering root of the tile map controller
			/// </summary>
			public Transform render_root;
			/// <summary>
			/// Determines whether the tile map controller has been initialized and can be used at all.
			/// If the controller is not initialized, call Init().
			/// </summary>
			public bool initialized
			{
				get
				{
					return (loaded_chunks != null && transform != null);
				}
			}
			/// <summary>
			/// Determines whether chunks are ready to be loaded and unloaded from the tile map.
			/// If the controller is not ready, call Begin().
			/// </summary>
			public bool ready
			{
				get
				{
					return (tile_map != null && render_root != null);
				}
			}
			/// <summary>
			/// Initializes the controller when the controller is given the awake event
			/// </summary>
			public void Awake()
			{
				Init();
				Begin();
			}
			/// <summary>
			/// Initializes the controller
			/// </summary>
			public void Init()
			{
				loaded_chunks = new ChunkMap();
				transform = GetComponent<Transform>();
			}
			/// <summary>
			/// Readies the controller for loading and unloading chunks
			/// </summary>
			public void Begin()
			{
				if (!initialized)
					Init();
				if (ready)
					throw new System.InvalidOperationException("End() never called on tile map");

				GameObject rr_go = new GameObject("render_root");
				render_root = rr_go.GetComponent<Transform>();
				
				render_root.parent = transform;
				render_root.localPosition = Vector3.zero;
				render_root.localRotation = Quaternion.identity;
				render_root.localScale = Vector3.one;
			}
			/// <summary>
			/// Unloads all chunks and cleans up the controller
			/// </summary>
			public void End()
			{
				if (!initialized)
					Init();
				if (!ready)
					throw new System.InvalidOperationException("Begin() never called on tile map");

				Utility.Object.ContextualDestroy(render_root.gameObject);
				loaded_chunks.Clear();
			}
			/// <summary>
			/// Determines whether a specific chunk is currently loaded by the controller
			/// </summary>
			/// <param name="x">The X position of the chunk</param>
			/// <param name="y">The Y position of the chunk</param>
			/// <param name="z">The Z position of the chunk</param>
			/// <returns>Whether the chunk is currently loaded</returns>
			public bool IsChunkLoaded(int x = 0, int y = 0, int z = 0)
			{
				if (!initialized)
					Init();
				if (!ready)
					throw new System.InvalidOperationException("Chunks cannot be checked before the tile map is ready");

				return loaded_chunks.ContainsKey(new ITuple3(x, y, z));
			}
			/// <summary>
			/// Gets a currently-loaded chunk
			/// </summary>
			/// <param name="x">The X position of the chunk</param>
			/// <param name="y">The Y position of the chunk</param>
			/// <param name="z">The Z position of the chunk</param>
			/// <returns>The chunk or null if the chunk is not loaded</returns>
			public GameObject GetChunk(int x = 0, int y = 0, int z = 0)
			{
				if (!initialized)
					Init();
				if (!ready)
					throw new System.InvalidOperationException("Chunks cannot be gotten before the tile map is ready");

				if (loaded_chunks.ContainsKey(new ITuple3(x, y, z)))
					return loaded_chunks[new ITuple3(x, y, z)];
				return null;
			}
			/// <summary>
			/// Loads a specific chunk
			/// </summary>
			/// <param name="x">The X position of the chunk</param>
			/// <param name="y">The Y position of the chunk</param>
			/// <param name="z">The Z position of the chunk</param>
			/// <returns>Whether the chunk was successfully loaded</returns>
			public bool LoadChunk(int x = 0, int y = 0, int z = 0)
			{
				if (!initialized)
					Init();
				if (!ready)
					throw new System.InvalidOperationException("Chunks cannot be loaded before the tile map is ready");

				if (!loaded_chunks.ContainsKey(new ITuple3(x, y, z)))
				{
					Chunk chunk = tile_map.GetChunk(x, y, z);

					if (chunk == null)
						return false;
					
					GameObject chunk_go = new GameObject("chunk_" + x + "_" + y + "_" + z);
					Transform chunk_root = chunk_go.GetComponent<Transform>();
					chunk_root.parent = render_root;
					chunk_root.localPosition = Vector3.zero;
					chunk_root.localRotation = Quaternion.identity;
					chunk_root.localScale = Vector3.one;

					if (tile_map.chunk_renderer == null)
						throw new System.InvalidOperationException("Attempted to load chunk while no chunk renderer was assigned");

					if (!tile_map.chunk_renderer.Render(this, chunk, chunk_go))
					{
						Utility.Object.ContextualDestroy(chunk_go);
						return false;
					}

					loaded_chunks.Add(new ITuple3(x, y, z), chunk_go);
					return true;
				}

				return false;
			}
			/// <summary>
			/// Unloads a specific chunk
			/// </summary>
			/// <param name="x">The X position of the chunk</param>
			/// <param name="y">The Y position of the chunk</param>
			/// <param name="z">The Z position of the chunk</param>
			/// <returns>Whether the chunk was successfully unloaded</returns>
			public bool UnloadChunk(int x = 0, int y = 0, int z = 0)
			{
				if (!initialized)
					Init();
				if (!ready)
					throw new System.InvalidOperationException("Chunks cannot be unloaded before the tile map is ready");

				if (loaded_chunks.ContainsKey(new ITuple3(x, y, z)))
				{
					Utility.Object.ContextualDestroy(loaded_chunks[new ITuple3(x, y, z)]);
					loaded_chunks.Remove(new ITuple3(x, y, z));
					return true;
				}
				
				return false;
			}
		}
	}
}