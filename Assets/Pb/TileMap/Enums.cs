namespace Pb
{
	namespace TileMap
	{
		/// <summary>
		/// The orientations the axes of a tile map may assume
		/// </summary>
		public enum Orientation
		{
			RightUp = 0,
			LeftUp = 1,
			RightDown = 2,
			LeftDown = 3
		}
		/// <summary>
		/// The tilings that a tile map may use
		/// </summary>
		public enum Tiling
		{
			Rectangular,
			Isometric,
			StaggeredOdd,
			StaggeredEven
		}
	}
}