using UnityEngine;
using Pb.Collections;

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
					new IVector3(chunk_manager.chunk_size.x, chunk_manager.chunk_size.y, 1),
					(IVector3)chunk_manager.chunk_least, (IVector3)chunk_manager.chunk_greatest,
					gizmo_color_tile, gizmo_color_chunk,
					draw_tile_boundaries, draw_chunk_boundaries
				);
			}
			/// <summary>
			/// Gets the ID of the tile at the given location
			/// </summary>
			/// <param name="v">The coordinates of the tile</param>
			/// <param name="l">The layer of the tile</param>
			/// <returns>The ID of the tile</returns>
			public int GetTile(IVector2 v, int l)
			{
				if (base.tile_map == null)
					throw new System.InvalidOperationException("Tile ID requested without a tile map to reference");
				TileMap tile_map = base.tile_map as TileMap;
				if (tile_map == null)
					throw new System.InvalidOperationException("Attached tile map is not a TileMap2D tile map");

				if (tile_map.chunk_manager == null)
					throw new System.InvalidOperationException("Tile ID requested without a chunk manager to reference");
				ChunkManager2D chunk_manager = (ChunkManager2D)tile_map.chunk_manager;
				if (chunk_manager == null)
					throw new System.InvalidOperationException("Attached chunk manager is not a TileMap2D chunk manager");

				IVector2 chunk_index = Pb.Math.FloorDivide(v, chunk_manager.chunk_size);

				if (!IsChunkLoaded((IVector3)chunk_index))
					throw new System.InvalidOperationException("The chunk containing the requested tile is not loaded");

				Chunk chunk = (Chunk)GetChunk((IVector3)chunk_index);

				if (chunk == null)
					throw new System.ArgumentException("Given tile coordinates do not have a valid chunk associated with them");

				IVector2 local = v - IVector2.Scale(chunk_index, chunk_manager.chunk_size);

				return chunk.ids[IVector2.ToIndex(local, chunk_manager.chunk_size)];
			}
			/// <summary>
			/// Changes the tile at the given location in the chunk and rerenders the tile if the chunk is loaded
			/// </summary>
			/// <param name="v">The coordinates of the tile</param>
			/// <param name="l">The layer of the tile</param>
			/// <param name="new_id">The ID to set the tile to</param>
			/// <param name="change_chunk">Whether to change the ID in the chunk as well</param>
			public void ChangeTile(IVector2 v, int l, int new_id, bool change_chunk = true)
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

				IVector2 chunk_index = Pb.Math.FloorDivide(v, chunk_manager.chunk_size);

				if (!IsChunkLoaded((IVector3)chunk_index))
					throw new System.InvalidOperationException("The chunk containing the requested tile is not loaded");

				Chunk chunk = (Chunk)GetChunk((IVector3)chunk_index);

				if (chunk == null)
					throw new System.ArgumentException("Given tile coordinates do not have a valid chunk associated with them");

				IVector2 local = v - IVector2.Scale(chunk_index, chunk_manager.chunk_size);

				if (change_chunk)
					chunk.ids[IVector2.ToIndex(local, chunk_manager.chunk_size)] = new_id;

				GetChunkRoot((IVector3)chunk_index).GetComponent<ChunkController>().RerenderTile(tile_map, chunk.index, local, l, new_id);
			}
		}
	}
}