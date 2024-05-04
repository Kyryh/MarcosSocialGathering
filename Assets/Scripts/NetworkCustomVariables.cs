using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;

/// <summary>
/// Represents a network variable that stores a series of boolean values
/// </summary>
public class NetworkBools : NetworkVariable<byte> {

    /// <summary>
    /// Gets the length of the sequence of bools
    /// </summary>
    public int Length => 8;

    /// <summary>
    /// Gets or sets the boolean value at the specified index
    /// </summary>
    /// <param name="index">The index of the boolean value to get or set</param>
    /// <returns>The boolean value at the specified index</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the specified index is outside the valid range</exception>
    public bool this[int index] {
        get {
            if (index < 0 || index >= Length)
                throw new IndexOutOfRangeException(index.ToString());
            return (Value >> index & 0b1) == 1;
        }
        set {
            if (index < 0 || index >= Length)
                throw new IndexOutOfRangeException(index.ToString());
            if (value)
                Value |= (byte)(1 << index);
            else 
                Value &= (byte)~(1 << index);
        }
    }
}

/// <summary>
/// Represents a network variable that stores a series of signed byte values
/// </summary>
internal class NetworkNums : NetworkVariable<long> {

    /// <summary>
    /// Gets the length of the sequence of signed bytes
    /// </summary>
    public int Length => 8;

    /// <summary>
    /// Gets or sets the signed byte value at the specified index
    /// </summary>
    /// <param name="index">The index of the signed byte value to get or set</param>
    /// <returns>The signed byte value at the specified index</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the specified index is outside the valid range</exception>
    public sbyte this[int index] {
        get {
            if (index < 0 || index >= Length)
                throw new IndexOutOfRangeException(index.ToString());
            return (sbyte)(Value >> (index * 8));
        }
        set {
            if (index < 0 || index >= Length)
                throw new IndexOutOfRangeException(index.ToString());
            Value &= ~(0xFFL << (index * 8));
            Value |= (long)value << (index * 8);
        }
    }
}