using Pb.Collections;

namespace Pb.TileMap
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
	/// <summary>
	/// A generalization of directions and their properties in two dimensions
	/// </summary>
	public static class Dir2
	{
		/// <summary>
		/// The first value of the directions
		/// </summary>
		public static int Begin = 0;
		/// <summary>
		/// One past the last value of the directions
		/// </summary>
		public static int End = 4;
		/// <summary>
		/// The left direction
		/// </summary>
		public static int Left = 0;
		/// <summary>
		/// The right direction
		/// </summary>
		public static int Right = 1;
		/// <summary>
		/// The down direction
		/// </summary>
		public static int Down = 2;
		/// <summary>
		/// The up direction
		/// </summary>
		public static int Up = 3;
		/// <summary>
		/// The vector axes corresponding to the directions
		/// </summary>
		public static IVector2[] Axes = new IVector2[4]{
			IVector2.left,
			IVector2.right,
			IVector2.down,
			IVector2.up
		};
		/// <summary>
		/// The directions that are the reverse of the direction of their index
		/// </summary>
		public static int[] Reverse = new int[4]{
			1,
			0,
			3,
			2
		};
	}
	/// <summary>
	/// A generalization of directions and their properties in three dimensions
	/// </summary>
	public static class Dir3
	{
		/// <summary>
		/// The first value of the directions
		/// </summary>
		public static int Begin = 0;
		/// <summary>
		/// One past the last value of the directions
		/// </summary>
		public static int End = 6;
		/// <summary>
		/// The left direction
		/// </summary>
		public static int Left = 0;
		/// <summary>
		/// The right direction
		/// </summary>
		public static int Right = 1;
		/// <summary>
		/// The down direction
		/// </summary>
		public static int Down = 2;
		/// <summary>
		/// The up direction
		/// </summary>
		public static int Up = 3;
		/// <summary>
		/// The back direction
		/// </summary>
		public static int Back = 4;
		/// <summary>
		/// The forward direction
		/// </summary>
		public static int Forward = 5;
		/// <summary>
		/// The vector axes corresponding to the directions
		/// </summary>
		public static IVector3[] Axes = new IVector3[6]{
			IVector3.left,
			IVector3.right,
			IVector3.down,
			IVector3.up,
			IVector3.back,
			IVector3.forward
		};
		/// <summary>
		/// The directions that are the reverse of the direction of their index
		/// </summary>
		public static int[] Reverse = new int[6]{
			1,
			0,
			3,
			2,
			5,
			4
		};
	}
}