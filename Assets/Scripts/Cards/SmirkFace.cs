using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SmirkFace : AttackCard
{
    public override void OnUpgrade()
    {
        player.DrawAttack();
        rectTransform.DOShakePosition(0.25f, Vector3.one * 15, 20, 90);
    }
}
