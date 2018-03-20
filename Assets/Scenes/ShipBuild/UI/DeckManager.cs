using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Utils.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.ShipBuild
{
    public class DeckManager : MonoBehaviour
    {
        public Button NewUpperDeck;
        public Button NewLowerDeck;

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

        void Start()
        {
            var gameObjects = GameObject.FindGameObjectsWithTag("Deck Button");

            _deckButtons = new Dictionary<int, GameObject>();

            foreach (var go in gameObjects)
                _deckButtons.Add(int.Parse(go.GetComponentInChildren<Text>().text), go);

            CurrentDeck = 1;

            foreach (var deckKvp in _deckButtons)
                deckKvp.Value.GetComponent<Image>().color = deckKvp.Key == CurrentDeck ? activeColor : inactiveColor;
        }

        void Update()
        {
            
        }

        public void DisableDeck(int deckNumber, bool autoAddNewDeck = false)
        {
            var deck = _deckButtons[deckNumber];
            if (deck == null) return;

            deck.GetComponent<Button>().interactable = false;
            if (autoAddNewDeck && !_deckButtons.ContainsKey(deckNumber - 1))
                AddLowerDeck();

            SelectDeck(deckNumber - 1);
        }

        public void DisableNewDeckButtons(NewDeckButtons button)
        {
            switch (button)
            {
                case NewDeckButtons.Upper:
                    NewUpperDeck.interactable = false;
                    break;
                case NewDeckButtons.Lower:
                    NewLowerDeck.interactable = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("button", button, null);
            }
        }

        public void EnableNewDeckButtons(NewDeckButtons button)
        {
            switch (button)
            {
                case NewDeckButtons.Upper:
                    NewUpperDeck.interactable = true;
                    break;
                case NewDeckButtons.Lower:
                    NewLowerDeck.interactable = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("button", button, null);
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

            SelectDeck(topDeck.Key + 1);
        }

        public void AddLowerDeck()
        {
            var bottomDeck = _deckButtons.OrderBy(x => x.Key).First();
            var go = Instantiate(DeckPrefab, Content.transform);

            go.GetComponent<Button>().onClick.AddListener(() => { OnDeckSelected(go); });
            var key = bottomDeck.Key - 1;

            go.GetComponentInChildren<Text>().text = key.ToString();
            go.name = key + " Deck Button";

            _deckButtons.Add(bottomDeck.Key - 1, go);

            SelectDeck(bottomDeck.Key - 1);
        }

        public void SelectDeck(int deck)
        {
            foreach (var deckKvp in _deckButtons)
                deckKvp.Value.GetComponent<Image>().color = deckKvp.Key == deck ? activeColor : inactiveColor;

            foreach (var module in BuildManager.Modules)
            {
                Color color;

                if (module.Position.Z == deck)
                    color = SelectedDeckModuleColor;
                else if (module.Position.Z == deck - 1 || module.Position.Z == deck + 1)
                    color = AdjacentDeckModuleColor;
                else
                    color = UnattachedDeckModuleColor;

                module.GameObject.GetComponent<SpriteRenderer>().color = color;
            }

            CurrentDeck = deck;
        }

        void OnDeckSelected(GameObject button)
        {
            SelectDeck(button.name.ParseUntil());
        }

        public enum NewDeckButtons
        {
            Upper,
            Lower
        }
    }
}
