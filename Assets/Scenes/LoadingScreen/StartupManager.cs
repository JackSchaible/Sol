using System.Collections;
using System.Collections.Generic;
using Assets.Common.Ui.Localization;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scenes.LoadingScreen
{
    public class StartupManager : MonoBehaviour
    {
        private const string MainMenu = "MainMenu";
        private const string ShipBuild = "ShipBuild";

        private IEnumerator Start()
        {
            Debug.Log("StartupManager.Start()");
            while (!LocalizationManager.Instance.IsReady)
                yield return null;

#if DEBUG
            var scene = ShipBuild;
#else
            scene = MainMenu;
#endif
            Debug.Log("Loading Next Scene: " + scene);
            SceneManager.LoadSceneAsync(scene);
        }
    }
}