using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HappyFace : AttackCard
{
    public int attackToAdd;
    public override void OnUpgrade()
    {
        attackDamage += attackToAdd;
        attackDefenseText.text = attackDamage.ToString();
        rectTransform.DOShakePosition(0.25f, Vector3.one * 15, 20, 90);
    }
}
