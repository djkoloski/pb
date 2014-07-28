using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pbtk
{
	namespace TileMap2D
	{
		/// <summary>
		/// A basic tile map implementation
		/// </summary>
		[System.Serializable]
		public class TileMap :
			Pb.TileMap.BasicTileMap
		{
			/// <summary>
			/// Information about each of the layers in the tile map
			/// </summary>
			public List<LayerInfo> layers;
			/// <summary>
			/// Generic properties of the tile map
			/// </summary>
			public PropertyMap properties;
			/// <summary>
			/// Initializes the tile map when it is first created
			/// </summary>
			public new void OnEnable()
			{
				base.OnEnable();

				if (layers == null)
					layers = new List<LayerInfo>();
				if (properties == null)
					properties = new PropertyMap();
			}
		}
	}
}