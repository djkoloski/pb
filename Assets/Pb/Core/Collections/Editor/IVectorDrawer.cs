using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Pb
{
	namespace Collections
	{
		/// <summary>
		/// Customizes the GUI for IVector2
		/// </summary>
		[CustomPropertyDrawer(typeof(IVector2))]
		public class IVector2Drawer :
			PropertyDrawer
		{
			/// <summary>
			/// Draws the X and Y components on the screen
			/// </summary>
			/// <param name="position">The position to draw the components to</param>
			/// <param name="property">The IVector2 to draw</param>
			/// <param name="label">The label to draw with the components</param>
			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
			{
				label = EditorGUI.BeginProperty(position, label, property);
				Rect content_rect = EditorGUI.PrefixLabel(position, label);
				EditorGUI.indentLevel = 0;

				float coord_width = content_rect.width / 2.0f;
				
				Rect x_rect = new Rect(content_rect.xMin, content_rect.yMin, coord_width, content_rect.height);
				Rect y_rect = new Rect(content_rect.xMin + coord_width, content_rect.yMin, coord_width, content_rect.height);
				
				EditorGUIUtility.labelWidth = 14.0f;
				EditorGUI.PropertyField(x_rect, property.FindPropertyRelative("x"), new GUIContent("X"));
				EditorGUI.PropertyField(y_rect, property.FindPropertyRelative("y"), new GUIContent("Y"));
				EditorGUI.EndProperty();
			}
		}

		/// <summary>
		/// Customizes the GUI for IVector3
		/// </summary>
		[CustomPropertyDrawer(typeof(IVector3))]
		public class IVector3Drawer :
			PropertyDrawer
		{
			/// <summary>
			/// Draws the X, Y, and Z components on the screen
			/// </summary>
			/// <param name="position">The position to draw the components to</param>
			/// <param name="property">The IVector3 to draw</param>
			/// <param name="label">The label to draw with the components</param>
			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
			{
				label = EditorGUI.BeginProperty(position, label, property);
				Rect content_rect = EditorGUI.PrefixLabel(position, label);
				EditorGUI.indentLevel = 0;

				float coord_width = content_rect.width / 3.0f;

				Rect x_rect = new Rect(content_rect.xMin, content_rect.yMin, coord_width, content_rect.height);
				Rect y_rect = new Rect(content_rect.xMin + coord_width, content_rect.yMin, coord_width, content_rect.height);
				Rect z_rect = new Rect(content_rect.xMin + 2.0f * coord_width, content_rect.yMin, coord_width, content_rect.height);

				EditorGUIUtility.labelWidth = 14.0f;
				EditorGUI.PropertyField(x_rect, property.FindPropertyRelative("x"), new GUIContent("X"));
				EditorGUI.PropertyField(y_rect, property.FindPropertyRelative("y"), new GUIContent("Y"));
				EditorGUI.PropertyField(z_rect, property.FindPropertyRelative("z"), new GUIContent("Z"));
				EditorGUI.EndProperty();
			}
		}
	}
}