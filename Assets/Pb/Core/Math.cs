namespace Pb
{
	/// <summary>
	/// Contains mathematical helper functions
	/// </summary>
	public static class Math
	{
		/// <summary>
		/// Divides the first integer by the second and rounds down to the nearest integer without loss of precision
		/// </summary>
		/// <param name="a">The dividend</param>
		/// <param name="b">The divisor</param>
		/// <returns>The quotient rounded down to the nearest integer</returns>
		public static int FloorDivide(int a, int b)
		{
			if (b < 0)
			{
				a = -a;
				b = -b;
			}

			if (a < 0)
				return (a - b + 1) / b;
			return a / b;
		}
		/// <summary>
		/// Divides the first integer by the second and rounds up to the nearest integer without loss of precision
		/// </summary>
		/// <param name="a">The dividend</param>
		/// <param name="b">The divisor</param>
		/// <returns>The quotient rounded up to the nearest integer</returns>
		public static int CeilDivide(int a, int b)
		{
			if (b < 0)
			{
				a = -a;
				b = -b;
			}

			if (a < 0)
				return a / b;
			return (a + b - 1) / b;
		}
		/// <summary>
		/// Gets the RGB color associated with a hex color code (eg. F0563D)
		/// </summary>
		/// <param name="hexcode">The hex color code</param>
		/// <returns>The color associated with the hex color code</returns>
		public static UnityEngine.Color32 HexToRGB(string hexcode)
		{
			int bytes = int.Parse(hexcode, System.Globalization.NumberStyles.HexNumber);
			return new UnityEngine.Color32(
				(byte)((bytes >> 16) & 0xFF),
				(byte)((bytes >> 8) & 0xFF),
				(byte)(bytes & 0xFF),
				0xFF
				);
		}
		/// <summary>
		/// Calculates the RGB color corresponding to the given HSV coordinates
		/// </summary>
		/// <param name="h">The hue component of the color</param>
		/// <param name="s">The saturation component of the color</param>
		/// <param name="v">The value component of the color</param>
		/// <returns>The RGB color corresponding to the given HSV coordinates</returns>
		public static UnityEngine.Color HSVToRGB(float h, float s = 1.0f, float v = 1.0f)
		{
			h = h * 360.0f % 360.0f;

			float c = v * s;
			float x = c * (1.0f - System.Math.Abs(h / 60.0f % 2.0f - 1.0f));
			float m = v - c;

			float rp = 0.0f;
			float gp = 0.0f;
			float bp = 0.0f;

			if (h >= 0.0f && h < 60.0f)
			{
				rp = c;
				gp = x;
			}
			else if (h >= 60.0f && h < 120.0f)
			{
				rp = x;
				gp = c;
			}
			else if (h >= 120.0f && h < 180.0f)
			{
				gp = c;
				bp = x;
			}
			else if (h >= 180.0f && h < 240.0f)
			{
				gp = x;
				bp = c;
			}
			else if (h >= 240.0f && h < 300.0f)
			{
				rp = x;
				bp = c;
			}
			else
			{
				rp = c;
				bp = x;
			}

			return new UnityEngine.Color(rp + m, gp + m, bp + m);
		}
		/// <summary>
		/// Maps integers to natural numbers (0 included)
		/// </summary>
		/// <param name="x">An integer</param>
		/// <returns>A unique natural number corresponding to that integer</returns>
		public static uint ZToN(int x)
		{
			return (uint)x;
		}
		/// <summary>
		/// Maps natural numbers (0 included) to integers
		/// </summary>
		/// <param name="x">A natural number</param>
		/// <returns>A unique integer corresponding to that natural number</returns>
		public static int NToZ(uint x)
		{
			return (int)x;
		}
		/// <summary>
		/// Pairs two integers into a natural number
		/// </summary>
		/// <param name="x">The first integer to pair</param>
		/// <param name="y">The second integer to pair</param>
		/// <returns>A natural number corresponding to the pair of the two integers</returns>
		public static uint Pair(int x, int y)
		{
			return Pair(ZToN(x), ZToN(y));
		}
		/// <summary>
		/// Pairs two natural numbers into a natural number
		/// </summary>
		/// <param name="x">The first natural number to pair</param>
		/// <param name="y">The second natural number to pair</param>
		/// <returns>A natural number corresponding to the pair of the two natural numbers</returns>
		public static uint Pair(uint x, uint y)
		{
			return (x + y) * (x + y + 1) / 2 + y;
		}
		/// <summary>
		/// Determines the pair of integers that a natural number corresponds to
		/// </summary>
		/// <param name="z">The natural number</param>
		/// <param name="x">The first integer of the pair</param>
		/// <param name="y">The second integer of the pair</param>
		public static void Unpair(uint z, out int x, out int y)
		{
			uint ux;
			uint uy;
			Unpair(z, out ux, out uy);
			x = NToZ(ux);
			y = NToZ(uy);
		}
		/// <summary>
		/// Determines the pair of natural numbers that a natural number corresponds to
		/// </summary>
		/// <param name="z">The natural number</param>
		/// <param name="x">The first natural number of the pair</param>
		/// <param name="y">The second natural number of the pair</param>
		public static void Unpair(uint z, out uint x, out uint y)
		{
			uint w = (uint)System.Math.Floor((System.Math.Sqrt(8.0 * z + 1.0) - 1.0) / 2.0);
			uint t = (w * w + w) / 2;
			y = z - t;
			x = w - y;
		}
	}
}