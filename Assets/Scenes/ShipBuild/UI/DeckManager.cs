using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.Experimental.UIElements.Button;

namespace Assets.Scenes.ShipBuild
{
    public class DeckManager : MonoBehaviour
    {
        public Button NewUpperDeck;
        public Button NewLowerDeck;

        public int CurrentDeck { get; private set; }

        private Dictionary<int, GameObject> _deckButtons;


        void Start()
        {
            var gameObjects = GameObject.FindGameObjectsWithTag("Deck Buttons");

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
            var deck = _deckButtons.FirstOrDefault(x => x.GetComponentInChildren<Text>().text == deckNumber.ToString());
            if (deck == null) return;

            deck.GetComponent<Button>().SetEnabled(false);

        }

        public void AddUpperDeck()
        {
            
        }

        public void AddLowerDeck()
        {
            
        }
    }
}
