using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

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
        rectTransform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutQuad);
        gameManager.player.activeDefenseCard = null;
        yield return new WaitForSeconds(0.25f);
    }
    public override void PlayCard()
    {
        if (player.activeDefenseCard ==null && !hasBeenPlayed)
        {
            rectTransform.SetParent(gameManager.defenseSlot);
            rectTransform.DOAnchorPos(gameManager.defenseSlotPos.anchoredPosition, 0.35f).SetEase(Ease.OutQuad);
            player.activeDefenseCard = this;    
            hasBeenPlayed = true;
        }
        else if(hasBeenPlayed)
        {
            gameManager.discardDefenseButton.SetActive(true);
            gameManager.discardDefenseButtonRect.DOScale(Vector3.one * 1.25f, 0.25f).SetEase(Ease.InOutBack);
        }
    }
}
