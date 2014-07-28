namespace Pbtk
{
	namespace TileMap2D
	{
		/// <summary>
		/// A collection of the properties of a TileMap2D layer
		/// </summary>
		[System.Serializable]
		public class LayerInfo
		{
			/// <summary>
			/// The name of the layer
			/// </summary>
			public string name;
			/// <summary>
			/// The default alpha value of all tiles in the layer
			/// </summary>
			public float default_alpha;
			/// <summary>
			/// The properties of the layer
			/// </summary>
			public PropertyMap properties;
			/// <summary>
			/// The name of the sorting layer this layer uses
			/// </summary>
			public string unity_sorting_layer_name;
			/// <summary>
			/// The unique ID of the sorting layer this layer uses
			/// </summary>
			public int unity_sorting_layer_unique_id;
			/// <summary>
			/// Sets the name of the layer to "undefined" and the default alpha to 1.0f
			/// </summary>
			public LayerInfo()
			{
				name = "undefined";
				default_alpha = 1.0f;
				properties = new PropertyMap();
			}
		}
	}
}