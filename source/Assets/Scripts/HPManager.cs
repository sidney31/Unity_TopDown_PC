using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    [SerializeField] private GameObject[] healthsRef;
    [SerializeField] private Sprite[] healthSprites;
    [SerializeField] private int playerCurrentHP;

    private void Update()
    {
        playerCurrentHP = GameObject.Find("Player").GetComponent<PlayerController>().currentHP;
        for (int i = 0; i < healthsRef.Length; i++)
        {
            if (i >= playerCurrentHP)
            {
                healthsRef[i].GetComponent<Animator>().SetTrigger("HPlose");
            }
        }



    }

    public void takeHealths()
    {
    }

    public void loseHealths()
    {

    }
}
