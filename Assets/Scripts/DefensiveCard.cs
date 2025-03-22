using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefensiveCard : Card
{
    public int health;

    public virtual void TakeDamage(int damageToTake)
    {
        health -= damageToTake;
        if (health<0)
        {
            health = 0;
        }
        attackDefenseText.text = health.ToString();
        if (health<=0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        GetComponent<EventTrigger>().enabled = false;
        rectTransform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutQuad);
        gameManager.player.activeDefenseCard = null;
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }
    public override void PlayCard()
    {
        if(hasBeenPlayed&& !gameManager.discardDefenseButton.activeInHierarchy)
        {
            gameManager.discardAttackButton.SetActive(false);
            gameManager.discardAttackButtonRect.DOScale(Vector3.zero, 0f);
            gameManager.discardDefenseButton.SetActive(true);
            gameManager.discardDefenseButtonRect.DOScale(Vector3.one * 1.25f, 0.25f).SetEase(Ease.InOutBack);
        }
        else if(hasBeenPlayed && gameManager.discardDefenseButton.activeInHierarchy)
        {
            gameManager.discardDefenseButtonRect.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InOutBack).OnComplete(()=>gameManager.discardDefenseButton.SetActive(false));
        }
        
        if (!gameManager.canPlayCard || player.activeDefenseCard!=null)
        {
            return;
        }

        gameManager.canPlayCard = false;
        gameManager.canDrawCard = false;
        if (player.activeDefenseCard ==null && !hasBeenPlayed)
        {
            rectTransform.SetParent(gameManager.defenseSlot);
            rectTransform.DOAnchorPos(gameManager.defenseSlotPos.anchoredPosition, 0.35f).SetEase(Ease.OutQuad).OnComplete(()=> EndScreenButton());
            player.activeDefenseCard = this;    
            hasBeenPlayed = true;
        }
    }
    void EndScreenButton()
    {
        gameManager.endTurnButton.DOScale(Vector3.one * 1.25f, .25f).SetEase(Ease.OutBack)
            .OnComplete(() => gameManager.canEndTurn = true);
    }
}
