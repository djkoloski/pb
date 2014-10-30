using UnityEditor;
using UnityEngine;

namespace Pb.Utility
{
	/// <summary>
	/// A utility for managing Unity undos
	/// </summary>
	public static class Undo
	{
		/// <summary>
		/// Registers a potential change in an object and returns whether a change occurred
		/// </summary>
		/// <typeparam name="T">The type of the values to compare</typeparam>
		/// <param name="new_val">The new value</param>
		/// <param name="old_val">The old value</param>
		/// <param name="container">The object that contains the value</param>
		/// <param name="undo_message">The message to log with the undo</param>
		/// <returns>Whether the value was changed</returns>
		public static bool RegisterChange<T>(T new_val, ref T old_val, UnityEngine.Object container, string undo_message)
		{
			bool changed = (new_val == null ? old_val != null : !new_val.Equals(old_val));
			if (changed)
			{
				UnityEditor.Undo.RecordObject(container, undo_message);
				old_val = new_val;
			}
			return changed;
		}
	}
}