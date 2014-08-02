using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace Pb
{
	namespace Utility
	{
		/// <summary>
		/// Helper class for dealing with serialized objects
		/// </summary>
		public static class Serialized
		{
			private static object GetObjectValue(object obj, string name)
			{
				if (obj == null)
					return null;
				
				System.Type type = obj.GetType();

				if (type == null)
					return null;

				FieldInfo field = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
				if (field == null)
				{
					PropertyInfo prop = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
					if (prop == null)
						return null;
					return prop.GetValue(obj, null);
				}
				return field.GetValue(obj);
			}
			private static object GetObjectValue(object obj, string name, int index)
			{
				System.Collections.IEnumerable enumerable = GetObjectValue(obj, name) as System.Collections.IEnumerable;
				System.Collections.IEnumerator enumerator = enumerable.GetEnumerator();
				while (index-- >= 0)
					enumerator.MoveNext();
				return enumerator.Current;
			}
			/// <summary>
			/// Gets the object instance that a serialized property represents
			/// </summary>
			/// <param name="property">The property to search for an instance of</param>
			/// <returns>The object instance or null</returns>
			public static object GetPropertyObject(SerializedProperty property)
			{
				string path = property.propertyPath.Replace(".Array.data[", "[");
				object obj = property.serializedObject.targetObject;
				string[] elements = path.Split('.');

				foreach (string element in elements)
				{
					if (element.Contains("["))
					{
						string elementName = element.Substring(0, element.IndexOf("["));
						int index = int.Parse(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
						obj = GetObjectValue(obj, elementName, index);
					}
					else
						obj = GetObjectValue(obj, element);
				}

				return obj;
			}
		}
	}
}