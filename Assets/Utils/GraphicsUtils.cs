using System.IO;
using UnityEngine;

namespace Assets.Utils
{
    public static class GraphicsUtils
    {
        public static Sprite GetSpriteFromPath(string path)
        {
            path = Application.persistentDataPath + "/" + path;
            var bytes = File.ReadAllBytes(path);
            var texture = new Texture2D(1, 1) { filterMode = FilterMode.Point };
            texture.LoadImage(bytes);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.0f), 1.0f);
        }
    }
}
