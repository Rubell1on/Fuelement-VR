using System;
using System.Collections.Generic;


namespace UnityEngine.Serialization
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector] int[] _Buckets;
        [SerializeField, HideInInspector] int[] _HashCodes;
        [SerializeField, HideInInspector] int[] _Next;
        [SerializeField, HideInInspector] int _Count = 0;
        [SerializeField, HideInInspector] int _Version;
        [SerializeField, HideInInspector] int _FreeList;
        [SerializeField, HideInInspector] int _FreeCount;
        [SerializeField, HideInInspector] TKey[] _keys;
        [SerializeField, HideInInspector] TValue[] _values;

        readonly IEqualityComparer<TKey> _Comparer;

        // Mainly for debugging purposes - to get the key-value pairs display
        public Dictionary<TKey, TValue> AsDictionary
        {
            get { return new Dictionary<TKey, TValue>(this); }
        }

        new public int Count
        {
            get { return _Count - _FreeCount; }
        }

        public TValue this[TKey key, TValue defaultValue]
        {
            get
            {
                int index = FindIndex(key);
                if (index >= 0)
                    return _values[index];
                return defaultValue;
            }
        }

        new public TValue this[TKey key]
        {
            get
            {
                int index = FindIndex(key);
                if (index >= 0)
                    return _values[index];
                throw new KeyNotFoundException(key.ToString());
            }

            set { Insert(key, value, false); }
        }

        public SerializableDictionary()
            : this(0, null)
        {
        }

        public SerializableDictionary(int capacity)
            : this(capacity, null)
        {
        }

        public SerializableDictionary(IEqualityComparer<TKey> comparer)
            : this(0, comparer)
        {
        }

        public SerializableDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity");

            Initialize(capacity);

            _Comparer = (comparer ?? EqualityComparer<TKey>.Default);
        }

        public SerializableDictionary(IDictionary<TKey, TValue> dictionary)
            : this(dictionary, null)
        {
        }

        public SerializableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            : this((dictionary != null) ? dictionary.Count : 0, comparer)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            foreach (KeyValuePair<TKey, TValue> current in dictionary)
                Add(current.Key, current.Value);
        }

        new public bool ContainsValue(TValue value)
        {
            if (value == null)
            {
                for (int i = 0; i < _Count; i++)
                {
                    if (_HashCodes[i] >= 0 && _values[i] == null)
                        return true;
                }
            }
            else
            {
                var defaultComparer = EqualityComparer<TValue>.Default;
                for (int i = 0; i < _Count; i++)
                {
                    if (_HashCodes[i] >= 0 && defaultComparer.Equals(_values[i], value))
                        return true;
                }
            }
            return false;
        }

        new public bool ContainsKey(TKey key)
        {
            return FindIndex(key) >= 0;
        }

        new public void Clear()
        {
            if (_Count <= 0)
                return;

            for (int i = 0; i < _Buckets.Length; i++)
                _Buckets[i] = -1;

            Array.Clear(_keys, 0, _Count);
            Array.Clear(_values, 0, _Count);
            Array.Clear(_HashCodes, 0, _Count);
            Array.Clear(_Next, 0, _Count);

            _FreeList = -1;
            _Count = 0;
            _FreeCount = 0;
            _Version++;
        }

        new public void Add(TKey key, TValue value)
        {
            Insert(key, value, true);
        }

        private void Resize(int newSize, bool forceNewHashCodes)
        {
            int[] bucketsCopy = new int[newSize];
            for (int i = 0; i < bucketsCopy.Length; i++)
                bucketsCopy[i] = -1;

            var keysCopy = new TKey[newSize];
            var valuesCopy = new TValue[newSize];
            var hashCodesCopy = new int[newSize];
            var nextCopy = new int[newSize];

            //keysCopy = new List<TKey>(_keys);
            //valuesCopy = new List<TValue>(_values);
            Array.Copy(_keys, 0, keysCopy, 0, _Count);
            Array.Copy(_values, 0, valuesCopy, 0, _Count);
            Array.Copy(_HashCodes, 0, hashCodesCopy, 0, _Count);
            Array.Copy(_Next, 0, nextCopy, 0, _Count);

            if (forceNewHashCodes)
            {
                for (int i = 0; i < _Count; i++)
                {
                    if (hashCodesCopy[i] != -1)
                        hashCodesCopy[i] = (_Comparer.GetHashCode(keysCopy[i]) & 2147483647);
                }
            }

            for (int i = 0; i < _Count; i++)
            {
                int index = hashCodesCopy[i] % newSize;
                nextCopy[i] = bucketsCopy[index];
                bucketsCopy[index] = i;
            }

            _Buckets = bucketsCopy;
            _keys = keysCopy;
            _values = valuesCopy;
            _HashCodes = hashCodesCopy;
            _Next = nextCopy;
        }

        private void Resize()
        {
            Resize(PrimeHelper.ExpandPrime(_Count), false);
        }

        new public bool Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            int hash = _Comparer.GetHashCode(key) & 2147483647;
            int index = hash % _Buckets.Length;
            int num = -1;
            for (int i = _Buckets[index]; i >= 0; i = _Next[i])
            {
                if (_HashCodes[i] == hash && _Comparer.Equals(_keys[i], key))
                {
                    if (num < 0)
                        _Buckets[index] = _Next[i];
                    else
                        _Next[num] = _Next[i];

                    _HashCodes[i] = -1;
                    _Next[i] = _FreeList;
                    _keys[i] = default(TKey);
                    _values[i] = default(TValue);
                    _FreeList = i;
                    _FreeCount++;
                    _Version++;
                    return true;
                }
                num = i;
            }
            return false;
        }

        private void Insert(TKey key, TValue value, bool add)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (_Buckets == null)
                Initialize(0);

            int hash = _Comparer.GetHashCode(key) & 2147483647;
            int index = hash % _Buckets.Length;
            int num1 = 0;
            for (int i = _Buckets[index]; i >= 0; i = _Next[i])
            {
                if (_HashCodes[i] == hash && _Comparer.Equals(_keys[i], key))
                {
                    if (add)
                        throw new ArgumentException("Key already exists: " + key);

                    _values[i] = value;
                    _Version++;
                    return;
                }
                num1++;
            }
            int num2;
            if (_FreeCount > 0)
            {
                num2 = _FreeList;
                _FreeList = _Next[num2];
                _FreeCount--;
            }
            else
            {
                if (_Count == _keys.Length)
                {
                    Resize();
                    index = hash % _Buckets.Length;
                }
                num2 = _Count;
                _Count++;
            }
            _HashCodes[num2] = hash;
            _Next[num2] = _Buckets[index];
            _keys[num2] = key;
            _values[num2] = value;
            _Buckets[index] = num2;
            _Version++;

            //if (num3 > 100 && HashHelpers.IsWellKnownEqualityComparer(comparer))
            //{
            //    comparer = (IEqualityComparer<TKey>)HashHelpers.GetRandomizedEqualityComparer(comparer);
            //    Resize(entries.Length, true);
            //}
        }

        private void Initialize(int capacity)
        {
            int prime = PrimeHelper.GetPrime(capacity);

            _Buckets = new int[prime];
            for (int i = 0; i < _Buckets.Length; i++)
                _Buckets[i] = -1;

            _keys = new TKey[prime];
            _values = new TValue[prime];
            _HashCodes = new int[prime];
            _Next = new int[prime];

            _FreeList = -1;
        }

        private int FindIndex(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (_Buckets != null)
            {
                int hash = _Comparer.GetHashCode(key) & 2147483647;
                for (int i = _Buckets[hash % _Buckets.Length]; i >= 0; i = _Next[i])
                {
                    if (_HashCodes[i] == hash && _Comparer.Equals(_keys[i], key))
                        return i;
                }
            }
            return -1;
        }

        new public bool TryGetValue(TKey key, out TValue value)
        {
            int index = FindIndex(key);
            if (index >= 0)
            {
                value = _values[index];
                return true;
            }
            value = default(TValue);
            return false;
        }

        private static class PrimeHelper
        {
            public static readonly int[] Primes = new int[]
            {
            3,
            7,
            11,
            17,
            23,
            29,
            37,
            47,
            59,
            71,
            89,
            107,
            131,
            163,
            197,
            239,
            293,
            353,
            431,
            521,
            631,
            761,
            919,
            1103,
            1327,
            1597,
            1931,
            2333,
            2801,
            3371,
            4049,
            4861,
            5839,
            7013,
            8419,
            10103,
            12143,
            14591,
            17519,
            21023,
            25229,
            30293,
            36353,
            43627,
            52361,
            62851,
            75431,
            90523,
            108631,
            130363,
            156437,
            187751,
            225307,
            270371,
            324449,
            389357,
            467237,
            560689,
            672827,
            807403,
            968897,
            1162687,
            1395263,
            1674319,
            2009191,
            2411033,
            2893249,
            3471899,
            4166287,
            4999559,
            5999471,
            7199369
            };

            public static bool IsPrime(int candidate)
            {
                if ((candidate & 1) != 0)
                {
                    int num = (int)Math.Sqrt((double)candidate);
                    for (int i = 3; i <= num; i += 2)
                    {
                        if (candidate % i == 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                return candidate == 2;
            }

            public static int GetPrime(int min)
            {
                if (min < 0)
                    throw new ArgumentException("min < 0");

                for (int i = 0; i < PrimeHelper.Primes.Length; i++)
                {
                    int prime = PrimeHelper.Primes[i];
                    if (prime >= min)
                        return prime;
                }
                for (int i = min | 1; i < 2147483647; i += 2)
                {
                    if (PrimeHelper.IsPrime(i) && (i - 1) % 101 != 0)
                        return i;
                }
                return min;
            }

            public static int ExpandPrime(int oldSize)
            {
                int num = 2 * oldSize;
                if (num > 2146435069 && 2146435069 > oldSize)
                {
                    return 2146435069;
                }
                return PrimeHelper.GetPrime(num);
            }
        }

        new public ICollection<TKey> Keys
        {
            get { return _keys; }
        }

        new public ICollection<TValue> Values
        {
            get { return _values; }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            int index = FindIndex(item.Key);
            return index >= 0 &&
                EqualityComparer<TValue>.Default.Equals(_values[index], item.Value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            if (index < 0 || index > array.Length)
                throw new ArgumentOutOfRangeException(string.Format("index = {0} array.Length = {1}", index, array.Length));

            if (array.Length - index < Count)
                throw new ArgumentException(string.Format("The number of elements in the dictionary ({0}) is greater than the available space from index to the end of the destination array {1}.", Count, array.Length));

            for (int i = 0; i < _Count; i++)
            {
                if (_HashCodes[i] >= 0)
                    array[index++] = new KeyValuePair<TKey, TValue>(_keys[i], _values[i]);
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        //public Enumerator GetEnumerator()
        //{
        //    return new Enumerator(this);
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return GetEnumerator();
        //}

        //IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        //{
        //    return GetEnumerator();
        //}

        //public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        //{
        //    private readonly SerializableDictionary<TKey, TValue> _Dictionary;
        //    private int _Version;
        //    private int _Index;
        //    private KeyValuePair<TKey, TValue> _Current;

        //    public KeyValuePair<TKey, TValue> Current
        //    {
        //        get { return _Current; }
        //    }

        //    internal Enumerator(SerializableDictionary<TKey, TValue> dictionary)
        //    {
        //        _Dictionary = dictionary;
        //        _Version = dictionary._Version;
        //        _Current = default(KeyValuePair<TKey, TValue>);
        //        _Index = 0;
        //    }

        //    public bool MoveNext()
        //    {
        //        if (_Version != _Dictionary._Version)
        //            throw new InvalidOperationException(string.Format("Enumerator version {0} != Dictionary version {1}", _Version, _Dictionary._Version));

        //        while (_Index < _Dictionary._Count)
        //        {
        //            if (_Dictionary._HashCodes[_Index] >= 0)
        //            {
        //                _Current = new KeyValuePair<TKey, TValue>(_Dictionary._Keys[_Index], _Dictionary._Values[_Index]);
        //                _Index++;
        //                return true;
        //            }
        //            _Index++;
        //        }

        //        _Index = _Dictionary._Count + 1;
        //        _Current = default(KeyValuePair<TKey, TValue>);
        //        return false;
        //    }

        //    void IEnumerator.Reset()
        //    {
        //        if (_Version != _Dictionary._Version)
        //            throw new InvalidOperationException(string.Format("Enumerator version {0} != Dictionary version {1}", _Version, _Dictionary._Version));

        //        _Index = 0;
        //        _Current = default(KeyValuePair<TKey, TValue>);
        //    }

        //    object IEnumerator.Current
        //    {
        //        get { return Current; }
        //    }

        //    public void Dispose()
        //    {
        //    }
        //}

        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            Array.Clear(_keys, 0, _Count);
            Array.Clear(_values, 0, _Count);
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                //_keys.Add(pair.Key);
                //_values.Add(pair.Value);
                _keys[Keys.Count] = pair.Key;
                _values[Keys.Count] = pair.Value;
            }
        }

        // load dictionary from lists
        public void OnAfterDeserialize()
        {
            this.Clear();

            if (_keys.Length != _values.Length)
                throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

            for (int i = 0; i < _keys.Length; i++)
                this.Add(_keys[i], _values[i]);
        }
    }
}
