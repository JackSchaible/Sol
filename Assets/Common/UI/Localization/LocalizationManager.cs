using System.Collections.Generic;
using System.IO;
using Common.UI.Localization;
using UnityEngine;

namespace Assets.Common.Ui.Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance;
        public bool IsReady { get; private set; }
        
        private Dictionary<string, string> LocalizedText { get; set; }
        private string MissingText = "Localized Text Not Found";

        private const string DefaultLocale = "en-us";

        public string GetLocalizedValue(string key)
        {
            string result = MissingText;

            if (LocalizedText.ContainsKey(key))
                result = LocalizedText[key];

            return result;
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            LoadLocalizeText(DefaultLocale);
        }

        public void ReloadText()
        {
            //TODO: Get user pref's current lang, and reload
        }

        private void LoadLocalizeText(string language)
        {
            LocalizedText = new Dictionary<string, string>();
            string filepath = Path.Combine(Application.streamingAssetsPath, "strings_" + language + ".json");

            if (File.Exists(filepath))
            {
                string json = File.ReadAllText(filepath);
                LocalizationData data = JsonUtility.FromJson<LocalizationData>(json);

                foreach (var item in data.Items)
                    LocalizedText.Add(item.Key, item.Value);

                Debug.Log(filepath + " loaded " + data.Items.Length + " items!");
            }
            else
                Debug.LogError("Localization file " + filepath + " does not exist");

            IsReady = true;
        }
    }
}