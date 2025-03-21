using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameManager gameManager;
    private RectTransform attackPos;
    [HideInInspector]public RectTransform spawnPosition;
    private RectTransform rectTransform;
    public TMP_Text healthText;
    public TMP_Text attackText;
    private Canvas canvas;
    public int health;
    public int attack;

    void Start()
    {
        canvas = GetComponent<Canvas>();
        gameManager = FindObjectOfType<GameManager>();
        rectTransform = GetComponent<RectTransform>();
        attackPos = gameManager.enemyAttackPos;
        
    }

    public IEnumerator Attack()
    {
        canvas.sortingOrder = 101;
        rectTransform.DOAnchorPos(attackPos.anchoredPosition, 0.5f).SetEase(Ease.InOutBack);
        rectTransform.DORotate(new Vector3(0, 0, 15), 0.5f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.5f);
        rectTransform.DOShakePosition(0.25f, Vector3.one * 15, 20, 90);

        if (gameManager.player.activeDefenseCard!=null)
        {
            gameManager.player.activeDefenseCard.TakeDamage(attack);
        }
        else
        {
            gameManager.player.TakeDamage(attack);
        }
        
        
        
        yield return new WaitForSeconds(0.5f);
        rectTransform.DOAnchorPos(spawnPosition.anchoredPosition, .75f).SetEase(Ease.InOutQuad);
        rectTransform.DORotate(new Vector3(0, 0, 0), .75f).SetEase(Ease.InOutQuad).OnComplete(()=> canvas.sortingOrder = 1);
        
    }

    public void TakeDamage(int damageToTake)
    {
        health -= damageToTake;
        if (health<0)
        {
            health = 0;
        }
        healthText.text = health.ToString();
        if (health<=0)
        {
            rectTransform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutQuad);
            gameManager.currentEnemy.Remove(this);
            //NEXT ROUND
            gameManager.CompletedRound();
        }
    }

}
