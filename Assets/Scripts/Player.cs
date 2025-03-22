using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public int playerHealth;
    public int maxHealth;
    public int cardsToDrawStartOfRound;

    public RectTransform deckParent;
    public List<GameObject> defenseCardsInDeck = new List<GameObject>();
    public List<GameObject> attackCardsInDeck = new List<GameObject>();
    public List<GameObject> cardsInHand = new List<GameObject>();

    public List<GameObject> defenseCardsInDeckForRound = new List<GameObject>();
    public List<GameObject> attackCardsInDeckForRound = new List<GameObject>();
    
    public GameManager gameManager;
    public AttackCard activeAttackCard;
    public DefensiveCard activeDefenseCard;
    public Slider healthBar;
    public TMP_Text healthBarText;
    public DeathManager deathManager;
    
    private int cardsAdded=1;
    // Start is called before the first frame update
    
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
                cardInstance = Instantiate(defenseCardsInDeck[i], deckParent.anchoredPosition3D, Quaternion.identity, deckParent);
            }
            else
            {
                cardInstance = Instantiate(attackCardsInDeck[i-(cardsToDrawStartOfRound/2)], deckParent.anchoredPosition3D, Quaternion.identity, deckParent);
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
        gameManager.canPlayCard = true;
        gameManager.canDrawCard = true;
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

    public void TakeDamage(int toTake)
    {
        gameManager.gameContent.DOShakePosition(0.25f, Vector3.one * 15, 20, 90);
        playerHealth -= toTake;
        if (playerHealth<0)
        {
            playerHealth = 0;
        }
        healthBar.value = playerHealth;
        healthBarText.text = playerHealth + "/" + maxHealth;
        if (playerHealth==0)
        {
            deathManager.anim.SetTrigger("Start");
        }
    }

    public void ResetAll()
    {
        attackCardsInDeckForRound.Clear();
        defenseCardsInDeckForRound.Clear();
        cardsInHand.Clear();
        if (activeAttackCard!=null)
        {
            Destroy(activeAttackCard);
        }
        if (activeDefenseCard!=null)
        { Destroy(activeDefenseCard);
            
        }
        foreach (var card in FindObjectsByType<Card>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            if (card.transform.parent.name!="ShopCard")
            {
                Destroy(card.transform.parent.gameObject);   
            }
        }
        
    }
    
}
