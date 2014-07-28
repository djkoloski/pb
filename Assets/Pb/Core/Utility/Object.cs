namespace Pb
{
	namespace Utility
	{
		/// <summary>
		/// A utility for managing Unity Objects
		/// </summary>
		public static class Object
		{
			/// <summary>
			/// Destroys the Object with context in mind.
			/// If the build is for the editor, the Object is destroyed immediately.
			/// Otherwise, the Object is destroyed normally.
			/// </summary>
			/// <param name="obj">The Unity Object to destroy</param>
			public static void ContextualDestroy(UnityEngine.Object obj)
			{
#if UNITY_EDITOR
				UnityEngine.Object.DestroyImmediate(obj);
#else
				UnityEngine.Object.Destroy(obj);
#endif
			}
		}
	}
}