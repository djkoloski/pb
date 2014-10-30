using Pb.Collections;

namespace Pb.TileMap
{
	/// <summary>
	/// A serializable map of tile sets
	/// </summary>
	[System.Serializable]
	public class TileSetMap :
		Map<int, TileSet>
	{ }
	/// <summary>
	/// A map from a set of chunk coordinates to loaded chunks
	/// </summary>
	[System.Serializable]
	public class LoadedChunkMap :
		Map<IVector3, Chunk>
	{ }
	/// <summary>
	/// A map from a set of chunk coordinates to rendered chunks
	/// </summary>
	[System.Serializable]
	public class RenderedChunkMap :
		Map<IVector3, UnityEngine.GameObject>
	{ }
}