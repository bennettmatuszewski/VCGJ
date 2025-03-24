using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Hoverable : MonoBehaviour
{
    public Canvas canvas;
    public RectTransform rectTransform; 
    protected RectTransform rectTransform2;
    private Tween enterTween;
    private Tween exitTween;
    private int ogSortingOrder;
    public bool isCardPile;
    private IEnumerator Start()
    {
        rectTransform2 = GetComponent<RectTransform>();
        yield return new WaitForSeconds(0.1f);
        ogSortingOrder = canvas.sortingOrder;
    }

    public void HoverOver()
    {
        if (exitTween!=null)
        {
            exitTween.Kill();
        }
        AudioManager.instance.Play("hoverCard");
        enterTween = rectTransform.DOScale(new Vector3(1.25f, 1.25f, 1.25f), 0.15f).SetEase(Ease.InOutBack);
        if (isCardPile)
        {
            canvas.sortingOrder = 104;
        }
        else
        {
            canvas.sortingOrder = 100;
        }
    }

    public void HoverExit()
    {
        if (enterTween!=null)
        {
            enterTween.Kill();
        }
        exitTween = rectTransform.DOScale(Vector3.one, 0.1f).SetEase(Ease.InOutQuad);
        canvas.sortingOrder = ogSortingOrder;
    }
}
