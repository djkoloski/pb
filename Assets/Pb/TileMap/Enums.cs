namespace Pb
{
	namespace TileMap
	{
		/// <summary>
		/// The orientations the axes of a tile map may assume
		/// </summary>
		public enum Orientation
		{
			/// <summary>
			/// Indicates that the X and Y axes point right and up respectively
			/// </summary>
			RightUp = 0,
			/// <summary>
			/// Indicates that the X and Y axes point left and up respectively
			/// </summary>
			LeftUp = 1,
			/// <summary>
			/// Indicates that the X and Y axes point right and down respectively
			/// </summary>
			RightDown = 2,
			/// <summary>
			/// Indicates that the X and Y axes point left and down respectively
			/// </summary>
			LeftDown = 3
		}
		/// <summary>
		/// The tilings that a tile map may use
		/// </summary>
		public enum Tiling
		{
			/// <summary>
			/// Indicates that tiles are rectangular
			/// </summary>
			Rectangular,
			/// <summary>
			/// Indicates that tiles are laid out isometrically
			/// </summary>
			Isometric,
			/// <summary>
			/// Indicates that tiles are laid out isometrically with staggered coordinates and odd rows are staggered in
			/// </summary>
			StaggeredOdd,
			/// <summary>
			/// Indicates that tiles are laid out isometrically with staggered coordinates and even rows are staggered in
			/// </summary>
			StaggeredEven
		}
	}
}