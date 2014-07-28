using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pbtk
{
	namespace TileMap2D
	{
		/// <summary>
		/// A user-friendly GUI for automatic tile set creation
		/// </summary>
		public class TileSetCreatorWindow :
			EditorWindow
		{
			/// <summary>
			/// The path to the source image
			/// </summary>
			public string source_path;
			/// <summary>
			/// The directory relative to the assets file to put the tile set into
			/// </summary>
			public string tile_sets_dir;
			/// <summary>
			/// The pixels to unit scale for each tile
			/// </summary>
			public float pixels_per_unit;
			/// <summary>
			/// Whether the tile set has a transparent color
			/// </summary>
			public bool has_transparent_color;
			/// <summary>
			/// The transparent color (if any)
			/// The alpha value will be ignored and replaced with 255
			/// </summary>
			public Color32 transparent_color;
			/// <summary>
			/// The margin on each side of the tile set
			/// </summary>
			public Vector2 margin;
			/// <summary>
			/// The spacing between each tile in the tile set
			/// </summary>
			public Vector2 spacing;
			/// <summary>
			/// The size of each tile in the tile set
			/// </summary>
			public Vector2 slice_size;
			/// <summary>
			/// The offset to draw the tiles with
			/// </summary>
			public Vector2 draw_offset;
			/// <summary>
			/// Whether to flip tile ordering along the X axis
			/// </summary>
			bool flip_x;
			/// <summary>
			/// Whether to flip tile ordering along the Y axis
			/// </summary>
			bool flip_y;
			/// <summary>
			/// Whether to force tile set verification
			/// </summary>
			bool force_rebuild;
			/// <summary>
			/// Opens a new Tile Set Creator window
			/// </summary>
			[MenuItem("Pbtk/TileMap2D/Tile Set Creator")]
			static void Init()
			{
				EditorWindow.GetWindow(typeof(TileSetCreatorWindow), false, "Tile Set Creator");
			}
			/// <summary>
			/// Draws the window GUI
			/// </summary>
			public void OnGUI()
			{
				EditorGUILayout.BeginHorizontal();
				source_path = EditorGUILayout.TextField("Source image", source_path);
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("..."))
					source_path = EditorUtility.OpenFilePanel("Image", source_path, "");
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				tile_sets_dir = EditorGUILayout.TextField("Tile sets directory", tile_sets_dir);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				pixels_per_unit = EditorGUILayout.FloatField("Pixels per unit", pixels_per_unit);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				has_transparent_color = EditorGUILayout.BeginToggleGroup("Transparent color", has_transparent_color);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				transparent_color = EditorGUILayout.ColorField(transparent_color);
				EditorGUILayout.EndToggleGroup();
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				slice_size = EditorGUILayout.Vector2Field("Slice size", slice_size);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				margin = EditorGUILayout.Vector2Field("Margin", margin);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				spacing = EditorGUILayout.Vector2Field("Spacing", spacing);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				draw_offset = EditorGUILayout.Vector2Field("Draw offset", draw_offset);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				flip_x = EditorGUILayout.Toggle("Flip X order", flip_x);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				flip_y = EditorGUILayout.Toggle("Flip Y order", flip_y);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				force_rebuild = EditorGUILayout.Toggle("Force rebuild", force_rebuild);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Import"))
					ImportTileSet();
				EditorGUILayout.EndHorizontal();
			}
			/// <summary>
			/// Imports and builds the tile set
			/// </summary>
			public void ImportTileSet()
			{
				TileSet tile_set = ConverterUtility.SliceTileSetFromImage(
					source_path,
					"Assets",
					pixels_per_unit,
					Mathf.FloorToInt(margin.x),
					Mathf.FloorToInt(margin.y),
					Mathf.FloorToInt(spacing.x),
					Mathf.FloorToInt(spacing.y),
					Mathf.FloorToInt(slice_size.x),
					Mathf.FloorToInt(slice_size.y),
					Mathf.FloorToInt(draw_offset.x),
					Mathf.FloorToInt(draw_offset.y),
					(has_transparent_color ? new Color32(transparent_color.r, transparent_color.g, transparent_color.b, 0xFF) : new Color32(0, 0, 0, 0)),
					flip_x,
					flip_y,
					force_rebuild
					);
				EditorGUIUtility.PingObject(tile_set);
			}
		}
	}
}