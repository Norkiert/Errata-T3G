using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
public struct TrackMapPosition
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
{
    public int x;
    public int y;
    public int z;

    public TrackMapPosition((int, int, int) data)
    {
        x = data.Item1;
        y = data.Item2;
        z = data.Item3;
    }
    public void Deconstruct(out int _x, out int _y, out int _z)
    {
        _x = x;
        _y = y;
        _z = z;
    }
    public static implicit operator (int, int, int)(TrackMapPosition v) => (v.x, v.y, v.z);
    public static implicit operator TrackMapPosition((int, int, int) v) => new TrackMapPosition(v);
    public static TrackMapPosition operator +(TrackMapPosition lhs, TrackMapPosition rhs)
    {
        lhs.x += rhs.x;
        lhs.y += rhs.y;
        lhs.z += rhs.z;
        return lhs;
    }
    public static TrackMapPosition operator -(TrackMapPosition v)
    {
        v.x = -v.x;
        v.y = -v.y;
        v.z = -v.z;
        return v;
    }
    public static TrackMapPosition operator -(TrackMapPosition lhs, TrackMapPosition rhs) => lhs + (-rhs);
    public static TrackMapPosition operator *(TrackMapPosition lhs, TrackMapPosition rhs)
    {
        lhs.x *= rhs.x;
        lhs.y *= rhs.y;
        lhs.z *= rhs.z;
        return lhs;
    }
    public static TrackMapPosition operator *(TrackMapPosition lhs, int rhs)
    {
        lhs.x *= rhs;
        lhs.y *= rhs;
        lhs.z *= rhs;
        return lhs;
    }
    public static TrackMapPosition operator /(TrackMapPosition lhs, TrackMapPosition rhs)
    {
        lhs.x /= rhs.x;
        lhs.y /= rhs.y;
        lhs.z /= rhs.z;
        return lhs;
    }
    public static TrackMapPosition operator /(TrackMapPosition lhs, int rhs)
    {
        lhs.x /= rhs;
        lhs.y /= rhs;
        lhs.z /= rhs;
        return lhs;
    }
    public static bool operator ==(TrackMapPosition lhs, TrackMapPosition rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
    }
    public static bool operator !=(TrackMapPosition lhs, TrackMapPosition rhs) => !(lhs == rhs);
}
