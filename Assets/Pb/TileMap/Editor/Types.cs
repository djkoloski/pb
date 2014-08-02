using UnityEngine;
using UnityEditor;

namespace Pb
{
	namespace TileMap
	{
		/// <summary>
		/// A drawer for tile set maps
		/// </summary>
		[CustomPropertyDrawer(typeof(TileSetMap))]
		public class TileSetMapDrawer :
			Pb.Collections.MapDrawer<int, TileSet>
		{ }
		/// <summary>
		/// A drawer for chunk maps
		/// </summary>
		[CustomPropertyDrawer(typeof(ChunkMap))]
		public class ChunkMapDrawer :
			Pb.Collections.MapDrawer<ITuple3, GameObject>
		{ }
	}
}