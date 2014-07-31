using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pb.Collections;

namespace Pb
{
	namespace TileMap
	{
		/// <summary>
		/// An interface for tile sets
		/// </summary>
		[System.Serializable]
		public abstract class TileSet :
			ScriptableObject
		{
			/// <summary>
			/// The number of tiles in the tile set
			/// </summary>
			public abstract int Count
			{ get; }
			/// <summary>
			/// Gets the data associated with a specific tile index
			/// </summary>
			/// <param name="index">The index of the tile</param>
			/// <returns>The data associated with that tile</returns>
			public abstract object GetTile(int index);
			/// <summary>
			/// Gets the data associated with a specific tile index with type contraints
			/// </summary>
			/// <typeparam name="T">The type of the object to return</typeparam>
			/// <param name="index">The index of the tile</param>
			/// <returns>The data associated with that tile</returns>
			public T GetTile<T>(int index) where T : class
			{
				return GetTile(index) as T;
			}
		}

		/// <summary>
		/// A generic implementation of a basic tile set
		/// </summary>
		/// <typeparam name="TileInfo">The data type associated with each tile</typeparam>
		[System.Serializable]
		public class GenericTileSet<TileInfo> :
			TileSet
		{
			/// <summary>
			/// The data associated with the tiles
			/// </summary>
			public List<TileInfo> tiles;
			/// <summary>
			/// The number of tiles in the tile set
			/// </summary>
			public override int Count
			{
				get
				{
					return tiles.Count;
				}
			}
			/// <summary>
			/// Gets the data associated with a specific tile index
			/// </summary>
			/// <param name="index">The index of the tile</param>
			/// <returns>The data associated with that tile</returns>
			public override object GetTile(int index)
			{
				return tiles[index];
			}
			/// <summary>
			/// Initializes the tile set when it is newly instantiated
			/// </summary>
			public void OnEnable()
			{
				if (tiles == null)
					tiles = new List<TileInfo>();
			}
		}
	}
}