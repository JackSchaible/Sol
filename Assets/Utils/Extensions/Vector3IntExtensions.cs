using UnityEngine;

namespace Assets.Utils.Extensions
{
    public static class Vector3IntExtensions
    {
        /// <summary>
        /// Does a contain b?
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Contains(this Vector3Int a, Vector3Int b)
        {
            return (a.x >= b.x && b.x >= 0) &&
                   (a.y >= b.y && b.y >= 0) &&
                   (a.z >= b.z && b.z >= 0);
        }
    }
}
