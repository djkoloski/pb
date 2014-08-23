using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pb
{
	namespace TileMap
	{
		/// <summary>
		/// Manages and provides the geometry of a tile map
		/// </summary>
		[System.Serializable]
		public class Geometry
		{
			/// <summary>
			/// The tiling of the tile map
			/// </summary>
			public Tiling tiling;
			/// <summary>
			/// The orientation of the tile map
			/// </summary>
			public Orientation orientation;
			/// <summary>
			/// The size of each tile in renorm space
			/// </summary>
			public Vector3 size;
			/// <summary>
			/// Basic constructor
			/// </summary>
			/// <param name="t">The tiling of the tile map</param>
			/// <param name="o">The orientation of the tile map</param>
			/// <param name="s">The size of each tile in renorm space</param>
			public Geometry(Tiling t, Orientation o, Vector3 s)
			{
				tiling = t;
				orientation = o;
				size = s;
			}
			/// <summary>
			/// Gets the normal position of a tile with the given coordinates
			/// </summary>
			/// <param name="x">The X coordinate of the tile</param>
			/// <param name="y">The Y coordinate of the tile</param>
			/// <param name="z">The Z coordinate of the tile</param>
			/// <returns>The normal position of the desired tile</returns>
			public Vector3 TileToNormal(int x, int y, int z)
			{
				switch (tiling)
				{
					case Tiling.Rectangular:
					case Tiling.Isometric:
						return new Vector3(x, y, z);
					case Tiling.StaggeredEven:
						return new Vector3(x - y / 2 - y % 2, x + y / 2, z);
					case Tiling.StaggeredOdd:
						return new Vector3(x - y / 2, x + y / 2 + y % 2, z);
					default:
						throw new System.InvalidOperationException("Unsupported tile map tiling");
				}
			}
			/// <summary>
			/// Gets the tile under the given normal position
			/// </summary>
			/// <param name="normal">A point in normal space on top of the tile</param>
			/// <param name="x">The X coordinate of the tile</param>
			/// <param name="y">The Y coordinate of the tile</param>
			/// <param name="z">The Z coordinate of the tile</param>
			public void NormalToTile(Vector3 normal, out int x, out int y, out int z)
			{
				switch (tiling)
				{
					case Tiling.Rectangular:
					case Tiling.Isometric:
					{
						x = Mathf.FloorToInt(normal.x);
						y = Mathf.FloorToInt(normal.y);
						z = Mathf.FloorToInt(normal.z);
						break;
					}
					case Tiling.StaggeredEven:
					{
						int near_x = Mathf.FloorToInt(normal.x);
						int near_y = Mathf.FloorToInt(normal.y);
						x = Math.CeilDivide(near_x + near_y, 2);
						y = near_y - near_x;
						z = Mathf.FloorToInt(normal.z);
						break;
					}
					case Tiling.StaggeredOdd:
					{
						int near_x = Mathf.FloorToInt(normal.x);
						int near_y = Mathf.FloorToInt(normal.y);
						x = Math.FloorDivide(near_x + near_y, 2);
						y = near_y - near_x;
						z = Mathf.FloorToInt(normal.z);
						break;
					}
					default:
						throw new System.InvalidOperationException("Unsupported tile map tiling");
				}
			}
			/// <summary>
			/// Gets the sorting order of a tile
			/// </summary>
			/// <param name="x">The X position of the tile</param>
			/// <param name="y">The Y position of the tile</param>
			/// <param name="flip_x">Whether to flip precedence along the X axis</param>
			/// <param name="flip_y">Whether to flip precedence along the Y axis</param>
			/// <returns>The sorting order of the tile</returns>
			public int TileSortingOrder(int x, int y, bool flip_x, bool flip_y)
			{
				int sx = (flip_x ? -1 : 1);
				int sy = (flip_y ? -1 : 1);

				switch (tiling)
				{
					case Tiling.Rectangular:
					case Tiling.Isometric:
						return x * sx + y * sy;
					case Tiling.StaggeredEven:
						return x * (sx + sy) + (y / 2) * (sy - sx) - (y > 0 ? 1 : -1) * (y % 2 == 0 ? 0 : sx);
					case Tiling.StaggeredOdd:
						return x * (sx + sy) + (y / 2) * (sy - sx) + (y > 0 ? 1 : -1) * (y % 2 == 0 ? 0 : sy);
					default:
						throw new System.InvalidOperationException("Unsupported tile map tiling");
				}
			}
			/// <summary>
			/// The matrix that transforms normal space into renorm space
			/// </summary>
			public Matrix4x4 normalToRenormMatrix
			{
				get
				{
					Matrix4x4 scale = Matrix4x4.Scale(
						   new Vector3(
							   (orientation == Orientation.RightUp || orientation == Orientation.RightDown ? 1.0f : -1.0f),
							   (orientation == Orientation.RightUp || orientation == Orientation.LeftUp ? 1.0f : -1.0f),
							   1.0f
						   )
					   );

					switch (tiling)
					{
						case Tiling.Rectangular:
							return scale;
						case Tiling.Isometric:
						case Tiling.StaggeredEven:
						case Tiling.StaggeredOdd:
							Matrix4x4 iso = new Matrix4x4();
							iso.SetColumn(0, new Vector4(0.5f, -0.5f, 0.0f, 0.0f));
							iso.SetColumn(1, new Vector4(0.5f, 0.5f, 0.0f, 0.0f));
							iso.SetColumn(2, new Vector4(0.0f, 0.0f, 1.0f, 0.0f));
							iso.SetColumn(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));

							if (tiling == Tiling.Isometric)
								return iso * scale;
							else
								return scale * iso;
						default:
							throw new System.InvalidOperationException("Unsupported tile map tiling");
					}
				}
			}
			/// <summary>
			/// The matrix that transforms renorm space into normal space
			/// </summary>
			public Matrix4x4 renormToNormalMatrix
			{
				get
				{
					return normalToRenormMatrix.inverse;
				}
			}
			/// <summary>
			/// The matrix that transforms renorm space into map space
			/// </summary>
			public Matrix4x4 renormToMapMatrix
			{
				get
				{
					return Matrix4x4.Scale(size);
				}
			}
			/// <summary>
			/// The matrix that transforms map space into renorm space
			/// </summary>
			public Matrix4x4 mapToRenormMatrix
			{
				get
				{
					return renormToMapMatrix.inverse;
				}
			}
			/// <summary>
			/// The matrix that transforms normal space into map space
			/// </summary>
			public Matrix4x4 normalToMapMatrix
			{
				get
				{
					return renormToMapMatrix * normalToRenormMatrix;
				}
			}
			/// <summary>
			/// The matrix that transforms map space into normal space
			/// </summary>
			public Matrix4x4 mapToNormalMatrix
			{
				get
				{
					return normalToMapMatrix.inverse;
				}
			}
			/// <summary>
			/// Sets the color of the drawing gizmo based on whether a chunk boundary is being drawn
			/// </summary>
			/// <param name="param">Whether a chunk boundary is being drawn</param>
			/// <param name="gizmos_color_tile">The color to draw tile boundaries</param>
			/// <param name="gizmos_color_chunk">The color to draw chunk boundaries</param>
			/// <param name="draw_tiles">Whether to draw tile boundaries</param>
			/// <param name="draw_chunks">Whether to draw chunk boundaries</param>
			/// <returns>Whether to draw the boundary</returns>
			private bool SetGizmosColor(bool param, Color gizmos_color_tile, Color gizmos_color_chunk, bool draw_tiles, bool draw_chunks)
			{
				if (draw_chunks && param)
					Gizmos.color = gizmos_color_chunk;
				else if (draw_tiles)
					Gizmos.color = gizmos_color_tile;
				else
					return false;
				return true;
			}
			/// <summary>
			/// Draws a line-only version of the tile map
			/// </summary>
			/// <param name="size_x">The size of each chunk along the X axis</param>
			/// <param name="size_y">The size of each chunk along the Y axis</param>
			/// <param name="size_z">The size of each chunk along the Z axis</param>
			/// <param name="chunk_left">The X coordinates of the leftmost chunks</param>
			/// <param name="chunk_right">The X coordinates of the rightmost chunks</param>
			/// <param name="chunk_bottom">The Y coordinates of the bottommost chunks</param>
			/// <param name="chunk_top">The Y coordinates of the topmost chunks</param>
			/// <param name="chunk_back">The Z coordinates of the backmost chunks</param>
			/// <param name="chunk_front">The Z coordinates of the frontmost chunks</param>
			/// <param name="gizmos_color_tile">The color to draw tile boundaries with</param>
			/// <param name="gizmos_color_chunk">The color to draw chunk boundaries with</param>
			/// <param name="draw_tiles">Whether to draw tile boundaries</param>
			/// <param name="draw_chunks">Whether to draw chunk boundaries</param>
			public void DrawGizmos(
				int size_x, int size_y, int size_z,
				int chunk_left, int chunk_right,
				int chunk_bottom, int chunk_top,
				int chunk_back, int chunk_front,
				Color gizmos_color_tile, Color gizmos_color_chunk,
				bool draw_tiles, bool draw_chunks
			)
			{
				Gizmos.matrix *= normalToMapMatrix;

				int left = chunk_left * size_x;
				int right = (chunk_right + 1) * size_x;
				int bottom = chunk_bottom * size_y;
				int top = (chunk_top + 1) * size_y;
				int back = chunk_back * size_z;
				int front = (chunk_front + 1) * size_z;

				switch (tiling)
				{
					case Tiling.Rectangular:
					case Tiling.Isometric:
						{
							// Z-aligned lines
							for (int x = left; x <= right; ++x)
								for (int y = bottom; y <= top; ++y)
									if (SetGizmosColor(x % size_x == 0 && y % size_y == 0, gizmos_color_tile, gizmos_color_chunk, draw_tiles, draw_chunks))
										Gizmos.DrawLine(TileToNormal(x, y, back), TileToNormal(x, y, front));

							// Y-aligned lines
							for (int x = left; x <= right; ++x)
								for (int z = back; z <= front; ++z)
									if (SetGizmosColor(x % size_x == 0 && z % size_z == 0, gizmos_color_tile, gizmos_color_chunk, draw_tiles, draw_chunks))
										Gizmos.DrawLine(TileToNormal(x, bottom, z), TileToNormal(x, top, z));

							// X-aligned lines
							for (int y = bottom; y <= top; ++y)
								for (int z = back; z <= front; ++z)
									if (SetGizmosColor(y % size_y == 0 && z % size_z == 0, gizmos_color_tile, gizmos_color_chunk, draw_tiles, draw_chunks))
										Gizmos.DrawLine(TileToNormal(left, y, z), TileToNormal(right, y, z));

							break;
						}
					case Tiling.StaggeredEven:
					case Tiling.StaggeredOdd:
						{
							bool border_even = (tiling == Tiling.StaggeredOdd);

							for (int z = back; z < front; ++z)
							{
								for (int y = bottom; y < top; ++y)
								{
									for (int x = left; x < right; ++x)
									{
										bool border_chunk_down = (y % size_y == 0);
										bool border_chunk_up = ((y + 1) % size_y == 0);
										bool border_chunk_left = (x % size_x == 0) && ((y % 2 == 0) == border_even);
										bool border_chunk_right = ((x + 1) % size_x == 0) && ((y % 2 == 0) != border_even);
										bool border_chunk_back = (z % size_z == 0);
										bool border_chunk_front = ((z + 1) % size_z == 0);
										Vector3 pos = TileToNormal(x, y, z);

										// back square
										if (SetGizmosColor((border_chunk_down || border_chunk_left) && border_chunk_back, gizmos_color_tile, gizmos_color_chunk, draw_tiles, draw_chunks))
											Gizmos.DrawLine(pos, pos + new Vector3(1, 0, 0));
										if (SetGizmosColor((border_chunk_right || border_chunk_down) && border_chunk_back, gizmos_color_tile, gizmos_color_chunk, draw_tiles, draw_chunks))
											Gizmos.DrawLine(pos + new Vector3(1, 0, 0), pos + new Vector3(1, 1, 0));
										if (SetGizmosColor((border_chunk_up || border_chunk_right) && border_chunk_back, gizmos_color_tile, gizmos_color_chunk, draw_tiles, draw_chunks))
											Gizmos.DrawLine(pos + new Vector3(1, 1, 0), pos + new Vector3(0, 1, 0));
										if (SetGizmosColor((border_chunk_left || border_chunk_up) && border_chunk_back, gizmos_color_tile, gizmos_color_chunk, draw_tiles, draw_chunks))
											Gizmos.DrawLine(pos + new Vector3(0, 1, 0), pos);

										// midlines
										if (SetGizmosColor(border_chunk_down && border_chunk_left, gizmos_color_tile, gizmos_color_chunk, draw_tiles, draw_chunks))
											Gizmos.DrawLine(pos, pos + new Vector3(0, 0, 1));
										if (SetGizmosColor(border_chunk_right && border_chunk_down, gizmos_color_tile, gizmos_color_chunk, draw_tiles, draw_chunks))
											Gizmos.DrawLine(pos + new Vector3(1, 0, 0), pos + new Vector3(1, 0, 1));
										if (SetGizmosColor(border_chunk_up && border_chunk_right, gizmos_color_tile, gizmos_color_chunk, draw_tiles, draw_chunks))
											Gizmos.DrawLine(pos + new Vector3(1, 1, 0), pos + new Vector3(1, 1, 1));
										if (SetGizmosColor(border_chunk_left && border_chunk_up, gizmos_color_tile, gizmos_color_chunk, draw_tiles, draw_chunks))
											Gizmos.DrawLine(pos + new Vector3(0, 1, 0), pos + new Vector3(0, 1, 1));

										// front square
										if (SetGizmosColor((border_chunk_down || border_chunk_left) && border_chunk_front, gizmos_color_tile, gizmos_color_chunk, draw_tiles, draw_chunks))
											Gizmos.DrawLine(pos + new Vector3(0, 0, 1), pos + new Vector3(1, 0, 1));
										if (SetGizmosColor((border_chunk_right || border_chunk_down) && border_chunk_front, gizmos_color_tile, gizmos_color_chunk, draw_tiles, draw_chunks))
											Gizmos.DrawLine(pos + new Vector3(1, 0, 1), pos + new Vector3(1, 1, 1));
										if (SetGizmosColor((border_chunk_up || border_chunk_right) && border_chunk_front, gizmos_color_tile, gizmos_color_chunk, draw_tiles, draw_chunks))
											Gizmos.DrawLine(pos + new Vector3(1, 1, 1), pos + new Vector3(0, 1, 1));
										if (SetGizmosColor((border_chunk_left || border_chunk_up) && border_chunk_front, gizmos_color_tile, gizmos_color_chunk, draw_tiles, draw_chunks))
											Gizmos.DrawLine(pos + new Vector3(0, 1, 1), pos + new Vector3(0, 0, 1));
									}
								}
							}

							break;
						}
					default:
						throw new System.InvalidOperationException("Unsupported tile map tiling");
				}
			}
		}
	}
}