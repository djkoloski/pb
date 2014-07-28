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
	}
}