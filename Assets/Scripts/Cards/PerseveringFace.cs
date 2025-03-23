using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PerseveringFace : AttackCard
{
    public override void OnUpgrade()
    {
        foreach (var card in FindObjectsOfType<AttackCard>())
        {
            if (card!=player.activeAttackCard)
            {
                card.attackDamage += 2;
                card.attackDefenseText.text = card.attackDamage.ToString();
            }
        }
        rectTransform.DOShakePosition(0.25f, Vector3.one * 15, 20, 90);
    }
}
