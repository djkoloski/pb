using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using Pb.Collections;

namespace Pbtk
{
	namespace TileMap2D
	{
		/// <summary>
		/// Converts TMX files into TileMap2D tile maps
		/// </summary>
		public static class TMXConverter
		{
			private static PropertyMap ReadProperties(XmlReader reader)
			{
				PropertyMap properties = new PropertyMap();

				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "properties")
						break;

					if (reader.NodeType == XmlNodeType.Element && reader.Name == "property")
					{
						string name = reader.GetAttribute("name");
						string val = reader.GetAttribute("value");
						if (val == null)
							val = reader.ReadElementContentAsString();
						properties.Add(name, val);
					}
				}

				return properties;
			}
			private static TileAnimation ReadTileAnimation(XmlReader reader)
			{
				TileAnimation animation = new TileAnimation();

				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "animation")
						break;

					if (reader.NodeType == XmlNodeType.Element && reader.Name == "frame")
						animation.AddFrame(int.Parse(reader.GetAttribute("tileid")), int.Parse(reader.GetAttribute("duration")));
				}

				return animation;
			}
			private static int GetIntAttribute(XmlReader reader, string name, int default_value)
			{
				string attr = reader.GetAttribute(name);
				if (attr == null)
					return default_value;
				return int.Parse(attr);
			}
			private static float GetFloatAttribute(XmlReader reader, string name, float default_value)
			{
				string attr = reader.GetAttribute(name);
				if (attr == null)
					return default_value;
				return float.Parse(attr);
			}
			private static TileSet ReadTileSet(XmlReader reader, string tile_sets_dir, float pixels_per_unit, bool force_rebuild_tile_sets, string relative_dir)
			{
				int slice_size_x = int.Parse(reader.GetAttribute("tilewidth"));
				int slice_size_y = int.Parse(reader.GetAttribute("tileheight"));
				int spacing = GetIntAttribute(reader, "spacing", 0);
				int margin = GetIntAttribute(reader, "margin", 0);
				float draw_offset_x = 0;
				float draw_offset_y = 0;

				TileSet tile_set = null;

				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "tileset")
						break;

					if (reader.NodeType == XmlNodeType.Element)
					{
						switch (reader.Name)
						{
							case "tileoffset":
								draw_offset_x = GetIntAttribute(reader, "x", 0) / pixels_per_unit;
								draw_offset_y = GetIntAttribute(reader, "y", 0) / pixels_per_unit;
								break;
							case "image":
								string image_path = Pb.Path.Combine(relative_dir, reader.GetAttribute("source"));

								Color32 transparent_color = new Color32(0, 0, 0, 0);
								string transparent_attr = reader.GetAttribute("trans");
								if (transparent_attr != null)
									transparent_color = Pb.Math.HexToRGB(transparent_attr);

								tile_set = ConverterUtility.SliceTileSetFromImage(
									image_path,
									tile_sets_dir,
									pixels_per_unit,
									margin, margin,
									spacing, spacing,
									slice_size_x, slice_size_y,
									draw_offset_x, draw_offset_y,
									transparent_color,
									false,
									true,
									force_rebuild_tile_sets
									);

								break;
							case "tile":
								int id = GetIntAttribute(reader, "id", 0);

								if (!reader.IsEmptyElement)
								{
									while (reader.Read())
									{
										if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "tile")
											break;

										if (reader.NodeType == XmlNodeType.Element)
										{
											switch (reader.Name)
											{
												case "animation":
													tile_set.tiles[id].animation = ReadTileAnimation(reader);
													break;
												case "properties":
													tile_set.tiles[id].properties = ReadProperties(reader);
													break;
												default:
													break;
											}
										}
									}
								}

								break;
							default:
								break;
						}
					}
				}

				return tile_set;
			}
			/// <summary>
			/// Converts a TMX file into a TileMap2D tile map and returns the map
			/// </summary>
			/// <param name="input_system_path">The path to the TMX file</param>
			/// <param name="output_name">The name of the output tile map</param>
			/// <param name="tile_sets_dir">The directory to look for tile sets in</param>
			/// <param name="pixels_per_unit">The number of pixels per unit to render the tile sets at</param>
			/// <param name="chunk_size_x">The size of each chunk along the X axis</param>
			/// <param name="chunk_size_y">The size of each chunk along the Y axis</param>
			/// <param name="force_rebuild_tile_sets">Whether to force tile sets to be rebuilt</param>
			/// <returns>The converted TileMap2D tile map</returns>
			public static TileMap ConvertTMX(
				string input_system_path,
				string output_name,
				string tile_sets_dir,
				float pixels_per_unit,
				int chunk_size_x,
				int chunk_size_y,
				bool force_rebuild_tile_sets,
				TMXImportDelegate import_delegate
				)
			{
				string input_system_dir = Path.GetDirectoryName(input_system_path);
				string output_path = Pb.Path.Combine("Assets", output_name + ".asset");

				TileMap tile_map = Pb.Utility.Asset.GetAndEdit<TileMap>(output_path);

				List<int[]> layers = new List<int[]>();
				int map_width = 0;
				int map_height = 0;

				List<TMXObjectLayer> object_layers = new List<TMXObjectLayer>();

				XmlReader reader = XmlReader.Create(input_system_path);

				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						switch (reader.Name)
						{
							case "map":
								map_width = GetIntAttribute(reader, "width", map_width);
								map_height = GetIntAttribute(reader, "height", map_height);

								if (chunk_size_x < 1)
									chunk_size_x = map_width;
								if (chunk_size_y < 1)
									chunk_size_y = map_height;

								switch (reader.GetAttribute("orientation"))
								{
									case "orthogonal":
										tile_map.geometry.tiling = Pb.TileMap.Tiling.Rectangular;
										break;
									case "isometric":
										tile_map.geometry.tiling = Pb.TileMap.Tiling.Isometric;
										break;
									case "staggered":
										tile_map.geometry.tiling = Pb.TileMap.Tiling.StaggeredOdd;
										break;
									default:
										throw new System.ArgumentException("Invalid TMX attribute 'orientation'");
								}

								tile_map.geometry.orientation = Pb.TileMap.Orientation.RightDown;
								tile_map.geometry.size = new Vector3(GetIntAttribute(reader, "tilewidth", 1) / pixels_per_unit, GetIntAttribute(reader, "tileheight", 1) / pixels_per_unit, 1.0f);

								break;
							case "properties":
								tile_map.properties = ReadProperties(reader);
								break;
							case "tileset":
								int first_id = GetIntAttribute(reader, "firstgid", 0);
								string source = reader.GetAttribute("source");

								TileSet tile_set = null;

								if (source == null)
									tile_set = ReadTileSet(reader, tile_sets_dir, pixels_per_unit, force_rebuild_tile_sets, input_system_dir);
								else
								{
									string tsx_system_path = Pb.Path.Combine(input_system_dir, source);
									string tsx_system_dir = Path.GetDirectoryName(tsx_system_path);
									XmlReader tsx_reader = XmlReader.Create(tsx_system_path);

									while (tsx_reader.Read())
										if (tsx_reader.NodeType == XmlNodeType.Element)
											if (tsx_reader.Name == "tileset")
												tile_set = ReadTileSet(tsx_reader, tile_sets_dir, pixels_per_unit, force_rebuild_tile_sets, tsx_system_dir);
								}

								if (tile_map.library.AddTileSet(tile_set, first_id) < 0)
									throw new System.ArgumentException("Conflicting first GIDs for tilesets!");

								break;
							case "layer":
								LayerInfo info = new LayerInfo();

								info.name = reader.GetAttribute("name");
								info.default_alpha = GetFloatAttribute(reader, "opacity", 1.0f);
								info.unity_sorting_layer_name = Pb.Utility.SortingLayers.StandardSortingLayerName(layers.Count);
								info.unity_sorting_layer_unique_id = Pb.Utility.SortingLayers.StandardSortingLayerID(layers.Count);

								int[] ids = new int[map_height * map_width];

								while (reader.Read())
								{
									if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "layer")
										break;

									if (reader.NodeType == XmlNodeType.Element)
									{
										switch (reader.Name)
										{
											case "data":
												string encoding = reader.GetAttribute("encoding");
												string compression = reader.GetAttribute("compression");

												if (encoding == "base64")
												{
													int bytes_total = 4 * map_height * map_width;
													byte[] buffer = new byte[bytes_total];
													string base64 = reader.ReadElementContentAsString();
													byte[] input = System.Convert.FromBase64String(base64);

													if (compression == "zlib")
														Pb.Utility.Decompress.Zlib(input, buffer, bytes_total);
													else if (compression == "gzip")
														Pb.Utility.Decompress.GZip(input, buffer, bytes_total);
													else
														buffer = input;

													for (int i = 0; i < map_height * map_width; ++i)
														ids[i] =
															buffer[4 * i] |
															(buffer[4 * i + 1] << 8) |
															(buffer[4 * i + 2] << 16) |
															(buffer[4 * i + 3] << 24);
												}
												else if (encoding == "csv")
												{
													string[] indices = reader.ReadElementContentAsString().Split(new char[] { ',' });

													for (int i = 0; i < map_height * map_width; ++i)
														ids[i] = (int)uint.Parse(indices[i]);
												}
												else
												{
													int i = 0;

													while (reader.Read())
													{
														if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "data")
															break;

														if (reader.NodeType == XmlNodeType.Element && reader.Name == "tile")
															ids[i++] = (int)uint.Parse(reader.GetAttribute("gid"));
													}
												}

												break;
											case "properties":
												info.properties = ReadProperties(reader);
												break;
											default:
												break;
										}
									}
								}

								layers.Add(ids);
								tile_map.layers.Add(info);

								break;
							case "objectgroup":
								TMXObjectLayer object_layer = new TMXObjectLayer();

								object_layer.name = reader.GetAttribute("name");
								object_layer.default_alpha = GetFloatAttribute(reader, "opacity", 1.0f);

								while (reader.Read())
								{
									if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "objectgroup")
										break;

									if (reader.NodeType == XmlNodeType.Element)
									{
										switch (reader.Name)
										{
											case "object":
												TMXObject obj = new TMXObject();
												obj.name = reader.GetAttribute("name");
												obj.rotation = GetFloatAttribute(reader, "rotation", 0.0f);
												IVector2 position = new IVector2(GetIntAttribute(reader, "x", 0), GetIntAttribute(reader, "y", 0));
												IVector2 size = new IVector2(GetIntAttribute(reader, "width", 0), GetIntAttribute(reader, "height", 0));

												if (!reader.IsEmptyElement)
												{
													while (reader.Read())
													{
														if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "object")
															break;

														if (reader.NodeType == XmlNodeType.Element)
														{
															switch (reader.Name)
															{
																case "ellipse":
																	obj.type = TMXObjectType.Ellipse;
																	obj.points.Add(position);
																	obj.points.Add(size);
																	break;
																case "polygon":
																	{
																		obj.type = TMXObjectType.Polygon;
																		string points_str = reader.GetAttribute("points");
																		foreach (string point_str in points_str.Split(new char[] { ' ' }))
																		{
																			string[] coords = point_str.Split(new char[] { ',' });
																			obj.points.Add(position + new IVector2(int.Parse(coords[0]), int.Parse(coords[1])));
																		}
																		break;
																	}
																case "polyline":
																	{
																		obj.type = TMXObjectType.Polyline;
																		string points_str = reader.GetAttribute("points");
																		foreach (string point_str in points_str.Split(new char[] { ' ' }))
																		{
																			string[] coords = point_str.Split(new char[] { ',' });
																			obj.points.Add(position + new IVector2(int.Parse(coords[0]), int.Parse(coords[1])));
																		}
																		break;
																	}
																case "properties":
																	obj.properties = ReadProperties(reader);
																	break;
															}
														}
													}
												}

												if (obj.type == TMXObjectType.None)
												{
													obj.type = TMXObjectType.Rectangle;
													obj.points.Add(position);
													obj.points.Add(size);
												}

												object_layer.objects.Add(obj);
												break;
											case "properties":
												object_layer.properties = ReadProperties(reader);
												break;
											default:
												break;
										}
									}
								}

								object_layers.Add(object_layer);

								break;
						}
					}
				}

				tile_map.chunk_manager = ConverterUtility.BuildStaticChunks(map_width, map_height, layers, chunk_size_x, chunk_size_y, output_name);
				tile_map.chunk_renderer = Pb.Utility.Asset.GetAndEdit<ChunkRenderer>(Pb.Path.Combine("Assets", output_name + "_renderer.asset"));

				import_delegate.ImportObjects(tile_map, object_layers);

				return tile_map;
			}
		}
	}
}
