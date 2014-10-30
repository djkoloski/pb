using System.Collections;
using System.Collections.Generic;

namespace Pb.Collections
{
	/// <summary>
	/// A relational map class that supports Unity serialization
	/// </summary>
	/// <typeparam name="TKey">The type of object to use as a key</typeparam>
	/// <typeparam name="TValue">The type of object to use as a value</typeparam>
	[System.Serializable]
	public class Map<TKey, TValue> :
		IDictionary,
		IDictionary<TKey, TValue>,
		ICollection,
		ICollection<KeyValuePair<TKey, TValue>>,
		IEnumerable,
		IEnumerable<KeyValuePair<TKey, TValue>>
	{
		/// <summary>
		/// A list of the keys in sorted order
		/// </summary>
		[UnityEngine.SerializeField]
		private List<TKey> keys_;
		/// <summary>
		/// The values in corresponding order with the keys
		/// </summary>
		[UnityEngine.SerializeField]
		private List<TValue> values_;

		/// <summary>
		/// An enumerator class for the map
		/// </summary>
		public class Enumerator :
			System.IDisposable,
			IEnumerator,
			IEnumerator<KeyValuePair<TKey, TValue>>,
			IDictionaryEnumerator
		{
			/// <summary>
			/// The map the enumerator is enumerating
			/// </summary>
			private Map<TKey, TValue> map_;
			/// <summary>
			/// The current index of the enumerator in the map
			/// </summary>
			private int index_ = -1;

			private KeyValuePair<TKey, TValue> Current_()
			{
				try
				{
					return new KeyValuePair<TKey, TValue>(map_.keys_[index_], map_.values_[index_]);
				}
				catch (System.IndexOutOfRangeException)
				{
					throw new System.InvalidOperationException();
				}
			}
			private bool MoveNext_()
			{
				++index_;
				return (index_ < map_.keys_.Count);
			}
			private void Reset_()
			{
				index_ = -1;
			}
			private void Dispose_()
			{
				map_ = null;
				index_ = -1;
			}

			/// <summary>
			/// Constructs the enumerator from a map to enumerate over
			/// </summary>
			/// <param name="map">The map to enumerate over</param>
			public Enumerator(Map<TKey, TValue> map)
			{
				map_ = map;
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

			KeyValuePair<TKey, TValue> IEnumerator<KeyValuePair<TKey, TValue>>.Current
			{
				get
				{
					return Current_();
				}
			}

			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					try
					{
						return new DictionaryEntry(map_.keys_[index_], map_.values_[index_]);
					}
					catch (System.IndexOutOfRangeException)
					{
						throw new System.InvalidOperationException();
					}
				}
			}
			object IDictionaryEnumerator.Key
			{
				get
				{
					try
					{
						return map_.keys_[index_];
					}
					catch (System.IndexOutOfRangeException)
					{
						throw new System.InvalidOperationException();
					}
				}
			}
			object IDictionaryEnumerator.Value
			{
				get
				{
					try
					{
						return map_.values_[index_];
					}
					catch (System.IndexOutOfRangeException)
					{
						throw new System.InvalidOperationException();
					}
				}
			}
		}

		private int Count_()
		{
			return keys_.Count;
		}
		private int GetKeyIndex_(TKey key)
		{
			if (key == null)
				throw new System.ArgumentNullException();
			return keys_.BinarySearch(key);
		}
		private TValue GetValue_(TKey key)
		{
			int index = GetKeyIndex_(key);
			if (index < 0)
				throw new KeyNotFoundException();
			return values_[index];
		}
		private bool TryGetValue_(TKey key, out TValue value)
		{
			int index = GetKeyIndex_(key);
			if (index < 0)
			{
				value = default(TValue);
				return false;
			}
			else
			{
				value = values_[index];
				return true;
			}
		}
		private bool ContainsKey_(TKey key)
		{
			return (GetKeyIndex_(key) >= 0);
		}
		private bool ContainsValue_(TValue value)
		{
			foreach (TValue val in values_)
				if (val.Equals(value))
					return true;
			return false;
		}
		private bool ContainsKeyValue_(TKey key, TValue value)
		{
			int index = GetKeyIndex_(key);
			return (index > 0 && values_[index].Equals(value));
		}
		private void Clear_()
		{
			keys_.Clear();
			values_.Clear();
		}
		private void AddKeyValue_(TKey key, TValue value)
		{
			int index = GetKeyIndex_(key);
			if (index >= 0)
				throw new System.ArgumentException();
			keys_.Insert(~index, key);
			values_.Insert(~index, value);
		}
		private void SetKeyValue_(TKey key, TValue value)
		{
			int index = GetKeyIndex_(key);
			if (index < 0)
			{
				keys_.Insert(~index, key);
				values_.Insert(~index, value);
			}
			else
				values_[index] = value;
		}
		private bool RemoveKey_(TKey key)
		{
			int index = GetKeyIndex_(key);
			if (index < 0)
				return false;
			keys_.RemoveAt(index);
			values_.RemoveAt(index);
			return true;
		}
		private bool RemoveKeyValue_(TKey key, TValue value)
		{
			int index = GetKeyIndex_(key);
			if (index < 0 || !values_[index].Equals(value))
				return false;
			keys_.RemoveAt(index);
			values_.RemoveAt(index);
			return true;
		}
		private void CopyTo_(System.Array array, int offset)
		{
			if (array == null)
				throw new System.ArgumentNullException();

			if (offset < 0)
				throw new System.ArgumentOutOfRangeException();

			if (array.Length - offset < keys_.Count)
				throw new System.ArgumentException();

			for (int i = 0; i < keys_.Count; ++i)
				array.SetValue(new KeyValuePair<TKey, TValue>(keys_[i], values_[i]), offset + i);
		}
		private Enumerator GetEnumerator_()
		{
			return new Enumerator(this);
		}

