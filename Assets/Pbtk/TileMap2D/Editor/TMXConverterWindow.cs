using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Pbtk
{
	namespace TileMap2D
	{
		/// <summary>
		/// Converts TMX source files into compatible TileMap2D files
		/// </summary>
		public class TMXConverterWindow :
			EditorWindow
		{
			/// <summary>
			/// The path to the TMX
			/// </summary>
			public string input_path;
			/// <summary>
			/// The name to give to the output TM2D files
			/// </summary>
			public string output_name;
			/// <summary>
			/// The tile sets directory relative to Assets/
			/// </summary>
			public string tile_sets_dir;
			/// <summary>
			/// The number of pixels per unit to render tile sets imported with
			/// </summary>
			public float pixels_per_unit;
			/// <summary>
			/// The chunk width in tiles
			/// </summary>
			public int chunk_size_x;
			/// <summary>
			/// The chunk height in tiles
			/// </summary>
			public int chunk_size_y;
			/// <summary>
			/// Whether to force tile sets to be rebuilt
			/// </summary>
			public bool force_rebuild_tile_sets;
			/// <summary>
			/// The import delegate
			/// </summary>
			public TMXImportDelegate import_delegate;
			/// <summary>
			/// Opens a new TMX converter window
			/// </summary>
			[MenuItem("Pbtk/TileMap2D/TMX Converter")]
			static void Init()
			{
				EditorWindow.GetWindow(typeof(TMXConverterWindow), false, "TMX Converter");
			}
			/// <summary>
			/// Draws the window's GUI
			/// </summary>
			public void OnGUI()
			{
				EditorGUILayout.BeginHorizontal();
				Pb.Utility.Undo.RegisterChange<string>(
					EditorGUILayout.TextField("TMX File", input_path),
					ref input_path, this,
					"Changed TMX input path");
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("..."))
				{
					Pb.Utility.Undo.RegisterChange<string>(
						EditorUtility.OpenFilePanel("TMX file", input_path, "tmx"),
						ref input_path, this,
						"Changed TMX input path");
					output_name = Path.GetFileNameWithoutExtension(input_path);
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				Pb.Utility.Undo.RegisterChange<string>(
					EditorGUILayout.TextField("Output name", output_name),
					ref output_name, this,
					"Changed tile map output name");
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
				Pb.Utility.Undo.RegisterChange<int>(
					EditorGUILayout.IntField("Chunk width", chunk_size_x),
					ref chunk_size_x, this,
					"Changed tile map chunk width");
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				Pb.Utility.Undo.RegisterChange<int>(
					EditorGUILayout.IntField("Chunk height", chunk_size_y),
					ref chunk_size_y, this,
					"Changed tile map chunk height");
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				Pb.Utility.Undo.RegisterChange<bool>(
					EditorGUILayout.Toggle("Force rebuild tile sets", force_rebuild_tile_sets),
					ref force_rebuild_tile_sets, this,
					"Changed whether to force tile set rebuilding");
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				Pb.Utility.Undo.RegisterChange<TMXImportDelegate>(
					EditorGUILayout.ObjectField("Import delegate", import_delegate, typeof(TMXImportDelegate), false) as TMXImportDelegate,
					ref import_delegate, this,
					"Changed the import delegate");
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Convert"))
					ConvertTMX();
				EditorGUILayout.EndHorizontal();
			}
			/// <summary>
			/// Converts the TMX into TM2D files
			/// </summary>
			public void ConvertTMX()
			{
				TileMap tile_map = TMXConverter.ConvertTMX(
					input_path,
					output_name,
					Pb.Path.Combine("Assets", tile_sets_dir),
					pixels_per_unit,
					chunk_size_x,
					chunk_size_y,
					force_rebuild_tile_sets,
					import_delegate
					);
				EditorGUIUtility.PingObject(tile_map);
			}
		}
	}
}