using UnityEngine;

namespace Assets.Utils
{
    public static class GraphicsUtils
    {
        public static Sprite GetSpriteFromPath(string path, bool center = false)
        {
            var texture = Resources.Load<Texture2D>(path);
            var pivot = center ? new Vector2(0.5f, 0.5f) : Vector2.zero;

            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot, 1f);
        }
    }
}
