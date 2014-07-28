using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pb
{
	namespace Editor
	{
		/// <summary>
		/// The Pb unit testing window
		/// </summary>
		public class UnitTestWindow : UnityEditor.EditorWindow
		{
			/// <summary>
			/// Whether to run the collections unit tests
			/// </summary>
			public bool collections = false;

			/// <summary>
			/// Opens the unit testing window
			/// </summary>
			[MenuItem("Pb/Tools/Unit Tests")]
			static void Init()
			{
				EditorWindow.GetWindow(typeof(UnitTestWindow), false, "Pb Unit Testing");
			}

			/// <summary>
			/// The unit testing GUI
			/// </summary>
			void OnGUI()
			{
				EditorGUILayout.BeginHorizontal();
				collections = EditorGUILayout.ToggleLeft("Pb.Collections", collections);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Run tests"))
					RunUnitTests();
				EditorGUILayout.EndHorizontal();
			}

			/// <summary>
			/// Runs the selected unit tests
			/// </summary>
			void RunUnitTests()
			{
				if (collections)
					UnitTests.Collections.AllUnitTests();

				EditorUtility.DisplayDialog("Pb Unit tests passed", "", "OK");
			}
		}
	}
}