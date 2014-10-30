using Pb.Collections;

namespace Pbtk
{
	namespace TileMap2D
	{
		/// <summary>
		/// A map from strings (properties) to strings (values)
		/// </summary>
		[System.Serializable]
		public class PropertyMap :
			Map<string, string>
		{ }
		/// <summary>
		/// A map from a frame time to a frame index
		/// </summary>
		[System.Serializable]
		public class FrameMap :
			Map<int, int>
		{ }
	}
}