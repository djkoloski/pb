using UnityEditor;
using UnityEngine;

namespace Pb
{
	namespace Collections
	{
		/// <summary>
		/// Formats the GUI of maps
		/// </summary>
		public class MapDrawer<TKey, TValue> :
			PropertyDrawer
		{
			/// <summary>
			/// The width of the buttons to draw with the map
			/// </summary>
			public static int button_width = 30;
			/// <summary>
			/// The margin between the buttons to draw with the map
			/// </summary>
			public static int button_margin = 4;
			/// <summary>
			/// Whether the map is expanded or not
			/// </summary>
			public bool expanded = false;
			/// <summary>
			/// The map element that is selected (if any)
			/// </summary>
			public int selected = -1;
			/// <summary>
			/// The new key to add to the map
			/// </summary>
			public TKey new_key;
			/// <summary>
			/// Gets the height of the map drawer
			/// </summary>
			/// <param name="property">The map as a serialized property</param>
			/// <param name="label">The label </param>
			/// <returns></returns>
			public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
			{
				if (expanded)
					return Utility.GUI.block_height * (property.FindPropertyRelative("keys_").arraySize + 2);
				return Utility.GUI.block_height;
			}
			/// <summary>
			/// Draws the GUI for the map drawer
			/// </summary>
			/// <param name="position">The position to draw the GUI at</param>
			/// <param name="property">The property to draw the GUI with</param>
			/// <param name="label">The label to put on the GUI</param>
			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
			{
				float cur_y = position.y;
				expanded = EditorGUI.Foldout(new Rect(position.x, cur_y, position.width, Utility.GUI.block_height), expanded, label);
				cur_y += Utility.GUI.block_height;

				if (!expanded)
					return;

				++EditorGUI.indentLevel;

				SerializedProperty keys = property.FindPropertyRelative("keys_");
				SerializedProperty values = property.FindPropertyRelative("values_");

				for (int i = 0; i < keys.arraySize; ++i, cur_y += Utility.GUI.block_height)
				{
					Rect row = new Rect(position.x, cur_y, position.width, Utility.GUI.line_height);
					bool row_selected = (Event.current != null && Event.current.isMouse && row.Contains(Event.current.mousePosition));
					
					if (row_selected)
						selected = i;

					EditorGUI.SelectableLabel(new Rect(position.x, cur_y, position.width / 2.0f, Utility.GUI.block_height), Utility.GUI.PropertyToString(keys.GetArrayElementAtIndex(i)), (i == selected ? EditorStyles.boldLabel : GUIStyle.none));
					EditorGUI.PropertyField(new Rect(position.x + position.width / 2.0f, cur_y, position.width / 2.0f, Utility.GUI.block_height), values.GetArrayElementAtIndex(i), GUIContent.none);
				}

				new_key = (TKey)Utility.GUI.GenericField(new Rect(position.x, cur_y, position.width - 2 * (button_width + button_margin), Utility.GUI.block_height), new_key, typeof(TKey));
				
				if (GUI.Button(new Rect(position.x + position.width - button_width * 2 - button_margin, cur_y, button_width, Utility.GUI.block_height), new GUIContent("+")))
				{
					Map<TKey, TValue> map = Utility.Serialized.GetPropertyObject(property) as Map<TKey, TValue>;
					if (map == null)
						throw new System.InvalidOperationException("Unable to get associated map object");
					if (!map.ContainsKey(new_key))
						map.Add(new_key, default(TValue));
				}

				if (GUI.Button(new Rect(position.x + position.width - button_width, cur_y, button_width, Utility.GUI.block_height), new GUIContent("-")))
				{
					Map<TKey, TValue> map = Utility.Serialized.GetPropertyObject(property) as Map<TKey, TValue>;
					if (map == null)
						throw new System.InvalidOperationException("Unable to get associated map object");
					if (selected >= 0)
						map.Remove(map.Keys[selected]);
				}

				--EditorGUI.indentLevel;
			}
		}
	}
}