using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pbtk
{
	namespace TileMap2D
	{
		/// <summary>
		/// A basic chunk implementation
		/// </summary>
		[System.Serializable]
		public class Chunk :
			Pb.TileMap.Chunk
		{
			/// <summary>
			/// The X index of the chunk
			/// </summary>
			public int index_x;
			/// <summary>
			/// The Y index of the chunk
			/// </summary>
			public int index_y;
			/// <summary>
			/// The IDs of the tiles in order X, then Y, then Z
			/// </summary>
			public int[] ids;
		}
	}
}