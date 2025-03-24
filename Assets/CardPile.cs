using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class CardPile : Hoverable
{
    public Player player;
    public Sprite altSprite;
    public Sprite defaultSprite;
    public Image image;
    public bool attackPile;
    public void DrawCard()
    {
        if (!player.gameManager.canDrawCard)
        {
            return;
        }
        player.gameManager.canDrawCard = false;
        if (attackPile)
        {
            if (player.attackCardsInDeckForRound.Count>0)
            {
                player.AddCardToHand(player.attackCardsInDeckForRound[0]);
                player.attackCardsInDeckForRound.RemoveAt(0);
                if (player.attackCardsInDeckForRound.Count==0)
                {
                    image.sprite = altSprite;
                }
            }
        }
        else
        {
            if (player.defenseCardsInDeckForRound.Count>0)
            {
                player.AddCardToHand(player.defenseCardsInDeckForRound[0]);
                player.defenseCardsInDeckForRound.RemoveAt(0);
                if (player.defenseCardsInDeckForRound.Count==0)
                {
                    image.sprite = altSprite;
                }
            }
        }
        
        player.gameManager.StartCoroutine("EndTurnButton");

    }

    public void UpdateSprite()
    {
        image.sprite = altSprite;
    }
    public void DefaultSprite()
    {
        image.sprite = defaultSprite;
    }
    
}
