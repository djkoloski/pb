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
				Pb.Utility.Undo.RegisterChange<string>(
					EditorGUILayout.TextField("Source image", source_path),
					ref source_path, this,
					"Changed image source path");
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("..."))
					Pb.Utility.Undo.RegisterChange<string>(
						EditorUtility.OpenFilePanel("Image", source_path, ""),
						ref source_path, this,
						"Changed image source path");
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				Pb.Utility.Undo.RegisterChange<string>(
					EditorGUILayout.TextField("Tile sets directory", tile_sets_dir),
					ref tile_sets_dir, this,
					"Changed tile sets directory");
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				Pb.Utility.Undo.RegisterChange<float>(
					EditorGUILayout.FloatField("Pixels per unit", pixels_per_unit),
					ref pixels_per_unit, this,
					"Changed tile set pixels per unit");
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				Pb.Utility.Undo.RegisterChange<bool>(
					EditorGUILayout.BeginToggleGroup("Transparent color", has_transparent_color),
					ref has_transparent_color, this,
					"Changed whether the tile set has a transparent color");
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				Pb.Utility.Undo.RegisterChange<Color32>(
					EditorGUILayout.ColorField(transparent_color),
					ref transparent_color, this,
					"Changed tile set transparent color");
				EditorGUILayout.EndToggleGroup();
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				Pb.Utility.Undo.RegisterChange<Vector2>(
					EditorGUILayout.Vector2Field("Slice size", slice_size),
					ref slice_size, this,
					"Changed tile set slice size");
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				Pb.Utility.Undo.RegisterChange<Vector2>(
					EditorGUILayout.Vector2Field("Margin", margin),
					ref margin, this,
					"Changed tile set margin");
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				Pb.Utility.Undo.RegisterChange<Vector2>(
					EditorGUILayout.Vector2Field("Spacing", spacing),
					ref spacing, this,
					"Changed tile set spacing");
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				Pb.Utility.Undo.RegisterChange<Vector2>(
					EditorGUILayout.Vector2Field("Draw offset", draw_offset),
					ref draw_offset, this,
					"Changed tile set draw offset");
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				Pb.Utility.Undo.RegisterChange<bool>(
					EditorGUILayout.Toggle("Flip X order", flip_x),
					ref flip_x, this,
					"Changed whether to flip tile set X order");
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				Pb.Utility.Undo.RegisterChange<bool>(
					EditorGUILayout.Toggle("Flip Y order", flip_y),
					ref flip_y, this,
					"Changed whether to flip tile set Y order");
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				Pb.Utility.Undo.RegisterChange<bool>(
					EditorGUILayout.Toggle("Force rebuild", force_rebuild),
					ref force_rebuild, this,
					"Changed whether to force a tile set rebuild");
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