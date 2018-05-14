using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Utils.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.ShipBuild
{
    public class DeckManager : MonoBehaviour
    {
        public GameObject Content;
        public ShipBuildManager BuildManager;
        public GameObject DeckPrefab;

        public int CurrentDeck { get; private set; }

        private List<GameObject> _deckButtons;

        private Color activeColor = new Color(1, 1, 1, 0.8f);
        private Color inactiveColor = new Color(1, 1, 1, 0.5f);

        private Color SelectedDeckModuleColor = new Color(1, 1, 1, 1);
        private Color AdjacentDeckModuleColor = new Color(1, 1, 1, 0.5f);
        private Color UnattachedDeckModuleColor = new Color(1, 1, 1, 0);

        void Awake()
        {
            var gameObjects = GameObject.FindGameObjectsWithTag("Deck Button");
            _deckButtons = new List<GameObject>();

            foreach (var go in gameObjects.OrderByDescending(x => int.Parse(x.GetComponentInChildren<Text>().text)))
                _deckButtons.Add(go);

            CurrentDeck = 0;

            for (var i = 0; i < _deckButtons.Count; i++)
                _deckButtons[i].GetComponent<Image>().color = i == CurrentDeck ? activeColor : inactiveColor;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z) && CurrentDeck - 1 >= 0)
                SelectDeck(CurrentDeck - 1);

            if (Input.GetKeyDown(KeyCode.Q) && CurrentDeck + 1 <= _deckButtons.Count)
                SelectDeck(CurrentDeck + 1);
        }

        public void AddUpperDeck()
        {
            var go = Instantiate(DeckPrefab, Content.transform);

            InitializeDeckButton(go, _deckButtons.Count);
            go.transform.SetAsFirstSibling();

            _deckButtons.Add(go);
        }

        public void AddLowerDeck()
        {
            var go = Instantiate(DeckPrefab, Content.transform);
            go.transform.SetAsLastSibling();
            _deckButtons.Insert(0, go);

            for (int i = 0; i < _deckButtons.Count; i++)
                InitializeDeckButton(_deckButtons[i], i);
        }

        public void SelectDeck(int deck)
        {
            if (deck < 0 || deck > _deckButtons.Count - 1)
                throw new Exception("Deck " + deck + " does not exist.");

            for (var i = 0; i < _deckButtons.Count; i++)
                _deckButtons[i].GetComponent<Image>().color = i == deck ? activeColor : inactiveColor;

            CurrentDeck = deck;

            if (BuildManager.Grid == null) return;

            var xSize = BuildManager.Cells.GetLength(0);
            var ySize = BuildManager.Cells.GetLength(1);
            var zSize = BuildManager.Cells.GetLength(2);

            for (var z = 0; z < zSize; z++)
                for (var x = 0; x < xSize; x++)
                    for (var y = 0; y < ySize; y++)
                    {
                        if (z > deck + 1 || z < deck - 1) continue;

                        Color color;

                        if (z == deck)
                            color = SelectedDeckModuleColor;
                        else if (z == deck - 1 || z == deck + 1)
                            color = AdjacentDeckModuleColor;
                        else
                            color = UnattachedDeckModuleColor;

                        var go = BuildManager.Grid[x, y, z].GameObject;
                        if (go != null)
                            go.GetComponent<SpriteRenderer>().color = color;

                        if (z != deck)
                            BuildManager.Cells[x, y, z].SetActive(false);
                        else
                            BuildManager.Cells[x, y, z].SetActive(true);
                    }
        }

        private void OnDeckSelected(GameObject button)
        {
            SelectDeck(button.name.ParseUntil());
        }

        private void InitializeDeckButton(GameObject deckButton, int deckNumber)
        {
            deckButton.GetComponentInChildren<Text>().text = deckNumber.ToString();
            deckButton.name = "Deck " + (deckNumber + 1) + " Button";
            deckButton.GetComponent<Button>().onClick.AddListener(() => { OnDeckSelected(deckButton); });
        }
    }
}
