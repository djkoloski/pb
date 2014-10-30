using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Pbtk
{
	namespace TileMap2D
	{
		/// <summary>
		/// A collection of utilities for making TileMap2D tile maps
		/// </summary>
		public static class ConverterUtility
		{
			/// <summary>
			/// Gets the name of the sprite asset corresponding to a size and index
			/// </summary>
			/// <param name="size_x">The width of the sprite</param>
			/// <param name="size_y">The height of the sprite</param>
			/// <param name="index">The index of the sprite</param>
			/// <returns>The name of the sprite asset</returns>
			public static string MakeTileSpriteAssetName(int size_x, int size_y, int index)
			{
				return "tile_" + size_x + "x" + size_y + "_" + index;
			}
			/// <summary>
			/// Parses the size and index from a sprite asset name
			/// </summary>
			/// <param name="asset_name">The name of the sprite asset</param>
			/// <param name="size_x">The width of the sprite</param>
			/// <param name="size_y">The height of the sprite</param>
			/// <param name="index">The index of the sprite</param>
			/// <returns>Whether the name was correctly parsed</returns>
			public static bool ParseTileSpriteAssetName(string asset_name, out int size_x, out int size_y, out int index)
			{
				string[] coords = asset_name.Split(new char[] { 'x', '_' });

				size_x = 0;
				size_y = 0;
				index = 0;

				if (coords.Length != 4)
					return false;

				if (coords[0] != "tile")
					return false;

				if (!int.TryParse(coords[1], out size_x) || !int.TryParse(coords[2], out size_y) || !int.TryParse(coords[3], out index))
					return false;

				return true;
			}
			/// <summary>
			/// Makes a new tile set from an image with the given parameters
			/// </summary>
			/// <param name="source_system_path">The path to the original image file</param>
			/// <param name="tile_set_dir">Where to put the image and tile set and look for them (asset path)</param>
			/// <param name="pixels_per_unit">The ratio between pixels and units</param>
			/// <param name="margin_x">The number of pixels around the edge of the image to cut around horizontally</param>
			/// <param name="margin_y">The number of pixels around the edge of the image to cut around vertically</param>
			/// <param name="spacing_x">The number of pixels between each tile horizontally</param>
			/// <param name="spacing_y">The number of pixels between each tile vertically</param>
			/// <param name="slice_size_x">The width of each tile</param>
			/// <param name="slice_size_y">The height of each tile</param>
			/// <param name="offset_x">The drawing offset along the X axis</param>
			/// <param name="offset_y">The drawing offset along the Y axis</param>
			/// <param name="transparent_color">A color to set to transparent in the image or a color with an alpha of 0 to indicate no transparent color</param>
			/// <param name="flip_x_indices">Whether to flip X indices from left to right</param>
			/// <param name="flip_y_indices">Whether to flip Y indices from bottom to top</param>
			/// <param name="force_rebuild">Whether to rebuild sprite metadata for sprites that already exist (usually false)</param>
			/// <returns>The newly-created tile set</returns>
			public static TileSet SliceTileSetFromImage(
				string source_system_path,
				string tile_set_dir,
				float pixels_per_unit,
				int margin_x, int margin_y,
				int spacing_x, int spacing_y,
				int slice_size_x, int slice_size_y,
				float offset_x, float offset_y,
				Color32 transparent_color,
				bool flip_x_indices,
				bool flip_y_indices,
				bool force_rebuild
				)
			{
				string image_path = Pb.Path.Combine(tile_set_dir, Path.GetFileName(source_system_path));
				string image_system_path = Pb.Path.AssetPathToSystemPath(image_path);
				string tile_set_name = Path.GetFileNameWithoutExtension(source_system_path) + "_" + slice_size_x + "x" + slice_size_y;
				string tile_set_path = Pb.Path.Combine(tile_set_dir, tile_set_name + ".asset");
				string tile_set_system_path = Pb.Path.AssetPathToSystemPath(tile_set_path);

				if (File.Exists(tile_set_system_path) && !force_rebuild)
					return AssetDatabase.LoadAssetAtPath(tile_set_path, typeof(TileSet)) as TileSet;

				if (!File.Exists(image_system_path))
				{
					File.Copy(source_system_path, image_system_path, false);
					AssetDatabase.Refresh();
					AssetDatabase.ImportAsset(image_path);
				}

				TextureImporter importer = AssetImporter.GetAtPath(image_path) as TextureImporter;

				importer.textureType = TextureImporterType.Sprite;
				importer.textureFormat = (transparent_color.a > 0 ? TextureImporterFormat.ARGB32 : TextureImporterFormat.AutomaticTruecolor);
				importer.spriteImportMode = SpriteImportMode.Multiple;
				importer.filterMode = FilterMode.Point;
				importer.spritePivot = Vector2.zero;
				importer.spritePixelsToUnits = pixels_per_unit;
				importer.isReadable = (transparent_color.a > 0);

				AssetDatabase.ImportAsset(image_path);

				Texture2D atlas = AssetDatabase.LoadAssetAtPath(image_path, typeof(Texture2D)) as Texture2D;

				int tiles_x = (atlas.width - 2 * margin_x + spacing_x) / (slice_size_x + spacing_x);
				int tiles_y = (atlas.height - 2 * margin_y + spacing_y) / (slice_size_y + spacing_y);
				int tile_count = tiles_x * tiles_y;
				int nudge_x = atlas.width + spacing_x - 2 * margin_x - tiles_x * (slice_size_x + spacing_x);
				int nudge_y = atlas.height + spacing_y - 2 * margin_y - tiles_y * (slice_size_y + spacing_y);

				TileSet tile_set = Pb.Utility.Asset.GetAndEdit<TileSet>(tile_set_path);

				tile_set.draw_offset = new Vector2(offset_x, offset_y);

				bool reimport_required = false;
				importer = AssetImporter.GetAtPath(image_path) as TextureImporter;

				importer.textureType = TextureImporterType.Sprite;
				importer.textureFormat = (transparent_color.a > 0 ? TextureImporterFormat.ARGB32 : TextureImporterFormat.AutomaticTruecolor);
				importer.spriteImportMode = SpriteImportMode.Multiple;
				importer.filterMode = FilterMode.Point;
				importer.spritePivot = Vector2.zero;
				importer.spritePixelsToUnits = pixels_per_unit;
				importer.isReadable = (transparent_color.a > 0);

				bool[] skip_tiles = new bool[tile_count];
				int add_tiles = tile_count;

				int add_index = 0;

				if (importer.spritesheet != null)
				{
					add_index = importer.spritesheet.Length;

					foreach (SpriteMetaData meta in importer.spritesheet)
					{
						int size_x = 0;
						int size_y = 0;
						int index = 0;

						if (!ParseTileSpriteAssetName(meta.name, out size_x, out size_y, out index))
							continue;

						if (size_x != slice_size_x || size_y != slice_size_y)
							continue;

						skip_tiles[index] = true;
						--add_tiles;
					}
				}

				if (add_tiles > 0)
				{
					SpriteMetaData[] new_spritesheet = new SpriteMetaData[add_index + add_tiles];
					System.Array.Copy(importer.spritesheet, new_spritesheet, add_index);

					for (int i = 0; i < tile_count; ++i)
					{
						if (skip_tiles[i])
							continue;

						int x = i % tiles_x;
						if (flip_x_indices)
							x = tiles_x - x - 1;
						int y = i / tiles_x;
						if (flip_y_indices)
							y = tiles_y - y - 1;

						SpriteMetaData new_meta = new SpriteMetaData();

						new_meta.alignment = (int)SpriteAlignment.Center;
						new_meta.name = MakeTileSpriteAssetName(slice_size_x, slice_size_y, i);
						new_meta.pivot = Vector2.zero;
						new_meta.rect =
							new Rect(
								margin_x + x * (slice_size_x + spacing_x) + nudge_x,
								margin_y + y * (slice_size_y + spacing_y) + nudge_y,
								slice_size_x,
								slice_size_y
							);

						new_spritesheet[add_index++] = new_meta;
					}

					importer.spritesheet = new_spritesheet;

					reimport_required = true;
				}

				if (reimport_required)
					AssetDatabase.ImportAsset(image_path);

				if (transparent_color.a > 0)
				{
					Color32[] pixels = atlas.GetPixels32();

					for (int i = 0; i < pixels.Length; ++i)
					{
						if (pixels[i].r == transparent_color.r && pixels[i].g == transparent_color.g && pixels[i].b == transparent_color.b && pixels[i].a == transparent_color.a)
							pixels[i] = new Color32(0, 0, 0, 0);
					}

					atlas.SetPixels32(pixels);
					atlas.Apply();

					File.WriteAllBytes(image_path, atlas.EncodeToPNG());

					importer.isReadable = false;
					AssetDatabase.ImportAsset(image_path);
				}

				while (tile_set.tiles.Count < tile_count)
					tile_set.tiles.Add(new TileInfo());

				Object[] assets = AssetDatabase.LoadAllAssetRepresentationsAtPath(image_path);

				foreach (Object asset in assets)
				{
					int size_x = 0;
					int size_y = 0;
					int index = 0;

					if (!ParseTileSpriteAssetName(asset.name, out size_x, out size_y, out index))
						continue;

					if (size_x != slice_size_x || size_y != slice_size_y)
						continue;

					tile_set.tiles[index].sprite = asset as Sprite;
				}

				return tile_set;
			}
			/// <summary>
			/// Builds a number of chunks in the Resources directory from the given data
			/// </summary>
			/// <param name="map_width">The width of the map</param>
			/// <param name="map_height">The height of the map</param>
			/// <param name="layers">The layers of IDs</param>
			/// <param name="chunk_size_x">The size of each chunk along the X axis</param>
			/// <param name="chunk_size_y">The size of each chunk along the Y axis</param>
			/// <param name="resources_name">The name (or path) of the folder to make inside the Resources directory</param>
			/// <returns>A StaticChunkManager that can load the newly-created chunks</returns>
			public static StaticChunkManager BuildStaticChunks(
				int map_width,
				int map_height,
				List<int[]> layers,
				int chunk_size_x,
				int chunk_size_y,
				string resources_name
				)
			{
				string resources_dir = Pb.Path.Combine("Assets/Resources", resources_name);
				string resources_system_dir = Pb.Path.AssetPathToSystemPath(resources_dir);

				if (Directory.Exists(resources_system_dir))
					Directory.Delete(resources_system_dir, true);

				Directory.CreateDirectory(resources_system_dir);

				int chunks_x = Pb.Math.CeilDivide(map_width, chunk_size_x);
				int chunks_y = Pb.Math.CeilDivide(map_height, chunk_size_y);

				int count = 0;
				for (int chunk_y = 0; chunk_y < chunks_y; ++chunk_y)
				{
					int pos_y = chunk_y * chunk_size_y;
					for (int chunk_x = 0; chunk_x < chunks_x; ++chunk_x)
					{
						string chunk_path = Pb.Path.Combine(resources_dir, "chunk_" + chunk_x + "_" + chunk_y + ".asset");
						Chunk chunk = Pb.Utility.Asset.GetAndEdit<Chunk>(chunk_path);

						chunk.index = new Pb.Collections.IVector2(chunk_x, chunk_y);
						chunk.ids = new int[layers.Count * chunk_size_y * chunk_size_x];

						int pos_x = chunk_x * chunk_size_x;

						int index = 0;
						for (int l = 0; l < layers.Count; ++l)
						{
							for (int y = 0; y < chunk_size_y; ++y)
							{
								for (int x = 0; x < chunk_size_x; ++x, ++index)
								{
									if (pos_x + x >= map_width || pos_y + y >= map_height)
										chunk.ids[index] = 0;
									else
										chunk.ids[index] = layers[l][(pos_y + y) * map_width + pos_x + x];
								}
							}
						}

						++count;
					}
				}

				AssetDatabase.Refresh();

				string chunk_manager_path = Pb.Path.Combine(resources_dir, "chunk_manager.asset");
				StaticChunkManager chunk_manager = Pb.Utility.Asset.GetAndEdit<StaticChunkManager>(chunk_manager_path);

				chunk_manager.chunk_size = new Pb.Collections.IVector2(chunk_size_x, chunk_size_y);
				chunk_manager.chunk_least = Pb.Collections.IVector2.zero;
				chunk_manager.chunk_greatest = new Pb.Collections.IVector2(chunks_x - 1, chunks_y - 1);
				chunk_manager.resources_path = resources_name;

				return chunk_manager;
			}
		}
	}
}