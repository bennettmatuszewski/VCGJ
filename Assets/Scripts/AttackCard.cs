using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AttackCard : Card
{
    public int attackDamage;
    
    
    public virtual void Attack()
    {
        StartCoroutine(AttackCo());
    }

    IEnumerator AttackCo()
    {
        for (int i = 0; i < gameManager.currentEnemy.Count; i++)
        {
            RectTransform rect = gameManager.currentEnemy[i].GetComponent<RectTransform>();
            rectTransform.DOAnchorPos(new Vector2(rect.anchoredPosition.x+285, rect.anchoredPosition.y), 0.5f).SetEase(Ease.InOutBack);
            rectTransform.DORotate(new Vector3(0, 0, 15), 0.5f).SetEase(Ease.InOutQuad);
            yield return new WaitForSeconds(0.5f);
            rectTransform.DOShakePosition(0.25f, Vector3.one * 15, 20, 90);
        
            gameManager.currentEnemy[i].TakeDamage(attackDamage);
        
            yield return new WaitForSeconds(0.5f);
            rectTransform.DOAnchorPos(gameManager.attackSlotPos.anchoredPosition, .75f).SetEase(Ease.InOutQuad);
            rectTransform.DORotate(new Vector3(0, 0, 0), .75f).SetEase(Ease.InOutQuad);   
        }
    }
    public override void PlayCard()
    {
        if (player.activeAttackCard ==null && !hasBeenPlayed)
        {
            rectTransform.SetParent(gameManager.attackSlot);
            rectTransform.DOAnchorPos(gameManager.attackSlotPos.anchoredPosition, 0.35f).SetEase(Ease.OutQuad);
            player.activeAttackCard = this;
            hasBeenPlayed = true;
            player.cardsInHand.Remove(gameObject);
        }
        else if(hasBeenPlayed)
        {
            gameManager.discardAttackButton.SetActive(true);
            gameManager.discardAttackButtonRect.DOScale(Vector3.one * 1.25f, 0.25f).SetEase(Ease.InOutBack);
        }
    }
    
}
