using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public RectTransform content;
    public LevelLoader ll;
    private bool cantNext;
    private bool cantNext2;
    private bool cantExit;
    public void Next(Animator animator)
    {
        if (!cantNext)
        {
            AudioManager.instance.Play("clickButton");
            cantNext = true;
            animator.SetTrigger("Press");
            content.DOAnchorPosX(2029, 0.75f).SetEase(Ease.InOutQuad);
        }
    }
    public void Next2(Animator animator)
    {
        if (!cantNext2)
        {
            AudioManager.instance.Play("clickButton");
            cantNext2 = true;
            animator.SetTrigger("Press");
            content.DOAnchorPosX(4385, 0.75f).SetEase(Ease.InOutQuad);
        }
    }

    public void Exit(Animator animator)
    {
        if (!cantExit)
        {
            AudioManager.instance.Play("clickButton");
            cantExit = true;
            animator.SetTrigger("Press");
            ll.LoadCertainScene("Start");
        }
    }
}
