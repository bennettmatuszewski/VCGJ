using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    public Animator anim;
    public LevelLoader levelLoader;
    public TMP_Text roundText;

    public void Restart(Animator animator)
    {
        AudioManager.instance.Play("clickButton");
        animator.SetTrigger("Press");
        levelLoader.LoadCertainScene("Game");
        
    }
    public void HomeScreen(Animator animator)
    {
        AudioManager.instance.Play("clickButton");
        animator.SetTrigger("Press");
        levelLoader.LoadCertainScene("Start");
    }
}
