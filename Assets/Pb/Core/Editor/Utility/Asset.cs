using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Pb
{
	namespace Utility
	{
		/// <summary>
		/// A utility class for dealing with Unity assets
		/// </summary>
		public static class Asset
		{
			/// <summary>
			/// Loads or creates a new asset of the given type and marks it as needing to be saved
			/// </summary>
			/// <typeparam name="T">The type of the asset to create</typeparam>
			/// <param name="path">The path to the asset</param>
			/// <returns>The asset</returns>
			public static T GetAndEdit<T>(string path) where T : ScriptableObject
			{
				T result = null;

				if (File.Exists(path))
					result = AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
				else
				{
					result = ScriptableObject.CreateInstance<T>();
					AssetDatabase.CreateAsset(result, path);
				}

				EditorUtility.SetDirty(result);

				return result;
			}
		}
	}
}