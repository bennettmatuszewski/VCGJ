using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : Hoverable
{
    protected GameManager gameManager;
    protected Player player;
    public TMP_Text attackDefenseText;
    public TMP_Text turnsText;
    public int turnsToUpgrade;
    protected bool hasBeenPlayed;
    protected bool upgraded;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = gameManager.player;
    }

    public void EndedTurn()
    {
        if (turnsToUpgrade>0)
        {
            turnsToUpgrade--;
            turnsText.text = turnsToUpgrade.ToString();
        }

        if (turnsToUpgrade==0 && !upgraded)
        {
            OnUpgrade();
            upgraded = true;
        }
        
    }

    public virtual void OnUpgrade()
    {
        
    }

    public virtual void PlayCard() { }

}
