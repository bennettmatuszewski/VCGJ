using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BlankFace : DefensiveCard
{
    public int healthChange;
    public override void OnUpgrade()
    {
        health += healthChange;
        attackDefenseText.text = health.ToString();
        rectTransform.DOShakePosition(0.25f, Vector3.one * 15, 20, 90);
        
    }
}
