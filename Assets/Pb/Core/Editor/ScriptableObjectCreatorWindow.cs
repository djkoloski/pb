using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pb
{
	namespace Editor
	{
		/// <summary>
		/// A simple ScriptableObject creator
		/// </summary>
		public class ScriptableObjectCreatorWindow :
			EditorWindow
		{
			/// <summary>
			/// The mono script the scriptable object class is from
			/// </summary>
			public MonoScript script;
			/// <summary>
			/// The type to make a scriptable object from
			/// </summary>
			public System.Type type;
			/// <summary>
			/// The name of the asset when it is created
			/// </summary>
			public string asset_name;
			/// <summary>
			/// Opens a new ScriptableObject Creator window
			/// </summary>
			[MenuItem("Pb/Tools/ScriptableObject Creator")]
			static void Init()
			{
				EditorWindow.GetWindow(typeof(ScriptableObjectCreatorWindow), false, "Pb ScriptableObject Creator");
			}
			/// <summary>
			/// Draws the window GUI
			/// </summary>
			public void OnGUI()
			{
				EditorGUILayout.BeginHorizontal();
				MonoScript new_script = EditorGUILayout.ObjectField("Source file", script, typeof(MonoScript), false) as MonoScript;
				if (new_script != script)
				{
					script = new_script;
					if (ParseScriptType())
						asset_name = type.Name;
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				asset_name = EditorGUILayout.TextField("Name", asset_name);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Create ScriptableObject"))
					CreateScriptableObject();
				EditorGUILayout.EndHorizontal();
			}
			/// <summary>
			/// Gets the type of the scriptable object from the mono script
			/// </summary>
			public bool ParseScriptType()
			{
				if (script == null)
					return false;

				type = script.GetClass();
				if (type == null)
				{
					ShowNotification(new GUIContent("No class in source file"));
					return false;
				}
				if (!type.IsSubclassOf(typeof(ScriptableObject)))
				{
					ShowNotification(new GUIContent("Class doesn't inherit from Scriptable Object"));
					return false;
				}

				return true;
			}
			/// <summary>
			/// Creates a new ScriptableObject using the given MonoScript
			/// </summary>
			public void CreateScriptableObject()
			{
				if (type == null)
					if (!ParseScriptType())
						return;

				ScriptableObject so = ScriptableObject.CreateInstance(type);
				string path = "Assets/" + type.Name + ".asset";
				AssetDatabase.CreateAsset(so, path);
				EditorGUIUtility.PingObject(so);
			}
		}
	}
}