using UnityEngine;

namespace Pbtk
{
	namespace TileMap2D
	{
		/// <summary>
		/// A collection of information about each tile in each tile set
		/// </summary>
		[System.Serializable]
		public class TileInfo
		{
			/// <summary>
			/// The sprite the tile uses for rendering
			/// </summary>
			public Sprite sprite;
			/// <summary>
			/// The animation (if any) the tile has
			/// </summary>
			public TileAnimation animation;
			/// <summary>
			/// The generic properties of the tile
			/// </summary>
			public PropertyMap properties;
			/// <summary>
			/// Default constructor
			/// </summary>
			public TileInfo()
			{
				sprite = null;
				animation = null;
				properties = new PropertyMap();
			}
		}
	}
}