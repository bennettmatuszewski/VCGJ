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

        List<Enemy> deadEnemies = new List<Enemy>();
        for (int i = 0; i < gameManager.currentEnemy.Count; i++)
        {
            RectTransform rect = gameManager.currentEnemy[i].GetComponent<RectTransform>();
            rectTransform.DOAnchorPos(new Vector2(rect.anchoredPosition.x+285, rect.anchoredPosition.y), 0.5f).SetEase(Ease.InOutBack);
            rectTransform.DORotate(new Vector3(0, 0, 15), 0.5f).SetEase(Ease.InOutQuad);
            yield return new WaitForSeconds(0.5f);
            rectTransform.DOShakePosition(0.25f, Vector3.one * 15, 20, 90);
        
            gameManager.currentEnemy[i].TakeDamage(attackDamage);
            if (gameManager.currentEnemy[i].health<=0)
            {
                deadEnemies.Add(gameManager.currentEnemy[i]);
            }
        
            yield return new WaitForSeconds(0.5f);
            rectTransform.DOAnchorPos(gameManager.attackSlotPos.anchoredPosition, .5f).SetEase(Ease.InOutQuad);
            rectTransform.DORotate(new Vector3(0, 0, 0), .5f).SetEase(Ease.InOutQuad);
        }
        
        for (int i = 0; i < deadEnemies.Count; i++)
        {
            gameManager.currentEnemy.Remove(deadEnemies[i]);  
        }
        if (gameManager.currentEnemy.Count==0)
        {
            gameManager.StartCoroutine("CompletedRound");   
        }
        else
        {
            gameManager.StartCoroutine("EnemiesAttack");
        }
    }
    public override void PlayCard()
    {
        if(hasBeenPlayed && !gameManager.discardAttackButton.activeInHierarchy)
        {
            gameManager.discardDefenseButton.SetActive(false);
            gameManager.discardDefenseButtonRect.DOScale(Vector3.zero, 0f);
            gameManager.discardAttackButton.SetActive(true);
            gameManager.discardAttackButtonRect.DOScale(Vector3.one * 1.25f, 0.25f).SetEase(Ease.InOutBack);
        }
        else if(hasBeenPlayed && gameManager.discardAttackButton.activeInHierarchy)
        {
            gameManager.discardAttackButtonRect.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InOutBack).OnComplete(()=>gameManager.discardAttackButton.SetActive(false));
        }
        
        if (!gameManager.canPlayCard || player.activeAttackCard!=null)
        {
            return;
        }
        gameManager.canPlayCard = false;
        gameManager.canDrawCard = false;
        if (player.activeAttackCard ==null && !hasBeenPlayed)
        {
            rectTransform.SetParent(gameManager.attackSlot);
            rectTransform.DOAnchorPos(gameManager.attackSlotPos.anchoredPosition, 0.35f).SetEase(Ease.OutQuad).OnComplete(()=> EndScreenButton());
            player.activeAttackCard = this;
            hasBeenPlayed = true;
            player.cardsInHand.Remove(gameObject);
        }
    }

    void EndScreenButton()
    {
        gameManager.endTurnButton.DOScale(Vector3.one * 1.25f, .25f).SetEase(Ease.OutBack)
            .OnComplete(() => gameManager.canEndTurn = true);
    }
}
