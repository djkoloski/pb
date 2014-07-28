using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pbtk
{
	namespace TileMap2D
	{
		/// <summary>
		/// A basic tile set implementation
		/// </summary>
		[System.Serializable]
		public class TileSet :
			Pb.TileMap.GenericTileSet<TileInfo>
		{
			/// <summary>
			/// The offset to draw the tiles of the tile set with in pixels along the X axis
			/// </summary>
			public float draw_offset_x;
			/// <summary>
			/// The offset to draw the tiles of the tile set with in pixels along the Y axis
			/// </summary>
			public float draw_offset_y;
		}
	}
}