		bool IDictionary.IsFixedSize
		{
			get
			{
				return false;
			}
		}
		bool IDictionary.IsReadOnly
		{
			get
			{
				return false;
			}
		}
		object IDictionary.this[object key]
		{
			get
			{
				return GetValue_((TKey)key);
			}
			set
			{
				SetKeyValue_((TKey)key, (TValue)value);
			}
		}
		ICollection IDictionary.Keys
		{
			get
			{
				return keys_;
			}
		}
		ICollection IDictionary.Values
		{
			get
			{
				return values_;
			}
		}
		void IDictionary.Add(object key, object value)
		{
			AddKeyValue_((TKey)key, (TValue)value);
		}
		void IDictionary.Clear()
		{
			Clear_();
		}
		bool IDictionary.Contains(object key)
		{
			return ContainsKey_((TKey)key);
		}
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return GetEnumerator_();
		}
		void IDictionary.Remove(object key)
		{
			RemoveKey_((TKey)key);
		}

		TValue IDictionary<TKey, TValue>.this[TKey key]
		{
			get
			{
				return GetValue_(key);
			}
			set
			{
				SetKeyValue_(key, value);
			}
		}
		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get
			{
				return keys_;
			}
		}
		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get
			{
				return values_;
			}
		}
		void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
		{
			AddKeyValue_(key, value);
		}
		bool IDictionary<TKey, TValue>.ContainsKey(TKey key)
		{
			return ContainsKey_(key);
		}
		bool IDictionary<TKey, TValue>.Remove(TKey key)
		{
			return RemoveKey_(key);
		}
		bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
		{
			return TryGetValue_(key, out value);
		}

		int ICollection.Count
		{
			get
			{
				return Count_();
			}
		}
		bool ICollection.IsSynchronized
		{
			get
			{
				return true;
			}
		}
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}
		void ICollection.CopyTo(System.Array array, int offset)
		{
			CopyTo_(array, offset);
		}

		int ICollection<KeyValuePair<TKey, TValue>>.Count
		{
			get
			{
				return Count_();
			}
		}
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> pair)
		{
			AddKeyValue_(pair.Key, pair.Value);
		}
		void ICollection<KeyValuePair<TKey, TValue>>.Clear()
		{
			Clear_();
		}
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> pair)
		{
			return ContainsKeyValue_(pair.Key, pair.Value);
		}
		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int offset)
		{
			CopyTo_(array, offset);
		}
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> pair)
		{
			return RemoveKeyValue_(pair.Key, pair.Value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator_();
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return GetEnumerator_();
		}

		/// <summary>
		/// Constructs the map with no key-value pairs
		/// </summary>
		public Map()
		{
			keys_ = new List<TKey>();
			values_ = new List<TValue>();
		}
		/// <summary>
		/// Constructs the map with a copy of the key-value pairs in the given map
		/// </summary>
		/// <param name="other">The map to copy the key-value pairs of</param>
		public Map(Map<TKey, TValue> other)
		{
			keys_ = new List<TKey>(other.keys_);
			values_ = new List<TValue>(other.values_);
		}
		/// <summary>
		/// The number of key-value pairs in the map
		/// </summary>
		public int Count
		{
			get
			{
				return Count_();
			}
		}
		/// <summary>
		/// Gets or sets a value in the map
		/// </summary>
		/// <param name="key">The key of the value to get or set</param>
		/// <returns>The value corresponding to the given key</returns>
		public TValue this[TKey key]
		{
			get
			{
				return GetValue_(key);
			}
			set
			{
				SetKeyValue_(key, value);
			}
		}
		/// <summary>
		/// The keys in the map
		/// </summary>
		public List<TKey> Keys
		{
			get
			{
				return keys_;
			}
		}
		/// <summary>
		/// The values in the map
		/// </summary>
		public List<TValue> Values
		{
			get
			{
				return values_;
			}
		}
		/// <summary>
		/// Adds the given key and value to the map
		/// </summary>
		/// <param name="key">The key to add</param>
		/// <param name="value">The value to pair with the key</param>
		public void Add(TKey key, TValue value)
		{
			AddKeyValue_(key, value);
		}
		/// <summary>
		/// Removes all key-value pairs from the map
		/// </summary>
		public void Clear()
		{
			Clear_();
		}
		/// <summary>
		/// Determines whether the given key is in the map
		/// </summary>
		/// <param name="key">The key to search for</param>
		/// <returns>Whether the key is in the map</returns>
		public bool ContainsKey(TKey key)
		{
			return ContainsKey_(key);
		}
		/// <summary>
		/// Determines whether the given value is in the map
		/// </summary>
		/// <param name="value">The value to search for</param>
		/// <returns>Whether the value is in the map</returns>
		public bool ContainsValue(TValue value)
		{
			return ContainsValue_(value);
		}
		/// <summary>
		/// Removes the given key from the map
		/// </summary>
		/// <param name="key">The key to remove from the map</param>
		/// <returns>Whether the key was removed from the map</returns>
		public bool Remove(TKey key)
		{
			return RemoveKey_(key);
		}
		/// <summary>
		/// Tries to get the value corresponding to the given key
		/// </summary>
		/// <param name="key">The key to search with</param>
		/// <param name="value">The value that matches the key</param>
		/// <returns>Whether the key was found in the map</returns>
		public bool TryGetValue(TKey key, out TValue value)
		{
			return TryGetValue_(key, out value);
		}
	}
}