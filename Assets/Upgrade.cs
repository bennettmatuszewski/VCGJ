using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public bool isCherry;
    public bool isApple;
    public bool isOrange;
    public bool isBanana;
    public bool isPear;
    private void Awake()
    {
        if (isCherry)
        {
            Cherry();
        }
        if (isApple)
        {
            Apple();
        }
        if (isPear)
        {
            Pear();
        }
        if (isOrange)
        {
            Orange();
        }
        if (isBanana)
        {
            Banana();
        }
    }


    void Cherry()
    {
        FindObjectOfType<Player>().Cherry();
    }
    void Apple()
    {
        FindObjectOfType<Player>().drawExtraDefense = true;
    }
    void Orange()
    {
        FindObjectOfType<Player>().extraAttack = true;
    }
    void Banana()
    {
        FindObjectOfType<Player>().extraDefense = true;
    }
    void Pear()
    {
        FindObjectOfType<Player>().drawExtraAttack = true;
    }
}
