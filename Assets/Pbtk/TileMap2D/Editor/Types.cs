using UnityEditor;
using UnityEngine;

namespace Pbtk
{
	namespace TileMap2D
	{
		/// <summary>
		/// A drawer for property maps
		/// </summary>
		[CustomPropertyDrawer(typeof(PropertyMap))]
		public class PropertyMapDrawer :
			Pb.Collections.MapDrawer<string, string>
		{ }
	}
}