using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pb.Collections;

namespace Pb.TileMap
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
				
			Utility.Parent.ZeroLocal(render_root, transform);
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

			foreach (KeyValuePair<IVector3, Chunk> pair in loaded_chunks)
				tile_map.UnloadChunk(pair.Value, pair.Key);
			loaded_chunks.Clear();
			Utility.Object.ContextualDestroy(render_root.gameObject);
			chunk_roots.Clear();
		}
		/// <summary>
		/// Determines whether a specific chunk is currently loaded by the tile map
		/// </summary>
		/// <param name="v">The coordinates of the chunk</param>
		/// <returns>Whether the chunk is currently loaded</returns>
		public bool IsChunkLoaded(IVector3 v)
		{
			if (!initialized)
				Init();
			if (!ready)
				throw new System.InvalidOperationException("No tile map or Begin() not yet called");
			return loaded_chunks.ContainsKey(v);
		}
		/// <summary>
		/// Determines whether a specific chunk is currently rendered by the tile map controller
		/// </summary>
		/// <param name="v">The coordinates of the chunk</param>
		/// <returns>Whether the chunk is currently rendered</returns>
		public bool IsChunkRendered(IVector3 v)
		{
			if (!initialized)
				Init();
			return chunk_roots.ContainsKey(v);
		}
		/// <summary>
		/// Gets a currently-loaded chunk
		/// </summary>
		/// <param name="v">The coordinates of the chunk</param>
		/// <returns>The chunk root or null if the chunk is not loaded</returns>
		public Chunk GetChunk(IVector3 v)
		{
			if (!initialized)
				Init();
			if (!ready)
				throw new System.InvalidOperationException("No tile map or Begin() not yet called");
			if (IsChunkLoaded(v))
				return loaded_chunks[v];
			return null;
		}
		/// <summary>
		/// Gets a currently-rendered chunk root
		/// </summary>
		/// <param name="v">The coordinates of the chunk</param>
		/// <returns>The chunk root or null if the chunk is not rendered</returns>
		public GameObject GetChunkRoot(IVector3 v)
		{
			if (!initialized)
				Init();
			if (IsChunkRendered(v))
				return chunk_roots[v];
			return null;
		}
		/// <summary>
		/// Loads a specific chunk
		/// </summary>
		/// <param name="v">The coordinates of the chunk</param>
		/// <returns>Whether the chunk was successfully loaded</returns>
		public bool LoadChunk(IVector3 v)
		{
			if (!initialized)
				Init();
			if (!ready)
				throw new System.InvalidOperationException("No tile map or Begin() not yet called");
			if (IsChunkLoaded(v))
				return false;

			Chunk chunk = tile_map.LoadChunk(v);
			if (chunk == null)
				return false;

			loaded_chunks.Add(v, chunk);
			return true;
		}
		/// <summary>
		/// Renders a specific chunk
		/// </summary>
		/// <param name="v">The coordinates of the chunk</param>
		/// <returns>Whether the chunk was successfully rendered</returns>
		public bool RenderChunk(IVector3 v)
		{
			if (!initialized)
				Init();
			if (!ready)
				throw new System.InvalidOperationException("No tile map or Begin() not yet called");
			if (!IsChunkLoaded(v))
				throw new System.InvalidOperationException("Chunk rendered before it was loaded");
			if (IsChunkRendered(v))
				return false;
				
			Chunk chunk = GetChunk(v);

			if (chunk == null)
				return false;
					
			GameObject chunk_go = new GameObject("chunk_" + v.x + "_" + v.y + "_" + v.z);
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

			chunk_roots.Add(v, chunk_go);
			return true;
		}
		/// <summary>
		/// Loads and renders a specific chunk
		/// </summary>
		/// <param name="v">The coordinates of the chunk</param>
		/// <returns>Whether the chunk was successfully loaded and rendered</returns>
		public bool LoadAndRenderChunk(IVector3 v)
		{
			if (!IsChunkLoaded(v))
				if (!LoadChunk(v))
					return false;
			if (!IsChunkRendered(v))
				if (!RenderChunk(v))
					return false;
			return true;
		}
		/// <summary>
		/// Unloads a specific chunk
		/// Chunks may not be unloaded until they are unrendered
		/// </summary>
		/// <param name="v">The coordinates of the chunk</param>
		/// <returns>Whether the chunk was successfully unloaded</returns>
		public bool UnloadChunk(IVector3 v)
		{
			if (!ready)
				throw new System.InvalidOperationException("No tile map or Begin() not yet called");
			if (IsChunkRendered(v))
				throw new System.InvalidOperationException("Chunk unloaded before it was unrendered");
			if (!IsChunkLoaded(v))
				return false;
			tile_map.UnloadChunk(loaded_chunks[v], v);
			loaded_chunks.Remove(v);
			return true;
		}
		/// <summary>
		/// Unrenders a specific chunk
		/// </summary>
		/// <param name="v">The coordinates of the chunk</param>
		/// <returns>Whether the chunk was successfully unrendered</returns>
		public bool UnrenderChunk(IVector3 v)
		{
			if (!initialized)
				Init();
			if (!IsChunkRendered(v))
				return false;
				
			GameObject chunk_root = GetChunkRoot(v);
			Utility.Object.ContextualDestroy(chunk_root);
			chunk_roots.Remove(v);
			return true;
		}
		/// <summary>
		/// Unloads and unrenders a specific chunk
		/// </summary>
		/// <param name="v">The coordinates of the chunk</param>
		/// <returns>Whether the chunk was successfully unloaded and unrendered</returns>
		public bool UnloadAndUnrenderChunk(IVector3 v)
		{
			if (IsChunkRendered(v))
				if (!UnrenderChunk(v))
					return false;
			if (IsChunkLoaded(v))
				if (!UnloadChunk(v))
					return false;
			return true;
		}
	}
}