using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [Header("Instances")] 
    public Player player;
    public ShopPanel shopPanel;
    public RectTransform gameContent;
    public RectTransform attackSlot;
    public RectTransform defenseSlot;
    public RectTransform attackSlotPos;
    public RectTransform defenseSlotPos;
    public RectTransform enemyAttackPos;
    public RectTransform enemySpawnLocation;
    public RectTransform enemyParent;
    public RectTransform endTurnButton;
    public GameObject discardAttackButton;
    public GameObject discardDefenseButton;
    [HideInInspector]public RectTransform discardAttackButtonRect;
    [HideInInspector]public RectTransform discardDefenseButtonRect;
    public TMP_Text roundText;
    public Animator roundAnim;
    public GameObject[] possibleEnemies;
    public int[] numberOfEnemiesPerRound;
    public RectTransform[] enemySpawnLocations;

    public List<Enemy> currentEnemy;

    [Header("Game Logic")] 
    public int round;
    

    private int minEnemies;
    private int maxEnemies;

    [HideInInspector] public bool canHoverCard;
    public bool canEndTurn;
    public bool canDrawCard;
    public bool canPlayCard;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        discardAttackButtonRect = discardAttackButton.GetComponent<RectTransform>();
        discardDefenseButtonRect = discardDefenseButton.GetComponent<RectTransform>();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(StartRound());
        maxEnemies = 2;
    }

    public IEnumerator StartRound()
    {
        roundText.text = "round " +(round+1) + "/15";
        roundAnim.SetTrigger("Start");
        if (round%2==0 && maxEnemies<6)
        {
            maxEnemies++;
        }
        if (round>3)
        {
            minEnemies++;
        }
        yield return new WaitForSeconds(2.25f);
        player.StartCoroutine("StartRound");
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        if (numberOfEnemiesPerRound[round] == 1)
        {
            StartCoroutine(SpawnEnemy(1));

        }
        else  if (numberOfEnemiesPerRound[round] == 2)
        {
            StartCoroutine(SpawnEnemy(3));
            StartCoroutine(SpawnEnemy(4));
        }
        else
        {
            StartCoroutine(SpawnEnemy(0));
            StartCoroutine(SpawnEnemy(1));
            StartCoroutine(SpawnEnemy(2));
        }
    }

    IEnumerator SpawnEnemy(int posIndex)
    {
        
        GameObject enemy = Instantiate(possibleEnemies[Random.Range(minEnemies, maxEnemies)], enemySpawnLocation.anchoredPosition3D, Quaternion.identity, enemyParent);
        enemy.GetComponent<Enemy>().spawnPosition = enemySpawnLocations[posIndex];
        //enemy.GetComponent<RectTransform>().parent = enemyParent;
        enemy.GetComponent<RectTransform>().anchoredPosition3D =
            new Vector3(enemySpawnLocation.anchoredPosition3D.x, enemySpawnLocation.anchoredPosition3D.y, 0);
        currentEnemy.Add(enemy.GetComponent<Enemy>());
        yield return new WaitForSeconds(0.1f);
        enemy.GetComponent<RectTransform>().DOAnchorPos(enemySpawnLocations[posIndex].anchoredPosition, 0.5f).SetEase(Ease.OutQuad);
    }
    public void EndTurn(Animator animator)
    {
        if (canEndTurn)
        {
            canEndTurn = false;
            StartCoroutine(EndTurnCo(animator));
        }
    }

    private IEnumerator EndTurnCo(Animator animator)
    {
        animator.SetTrigger("Press");
        yield return new WaitForSeconds(.25f);
        endTurnButton.DOScale(Vector3.zero, .25f).SetEase(Ease.OutQuad);
        foreach (var card in FindObjectsByType<Card>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            if (card!=player.activeAttackCard && card!= player.activeDefenseCard)
            {
               card.EndedTurn();
            }
        }
        if (player.activeAttackCard!=null)
        {
            player.activeAttackCard.Attack();
            yield return new WaitForSeconds(1.75f);
        }
        else
        {
            StartCoroutine(EnemiesAttack());
        }

        yield return new WaitForSeconds(1);
        canDrawCard = true;
        canPlayCard = true;
        if (player.defenseCardsInDeckForRound.Count<=0 && player.attackCardsInDeckForRound.Count<=0)
        {
            player.gameManager.StartCoroutine("EndTurnButton");
        }
    }

    public IEnumerator EnemiesAttack()
    {
        for (int i = 0; i < currentEnemy.Count; i++)
        {
            if (currentEnemy[i].health>0)
            {
                currentEnemy[i].StartCoroutine("Attack");
                yield return new WaitForSeconds(0.25f);   
            }
        }   
    }
    public void DiscardDefenseCard(Animator animator)
    {
        StartCoroutine(DiscardDefenseCardCo(animator));
    }
      public void DiscardAttackCard(Animator animator)
    {
        StartCoroutine(DiscardAttackCardCo(animator));
    }
    
    private IEnumerator DiscardDefenseCardCo(Animator animator)
    {
        player.activeDefenseCard.rectTransform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutQuad);
        animator.SetTrigger("Press");
        yield return new WaitForSeconds(0.25f);
        Destroy(player.activeDefenseCard.gameObject);
        yield return new WaitForSeconds(0.25f);
        discardDefenseButtonRect.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(0.25f);
        discardDefenseButton.SetActive(false);
    }
    
    private IEnumerator DiscardAttackCardCo(Animator animator)
    {
        player.activeAttackCard.rectTransform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutQuad);
        animator.SetTrigger("Press");
        yield return new WaitForSeconds(0.25f);
        Destroy(player.activeAttackCard.gameObject);
        yield return new WaitForSeconds(0.25f);
        discardAttackButtonRect.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(0.25f);
        discardAttackButton.SetActive(false);
    }

    public IEnumerator CompletedRound()
    {
        canEndTurn = false;
        yield return new WaitForSeconds(1.5f);
        shopPanel.EnterShop();
    }
    
    public IEnumerator EndTurnButton()
    {
        endTurnButton.DOScale(Vector3.one*1.25f, .25f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(.25f);
        canEndTurn = true;
    } 
}
