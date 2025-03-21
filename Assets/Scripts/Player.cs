using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int cardsToDrawStartOfRound;


    public RectTransform deckParent;
    public List<GameObject> defenseCardsInDeck = new List<GameObject>();
    public List<GameObject> attackCardsInDeck = new List<GameObject>();
    public List<GameObject> cardsInHand = new List<GameObject>();

    public List<GameObject> defenseCardsInDeckForRound = new List<GameObject>();
    public List<GameObject> attackCardsInDeckForRound = new List<GameObject>();

    private int cardsAdded=1;
    // Start is called before the first frame update
    IEnumerator Start()
    {

        yield return new WaitForSeconds(1);
        StartCoroutine(StartRound());
    }

    public IEnumerator StartRound()
    {
        //shuffle cards in deck and assign to cards inDeckForRound
        attackCardsInDeckForRound = attackCardsInDeck.OrderBy( x => Random.value ).ToList();
        defenseCardsInDeckForRound = defenseCardsInDeck.OrderBy( x => Random.value ).ToList();
        List<GameObject> tempCards = new List<GameObject>();
        for (int i = 0; i < cardsToDrawStartOfRound; i++)
        {
            GameObject cardInstance;
            if (i<cardsToDrawStartOfRound/2)
            {
                cardInstance = Instantiate(attackCardsInDeckForRound[i], deckParent.anchoredPosition3D, Quaternion.identity, deckParent);
            }
            else
            {
                cardInstance = Instantiate(defenseCardsInDeck[i-(cardsToDrawStartOfRound/2)], deckParent.anchoredPosition3D, Quaternion.identity, deckParent);
            }
            RectTransform cardRect = cardInstance.GetComponent<RectTransform>();
            cardRect.anchoredPosition3D =
                new Vector3(deckParent.anchoredPosition3D.x, deckParent.anchoredPosition3D.y, 0);
            cardInstance.GetComponent<Canvas>().sortingOrder = cardsAdded;
            cardsAdded++;
            tempCards.Add(cardInstance);
            cardsInHand.Add(cardInstance);
        }

        for (int i = 0; i < cardsToDrawStartOfRound; i++)
        {
            RectTransform cardRect = tempCards[i].GetComponent<RectTransform>();
            cardRect.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack);
            yield return new WaitForSeconds(0.25f);
        }
        tempCards.Clear();
        
        attackCardsInDeckForRound.RemoveRange(0,cardsToDrawStartOfRound/2);
        defenseCardsInDeckForRound.RemoveRange(0,cardsToDrawStartOfRound/2);
    }

    public void AddCardToHand(GameObject card)
    {

        GameObject cardInstance = Instantiate(card, deckParent.anchoredPosition3D, Quaternion.identity, deckParent);
        RectTransform cardRect = cardInstance.GetComponent<RectTransform>();
        cardRect.anchoredPosition3D =
            new Vector3(deckParent.anchoredPosition3D.x, deckParent.anchoredPosition3D.y, 0);
        cardRect.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack);
        cardInstance.GetComponent<Canvas>().sortingOrder = cardsAdded;
        cardsAdded++;
        cardsInHand.Add(cardInstance);
    }
    
}
