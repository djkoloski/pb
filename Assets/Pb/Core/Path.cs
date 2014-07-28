using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pb
{
	/// <summary>
	/// Helper functions for dealing with paths in Unity
	/// </summary>
	public static class Path
	{
		/// <summary>
		/// Converts an asset path (local to the Unity project, begins with "Assets") to a system path (compatible with file IO)
		/// </summary>
		/// <param name="path">The path to convert</param>
		/// <returns>The system path that corresponds to the given asset path</returns>
		public static string AssetPathToSystemPath(string path)
		{
			string remove_string = "Assets";
			int index = path.IndexOf(remove_string);
			if (index != 0)
				throw new System.ArgumentException("Asset path does not start with 'Assets'");
			return Application.dataPath + path.Remove(index, remove_string.Length);
		}
		/// <summary>
		/// Converts a system path (compatible with file IO) to an asset path (local to the Unity project, begins with "Assets")
		/// </summary>
		/// <param name="path">The path to convert</param>
		/// <returns>The asset path that corresponds to the given system path</returns>
		public static string SystemPathToAssetPath(string path)
		{
			path = path.Replace(@"\", @"/");
			string remove_string = Application.dataPath;
			int index = path.IndexOf(remove_string);
			if (index != 0)
				throw new System.ArgumentException("System path does not start with '" + Application.dataPath + "'");
			return "Assets" + path.Remove(index, remove_string.Length);
		}
		/// <summary>
		/// Combines two paths, fixing up slashes between them
		/// </summary>
		/// <param name="directory">The directory to prepend</param>
		/// <param name="path">The path to append</param>
		/// <returns>The combined path</returns>
		public static string Combine(string directory, string path)
		{
			directory = directory.Replace(@"\", @"/");
			path = path.Replace(@"\", @"/");

			if (directory == null || directory == "")
				return path;
			if (path == null || path == "")
				return directory;

			if (directory[directory.Length - 1] == '/')
			{
				if (path[path.Length - 1] == '/')
					return directory + path.Remove(0, 1);
				else
					return directory + path;
			}
			else
			{
				if (path[path.Length - 1] == '/')
					return directory + path;
				else
					return directory + '/' + path;
			}
		}
	}
}