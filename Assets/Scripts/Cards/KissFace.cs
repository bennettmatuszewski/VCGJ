using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class KissFace : DefensiveCard
{
    public override void OnUpgrade()
    {
        player.DrawDefense();
        rectTransform.DOShakePosition(0.25f, Vector3.one * 15, 20, 90);
        
    }
}
