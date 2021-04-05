using System;
using System.Collections;
using System.Linq;

namespace DepartmentalTraversal
{
    public class HashableArray
    {
        private readonly int[] _array;

        public int this[int index] => _array[index];

        public int Length => _array.Length;
        
        public HashableArray(int[] array)
        {
            _array = array;
            Array.Sort(_array);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return _array.SequenceEqual(((HashableArray)obj)._array);
        }

        public override int GetHashCode()
        {
            int num = 5381;
            int num2 = num;
            for (int i = 0; i < _array.Length; i += 2)
            {
                num = ((num << 5) + num ^ _array[i]);
                if (i + 1 == _array.Length)
                    break;
                num2 = ((num2 << 5) + num2 ^ _array[i + 1]);
            }
            return num + num2 * 1566083941;
        }
    }
}