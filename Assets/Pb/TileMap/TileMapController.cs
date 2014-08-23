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
			/// The tile map to use chunks from
			/// </summary>
			public TileMap tile_map;
			/// <summary>
			/// The chunks currently loaded by the controller
			/// </summary>
			public LoadedChunkMap loaded_chunks;
			/// <summary>
			/// The chunks loaded by the controller
			/// </summary>
			public RenderedChunkMap chunk_roots;
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
					return (chunk_roots != null && transform != null);
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
				chunk_roots = new RenderedChunkMap();
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
					throw new System.InvalidOperationException("Begin() called twice before End()");

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
					throw new System.InvalidOperationException("No tile map or Begin() not yet called");

				foreach (KeyValuePair<ITuple3, Chunk> pair in loaded_chunks)
					tile_map.UnloadChunk(pair.Value, pair.Key.Item1, pair.Key.Item2, pair.Key.Item3);
				loaded_chunks.Clear();
				Utility.Object.ContextualDestroy(render_root.gameObject);
				chunk_roots.Clear();
			}
			/// <summary>
			/// Determines whether a specific chunk is currently loaded by the tile map
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
					throw new System.InvalidOperationException("No tile map or Begin() not yet called");
				return loaded_chunks.ContainsKey(new ITuple3(x, y, z));
			}
			/// <summary>
			/// Determines whether a specific chunk is currently rendered by the tile map controller
			/// </summary>
			/// <param name="x">The X position of the chunk</param>
			/// <param name="y">The Y position of the chunk</param>
			/// <param name="z">The Z position of the chunk</param>
			/// <returns>Whether the chunk is currently rendered</returns>
			public bool IsChunkRendered(int x = 0, int y = 0, int z = 0)
			{
				if (!initialized)
					Init();
				return chunk_roots.ContainsKey(new ITuple3(x, y, z));
			}
			/// <summary>
			/// Gets a currently-loaded chunk
			/// </summary>
			/// <param name="x">The X position of the chunk</param>
			/// <param name="y">The Y position of the chunk</param>
			/// <param name="z">The Z position of the chunk</param>
			/// <returns>The chunk root or null if the chunk is not loaded</returns>
			public Chunk GetChunk(int x = 0, int y = 0, int z = 0)
			{
				if (!initialized)
					Init();
				if (!ready)
					throw new System.InvalidOperationException("No tile map or Begin() not yet called");
				if (IsChunkLoaded(x, y, z))
					return loaded_chunks[new ITuple3(x, y, z)];
				return null;
			}
			/// <summary>
			/// Gets a currently-rendered chunk root
			/// </summary>
			/// <param name="x">The X position of the chunk</param>
			/// <param name="y">The Y position of the chunk</param>
			/// <param name="z">The Z position of the chunk</param>
			/// <returns>The chunk root or null if the chunk is not rendered</returns>
			public GameObject GetChunkRoot(int x = 0, int y = 0, int z = 0)
			{
				if (!initialized)
					Init();
				if (IsChunkRendered(x, y, z))
					return chunk_roots[new ITuple3(x, y, z)];
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
					throw new System.InvalidOperationException("No tile map or Begin() not yet called");
				if (IsChunkLoaded(x, y, z))
					return false;

				Chunk chunk = tile_map.LoadChunk(x, y, z);
				if (chunk == null)
					return false;

				loaded_chunks.Add(new ITuple3(x, y, z), chunk);
				return true;
			}
			/// <summary>
			/// Renders a specific chunk
			/// </summary>
			/// <param name="x">The X position of the chunk</param>
			/// <param name="y">The Y position of the chunk</param>
			/// <param name="z">The Z position fo the chunk</param>
			/// <returns>Whether the chunk was successfully rendered</returns>
			public bool RenderChunk(int x = 0, int y = 0, int z = 0)
			{
				if (!initialized)
					Init();
				if (!ready)
					throw new System.InvalidOperationException("No tile map or Begin() not yet called");
				if (!IsChunkLoaded(x, y, z))
					throw new System.InvalidOperationException("Chunk rendered before it was loaded");
				if (IsChunkRendered(x, y, z))
					return false;
				
				Chunk chunk = GetChunk(x, y, z);

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

				chunk_roots.Add(new ITuple3(x, y, z), chunk_go);
				return true;
			}
			/// <summary>
			/// Loads and renders a specific chunk
			/// </summary>
			/// <param name="x">The X position of the chunk</param>
			/// <param name="y">The Y position of the chunk</param>
			/// <param name="z">The Z position fo the chunk</param>
			/// <returns>Whether the chunk was successfully loaded and rendered</returns>
			public bool LoadAndRenderChunk(int x = 0, int y = 0, int z = 0)
			{
				if (!IsChunkLoaded(x, y, z))
					if (!LoadChunk(x, y, z))
						return false;
				if (!IsChunkRendered(x, y, z))
					if (!RenderChunk(x, y, z))
						return false;
				return true;
			}
			/// <summary>
			/// Unloads a specific chunk
			/// Chunks may not be unloaded until they are unrendered
			/// </summary>
			/// <param name="x">The X position of the chunk</param>
			/// <param name="y">The Y position of the chunk</param>
			/// <param name="z">The Z position of the chunk</param>
			/// <returns>Whether the chunk was successfully unloaded</returns>
			public bool UnloadChunk(int x = 0, int y = 0, int z = 0)
			{
				if (!ready)
					throw new System.InvalidOperationException("No tile map or Begin() not yet called");
				if (IsChunkRendered(x, y, z))
					throw new System.InvalidOperationException("Chunk unloaded before it was unrendered");
				if (!IsChunkLoaded(x, y, z))
					return false;
				tile_map.UnloadChunk(loaded_chunks[new ITuple3(x, y, z)], x, y, z);
				loaded_chunks.Remove(new ITuple3(x, y, z));
				return true;
			}
			/// <summary>
			/// Unrenders a specific chunk
			/// </summary>
			/// <param name="x">The X position of the chunk</param>
			/// <param name="y">The Y position of the chunk</param>
			/// <param name="z">The Z position of the chunk</param>
			/// <returns>Whether the chunk was successfully unrendered</returns>
			public bool UnrenderChunk(int x = 0, int y = 0, int z = 0)
			{
				if (!initialized)
					Init();
				if (!IsChunkRendered(x, y, z))
					return false;
				
				GameObject chunk_root = GetChunkRoot(x, y, z);
				Utility.Object.ContextualDestroy(chunk_root);
				chunk_roots.Remove(new ITuple3(x, y, z));
				return true;
			}
			/// <summary>
			/// Unloads and unrenders a specific chunk
			/// </summary>
			/// <param name="x">The X position of the chunk</param>
			/// <param name="y">The Y position of the chunk</param>
			/// <param name="z">The Z position fo the chunk</param>
			/// <returns>Whether the chunk was successfully unloaded and unrendered</returns>
			public bool UnloadAndUnrenderChunk(int x = 0, int y = 0, int z = 0)
			{
				if (IsChunkRendered(x, y, z))
					if (!UnrenderChunk(x, y, z))
						return false;
				if (IsChunkLoaded(x, y, z))
					if (!UnloadChunk(x, y, z))
						return false;
				return true;
			}
		}
	}
}