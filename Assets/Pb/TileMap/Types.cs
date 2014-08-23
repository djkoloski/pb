using UnityEngine;
using Pb.Collections;

namespace Pb
{
	namespace TileMap
	{
		/// <summary>
		/// A tuple of two integers
		/// </summary>
		[System.Serializable]
		public class ITuple2 :
			Tuple<int, int>
		{
			/// <summary>
			/// A basic constructor
			/// </summary>
			/// <param name="a">The first value in the tuple</param>
			/// <param name="b">The second value in the tuple</param>
			public ITuple2(int a, int b) :
				base(a, b)
			{ }
		}
		/// <summary>
		/// A tuple of three integers
		/// </summary>
		[System.Serializable]
		public class ITuple3 :
			Tuple<int, int, int>
		{
			/// <summary>
			/// A basic constructor
			/// </summary>
			/// <param name="a">The first value in the tuple</param>
			/// <param name="b">The second value in the tuple</param>
			/// <param name="c">The third value in the tuple</param>
			public ITuple3(int a, int b, int c) :
				base(a, b, c)
			{ }
		}
		/// <summary>
		/// A serializable map of tile sets
		/// </summary>
		[System.Serializable]
		public class TileSetMap :
			Map<int, TileSet>
		{ }
		/// <summary>
		/// A map from a set of chunk coordinates to loaded chunks
		/// </summary>
		[System.Serializable]
		public class LoadedChunkMap :
			Map<ITuple3, Chunk>
		{ }
		/// <summary>
		/// A map from a set of chunk coordinates to rendered chunks
		/// </summary>
		[System.Serializable]
		public class RenderedChunkMap :
			Map<ITuple3, GameObject>
		{ }
	}
}