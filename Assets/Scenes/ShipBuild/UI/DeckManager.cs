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

        private Dictionary<int, GameObject> _deckButtons;

        private Color activeColor = new Color(1, 1, 1, 0.8f);
        private Color inactiveColor = new Color(1, 1, 1, 0.5f);

        private Color SelectedDeckModuleColor = new Color(1, 1, 1, 1);
        private Color AdjacentDeckModuleColor = new Color(1, 1, 1, 0.5f);
        private Color UnattachedDeckModuleColor = new Color(1, 1, 1, 0);

        void Awake()
        {
            var gameObjects = GameObject.FindGameObjectsWithTag("Deck Button");

            _deckButtons = new Dictionary<int, GameObject>();

            foreach (var go in gameObjects)
                _deckButtons.Add(int.Parse(go.GetComponentInChildren<Text>().text), go);

            CurrentDeck = 0;

            foreach (var deckKvp in _deckButtons)
                deckKvp.Value.GetComponent<Image>().color = deckKvp.Key == CurrentDeck ? activeColor : inactiveColor;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (_deckButtons.ContainsKey(CurrentDeck - 1))
                    SelectDeck(CurrentDeck - 1);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (_deckButtons.ContainsKey(CurrentDeck + 1))
                    SelectDeck(CurrentDeck + 1);
            }
        }

        public void AddUpperDeck()
        {
            var topDeck = _deckButtons.OrderByDescending(x => x.Key).First();
            var go = Instantiate(DeckPrefab, Content.transform);

            go.GetComponent<Button>().onClick.AddListener(() => { OnDeckSelected(go); });
            var key = topDeck.Key + 1;

            go.GetComponentInChildren<Text>().text = key.ToString();
            go.name = "Deck " + key + " Button";
            go.transform.SetAsFirstSibling();

            _deckButtons.Add(topDeck.Key + 1, go);
        }

        public void AddLowerDeck()
        {
            var bottomDeck = _deckButtons.OrderBy(x => x.Key).Last();
            var go = Instantiate(DeckPrefab, Content.transform);

            go.GetComponent<Button>().onClick.AddListener(() => { OnDeckSelected(go); });
            var key = bottomDeck.Key + 1;

            go.GetComponentInChildren<Text>().text = key.ToString();
            go.name = "Deck" + key + "Button";

            _deckButtons.Add(bottomDeck.Key + 1, go);
        }

        public void SelectDeck(int deck)
        {
            if (!_deckButtons.ContainsKey(deck))
                throw new Exception("Deck " + deck + " does not exist.");

            foreach (var deckKvp in _deckButtons)
                deckKvp.Value.GetComponent<Image>().color = deckKvp.Key == deck ? activeColor : inactiveColor;

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

        void OnDeckSelected(GameObject button)
        {
            SelectDeck(button.name.ParseUntil());
        }
    }
}
