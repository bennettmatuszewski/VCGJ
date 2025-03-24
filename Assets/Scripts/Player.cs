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
    public CardPile attackCardPile;
    public CardPile defenseCardPile;

    public bool drawExtraDefense;
    public bool drawExtraAttack;
    private int cardsAdded=1;
    public bool extraAttack;
    public bool extraDefense;
    // Start is called before the first frame update
    
    public IEnumerator StartRound()
    {
 
        //shuffle cards in deck and assign to cards inDeckForRound
        attackCardsInDeckForRound = ShuffleList(attackCardsInDeck);
        defenseCardsInDeckForRound = ShuffleList(defenseCardsInDeck);

        List<GameObject> tempCards = new List<GameObject>();
        for (int i = 0; i < cardsToDrawStartOfRound; i++)
        {
            GameObject cardInstance;
            if (i<cardsToDrawStartOfRound/2)
            {
                cardInstance = Instantiate(defenseCardsInDeckForRound[i], deckParent.anchoredPosition3D, Quaternion.identity, deckParent);
                if (extraDefense && Random.Range(0,2)==0)
                {
                    cardInstance.transform.GetChild(0).GetComponent<DefensiveCard>().health += 1;
                    cardInstance.transform.GetChild(0).GetComponent<DefensiveCard>().attackDefenseText.text =
                        cardInstance.transform.GetChild(0).GetComponent<DefensiveCard>().health.ToString();
                    Debug.Log("extra defense");
                }
                //defenseCardsInDeckForRound.RemoveAt(0);
            }
            else
            {
                cardInstance = Instantiate(attackCardsInDeckForRound[i-(cardsToDrawStartOfRound/2)], deckParent.anchoredPosition3D, Quaternion.identity, deckParent);
                //attackCardsInDeckForRound.RemoveAt(0);
                if (extraAttack && Random.Range(0,2)==0)
                {
                    cardInstance.transform.GetChild(0).GetComponent<AttackCard>().attackDamage += 1;
                    cardInstance.transform.GetChild(0).GetComponent<AttackCard>().attackDefenseText.text =
                        cardInstance.transform.GetChild(0).GetComponent<AttackCard>().attackDamage.ToString();
                    Debug.Log("extra attack");
                }
            }
            AudioManager.instance.Play("drawCard");
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

        if (drawExtraAttack)
        {
            DrawAttack();
        }

        if (drawExtraDefense)
        {
           DrawDefense();
        }
        gameManager.canPlayCard = true;
        gameManager.canDrawCard = true;
    }

    public void DrawDefense()
    {
        if (defenseCardsInDeckForRound.Count>0)
        {
            AddCardToHand(defenseCardsInDeckForRound[0]);
            defenseCardsInDeckForRound.RemoveAt(0);
            if (defenseCardsInDeckForRound.Count==0)
            {
                defenseCardPile.UpdateSprite();
            }
        }
    }

    public void DrawAttack()
    {
        if (attackCardsInDeckForRound.Count>0)
        {
            AddCardToHand(attackCardsInDeckForRound[0]);
            attackCardsInDeckForRound.RemoveAt(0);
            if (attackCardsInDeckForRound.Count==0)
            {
                attackCardPile.UpdateSprite();
            }
        }
    }
    public void AddCardToHand(GameObject card)
    {
        AudioManager.instance.Play("drawCard");
        GameObject cardInstance = Instantiate(card, deckParent.anchoredPosition3D, Quaternion.identity, deckParent);
        RectTransform cardRect = cardInstance.GetComponent<RectTransform>();
        cardRect.anchoredPosition3D =
            new Vector3(deckParent.anchoredPosition3D.x, deckParent.anchoredPosition3D.y, 0);
        cardRect.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack);
        cardInstance.GetComponent<Canvas>().sortingOrder = cardsAdded;
        cardsAdded++;
        cardsInHand.Add(cardInstance);
        if (extraAttack && Random.Range(0,2)==0 && cardInstance.GetComponent<AttackCard>())
        {
            cardInstance.transform.GetChild(0).GetComponent<AttackCard>().attackDamage += 1;
            cardInstance.transform.GetChild(0).GetComponent<AttackCard>().attackDefenseText.text =
                cardInstance.transform.GetChild(0).GetComponent<AttackCard>().attackDamage.ToString();
        }
        if (extraDefense && Random.Range(0,2)==0 &&cardInstance.GetComponent<DefensiveCard>())
        {
            cardInstance.transform.GetChild(0).GetComponent<DefensiveCard>().health += 1;
            cardInstance.transform.GetChild(0).GetComponent<DefensiveCard>().attackDefenseText.text =
                cardInstance.transform.GetChild(0).GetComponent<DefensiveCard>().health.ToString();
        }
    }

    public void TakeDamage(int toTake)
    {
        AudioManager.instance.Play("takeDamage");
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
            deathManager.roundText.text = "round " + (gameManager.round+1) + "/15";
            deathManager.anim.SetTrigger("Start");
        }
    }

    public void ResetAll()
    {
        attackCardPile.DefaultSprite();
        defenseCardPile.DefaultSprite();
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
    
    public List<GameObject> ShuffleList(List<GameObject> list)
    {
        List<GameObject> temp = new List<GameObject>();
        List<GameObject> temp2 = new List<GameObject>();
        temp.AddRange(list);

        for (int i = 0; i < list.Count; i++)
        {
            int index = Random.Range(0, temp.Count);
            temp2.Add(temp[index]);
            temp.RemoveAt(index);
        }

        return temp2;
    }

    public void Cherry()
    {
        maxHealth += 5;
        playerHealth += 5;
        healthBar.value = playerHealth;
        healthBarText.text = playerHealth + "/" + maxHealth;
    }
    
}
