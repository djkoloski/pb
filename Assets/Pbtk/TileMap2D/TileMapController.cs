using UnityEngine;

namespace Pbtk
{
	namespace TileMap2D
	{
		/// <summary>
		/// A tile map controller that adds gizmo drawing for 2D maps
		/// </summary>
		public class TileMapController :
			Pb.TileMap.TileMapController
		{
			/// <summary>
			/// A simple enum of when to draw tile gizmos
			/// </summary>
			public enum GizmosDrawTime
			{
				/// <summary>
				/// Indicates that gizmos should always be drawn
				/// </summary>
				Always,
				/// <summary>
				/// Indicates that gizmos should only be drawn when the controller is selected
				/// </summary>
				Selected,
				/// <summary>
				/// Indicates that gizmos should never be drawn
				/// </summary>
				Never
			}
			/// <summary>
			/// When to draw gizmos
			/// </summary>
			public GizmosDrawTime when_draw_gizmos = GizmosDrawTime.Always;
			/// <summary>
			/// Whether to draw tile boundaries
			/// </summary>
			public bool draw_tile_boundaries = true;
			/// <summary>
			/// Whether to draw chunk boundaries
			/// </summary>
			public bool draw_chunk_boundaries = true;
			/// <summary>
			/// The color to draw tile boundaries with
			/// </summary>
			public Color32 gizmo_color_tile = Color.gray;
			/// <summary>
			/// The color to draw chunk boundaries with
			/// </summary>
			public Color32 gizmo_color_chunk = Color.white;
			/// <summary>
			/// Draws gizmos if set to always
			/// </summary>
			public void OnDrawGizmos()
			{
				if (when_draw_gizmos == GizmosDrawTime.Always)
					DrawGizmos();
			}
			/// <summary>
			/// Draws gizmos if set to selected
			/// </summary>
			public void OnDrawGizmosSelected()
			{
				if (when_draw_gizmos == GizmosDrawTime.Selected)
					DrawGizmos();
			}
			/// <summary>
			/// Draws gizmos on the screen to show where the tiles of the map are
			/// </summary>
			public void DrawGizmos()
			{
				if (base.tile_map == null)
					return;
				TileMap tile_map = base.tile_map as TileMap;
				if (tile_map == null)
					return;

				if (tile_map.chunk_manager == null)
					return;
				ChunkManager2D chunk_manager = tile_map.chunk_manager as ChunkManager2D;
				if (chunk_manager == null)
					return;

				if (transform == null)
					Init();

				Gizmos.matrix = transform.localToWorldMatrix;
				tile_map.geometry.DrawGizmos(
					chunk_manager.chunk_size_x, chunk_manager.chunk_size_y,
					chunk_manager.chunk_left, chunk_manager.chunk_right, chunk_manager.chunk_bottom, chunk_manager.chunk_top,
					gizmo_color_tile, gizmo_color_chunk,
					draw_tile_boundaries, draw_chunk_boundaries
				);
			}
			/// <summary>
			/// Gets the ID of the tile at the given location
			/// </summary>
			/// <param name="x">The X coordinate of the tile</param>
			/// <param name="y">The Y coordinate of the tile</param>
			/// <param name="l">The layer of the tile</param>
			/// <returns>The ID of the tile</returns>
			public int GetTile(int x, int y, int l)
			{
				if (base.tile_map == null)
					throw new System.InvalidOperationException("Tile ID requested without a tile map to reference");
				TileMap tile_map = base.tile_map as TileMap;
				if (tile_map == null)
					throw new System.InvalidOperationException("Attached tile map is not a TileMap2D tile map");

				if (tile_map.chunk_manager == null)
					throw new System.InvalidOperationException("Tile ID requested without a chunk manager to reference");
				ChunkManager2D chunk_manager = tile_map.chunk_manager as ChunkManager2D;
				if (chunk_manager == null)
					throw new System.InvalidOperationException("Attached chunk manager is not a TileMap2D chunk manager");

				int chunk_x = Pb.Math.FloorDivide(x, chunk_manager.chunk_size_x);
				int chunk_y = Pb.Math.FloorDivide(y, chunk_manager.chunk_size_y);

				Chunk chunk = chunk_manager.GetChunk(chunk_x, chunk_y, 0) as Chunk;

				if (chunk == null)
					throw new System.ArgumentException("Given tile coordinates do not have a valid chunk associated with them");

				int local_x = x - chunk_x * chunk_manager.chunk_size_x;
				int local_y = y - chunk_y * chunk_manager.chunk_size_y;

				return chunk.ids[(l * chunk_manager.chunk_size_y + local_y) * chunk_manager.chunk_size_x + local_x];
			}
			/// <summary>
			/// Changes the tile at the given location in the chunk and rerenders the tile if the chunk is loaded
			/// </summary>
			/// <param name="x">The X coordinate of the tile</param>
			/// <param name="y">The Y coordinate of the tile</param>
			/// <param name="l">The layer of the tile</param>
			/// <param name="new_id">The ID to set the tile to</param>
			/// <param name="change_chunk">Whether to change the ID in the chunk as well</param>
			public void ChangeTile(int x, int y, int l, int new_id, bool change_chunk = true)
			{
				if (base.tile_map == null)
					throw new System.InvalidOperationException("Tile ID requested without a tile map to reference");
				TileMap tile_map = base.tile_map as TileMap;
				if (tile_map == null)
					throw new System.InvalidOperationException("Attached tile map is not a TileMap2D tile map");

				if (tile_map.chunk_manager == null)
					throw new System.InvalidOperationException("Tile ID requested without a chunk manager to reference");
				ChunkManager2D chunk_manager = tile_map.chunk_manager as ChunkManager2D;
				if (chunk_manager == null)
					throw new System.InvalidOperationException("Attached chunk manager is not a TileMap2D chunk manager");

				int chunk_x = Pb.Math.FloorDivide(x, chunk_manager.chunk_size_x);
				int chunk_y = Pb.Math.FloorDivide(y, chunk_manager.chunk_size_y);

				Chunk chunk = chunk_manager.GetChunk(chunk_x, chunk_y, 0) as Chunk;

				if (chunk == null)
					throw new System.ArgumentException("Given tile coordinates do not have a valid chunk associated with them");

				int local_x = x - chunk_x * chunk_manager.chunk_size_x;
				int local_y = y - chunk_y * chunk_manager.chunk_size_y;

				if (change_chunk)
					chunk.ids[(l * chunk_manager.chunk_size_y + local_y) * chunk_manager.chunk_size_x + local_x] = new_id;

				if (IsChunkLoaded(chunk_x, chunk_y, 0))
					GetChunk(chunk_x, chunk_y, 0).GetComponent<ChunkController>().RerenderTile(tile_map, chunk.index_x, chunk.index_y, local_x, local_y, l, new_id);
			}
		}
	}
}