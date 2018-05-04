using UnityEngine;

namespace Assets.Utils.Extensions
{
    public static class Vector3IntExtensions
    {
        public static bool Contains(this Vector3Int a, Vector3Int b)
        {
            return a.x < b.x || a.y < b.y || a.z < b.z || a.x < 0 || a.y < 0 || a.z < 0;
        }
    }
}
