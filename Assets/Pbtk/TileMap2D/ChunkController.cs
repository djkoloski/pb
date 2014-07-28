using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pbtk
{
	namespace TileMap2D
	{
		/// <summary>
		/// Controls a loaded and rendered chunk
		/// </summary>
		public class ChunkController :
			MonoBehaviour
		{
			/// <summary>
			/// The transform of the chunk
			/// </summary>
			public new Transform transform;
			/// <summary>
			/// Determines whether the chunk controller is initialized
			/// </summary>
			public bool initialized
			{
				get
				{
					return transform != null;
				}
			}
			/// <summary>
			/// Ensures the chunk is initialized
			/// </summary>
			public void Awake()
			{
				Init();
			}
			/// <summary>
			/// Initializes the chunk controller
			/// </summary>
			public void Init()
			{
				transform = GetComponent<Transform>();
			}
			/// <summary>
			/// Removes a rendered tile and replaces it with a newly-rendered tile
			/// </summary>
			/// <param name="tile_map">The tile map being rendered from</param>
			/// <param name="chunk_index_x">The X index of the chunk being rendered</param>
			/// <param name="chunk_index_y">The Y index of the chunk being rendered</param>
			/// <param name="x">The local X coordinate of the tile</param>
			/// <param name="y">The local Y coordinate of the tile</param>
			/// <param name="l">The layer index of the tile</param>
			/// <param name="id">The ID to render in place of the removed tile</param>
			public void RerenderTile(TileMap tile_map, int chunk_index_x, int chunk_index_y, int x, int y, int l, int id)
			{
				if (!initialized)
					Init();

				ChunkRenderer chunk_renderer = tile_map.chunk_renderer as ChunkRenderer;
				GameObject tile = chunk_renderer.RenderTile(tile_map, chunk_index_x, chunk_index_y, x, y, l, id);
				
				Transform layer_transform = transform.Find(tile_map.layers[l].name);
				Transform old_tile = layer_transform.Find(x + "_" + y);
				
				if (old_tile != null)
					Pb.Utility.Object.ContextualDestroy(old_tile.gameObject);

				Pb.Utility.Parent.PreserveLocal(tile.GetComponent<Transform>(), layer_transform);
			}
		}
	}
}