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
    [SerializeField] GameObject cardPrefab;
    [SerializeField] private Card[] cards;
    [SerializeField] private List<GameObject> activeCards;
    [Range(0, 10)]
    [SerializeField] private int maxCards;
    [SerializeField] private int selectedCard;
    [SerializeField] private Transform panelTransform;
    private void Start()
    {
        StartCoroutine(DrawCards());
    }
    private void Update()
    {
        UpdateSelectedCard();
        TryPlayCard();
    }
    private void TryPlayCard()
    {
        if (activeCards.Count <= 0)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            
            Destroy(activeCards[selectedCard]);
            activeCards.Remove(activeCards[selectedCard]);
        }
    }
    private void UpdateSelectedCard()
    {
        if (activeCards.Count > 0)
        {
            selectedCard = Mathf.Clamp((selectedCard + (Input.GetKeyDown(KeyCode.LeftArrow) ? -1 : Input.GetKeyDown(KeyCode.RightArrow) ? 1 : 0)), 0, activeCards.Count - 1);
            for (int i = 0; i < activeCards.Count; i++)
            {
                if (i == selectedCard)
                {
                    activeCards[i].transform.GetChild(0).localPosition = Vector3.up * 100;
                }
                else
                {
                    activeCards[i].transform.GetChild(0).localPosition = Vector3.zero;
                }
            }

        }
    }
    private bool AddCardToHand()
    {
        if (activeCards.Count >= maxCards)
        {
            return false;
        }
        GameObject newCard = UnityEditor.PrefabUtility.InstantiatePrefab(cardPrefab, panelTransform) as GameObject;
        Card cardTemplate = GetRandomCard();
        newCard.transform.GetChild(0).GetComponent<RawImage>().texture = cardTemplate.cardTexture;
        activeCards.Add(newCard);
        return true;
    }
    private Card GetRandomCard()
    {
        if (cards.Length == 0)
        {
            return null;
        }
        return cards[Random.Range(0, cards.Length)];
    }
    private IEnumerator DrawCards()
    {
        yield return new WaitForSeconds(1);
        AddCardToHand();
        StartCoroutine(DrawCards());
    }
}
