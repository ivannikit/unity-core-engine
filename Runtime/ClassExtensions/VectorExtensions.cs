using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolbox
{
    public static class VectorExtensions
    {
        public static Vector3 Translate(this Vector3 vector, float dx, float dy, float dz)
        {
            vector.x += dx;
            vector.y += dy;
            vector.z += dz;
            return vector;
        }

        public static Vector3 Translate(this Vector3 vector, Vector3 delta)
        {
            vector.x += delta.x;
            vector.y += delta.y;
            vector.z += delta.z;
            return vector;
        }

        public static Vector3 Translate(this Vector3 vector, Vector2 delta)
        {
            vector.x += delta.x;
            vector.y += delta.y;
            return vector;
        }

        public static Vector2 Translate(this Vector2 vector, float dx, float dy)
        {
            vector.x += dx;
            vector.y += dy;
            return vector;
        }

        public static Vector2 Translate(this Vector2 vector, Vector2 delta)
        {
            vector.x += delta.x;
            vector.y += delta.y;
            return vector;
        }

        public static Vector3 SetX(this Vector3 vector, float x)
        {
            vector.x = x;
            return vector;
        }

        public static Vector3 SetY(this Vector3 vector, float y)
        {
            vector.y = y;
            return vector;
        }

        public static Vector3 SetXY(this Vector3 vector, float x, float y)
        {
            vector.x = x;
            vector.y = y;
            return vector;
        }

        public static Vector3 SetXY(this Vector3 vector, Vector2 xy)
        {
            vector.x = xy.x;
            vector.y = xy.y;
            return vector;
        }

        public static Vector3 SetZ(this Vector3 vector, float z)
        {
            vector.z = z;
            return vector;
        }

        public static Vector2 SetX(this Vector2 vector, float x)
        {
            vector.x = x;
            return vector;
        }

        public static Vector2 SetY(this Vector2 vector, float y)
        {
            vector.y = y;
            return vector;
        }

        public static Vector3 Translate(this Vector2 vector, float dx, float dy, float dz)
        {
            return new Vector3(vector.x + dx, vector.y + dy, dz);
        }

        public static bool CompareTo(this Vector3 a, Vector3 b, float EPSILON = .00001f)
        {
            return Math.Abs(a.x - b.x) < EPSILON && Math.Abs(a.y - b.y) < EPSILON && Math.Abs(a.z - b.z) < EPSILON;
        }

        public static bool CompareTo(this Vector2 a, Vector2 b, float EPSILON = .00001f)
        {
            return Math.Abs(a.x - b.x) < EPSILON && Math.Abs(a.y - b.y) < EPSILON;
        }
    }
}
