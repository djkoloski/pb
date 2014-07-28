using System.Collections;
using System.Collections.Generic;
using Pb.Collections;

namespace Pb
{
	namespace TileMap
	{
		/// <summary>
		/// Maintains a list of tile sets and maps their local tiles to library IDs
		/// </summary>
		[System.Serializable]
		public class TileLibrary
		{
			/// <summary>
			/// A serializable map of tile sets
			/// </summary>
			[System.Serializable]
			public class TileSetMap :
				Map<int, TileSet>
			{ }
			/// <summary>
			/// The tile sets in the library
			/// </summary>
			public TileSetMap tile_sets;
			/// <summary>
			/// Default constructor
			/// </summary>
			public TileLibrary()
			{
				tile_sets = new TileSetMap();
			}
			/// <summary>
			/// Gets the tile set containing the given ID
			/// </summary>
			/// <param name="id">The ID to look for a matching tile set of</param>
			/// <returns>The tile set, or null if none match</returns>
			public TileSet GetTileSet(int id)
			{
				int local_id = 0;
				return GetTileSetAndID(id, out local_id);
			}
			/// <summary>
			/// Gets the tile set containing the given ID and the local ID of that tile
			/// </summary>
			/// <param name="id">The ID to look for a matching tile set of</param>
			/// <param name="tile_id">The local ID of the tile</param>
			/// <returns>The tile set, or null if none match</returns>
			public TileSet GetTileSetAndID(int id, out int local_id)
			{
				int index = tile_sets.Keys.BinarySearch(id);
				if (index < 0)
					index = ~index - 1;

				if (index < 0)
				{
					local_id = 0;
					return null;
				}

				local_id = id - tile_sets.Keys[index];

				if (local_id >= tile_sets.Values[index].Count)
				{
					local_id = 0;
					return null;
				}

				return tile_sets.Values[index];
			}
			/// <summary>
			/// Gets the tile information associated with a specific ID
			/// </summary>
			/// <param name="id">The ID of the tile in the library</param>
			/// <returns>The information associated with the ID</returns>
			public object GetTileInfo(int id)
			{
				return GetTileInfo<object>(id);
			}
			/// <summary>
			/// Gets the tile information associated with a specific ID
			/// </summary>
			/// <typeparam name="T">The type of the information to return</typeparam>
			/// <param name="id">The ID of the tile in the library</param>
			/// <returns>The information associated with the ID</returns>
			public T GetTileInfo<T>(int id) where T : class
			{
				int local_id = 0;
				TileSet tile_set = GetTileSetAndID(id, out local_id);
				return tile_set.GetTile<T>(local_id);
			}
			/// <summary>
			/// Adds a new tile set to the library
			/// </summary>
			/// <param name="tile_set"></param>
			/// <returns>The first ID of the added tile set or -1 if the tile set was not added</returns>
			public int AddTileSet(TileSet tile_set)
			{
				if (tile_sets.Count == 0)
				{
					tile_sets.Add(0, tile_set);
					return 0;
				}
				else
				{
					int last_index = tile_sets.Keys.Count - 1;
					int first_id = tile_sets.Keys[last_index] + tile_sets.Values[last_index].Count;
					tile_sets.Add(first_id, tile_set);
					return first_id;
				}
			}
			/// <summary>
			/// Adds a new tile set to the library with a positive desired first ID
			/// </summary>
			/// <param name="tile_set">The tile set to add</param>
			/// <param name="first_id">The desired first ID of the tile set</param>
			/// <returns>The first ID of the added tile set or -1 if the tile set was not added</returns>
			public int AddTileSet(TileSet tile_set, int first_id)
			{
				if (first_id < 0)
					throw new System.ArgumentException();

				if (tile_sets.Count == 0)
					tile_sets.Add(first_id, tile_set);
				else
				{
					int nearest_index = tile_sets.Keys.BinarySearch(first_id);
					if (nearest_index >= 0)
						return -1;
					nearest_index = ~nearest_index - 1;

					if (nearest_index >= 0 && tile_sets.Keys[nearest_index] + tile_sets.Values[nearest_index].Count > first_id)
						return -1;
					if (nearest_index < tile_sets.Keys.Count - 1 && first_id + tile_set.Count > tile_sets.Keys[nearest_index + 1])
						return -1;

					tile_sets[first_id] = tile_set;
				}

				return first_id;
			}
		}
	}
}