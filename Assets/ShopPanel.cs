using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class ShopPanel : MonoBehaviour
{
    public GameManager gameManager;
    public Player player;
    private RectTransform recttransform;
    public Animator anim;

    public GameObject[] defenseCards;
    public GameObject[] attackCards;
    public GameObject[] upgrades;
    private GameObject cardRef;
    private bool cantChoose;

    private void Start()
    {
        recttransform = GetComponent<RectTransform>();
    }

    public void ChooseUpgrade()
    {
        
    }

    public void ChooseDefense()
    {
        if (cantChoose)
        {
            return;
        }

        cantChoose = true;
        GameObject chosen = defenseCards[Random.Range(0, attackCards.Length)];
        GameObject card = Instantiate(chosen, new Vector3(0, recttransform.anchoredPosition.y-300), Quaternion.identity, recttransform);
        card.GetComponent<RectTransform>().anchoredPosition3D =
            new Vector3(0, recttransform.anchoredPosition3D.y-300, 0);
        card.GetComponent<Canvas>().sortingOrder = 105;
        card.GetComponent<RectTransform>().DOScale(Vector3.one*2.5f, 0.35f).SetEase(Ease.OutBack);
        card.transform.GetChild(0).GetComponent<EventTrigger>().enabled = false;
        cardRef = card;

        player.defenseCardsInDeck.Add(chosen);
    }

    public void ChooseAttack()
    {
        if (cantChoose)
        {
            return;
        }

        cantChoose = true;
        GameObject chosen = attackCards[Random.Range(0, attackCards.Length)];
        GameObject card = Instantiate(chosen, new Vector3(0, recttransform.anchoredPosition.y-300), Quaternion.identity, recttransform);
        card.GetComponent<RectTransform>().anchoredPosition3D =
            new Vector3(0, recttransform.anchoredPosition3D.y-300, 0);
        card.GetComponent<Canvas>().sortingOrder = 105;
        card.GetComponent<RectTransform>().DOScale(Vector3.one*2.5f, 0.35f).SetEase(Ease.OutBack);
        card.transform.GetChild(0).GetComponent<EventTrigger>().enabled = false;
        cardRef = card;
        player.attackCardsInDeck.Add(chosen);
        
    }
    
    public void ExitShop(Animator buttonAnim)
    {
        buttonAnim.SetTrigger("Press");
        StartCoroutine(NextRound());
    }
    public void EnterShop()
    {
        anim.SetTrigger("Enter");
    }

    IEnumerator NextRound()
    {
        if (cardRef!=null)
        {
            cardRef.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.35f).SetEase(Ease.OutQuad);
            yield return new WaitForSeconds(0.35f);   
        }
        anim.SetTrigger("Exit");
        Destroy(cardRef);
        player.ResetAll();
        yield return new WaitForSeconds(1.5f);
        gameManager.round++;
        gameManager.StartCoroutine("StartRound");
    }
}
