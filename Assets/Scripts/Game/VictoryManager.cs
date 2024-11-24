using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    [SerializeField] GameObject victoryScreen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            victoryScreen.SetActive(true);
    }

}
