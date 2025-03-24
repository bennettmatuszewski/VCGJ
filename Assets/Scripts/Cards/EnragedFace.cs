using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnragedFace : AttackCard
{
    public override void OnUpgrade()
    {
        foreach (var card in FindObjectsOfType<DefensiveCard>())
        {
            if (card==player.activeDefenseCard)
            {
                card.health += 5;
                card.attackDefenseText.text = card.health.ToString();
            }
        }
        rectTransform.DOShakePosition(0.25f, Vector3.one * 15, 20, 90);
    }
}
