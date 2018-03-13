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
            var height = go.GetComponent<RectTransform>().rect.height;
            var key = topDeck.Key + 1;
            go.GetComponentInChildren<Text>().text = key.ToString();
            go.name = "Deck " + key + " Button";
            go.transform.position =
                new Vector3(go.transform.position.x, topDeck.Value.transform.position.y + height, go.transform.position.z);
            _deckButtons.Add(topDeck.Key + 1, go);
            NewUpperDeck.transform.position = new Vector3(NewUpperDeck.transform.position.x,
                NewUpperDeck.transform.position.y + height, NewUpperDeck.transform.position.z);
        }

        public void AddLowerDeck()
        {
            var bottomDeck = _deckButtons.OrderBy(x => x.Key).First();
            var go = Instantiate(DeckPrefab, Content.transform);
            var height = go.GetComponent<RectTransform>().rect.height;
            var key = bottomDeck.Key - 1;
            go.GetComponentInChildren<Text>().text = key.ToString();
            go.name = "Deck " + key + " Button";
            go.transform.position =
                new Vector3(go.transform.position.x, bottomDeck.Value.transform.position.y - height, go.transform.position.z);
            _deckButtons.Add(bottomDeck.Key - 1, go);
            NewLowerDeck.transform.position = new Vector3(NewLowerDeck.transform.position.x,
                NewLowerDeck.transform.position.y - height, NewLowerDeck.transform.position.z);
        }

        public enum NewDeckButtons
        {
            Upper,
            Lower
        }
    }
}
