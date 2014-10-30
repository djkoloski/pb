using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pb.Collections
{
	/// <summary>
	/// An integer vector class with two values
	/// </summary>
	[System.Serializable]
	public struct IVector2 :
		System.IComparable,
		System.IComparable<IVector2>
	{
		public class IntervalEnumerable :
			IEnumerable,
			IEnumerable<IVector2>
		{
			public class Enumerator :
				System.IDisposable,
				IEnumerator,
				IEnumerator<IVector2>
			{
				private IntervalEnumerable enumerable_ = null;
				private IVector2 current_ = IVector2.zero;

				private IVector2 Current_()
				{
					return current_;
				}
				private bool MoveNext_()
				{
					if (current_.x < enumerable_.end.x)
						current_ = new IVector2(current_.x + 1, current_.y);
					else if (current_.y < enumerable_.end.y)
						current_ = new IVector2(enumerable_.begin.x, current_.y + 1);
					else
						return false;
					return true;
				}
				private void Reset_()
				{
					current_ = IVector2.zero;
				}
				private void Dispose_()
				{
					enumerable_ = null;
					current_ = IVector2.zero;
				}
				/// <summary>
				/// Constructs the enumerator from an interval enumerable to enumerate over
				/// </summary>
				/// <param name="enumerable">The interval enumerable to enumerate over</param>
				public Enumerator(IntervalEnumerable enumerable)
				{
					enumerable_ = enumerable;
					current_ = enumerable_.begin + IVector2.left;
				}

				void System.IDisposable.Dispose()
				{
					Dispose_();
				}

				object IEnumerator.Current
				{
					get
					{
						return Current_();
					}
				}
				bool IEnumerator.MoveNext()
				{
					return MoveNext_();
				}
				void IEnumerator.Reset()
				{
					Reset_();
				}

				IVector2 IEnumerator<IVector2>.Current
				{
					get
					{
						return Current_();
					}
				}
			}

			private IVector2 begin;
			private IVector2 end;

			private Enumerator GetEnumerator_()
			{
				return new Enumerator(this);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator_();
			}

			IEnumerator<IVector2> IEnumerable<IVector2>.GetEnumerator()
			{
				return GetEnumerator_();
			}

			/// <summary>
			/// Constructs the interval enumerable with a beginning and ending IVector2
			/// </summary>
			/// <param name="b">The beginning vector</param>
			/// <param name="e">The ending vector</param>
			public IntervalEnumerable(IVector2 b, IVector2 e)
			{
				begin = b;
				end = e;
			}
		}
		/// <summary>
		/// Gets an enumerator over the interval [a, b]
		/// </summary>
		/// <param name="a">The beginning IVector2</param>
		/// <param name="b">The ending IVector2</param>
		/// <returns>An enumerable over the interval between the IVector2s</returns>
		public static IntervalEnumerable Interval(IVector2 a, IVector2 b)
		{
			return new IntervalEnumerable(a, b);
		}
		/// <summary>
		/// Gets an enumerator over the interval [a, b)
		/// </summary>
		/// <param name="a">The beginning IVector2</param>
		/// <param name="b">The ending IVector2</param>
		/// <returns>An enumerable over the interval between the IVector2s</returns>
		public static IntervalEnumerable Range(IVector2 a, IVector2 b)
		{
			return new IntervalEnumerable(a, b - IVector2.one);
		}

		/// <summary>
		/// The zero integer vector
		/// </summary>
		public static readonly IVector2 zero = new IVector2(0, 0);
		/// <summary>
		/// The one integer vector
		/// </summary>
		public static readonly IVector2 one = new IVector2(1, 1);
		/// <summary>
		/// The left integer vector
		/// </summary>
		public static readonly IVector2 left = new IVector2(-1, 0);
		/// <summary>
		/// The right integer vector
		/// </summary>
		public static readonly IVector2 right = new IVector2(1, 0);
		/// <summary>
		/// The down integer vector
		/// </summary>
		public static readonly IVector2 down = new IVector2(0, -1);
		/// <summary>
		/// The up integer vector
		/// </summary>
		public static readonly IVector2 up = new IVector2(0, 1);
		/// <summary>
		/// Makes a new Vector2 out of an integer vector with two components
		/// </summary>
		/// <param name="iv">The IVector2</param>
		/// <returns>A new Vector2</returns>
		public static explicit operator Vector2(IVector2 iv)
		{
			return new Vector2(iv.x, iv.y);
		}
		/// <summary>
		/// Makes a new IVector3 out of an integer vector with two components
		/// </summary>
		/// <param name="iv"></param>
		/// <returns></returns>
		public static explicit operator IVector3(IVector2 iv)
		{
			return new IVector3(iv.x, iv.y, 0);
		}
		/// <summary>
		/// Makes a new Vector3 out of an integer vector with two components
		/// </summary>
		/// <param name="iv"></param>
		/// <returns></returns>
		public static explicit operator Vector3(IVector2 iv)
		{
			return new Vector3(iv.x, iv.y, 0.0f);
		}
		/// <summary>
		/// The X component
		/// </summary>
		public int x;
		/// <summary>
		/// The Y component
		/// </summary>
		public int y;

		private int CompareTo_(IVector2 other)
		{
			if (x > other.x)
				return 1;
			else if (x < other.x)
				return -1;
			else
				if (y > other.y)
					return 1;
				else if (y < other.y)
					return -1;
				else
					return 0;
		}

		int System.IComparable.CompareTo(object obj)
		{
			if (obj == null || !(obj is IVector2))
				return 1;

			return CompareTo_((IVector2)obj);
		}

		int System.IComparable<IVector2>.CompareTo(IVector2 other)
		{
			return CompareTo_(other);
		}

		/// <summary>
		/// Gets the X component if the index is 0, and the Y component if the index is 1
		/// </summary>
		/// <param name="i">The index of the component to get</param>
		/// <returns>The indexed component</returns>
		public int this[int i]
		{
			get
			{
				switch (i)
				{
					case 0:
						return x;
					case 1:
						return y;
					default:
						throw new System.ArgumentOutOfRangeException("Requested value not in range");
				}
			}
		}
		/// <summary>
		/// Constructor for the integer vector
		/// </summary>
		/// <param name="a">The X component</param>
		/// <param name="b">The Y component</param>
		public IVector2(int a = 0, int b = 0)
		{
			x = a;
			y = b;
		}
		/// <summary>
		/// Determines whether the integer vector is in the interval [lower, upper]
		/// </summary>
		/// <param name="lower">The lower vector</param>
		/// <param name="upper">The upper vector</param>
		/// <returns>Whether the integer vector is in the interval</returns>
		public bool inInterval(IVector2 lower, IVector2 upper)
		{
			return (x >= lower.x && x <= upper.x && y >= lower.y && y <= upper.y);
		}
		/// <summary>
		/// Determines whether the integer vector is in the range [lower, upper)
		/// </summary>
		/// <param name="lower">The lower vector</param>
		/// <param name="upper">The upper vector</param>
		/// <returns>Whether the vector is in the range</returns>
		public bool inRange(IVector2 lower, IVector2 upper)
		{
			return (x >= lower.x && x < upper.x && y >= lower.y && y < upper.y);
		}
		/// <summary>
		/// Calculates the dot product of two integer vectors
		/// </summary>
		/// <param name="lhs">The left-hand side argument</param>
		/// <param name="rhs">The right-hand size argument</param>
		/// <returns>The dot product of the two integer vectors</returns>
		public static int Dot(IVector2 lhs, IVector2 rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y;
		}
		/// <summary>
		/// Calculates a new integer vector with the larger components of both integer vectors
		/// </summary>
		/// <param name="lhs">The left-hand side argument</param>
		/// <param name="rhs">The right-hand side argument</param>
		/// <returns>A new integer vector with the largest components of both integer vectors</returns>
		public static IVector2 Max(IVector2 lhs, IVector2 rhs)
		{
			return new IVector2((lhs.x > rhs.x ? lhs.x : rhs.x), (lhs.y > rhs.y ? lhs.y : rhs.y));
		}
		/// <summary>
		/// Calculates a new integer vector with the smaller components of both integer vectors
		/// </summary>
		/// <param name="lhs">The left-hand side argument</param>
		/// <param name="rhs">The right-hand side argument</param>
		/// <returns>A new integer vector with the smallest components of both integer vectors</returns>
		public static IVector2 Min(IVector2 lhs, IVector2 rhs)
		{
			return new IVector2((lhs.x < rhs.x ? lhs.x : rhs.x), (lhs.y < rhs.y ? lhs.y : rhs.y));
		}
		/// <summary>
		/// Multiplies two integer vectors component-wise
		/// </summary>
		/// <param name="a">The integer to scale</param>
		/// <param name="b">The integer to scale by</param>
		/// <returns>A scaled integer vector</returns>
		public static IVector2 Scale(IVector2 a, IVector2 b)
		{
			return new IVector2(a.x * b.x, a.y * b.y);
		}
		/// <summary>
		/// Adds together two integer vectors
		/// </summary>
		/// <param name="a">The first integer vector to add</param>
		/// <param name="b">The second integer vector to add</param>
		/// <returns>The sum of the two integer vectors</returns>
		public static IVector2 operator +(IVector2 a, IVector2 b)
		{
			return new IVector2(a.x + b.x, a.y + b.y);
		}
		/// <summary>
		/// Subtracts one integer vector from another
		/// </summary>
		/// <param name="a">The integer vector to subtract from</param>
		/// <param name="b">The integer vector to subtract</param>
		/// <returns>The difference of the two integer vectors</returns>
		public static IVector2 operator -(IVector2 a, IVector2 b)
		{
			return new IVector2(a.x - b.x, a.y - b.y);
		}
		/// <summary>
		/// Multiplies an integer vector by a scalar
		/// </summary>
		/// <param name="a">The integer vector to scale</param>
		/// <param name="b">The amount to scale the integer vector by</param>
		/// <returns>The integer vector scaled by the given scalar</returns>
		public static IVector2 operator *(IVector2 a, int b)
		{
			return new IVector2(a.x * b, a.y * b);
		}
		/// <summary>
		/// Divides an integer vector by a scalar
		/// </summary>
		/// <param name="a">The integer vector to divide</param>
		/// <param name="b">The amount to divide the integer vector by</param>
		/// <returns>The integer vector divided by the given scalar</returns>
		public static IVector2 operator /(IVector2 a, int b)
		{
			return new IVector2(a.x / b, a.y / b);
		}
		/// <summary>
		/// Calculates the area of the rectangle with dimensions of the given vector
		/// </summary>
		/// <param name="v">The vector to use</param>
		/// <returns>The area of the rectangle corresponding to the vector</returns>
		public static int Area(IVector2 v)
		{
			return v.x * v.y;
		}
		/// <summary>
		/// Calculates the index of a position in an area with indices organized X then Y
		/// </summary>
		/// <param name="position">The position in an area</param>
		/// <param name="area">The area</param>
		/// <returns>The index of the position in the area</returns>
		public static int ToIndex(IVector2 position, IVector2 area)
		{
			return position.y * area.x + position.x;
		}
		/// <summary>
		/// Calculates the position of an index in an area with indices organized X then Y
		/// </summary>
		/// <param name="index">The index in an area</param>
		/// <param name="area">The area</param>
		/// <returns>The position of the index in the area</returns>
		public static IVector2 FromIndex(int index, IVector2 area)
		{
			return new IVector2(index % area.x, index / area.x);
		}
		/// <summary>
		/// Creates a new IVector2 from a Vector2 by flooring the components
		/// </summary>
		/// <param name="v">A Vector2</param>
		/// <returns>An IVector2</returns>
		public static IVector2 Floor(Vector2 v)
		{
			return new IVector2(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y));
		}
		/// <summary>
		/// Creates a new IVector2 from a Vector2 by ceiling the components
		/// </summary>
		/// <param name="v">A Vector2</param>
		/// <returns>An IVector2</returns>
		public static IVector2 Ceil(Vector2 v)
		{
			return new IVector2(Mathf.CeilToInt(v.x), Mathf.CeilToInt(v.y));
		}
		/// <summary>
		/// Creates a new IVector2 from a Vector2 by rounding the components
		/// </summary>
		/// <param name="v">A Vector2</param>
		/// <returns>An IVector2</returns>
		public static IVector2 Round(Vector2 v)
		{
			return new IVector2(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
		}
		/// <summary>
		/// Determines the hash code of the integer vector
		/// </summary>
		/// <returns>The hash code of the integer vector</returns>
		public override int GetHashCode()
		{
			return Math.NToZ(Math.Pair(Math.ZToN(x.GetHashCode()), Math.ZToN(y.GetHashCode())));
		}
		/// <summary>
		/// Gets a string representing the integer vector
		/// </summary>
		/// <returns>A string representing the integer vector</returns>
		public override string ToString()
		{
			return "(" + x + ", " + y + ")";
		}
	}

	/// <summary>
	/// An integer vector class with two values
	/// </summary>
	[System.Serializable]
	public struct IVector3 :
		System.IComparable,
		System.IComparable<IVector3>
	{
		public class RangeEnumerable :
			IEnumerable,
			IEnumerable<IVector3>
		{
			public class Enumerator :
				System.IDisposable,
				IEnumerator,
				IEnumerator<IVector3>
			{
				private RangeEnumerable enumerable_ = null;
				private IVector3 current_ = IVector3.zero;

				private IVector3 Current_()
				{
					return current_;
				}
				private bool MoveNext_()
				{
					if (current_.x < enumerable_.end.x)
						current_ = new IVector3(current_.x + 1, current_.y, current_.z);
					else if (current_.y < enumerable_.end.y)
						current_ = new IVector3(enumerable_.begin.x, current_.y + 1, current_.z);
					else if (current_.z < enumerable_.end.z)
						current_ = new IVector3(enumerable_.begin.x, enumerable_.begin.y, current_.z + 1);
					else
						return false;
					return true;
				}
				private void Reset_()
				{
					current_ = IVector3.zero;
				}
				private void Dispose_()
				{
					enumerable_ = null;
					current_ = IVector3.zero;
				}
				/// <summary>
				/// Constructs the enumerator from a range enumerable to enumerate over
				/// </summary>
				/// <param name="enumerable">The range enumerable to enumerate over</param>
				public Enumerator(RangeEnumerable enumerable)
				{
					enumerable_ = enumerable;
					current_ = enumerable_.begin + IVector3.left;
				}

				void System.IDisposable.Dispose()
				{
					Dispose_();
				}

				object IEnumerator.Current
				{
					get
					{
						return Current_();
					}
				}
				bool IEnumerator.MoveNext()
				{
					return MoveNext_();
				}
				void IEnumerator.Reset()
				{
					Reset_();
				}

				IVector3 IEnumerator<IVector3>.Current
				{
					get
					{
						return Current_();
					}
				}
			}

			private IVector3 begin;
			private IVector3 end;

			private Enumerator GetEnumerator_()
			{
				return new Enumerator(this);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator_();
			}

			IEnumerator<IVector3> IEnumerable<IVector3>.GetEnumerator()
			{
				return GetEnumerator_();
			}

			/// <summary>
			/// Constructs the range enumerable with a beginning and ending IVector2
			/// </summary>
			/// <param name="b"></param>
			/// <param name="e"></param>
			public RangeEnumerable(IVector3 b, IVector3 e)
			{
				begin = b;
				end = e;
			}
		}
		/// <summary>
		/// Gets an enumerator over the interval [a, b]
		/// </summary>
		/// <param name="a">The beginning IVector3</param>
		/// <param name="b">The ending IVector3</param>
		/// <returns>An enumerable over the interval between the IVector3s</returns>
		public static RangeEnumerable Interval(IVector3 a, IVector3 b)
		{
			return new RangeEnumerable(a, b);
		}
		/// <summary>
		/// Gets an enumerator over the interval [a, b)
		/// </summary>
		/// <param name="a">The beginning IVector3</param>
		/// <param name="b">The ending IVector3</param>
		/// <returns>An enumerable over the interval between the IVector3s</returns>
		public static RangeEnumerable Range(IVector3 a, IVector3 b)
		{
			return new RangeEnumerable(a, b - IVector3.one);
		}

		/// <summary>
		/// The zero integer vector
		/// </summary>
		public static readonly IVector3 zero = new IVector3(0, 0, 0);
		/// <summary>
		/// The one integer vector
		/// </summary>
		public static readonly IVector3 one = new IVector3(1, 1, 1);
		/// <summary>
		/// The left integer vector
		/// </summary>
		public static readonly IVector3 left = new IVector3(-1, 0, 0);
		/// <summary>
		/// The right integer vector
		/// </summary>
		public static readonly IVector3 right = new IVector3(1, 0, 0);
		/// <summary>
		/// The down integer vector
		/// </summary>
		public static readonly IVector3 down = new IVector3(0, -1, 0);
		/// <summary>
		/// The up integer vector
		/// </summary>
		public static readonly IVector3 up = new IVector3(0, 1, 0);
		/// <summary>
		/// The back integer vector
		/// </summary>
		public static readonly IVector3 back = new IVector3(0, 0, -1);
		/// <summary>
		/// The forward integer vector
		/// </summary>
		public static readonly IVector3 forward = new IVector3(0, 0, 1);
		/// <summary>
		/// Makes a new Vector3 out of an integer vector with three components
		/// </summary>
		/// <param name="iv">The IVector3</param>
		/// <returns>A new Vector3</returns>
		public static explicit operator Vector3(IVector3 iv)
		{
			return new Vector3(iv.x, iv.y, iv.z);
		}
		/// <summary>
		/// The X component
		/// </summary>
		public int x;
		/// <summary>
		/// The Y component
		/// </summary>
		public int y;
		/// <summary>
		/// The Z component
		/// </summary>
		public int z;

		private int CompareTo_(IVector3 other)
		{
			if (x > other.x)
				return 1;
			else if (x < other.x)
				return -1;
			else
				if (y > other.y)
					return 1;
				else if (y < other.y)
					return -1;
				else
					if (z > other.z)
						return 1;
					else if (z < other.z)
						return -1;
					else
						return 0;
		}

		int System.IComparable.CompareTo(object obj)
		{
			if (obj == null || !(obj is IVector3))
				return 1;

			return CompareTo_((IVector3)obj);
		}

		int System.IComparable<IVector3>.CompareTo(IVector3 other)
		{
			return CompareTo_(other);
		}

		/// <summary>
		/// Gets the X component if the index is 0, the Y component if the index is 1, and the Z component if the index is 2
		/// </summary>
		/// <param name="i">The index of the component to get</param>
		/// <returns>The indexed component</returns>
		public int this[int i]
		{
			get
			{
				switch (i)
				{
					case 0:
						return x;
					case 1:
						return y;
					case 2:
						return z;
					default:
						throw new System.ArgumentOutOfRangeException("Requested value not in range");
				}
			}
		}
		/// <summary>
		/// Constructor for the integer vector
		/// </summary>
		/// <param name="a">The X component</param>
		/// <param name="b">The Y component</param>
		/// <param name="c">The Z component</param>
		public IVector3(int a = 0, int b = 0, int c = 0)
		{
			x = a;
			y = b;
			z = c;
		}
		/// <summary>
		/// Determines whether the integer vector is in the interval [lower, upper]
		/// </summary>
		/// <param name="lower">The lower vector</param>
		/// <param name="upper">The upper vector</param>
		/// <returns>Whether the integer vector is in the interval</returns>
		public bool inInterval(IVector3 lower, IVector3 upper)
		{
			return (x >= lower.x && x <= upper.x && y >= lower.y && y <= upper.y && z >= lower.z && z <= upper.z);
		}
		/// <summary>
		/// Determines whether the integer vector is in the range [lower, upper)
		/// </summary>
		/// <param name="lower">The lower vector</param>
		/// <param name="upper">The upper vector</param>
		/// <returns>Whether the vector is in the range</returns>
		public bool inRange(IVector3 lower, IVector3 upper)
		{
			return (x >= lower.x && x < upper.x && y >= lower.y && y < upper.y && z >= lower.z && z < upper.z);
		}
		/// <summary>
		/// Creates a new IVector2 by discarding the Z component
		/// </summary>
		/// <returns>An IVector2</returns>
		public IVector2 discardZ()
		{
			return new IVector2(x, y);
		}
		/// <summary>
		/// Calculates the dot product of two integer vectors
		/// </summary>
		/// <param name="lhs">The left-hand side argument</param>
		/// <param name="rhs">The right-hand size argument</param>
		/// <returns>The dot product of the two integer vectors</returns>
		public static int Dot(IVector3 lhs, IVector3 rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
		}
		/// <summary>
		/// Calculates a new integer vector with the larger components of both integer vectors
		/// </summary>
		/// <param name="lhs">The left-hand side argument</param>
		/// <param name="rhs">The right-hand side argument</param>
		/// <returns>A new integer vector with the largest components of both integer vectors</returns>
		public static IVector3 Max(IVector3 lhs, IVector3 rhs)
		{
			return new IVector3((lhs.x > rhs.x ? lhs.x : rhs.x), (lhs.y > rhs.y ? lhs.y : rhs.y), (lhs.z > rhs.z ? lhs.z : rhs.z));
		}
		/// <summary>
		/// Calculates a new integer vector with the smaller components of both integer vectors
		/// </summary>
		/// <param name="lhs">The left-hand side argument</param>
		/// <param name="rhs">The right-hand side argument</param>
		/// <returns>A new integer vector with the smallest components of both integer vectors</returns>
		public static IVector3 Min(IVector3 lhs, IVector3 rhs)
		{
			return new IVector3((lhs.x < rhs.x ? lhs.x : rhs.x), (lhs.y < rhs.y ? lhs.y : rhs.y), (lhs.z < rhs.z ? lhs.z : rhs.z));
		}
		/// <summary>
		/// Multiplies two integer vectors component-wise
		/// </summary>
		/// <param name="a">The integer to scale</param>
		/// <param name="b">The integer to scale by</param>
		/// <returns>A scaled integer vector</returns>
		public static IVector3 Scale(IVector3 a, IVector3 b)
		{
			return new IVector3(a.x * b.x, a.y * b.y, a.z * b.z);
		}
		/// <summary>
		/// Adds together two integer vectors
		/// </summary>
		/// <param name="a">The first integer vector to add</param>
		/// <param name="b">The second integer vector to add</param>
		/// <returns>The sum of the two integer vectors</returns>
		public static IVector3 operator +(IVector3 a, IVector3 b)
		{
			return new IVector3(a.x + b.x, a.y + b.y, a.z + b.z);
		}
		/// <summary>
		/// Subtracts one integer vector from another
		/// </summary>
		/// <param name="a">The integer vector to subtract from</param>
		/// <param name="b">The integer vector to subtract</param>
		/// <returns>The difference of the two integer vectors</returns>
		public static IVector3 operator -(IVector3 a, IVector3 b)
		{
			return new IVector3(a.x - b.x, a.y - b.y, a.z - b.z);
		}
		/// <summary>
		/// Multiplies an integer vector by a scalar
		/// </summary>
		/// <param name="a">The integer vector to scale</param>
		/// <param name="b">The amount to scale the integer vector by</param>
		/// <returns>The integer vector scaled by the given scalar</returns>
		public static IVector3 operator *(IVector3 a, int b)
		{
			return new IVector3(a.x * b, a.y * b, a.z * b);
		}
		/// <summary>
		/// Divides an integer vector by a scalar
		/// </summary>
		/// <param name="a">The integer vector to divide</param>
		/// <param name="b">The amount to divide the integer vector by</param>
		/// <returns>The integer vector divided by the given scalar</returns>
		public static IVector3 operator /(IVector3 a, int b)
		{
			return new IVector3(a.x / b, a.y / b, a.z / b);
		}
		/// <summary>
		/// Calculates the volume of the rectangular prism with dimensions of the given vector
		/// </summary>
		/// <param name="v">The vector to use</param>
		/// <returns>The volume of the rectangular prism corresponding to the vector</returns>
		public static int Volume(IVector3 v)
		{
			return v.x * v.y * v.z;
		}
		/// <summary>
		/// Calculates the index of a position in a volume with indices organized X, Y, then Z
		/// </summary>
		/// <param name="position">The position in a volume</param>
		/// <param name="volume">The volume</param>
		/// <returns>The index of the position in the volume</returns>
		public static int ToIndex(IVector3 position, IVector3 volume)
		{
			return (position.z * volume.y + position.y) * volume.x + position.x;
		}
		/// <summary>
		/// Calculates the position of an index in an volume with indices organized X then Y
		/// </summary>
		/// <param name="index">The index in an volume</param>
		/// <param name="area">The volume</param>
		/// <returns>The position of the index in the volume</returns>
		public static IVector3 FromIndex(int index, IVector3 volume)
		{
			return new IVector3(index % volume.x, index / volume.x % volume.y, index / volume.x / volume.y);
		}
		/// <summary>
		/// Creates a new IVector3 from a Vector3 by flooring the components
		/// </summary>
		/// <param name="v">A Vector3</param>
		/// <returns>An IVector3</returns>
		public static IVector3 Floor(Vector3 v)
		{
			return new IVector3(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y), Mathf.FloorToInt(v.z));
		}
		/// <summary>
		/// Creates a new IVector3 from a Vector3 by ceiling the components
		/// </summary>
		/// <param name="v">A Vector3</param>
		/// <returns>An IVector3</returns>
		public static IVector3 Ceil(Vector3 v)
		{
			return new IVector3(Mathf.CeilToInt(v.x), Mathf.CeilToInt(v.y), Mathf.CeilToInt(v.z));
		}
		/// <summary>
		/// Creates a new IVector3 from a Vector3 by rounding the components
		/// </summary>
		/// <param name="v">A Vector3</param>
		/// <returns>An IVector3</returns>
		public static IVector3 Round(Vector3 v)
		{
			return new IVector3(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
		}
		/// <summary>
		/// Determines the hash code of the integer vector
		/// </summary>
		/// <returns>The hash code of the integer vector</returns>
		public override int GetHashCode()
		{
			return Math.NToZ(
				Math.Pair(
					Math.ZToN(x.GetHashCode()),
					Math.Pair(
						Math.ZToN(y.GetHashCode()),
						Math.ZToN(z.GetHashCode())
					)
				)
			);
		}
		/// <summary>
		/// Gets a string representing the integer vector
		/// </summary>
		/// <returns>A string representing the integer vector</returns>
		public override string ToString()
		{
			return "(" + x + ", " + y + ", " + z + ")";
		}
	}
}