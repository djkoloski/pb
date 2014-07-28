using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pb
{
	namespace Collections
	{
		/// <summary>
		/// A tuple class of two objects
		/// </summary>
		/// <typeparam name="T1">The first type to store in the tuple</typeparam>
		/// <typeparam name="T2">The second type to store in the tuple</typeparam>
		[System.Serializable]
		public class Tuple<T1, T2> :
			System.IComparable,
			System.IComparable<Tuple<T1, T2>>
		{
			/// <summary>
			/// The first object stored in the tuple
			/// </summary>
			[SerializeField]
			protected T1 item1_;
			/// <summary>
			/// The second object stored in the tuple
			/// </summary>
			[SerializeField]
			protected T2 item2_;

			private int CompareTo_(Tuple<T1, T2> other)
			{
				Comparer<T1> c1 = Comparer<T1>.Default;
				Comparer<T2> c2 = Comparer<T2>.Default;

				if (other == null)
					return 1;
				if (c1.Compare(item1_, other.item1_) > 0)
					return 1;
				else if (c1.Compare(item1_, other.item1_) < 0)
					return -1;
				else
					if (c2.Compare(item2_, other.item2_) > 0)
						return 1;
					else if (c2.Compare(item2_, other.item2_) < 0)
						return -1;
					else
						return 0;
			}

			int System.IComparable.CompareTo(object obj)
			{
				if (obj == null)
					return 1;

				return CompareTo_(obj as Tuple<T1, T2>);
			}

			int System.IComparable<Tuple<T1, T2>>.CompareTo(Tuple<T1, T2> other)
			{
				return CompareTo_(other);
			}

			/// <summary>
			/// Constructor for the tuple
			/// </summary>
			/// <param name="item1">The first item in the tuple</param>
			/// <param name="item2">The second item in the tuple</param>
			public Tuple(T1 item1, T2 item2)
			{
				item1_ = item1;
				item2_ = item2;
			}
			/// <summary>
			/// The first item in the tuple (readonly)
			/// </summary>
			public T1 Item1
			{
				get
				{
					return item1_;
				}
			}
			/// <summary>
			/// The second item in the tuple (readonly)
			/// </summary>
			public T2 Item2
			{
				get
				{
					return item2_;
				}
			}
			/// <summary>
			/// Determines whether the tuple is equal to another object
			/// </summary>
			/// <param name="obj">The object to compare the tuple to</param>
			/// <returns>Whether the two tuples are equal</returns>
			public override bool Equals(object obj)
			{
				if (obj == null)
					return false;

				Tuple<T1, T2> other = obj as Tuple<T1, T2>;
				if (other == null)
					return false;
				return (item1_.Equals(other.item1_) && item2_.Equals(other.item2_));
			}
			/// <summary>
			/// Determines the hash code of the tuple
			/// </summary>
			/// <returns>The hash code of the tuple</returns>
			public override int GetHashCode()
			{
				return Math.NToZ(Math.Pair(item1_.GetHashCode(), item2_.GetHashCode()));
			}
			/// <summary>
			/// Gets a string representing the tuple
			/// </summary>
			/// <returns>A string representing the tuple</returns>
			public override string ToString()
			{
				return "(" + item1_.ToString() + ", " + item2_.ToString() + ")";
			}
		}
		/// <summary>
		/// A tuple class of three objects
		/// </summary>
		/// <typeparam name="T1">The first type to store in the tuple</typeparam>
		/// <typeparam name="T2">The second type to store in the tuple</typeparam>
		/// <typeparam name="T3">The third type to store in the tuple</typeparam>
		[System.Serializable]
		public class Tuple<T1, T2, T3> :
			System.IComparable,
			System.IComparable<Tuple<T1, T2, T3>>
		{
			/// <summary>
			/// The first object stored in the tuple
			/// </summary>
			[SerializeField]
			protected T1 item1_;
			/// <summary>
			/// The second object stored in the tuple
			/// </summary>
			[SerializeField]
			protected T2 item2_;
			/// <summary>
			/// The third object stored in the tuple
			/// </summary>
			[SerializeField]
			protected T3 item3_;

			private int CompareTo_(Tuple<T1, T2, T3> other)
			{
				Comparer<T1> c1 = Comparer<T1>.Default;
				Comparer<T2> c2 = Comparer<T2>.Default;
				Comparer<T3> c3 = Comparer<T3>.Default;

				if (other == null)
					return 1;
				if (c1.Compare(item1_, other.item1_) > 0)
					return 1;
				else if (c1.Compare(item1_, other.item1_) < 0)
					return -1;
				else
					if (c2.Compare(item2_, other.item2_) > 0)
						return 1;
					else if (c2.Compare(item2_, other.item2_) < 0)
						return -1;
					else
						if (c3.Compare(item3_, other.item3_) > 0)
							return 1;
						else if (c3.Compare(item3_, other.item3_) < 0)
							return -1;
						else
							return 0;
			}

			int System.IComparable.CompareTo(object obj)
			{
				if (obj == null)
					return 1;

				return CompareTo_(obj as Tuple<T1, T2, T3>);
			}

			int System.IComparable<Tuple<T1, T2, T3>>.CompareTo(Tuple<T1, T2, T3> other)
			{
				return CompareTo_(other);
			}

			/// <summary>
			/// Constructor for the tuple
			/// </summary>
			/// <param name="item1">The first object to store in the tuple</param>
			/// <param name="item2">The second object to store in the tuple</param>
			/// <param name="item3">The third object to store in the tuple</param>
			public Tuple(T1 item1, T2 item2, T3 item3)
			{
				item1_ = item1;
				item2_ = item2;
				item3_ = item3;
			}
			/// <summary>
			/// The first object in the tuple (readonly)
			/// </summary>
			public T1 Item1
			{
				get
				{
					return item1_;
				}
			}
			/// <summary>
			/// The second object in the tuple (readonly)
			/// </summary>
			public T2 Item2
			{
				get
				{
					return item2_;
				}
			}
			/// <summary>
			/// The third object in the tuple (readonly)
			/// </summary>
			public T3 Item3
			{
				get
				{
					return item3_;
				}
			}
			/// <summary>
			/// Determines whether the tuple is equal to another object
			/// </summary>
			/// <param name="obj">The object to compare the tuple to</param>
			/// <returns>Whether the two tuples are equal</returns>
			public override bool Equals(object obj)
			{
				if (obj == null)
					return false;

				Tuple<T1, T2, T3> other = obj as Tuple<T1, T2, T3>;
				if (other == null)
					return false;
				return (item1_.Equals(other.item1_) && item2_.Equals(other.item2_) && item3_.Equals(other.item3_));
			}
			/// <summary>
			/// Gets the hash code of the tuple
			/// </summary>
			/// <returns>The hash code of the tuple</returns>
			public override int GetHashCode()
			{
				return Math.NToZ(
					Math.Pair(
						Math.ZToN(item1_.GetHashCode()),
						Math.Pair(
							Math.ZToN(item1_.GetHashCode()),
							Math.ZToN(item2_.GetHashCode())
						)
					)
				);
			}
			/// <summary>
			/// Gets a string representing the tuple
			/// </summary>
			/// <returns>A string representing the tuple</returns>
			public override string ToString()
			{
				return "(" + item1_.ToString() + ", " + item2_.ToString() + ", " + item3_.ToString() + ")";
			}
		}
	}
}