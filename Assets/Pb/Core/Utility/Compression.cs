using System.Collections;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;

namespace Pb
{
	namespace Utility
	{
		/// <summary>
		/// Decompresses blocks of compressed data
		/// </summary>
		public class Decompress
		{
			/// <summary>
			/// Zlib-decompresses the requested number of bytes from the input array to the output array
			/// </summary>
			/// <param name="input">The Zlib-compressed bytes</param>
			/// <param name="output">An array to put the Zlib-decompressed bytes into</param>
			/// <param name="request">The number of bytes to read</param>
			/// <returns>The number of bytes decompressed</returns>
			public static int Zlib(byte[] input, byte[] output, int request)
			{
				MemoryStream stream = new MemoryStream(input);
				ZlibStream zlib = new ZlibStream(stream, CompressionMode.Decompress);
				return zlib.Read(output, 0, request);
			}
			/// <summary>
			/// GZip-decompresses the requested number of bytes from the input array to the output array
			/// </summary>
			/// <param name="input">The GZip-compressed bytes</param>
			/// <param name="output">An array to put the GZip-decompressed bytes into</param>
			/// <param name="request">The number of bytes to read</param>
			/// <returns>The number of bytes decompressed</returns>
			public static int GZip(byte[] input, byte[] output, int request)
			{
				MemoryStream stream = new MemoryStream(input);
				GZipStream gzip = new GZipStream(stream, CompressionMode.Decompress);
				return gzip.Read(output, 0, request);
			}
		}
	}
}
