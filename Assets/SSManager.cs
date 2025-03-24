using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSManager : MonoBehaviour
{
    public bool cant;
    public LevelLoader ll;
    private void Start()
    {
        AudioManager.instance.Play("song");
    }

    public void StartGame(Animator animator)
    {
        if (!cant)
        {
            AudioManager.instance.Play("clickButton");
            animator.SetTrigger("Press");
            cant = true;
            ll.LoadCertainScene("Game");
        }
    }

    public void Tutorial(Animator animator)
    {
        if (!cant)
        {
            AudioManager.instance.Play("clickButton");
            animator.SetTrigger("Press");
            cant = true;
            ll.LoadCertainScene("Tutorial");
        }
    }

}
