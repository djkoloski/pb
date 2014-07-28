namespace Pb
{
	/// <summary>
	/// A simple Unity-friendly debugging class
	/// </summary>
	public static class Debug
	{
		/// <summary>
		/// Asserts the given condition at runtime, possibly supplying a message with it
		/// </summary>
		/// <param name="condition">The condition to assert as true</param>
		/// <param name="message">The message to supply along with the assertion</param>
		public static void Assert(bool condition, string message = null)
		{
			if (!condition)
			{
				if (message != null)
					throw new System.Exception("Assertion failed: '" + message + "'");
				else
					throw new System.Exception("Assertion failed");
			}
		}
	}
}