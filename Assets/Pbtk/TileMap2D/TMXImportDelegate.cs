using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pbtk
{
	namespace TileMap2D
	{
		/// <summary>
		/// An import delegate for TMX files
		/// </summary>
		public abstract class TMXImportDelegate :
			ScriptableObject
		{
			/// <summary>
			/// Imports the object layers from a TMX file
			/// </summary>
			/// <param name="tile_map">The imported tile map</param>
			/// <param name="object_layers">The object layers from the tile map</param>
			public abstract void ImportObjects(TileMap tile_map, List<TMXObjectLayer> object_layers);
		}
	}
}