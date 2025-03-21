using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPile : Hoverable
{
    public Player player;

    public bool attackPile;
    public void DrawCard()
    {
        if (attackPile)
        {
            if (player.attackCardsInDeckForRound.Count>0)
            {
                player.AddCardToHand(player.attackCardsInDeckForRound[0]);
                player.attackCardsInDeckForRound.RemoveAt(0);
            }
        }
        else
        {
            if (player.defenseCardsInDeckForRound.Count>0)
            {
                player.AddCardToHand(player.defenseCardsInDeckForRound[0]);
                player.defenseCardsInDeckForRound.RemoveAt(0);
            }
        }

    }
}
