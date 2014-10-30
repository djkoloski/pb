using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace Pb.Utility
{
	/// <summary>
	/// Contains helper functions to deal with Unity's GUI
	/// </summary>
	public static class GUI
	{
		/// <summary>
		/// The standard height of one line
		/// </summary>
		public static float line_height
		{
			get
			{
				return UnityEngine.GUI.skin.label.lineHeight;
			}
		}
		/// <summary>
		/// The standard margin above lines
		/// </summary>
		public static float margin_top
		{
			get
			{
				return UnityEngine.GUI.skin.label.margin.top;
			}
		}
		/// <summary>
		/// The standard margin below lines
		/// </summary>
		public static float margin_bottom
		{
			get
			{
				return UnityEngine.GUI.skin.label.margin.bottom;
			}
		}
		/// <summary>
		/// The height of the line plus the top and bottom margins
		/// </summary>
		public static float block_height
		{
			get
			{
				return line_height + margin_top + margin_bottom;
			}
		}
		/// <summary>
		/// Converts the given serialized property to a string
		/// </summary>
		/// <param name="property">The property to convert to a string</param>
		/// <returns>The string representation of the serialized property</returns>
		public static string PropertyToString(SerializedProperty property)
		{
			if (property == null)
				return "(null)";

			switch (property.propertyType)
			{
				case SerializedPropertyType.AnimationCurve:
					return property.animationCurveValue.ToString();
				case SerializedPropertyType.ArraySize:
					return property.arraySize.ToString();
				case SerializedPropertyType.Boolean:
					return property.boolValue.ToString();
				case SerializedPropertyType.Bounds:
					return property.boundsValue.ToString();
				case SerializedPropertyType.Character:
					return ((char)property.intValue).ToString();
				case SerializedPropertyType.Color:
					return property.colorValue.ToString();
				case SerializedPropertyType.Enum:
					return property.enumNames[property.enumValueIndex];
				case SerializedPropertyType.Float:
					return property.floatValue.ToString();
				case SerializedPropertyType.Generic:
				{
					object obj = Utility.Serialized.GetPropertyObject(property);
					if (obj == null)
						return "(null)";
					System.Type type = obj.GetType();
					if (type == null)
						return "(null)";
					MethodInfo to_string = type.GetMethod("ToString", BindingFlags.Public);
					if (to_string == null)
						return "(null)";
					return to_string.Invoke(obj, null) as string;
				}
				case SerializedPropertyType.Gradient:
					return "(Gradient)";
				case SerializedPropertyType.Integer:
					return property.intValue.ToString();
				case SerializedPropertyType.LayerMask:
					return LayerMask.LayerToName(property.intValue);
				case SerializedPropertyType.ObjectReference:
					return property.objectReferenceValue.ToString();
				case SerializedPropertyType.Quaternion:
					return property.quaternionValue.ToString();
				case SerializedPropertyType.Rect:
					return property.rectValue.ToString();
				case SerializedPropertyType.String:
					return property.stringValue;
				case SerializedPropertyType.Vector2:
					return property.vector2Value.ToString();
				case SerializedPropertyType.Vector3:
					return property.vector3Value.ToString();
				case SerializedPropertyType.Vector4:
					return property.vector4Value.ToString();
				default:
					return "UNDEFINED";
			}
		}
		/// <summary>
		/// Draws generic GUI
		/// </summary>
		/// <param name="position">The position to draw the GUI at</param>
		/// <param name="cur_val">The current value of the object</param>
		/// <param name="type">The type of the object to draw</param>
		/// <param name="label">The label of the object (if any)</param>
		/// <param name="allow_scene_objects">Whether to accept scene objects if drawing objects</param>
		/// <returns></returns>
		public static object GenericField(Rect position, object cur_val, System.Type type, string label = "", bool allow_scene_objects = false)
		{
			if (type == typeof(AnimationCurve))
				return EditorGUI.CurveField(position, cur_val as AnimationCurve);
			else if (type == typeof(bool))
				return EditorGUI.Toggle(position, (bool)cur_val);
			else if (type == typeof(Bounds))
				return EditorGUI.BoundsField(position, (Bounds)cur_val);
			else if (type == typeof(Color))
				return EditorGUI.ColorField(position, (Color)cur_val);
			else if (type == typeof(System.Enum))
				return EditorGUI.EnumPopup(position, cur_val as System.Enum);
			else if (type == typeof(float))
				return EditorGUI.FloatField(position, (float)cur_val);
			else if (type == typeof(int))
				return EditorGUI.IntField(position, (int)cur_val);
			else if (type == typeof(Object))
				return EditorGUI.ObjectField(position, cur_val as UnityEngine.Object, type, allow_scene_objects);
			else if (type == typeof(Rect))
				return EditorGUI.RectField(position, (Rect)cur_val);
			else if (type == typeof(string))
				return EditorGUI.TextField(position, (string)cur_val);
			else if (type == typeof(Vector2))
				return EditorGUI.Vector2Field(position, label, (Vector2)cur_val);
			else if (type == typeof(Vector3))
				return EditorGUI.Vector3Field(position, label, (Vector3)cur_val);
			else if (type == typeof(Vector4))
				return EditorGUI.Vector4Field(position, label, (Vector4)cur_val);
			else
				throw new System.ArgumentException("Unable to draw GUI with the given type");
		}
	}
}