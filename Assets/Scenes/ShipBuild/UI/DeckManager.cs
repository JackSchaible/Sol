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

        private readonly Color _activeColor = new Color(1, 1, 1, 0.8f);
        private readonly Color _inactiveColor = new Color(1, 1, 1, 0.5f);

        private readonly Color _selectedDeckModuleColor = new Color(1, 1, 1, 1);
        private readonly Color _adjacentDeckModuleColor = new Color(1, 1, 1, 0.5f);

        void Awake()
        {
            var gameObjects = GameObject.FindGameObjectsWithTag("Deck Button");
            _deckButtons = new List<GameObject>();

            foreach (var go in gameObjects.OrderByDescending(x => int.Parse(x.GetComponentInChildren<Text>().text)))
                _deckButtons.Add(go);

            CurrentDeck = 0;

            for (var i = 0; i < _deckButtons.Count; i++)
                _deckButtons[i].GetComponent<Image>().color = i == CurrentDeck ? _activeColor : _inactiveColor;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z) && CurrentDeck - 1 >= 0)
                SelectDeck(CurrentDeck - 1);

            if (Input.GetKeyDown(KeyCode.A) && CurrentDeck + 1 < _deckButtons.Count)
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

            for (int i = 1; i <= _deckButtons.Count; i++)
                InitializeDeckButton(_deckButtons[i - 1], i);

            MoveCells();
        }

        public void SelectDeck(int deck)
        {
            if (deck < 0 || deck > _deckButtons.Count - 1)
                throw new Exception("Deck " + deck + " does not exist.");

            for (var i = 0; i < _deckButtons.Count; i++)
                _deckButtons[i].GetComponent<Image>().color = i == deck ? _activeColor : _inactiveColor;

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

                        var go = BuildManager.Grid[x, y, z].GameObject;

                        if (go != null)
                            if (z == deck || z == deck - 1 || z == deck + 1)
                            {
                                Color color;
                                go.SetActive(true);

                                if (z == deck)
                                    color = _selectedDeckModuleColor;
                                else
                                    color = _adjacentDeckModuleColor;

                                go.GetComponent<SpriteRenderer>().color = color;
                            }
                            else
                                go.SetActive(false);

                        if (z != deck)
                            BuildManager.Cells[x, y, z].SetActive(false);
                        else
                            BuildManager.Cells[x, y, z].SetActive(true);
                    }
        }

        private void OnDeckSelected(GameObject button)
        {
            SelectDeck(button.name.ParseUntil() - 1);
        }
        private void InitializeDeckButton(GameObject deckButton, int deckNumber)
        {
            deckButton.GetComponentInChildren<Text>().text = deckNumber.ToString();
            deckButton.name = deckNumber.ToString();
            deckButton.GetComponent<Button>().onClick.AddListener(() => { OnDeckSelected(deckButton); });
        }
        private void MoveCells()
        {
            var cells = BuildManager.Cells;

            for(int x = 0; x < cells.GetLength(0); x++)
                for(int y = 0; y < cells.GetLength(1); y++)
                    for (int z = 0; z < cells.GetLength(2); z++)
                    {
                        var pos = cells[x, y, z].transform.position;

                        BuildManager.Cells[x, y, z].transform.position = 
                            new Vector3(pos.x, pos.y, -z);

                        var go = BuildManager.Grid[x, y, z].GameObject;

                        if (go == null) continue;

                        go.transform.position = new Vector3(pos.x, pos.y, -z);
                    }
        }
    }
}
