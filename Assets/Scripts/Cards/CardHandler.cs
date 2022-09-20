using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{
    #region Singleton

    private static CardHandler _singleton;
    public static CardHandler Singleton
    {
        get { return _singleton; }
        set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                Debug.LogWarning($"{nameof(value)} already exists in the current scene. Deleting clone");
                Destroy(value.gameObject);
            }
        }
    }
    private void OnValidate()
    {
        Singleton = this;
    }
    #endregion
    [Tooltip("UI image prefab to display cards")]
    [SerializeField] private GameObject cardPrefab;
    [Tooltip("List of saved scriptable object cards")]
    [SerializeField] private Card[] cards;
    //active cards in the player's inventory
    private List<KeyValuePair<GameObject, Card>> activeCards = new List<KeyValuePair<GameObject, Card>>();
    [Tooltip("Maximum cards in the player's inventory")]
    [Range(0, 10)]
    [SerializeField] private int maxCards;
    private int selectedCard;//index of the current selected card
    [Tooltip("Panel parent to which under to instanciate cards")]
    [SerializeField] private Transform panelTransform;
    private void Start()
    {
        StartCoroutine(DrawCards());//start drawing the cards (temporary?)
    }
    private void Update()
    {
        UpdateSelectedCard();//updates which card is selected in the UI to the player
        TryPlayCard();//plays the card
    }
    /// <summary>
    /// Attempts to play the selected card if the player has pressed forward
    /// </summary>
    /// <returns>If the selected card has been played</returns>
    private bool TryPlayCard()//boolean unused as of now
    {
        if (activeCards.Count <= 0)//if the player has not cards
        {
            return false;//the player can't play any cards
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            activeCards[selectedCard].Value.Play();//run the Play method for the card
            Destroy(activeCards[selectedCard].Key);//destroy the card gameobject on the next frame
            activeCards.RemoveAt(selectedCard);//remove the card from the list
            return true;
        }
        return false;
    }
    /// <summary>
    /// Updates which card is displayed above the others
    /// </summary>
    private void UpdateSelectedCard()
    {
        if (activeCards.Count <= 0) return;//skip this if there's no cards
        //allow the player to change chich cards is selected, and make sure the selected card index is under the card count
        selectedCard = Mathf.Clamp(selectedCard + (Input.GetKeyDown(KeyCode.LeftArrow) ? -1 : Input.GetKeyDown(KeyCode.RightArrow) ? 1 : 0), 0, activeCards.Count - 1);
        //check each card, and if it's the selected card, display it above the others
        for (int i = 0; i < activeCards.Count; i++)//check each card
        {
            if (i == selectedCard)//if it's the selected card
            {
                activeCards[i].Key.transform.GetChild(0).localPosition = Vector3.up * 100;//show the card above the others
            }
            else//if it's not
            {
                activeCards[i].Key.transform.GetChild(0).localPosition = Vector3.zero;//set the position back to normal
            }
        }
    }
    /// <summary>
    /// Add a card to the player's hand
    /// </summary>
    /// <param name="card">Card to Add</param>
    /// <returns>If the card was able to be added to the hand</returns>
    private bool AddCardToHand(Card card)
    {
        if (activeCards.Count >= maxCards)//if the hand is full
        {
            return false;//return that you can't add any more cards
        }
        //make a new card in the scene in the correct format ready to add to the list of cards
        KeyValuePair<GameObject, Card> newCard = new KeyValuePair<GameObject, Card>(UnityEditor.PrefabUtility.InstantiatePrefab(cardPrefab, panelTransform) as GameObject, card);
        newCard.Key.transform.GetChild(0).GetComponent<RawImage>().texture = newCard.Value.cardTexture;//set the correct texture
        activeCards.Add(newCard);//add the card to the list of cards in the hand
        return true;//return that the card has been added
    }
    /// <returns>A random card from the saved card list</returns>
    private Card GetRandomCard(Card[] list)
    {
        if (list.Length == 0)//if there are no saved cards 
        {
            return null;
        }
        return list[Random.Range(0, list.Length)];//otherwise return a random card
    }
    /// <summary>
    /// Continuous method to keep giving the player cards
    /// </summary>
    private IEnumerator DrawCards()
    {
        yield return new WaitForSeconds(1);//wait 1 second (for development)
        AddCardToHand(GetRandomCard(cards));//add a random card
        StartCoroutine(DrawCards());//start this method again
    }
}
