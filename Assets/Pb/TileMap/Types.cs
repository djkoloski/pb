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
		/// A single entry for a loaded chunk in a chunk map
		/// </summary>
		[System.Serializable]
		public class ChunkEntry
		{
			/// <summary>
			/// The chunk from the chunk manager
			/// </summary>
			public Chunk chunk;
			/// <summary>
			/// The gameobject acting as the chunk root
			/// </summary>
			public GameObject chunk_root;
			/// <summary>
			/// Basic constructor for a new chunk entry
			/// </summary>
			/// <param name="c">The chunk of the chunk entry</param>
			/// <param name="cr">The root gameobject of the chunk entry</param>
			public ChunkEntry(Chunk c, GameObject cr)
			{
				chunk = c;
				chunk_root = cr;
			}
		}
		/// <summary>
		/// A map from a set of chunk coordinates to chunk entries
		/// </summary>
		[System.Serializable]
		public class ChunkMap :
			Map<ITuple3, ChunkEntry>
		{ }
	}
}