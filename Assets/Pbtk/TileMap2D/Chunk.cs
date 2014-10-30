using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pb.Collections;

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
			/// The index of the chunk
			/// </summary>
			public IVector2 index;
			/// <summary>
			/// The IDs of the tiles in order X, then Y, then Z
			/// </summary>
			public int[] ids;
		}
	}
}