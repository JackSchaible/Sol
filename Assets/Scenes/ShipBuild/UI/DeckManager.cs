using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.ShipBuild
{
    public class DeckManager : MonoBehaviour
    {
        public Button NewUpperDeck;
        public Button NewLowerDeck;

        public GameObject Content;

        public GameObject DeckPrefab;

        public int CurrentDeck { get; private set; }

        private Dictionary<int, GameObject> _deckButtons;


        void Start()
        {
            var gameObjects = GameObject.FindGameObjectsWithTag("Deck Button");

            _deckButtons = new Dictionary<int, GameObject>();

            foreach (var go in gameObjects)
                _deckButtons.Add(int.Parse(go.GetComponentInChildren<Text>().text), go);

            CurrentDeck = 1;
        }

        void Update()
        {
            
        }

        public void DisableDeck(int deckNumber)
        {
            var deck = _deckButtons[deckNumber];
            if (deck == null) return;

            deck.GetComponent<Button>().interactable = false;

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

        public void AddUpperDeck()
        {
            var topDeck = _deckButtons.OrderByDescending(x => x.Key).First();
            var go = Instantiate(DeckPrefab, Content.transform);
            var key = topDeck.Key + 1;

            go.GetComponentInChildren<Text>().text = key.ToString();
            go.name = "Deck " + key + " Button";
            go.transform.SetAsFirstSibling();

            _deckButtons.Add(topDeck.Key + 1, go);
        }

        public void AddLowerDeck()
        {
            var bottomDeck = _deckButtons.OrderBy(x => x.Key).First();
            var go = Instantiate(DeckPrefab, Content.transform);
            var key = bottomDeck.Key - 1;

            go.GetComponentInChildren<Text>().text = key.ToString();
            go.name = "Deck " + key + " Button";

            _deckButtons.Add(bottomDeck.Key - 1, go);
        }

        public enum NewDeckButtons
        {
            Upper,
            Lower
        }
    }
}
