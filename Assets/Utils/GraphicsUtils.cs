using UnityEngine;

namespace Assets.Utils
{
    public static class GraphicsUtils
    {
        public static Sprite GetSpriteFromPath(string path)
        {
            var texture = Resources.Load<Texture2D>(path);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.0f), 1f);
        }
    }
}
