using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPile : Hoverable
{
    public Player player;
    
    public bool attackPile;
    public void DrawCard()
    {
        if (!player.gameManager.canDrawCard)
        {
            return;
        }
        player.gameManager.canPlayCard = false;
        player.gameManager.canDrawCard = false;
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

        player.gameManager.StartCoroutine("EndTurnButton");

    }
    
}
