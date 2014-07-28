using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pbtk
{
	namespace TileMap2D
	{
		/// <summary>
		/// A basic 2D tile map chunk renderer
		/// </summary>
		[System.Serializable]
		public class ChunkRenderer :
			Pb.TileMap.ChunkRenderer
		{
			/// <summary>
			/// Whether to flip drawing precedence along the X axis
			/// </summary>
			public bool flip_x_precedence;
			/// <summary>
			/// Whether to flip drawing precedence along the Y axis
			/// </summary>
			public bool flip_y_precedence;
			/// <summary>
			/// The material to put on the sprites when they are rendered
			/// </summary>
			public Material sprite_material;
			/// <summary>
			/// Renders a chunk of a 2D tile map
			/// </summary>
			/// <param name="tmc">The TileMapController loading the chunk</param>
			/// <param name="chunk">The chunk being rendered</param>
			/// <param name="chunk_go">The game object made for the chunk</param>
			/// <returns></returns>
			public override bool Render(Pb.TileMap.TileMapController tmc, Pb.TileMap.Chunk pb_chunk, GameObject chunk_go)
			{
				if (pb_chunk == null)
					return false;
				Chunk chunk = pb_chunk as Chunk;
				if (chunk == null)
					return false;

				if (tmc.tile_map == null)
					return false;
				TileMap tile_map = tmc.tile_map as TileMap;
				if (tile_map == null)
					return false;

				if (tile_map.chunk_manager == null)
					return false;
				ChunkManager2D chunk_manager = tile_map.chunk_manager as ChunkManager2D;
				if (chunk_manager == null)
					return false;

				chunk_go.AddComponent<ChunkController>();
				
				for (int l = 0; l < tile_map.layers.Count; ++l)
				{
					GameObject layer = new GameObject(tile_map.layers[l].name);
					Transform layer_transform = layer.GetComponent<Transform>();
					layer_transform.parent = chunk_go.GetComponent<Transform>();
					layer_transform.localPosition = Vector3.zero;
					layer_transform.localRotation = Quaternion.identity;
					layer_transform.localScale = Vector3.one;

					for (int y = 0; y < chunk_manager.chunk_size_y; ++y)
					{
						for (int x = 0; x < chunk_manager.chunk_size_x; ++x)
						{
							GameObject tile = RenderTile(tile_map, chunk.index_x, chunk.index_y, x, y, l, chunk.ids[(l * chunk_manager.chunk_size_y + y) * chunk_manager.chunk_size_x + x]);
							if (tile != null)
								Pb.Utility.Parent.PreserveLocal(tile.GetComponent<Transform>(), layer_transform);
						}
					}
				}

				return true;
			}
			/// <summary>
			/// Renders a single tile with the given parameters
			/// </summary>
			/// <param name="tile_map">The tile map to render the tile from</param>
			/// <param name="chunk_index_x">The X index of the chunk rendering the tile</param>
			/// <param name="chunk_index_y">The Y index of the chunk rendering the tile</param>
			/// <param name="x">The local X position of the tile in the chunk</param>
			/// <param name="y">The local Y position of the tile in the chunk</param>
			/// <param name="l">The layer index of the tile in the chunk</param>
			/// <param name="gid">The GID of the tile to render</param>
			/// <returns>The rendered tile</returns>
			public GameObject RenderTile(TileMap tile_map, int chunk_index_x, int chunk_index_y, int x, int y, int l, int gid)
			{
				int id = gid & 0x1FFFFFFF;
				bool flip_horiz = ((uint)gid & 0x80000000) == 0x80000000;
				bool flip_vert = ((uint)gid & 0x40000000) == 0x40000000;
				bool flip_diag = ((uint)gid & 0x20000000) == 0x20000000;
				if (flip_diag)
				{
					bool temp = flip_horiz;
					flip_horiz = !flip_vert;
					flip_vert = temp;
				}

				int local_id = 0;
				TileSet tile_set = tile_map.library.GetTileSetAndID(id, out local_id) as TileSet;
				if (tile_set == null)
					return null;

				TileInfo info = tile_set.GetTile<TileInfo>(local_id);

				if (info == null || info.sprite == null)
					return null;

				int pos_x = chunk_index_x * (tile_map.chunk_manager as ChunkManager2D).chunk_size_x + x;
				int pos_y = chunk_index_y * (tile_map.chunk_manager as ChunkManager2D).chunk_size_y + y;

				GameObject tile = new GameObject(x + "_" + y);
				Transform tile_transform = tile.GetComponent<Transform>();
				tile_transform.localPosition =
					tile_map.geometry.normalToMapMatrix.MultiplyPoint(
						tile_map.geometry.TileToNormal(pos_x, pos_y, 0) +
						new Vector3(0.5f, 0.5f, 0.0f)
					) +
					new Vector3(tile_set.draw_offset_x, tile_set.draw_offset_y, 0.0f);
				tile_transform.localRotation = Quaternion.Euler(0, 0, (flip_diag ? 90 : 0));
				tile_transform.localScale = new Vector3((flip_horiz ? -1.0f : 1.0f), (flip_vert ? -1.0f : 1.0f), 1.0f);

				SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
				sr.color = new Color(1.0f, 1.0f, 1.0f, tile_map.layers[l].default_alpha);
				sr.sprite = info.sprite;
				if (sprite_material != null)
					sr.material = sprite_material;
				sr.sortingLayerID = tile_map.layers[l].unity_sorting_layer_unique_id;
				sr.sortingLayerName = tile_map.layers[l].unity_sorting_layer_name;
				sr.sortingOrder = tile_map.geometry.TileSortingOrder(pos_x, pos_y, flip_x_precedence, flip_y_precedence);

				if (info.animation != null && info.animation.length != 0)
				{
					TileAnimator animator = tile.AddComponent<TileAnimator>();
					animator.tile_set = tile_set;
					animator.animation = info.animation;
				}
				
				return tile;
			}
		}
	}
}