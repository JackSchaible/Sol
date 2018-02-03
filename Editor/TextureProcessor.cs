using UnityEngine;
using UnityEditor;

public class TextureProcessor : AssetPostprocessor {

	void OnPostprocessTexture(Texture2D texture) {
		TextureImporter importer = assetImporter as TextureImporter;
		importer.filterMode = FilterMode.Point;

		var asset = AssetDatabase.LoadAssetAtPath (importer.assetPath, typeof(Texture2D));

		if (asset)
			EditorUtility.SetDirty (asset);
		else
			texture.filterMode = FilterMode.Point;
	}
}
