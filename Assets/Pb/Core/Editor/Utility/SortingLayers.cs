using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.Reflection;

namespace Pb
{
	namespace Utility
	{
		/// <summary>
		/// Provides a programmable API for using specific sorting layers
		/// </summary>
		public static class SortingLayers
		{
			private static System.Type secret = typeof(InternalEditorUtility);
			private static PropertyInfo sortingLayerNames = secret.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
			private static PropertyInfo sortingLayerUniqueIDs = secret.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
			private static MethodInfo AddSortingLayer = secret.GetMethod("AddSortingLayer", BindingFlags.Static | BindingFlags.NonPublic);
			private static MethodInfo SetSortingLayerName = secret.GetMethod("SetSortingLayerName", BindingFlags.Static | BindingFlags.NonPublic);
			/// <summary>
			/// Gets the names of all of the sorting layers
			/// </summary>
			/// <returns>The names of all of the sorting layers</returns>
			public static string[] GetSortingLayerNames()
			{
				return (string[])sortingLayerNames.GetValue(null, new object[0]);
			}
			/// <summary>
			/// Gets the unique IDs of all of the sorting layers
			/// </summary>
			/// <returns>The unique IDs of all of the sorting layers</returns>
			public static int[] GetSortingLayerUniqueIDs()
			{
				return (int[])sortingLayerUniqueIDs.GetValue(null, new object[0]);
			}
			/// <summary>
			/// Gets the name of a standard sorting layer
			/// </summary>
			/// <param name="index">The index of the sorting layer</param>
			/// <returns>The name of the desired standard sorting layer</returns>
			public static string StandardSortingLayerName(int index)
			{
				return ("pb_" + index);
			}
			/// <summary>
			/// Gets the ID of a standard sorting layer
			/// </summary>
			/// <param name="index">The index of the sorting layer</param>
			/// <returns>The name of the desired standard sorting layer</returns>
			public static int StandardSortingLayerID(int index)
			{
				return SortingLayerID(StandardSortingLayerName(index));
			}
			/// <summary>
			/// Gets the ID of a sorting layer or makes a new one if it doesn't exist and returns that ID
			/// </summary>
			/// <param name="name">The name of the sorting layer to get the ID of</param>
			/// <returns>The ID of the desired sorting layer</returns>
			public static int SortingLayerID(string name)
			{
				string[] names = GetSortingLayerNames();
				int[] ids = GetSortingLayerUniqueIDs();

				for (int i = 0; i < names.Length; ++i)
					if (names[i] == name)
						return ids[i];

				AddSortingLayer.Invoke(null, new object[0]);
				SetSortingLayerName.Invoke(null, new object[2]{ names.Length, name });

				ids = GetSortingLayerUniqueIDs();

				return ids[ids.Length - 1];
			}
		}
	}
}
