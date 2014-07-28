using System.Collections;
using System.Collections.Generic;
using Pb.Collections;

namespace Pb
{
	namespace UnitTests
	{
		/// <summary>
		/// A unit testing suite for the collections namespace
		/// </summary>
		public static class Collections
		{
			/// <summary>
			/// Runs all unit tests
			/// </summary>
			public static void AllUnitTests()
			{
				MapUnitTest();
			}

			/// <summary>
			/// The relational map unit test
			/// </summary>
			public static void MapUnitTest()
			{
				Map<string, string> string_map = new Map<string, string>();

				Debug.Assert(string_map.Count == 0);

				string_map["hello"] = "world";
				
				Debug.Assert(string_map.Count == 1);
				Debug.Assert(string_map.ContainsKey("hello"));
				Debug.Assert(string_map.ContainsValue("world"));
				Debug.Assert(string_map["hello"] == "world");
				
				string_map.Clear();

				Debug.Assert(string_map.Count == 0);
				Debug.Assert(!string_map.ContainsKey("hello"));
				
				string_map.Add("first", "second");
				string_map.Add("third", "fourth");
				string_map.Remove("first");
				
				Debug.Assert(string_map.Count == 1);
				Debug.Assert(!string_map.ContainsKey("first"));
				Debug.Assert(string_map.ContainsKey("third"));
				
				string test_value;

				Debug.Assert(string_map.TryGetValue("third", out test_value));
				Debug.Assert(test_value == "fourth");
				Debug.Assert(!string_map.TryGetValue("first", out test_value));
			}
		}
	}
}