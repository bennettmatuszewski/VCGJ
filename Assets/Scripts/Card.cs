using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Card : Hoverable
{
    public TMP_Text attackDefenseText;
    public TMP_Text turnsText;
    public int turnsToUpgrade;

    public virtual void OnUpgrade()
    {
        
    }

    public virtual void PlayCard()
    {
        
    }
}
