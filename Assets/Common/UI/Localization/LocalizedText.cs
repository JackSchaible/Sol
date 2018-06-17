using Assets.Common.Ui.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Common.Ui.Localization
{
    public class LocalizedText : MonoBehaviour
    {
        public string Key;

        private void Start()
        {
            GetComponent<Text>().text = LocalizationManager.Instance.GetLocalizedValue(Key);
        }
    }
}