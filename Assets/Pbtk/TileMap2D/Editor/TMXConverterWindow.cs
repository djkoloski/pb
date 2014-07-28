using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Pbtk
{
	namespace TileMap2D
	{
		public class TMXConverterWindow :
			EditorWindow
		{
			public string input_path;
			public string output_name;
			public string tile_sets_dir;
			public float pixels_per_unit;
			public int chunk_size_x;
			public int chunk_size_y;
			public bool force_rebuild_tile_sets;
			[MenuItem("Pbtk/TileMap2D/TMX Converter")]
			static void Init()
			{
				EditorWindow.GetWindow(typeof(TMXConverterWindow), false, "TMX Converter");
			}
			public void OnGUI()
			{
				EditorGUILayout.BeginHorizontal();
				input_path = EditorGUILayout.TextField("TMX File", input_path);
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("..."))
				{
					input_path = EditorUtility.OpenFilePanel("TMX file", input_path, "tmx");
					output_name = Path.GetFileNameWithoutExtension(input_path);
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				output_name = EditorGUILayout.TextField("Output name", output_name);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				tile_sets_dir = EditorGUILayout.TextField("Tile sets directory", tile_sets_dir);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				pixels_per_unit = EditorGUILayout.FloatField("Pixels per unit", pixels_per_unit);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				chunk_size_x = EditorGUILayout.IntField("Chunk width", chunk_size_x);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				chunk_size_y = EditorGUILayout.IntField("Chunk height", chunk_size_y);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				force_rebuild_tile_sets = EditorGUILayout.Toggle("Force rebuild tile sets", force_rebuild_tile_sets);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Convert"))
					ConvertTMX();
				EditorGUILayout.EndHorizontal();
			}
			public void ConvertTMX()
			{
				TileMap tile_map = TMXConverter.ConvertTMX(
					input_path,
					output_name,
					Pb.Path.Combine("Assets", tile_sets_dir),
					pixels_per_unit,
					chunk_size_x,
					chunk_size_y,
					force_rebuild_tile_sets
					);
				EditorGUIUtility.PingObject(tile_map);
			}
		}
	}
}