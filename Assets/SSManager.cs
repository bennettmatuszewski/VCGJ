using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSManager : MonoBehaviour
{
    public bool cant;
    public LevelLoader ll;
    public void StartGame(Animator animator)
    {
        if (!cant)
        {
            animator.SetTrigger("Press");
            cant = true;
            ll.LoadCertainScene("Game");
        }
    }

    public void Tutorial(Animator animator)
    {
        if (!cant)
        {
            animator.SetTrigger("Press");
            cant = true;
            ll.LoadCertainScene("Tutorial");
        }
    }

}
