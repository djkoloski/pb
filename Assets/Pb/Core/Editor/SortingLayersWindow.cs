using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pb
{
	namespace Editor
	{
		/// <summary>
		/// Displays detailed information about the sorting layers
		/// </summary>
		public class SortingLayersWindow :
			EditorWindow
		{
			/// <summary>
			/// The index of the sorting layer to make
			/// </summary>
			public int make_index = 0;
			/// <summary>
			/// Opens a new sorting layers window
			/// </summary>
			[MenuItem("Pb/Tools/Sorting Layers")]
			static void Init()
			{
				EditorWindow.GetWindow(typeof(SortingLayersWindow), false , "Pb Sorting Layers");
			}
			/// <summary>
			/// Draws the GUI for the window
			/// </summary>
			public void OnGUI()
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Sorting layers:");
				EditorGUILayout.EndHorizontal();

				string[] layer_names = Utility.SortingLayers.GetSortingLayerNames();
				int[] layer_ids = Utility.SortingLayers.GetSortingLayerUniqueIDs();

				// This is a hack around a bug regarding selectable labels having the wrong height
				// <hack>
				GUILayout.Space(18 * layer_names.Length);

				for (int i = layer_names.Length - 1; i >= 0; --i)
				{
					GUILayout.Space(-18);
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.SelectableLabel(layer_names[i]);
					EditorGUILayout.SelectableLabel(layer_ids[i].ToString());
					EditorGUILayout.EndHorizontal();
					GUILayout.Space(-34);
				}

				GUILayout.Space(18 * layer_names.Length);
				// </hack>

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Create standard sorting layer:");
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				make_index = EditorGUILayout.IntField(make_index);
				if (GUILayout.Button("Make sorting layer"))
					MakeSortingLayer();
				EditorGUILayout.EndHorizontal();
			}
			/// <summary>
			/// Makes a new standard sorting layer if it doesn't already exist
			/// </summary>
			public void MakeSortingLayer()
			{
				Utility.SortingLayers.StandardSortingLayerID(make_index);
			}
		}
	}
}