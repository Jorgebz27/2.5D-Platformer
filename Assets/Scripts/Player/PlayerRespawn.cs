using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform respawnPoint;
    private Vector3 _initialPosition;
    [SerializeField] private Health playerHealth;

    void Start()
    {
        _initialPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
            if (other.CompareTag("Respawn"))
            {
                if (playerHealth.currentHealth <= 0f)
                {
                    Respawn();
                }
            }
    }

    public void Respawn()
    {
        PlayerMovement.mSpeed = 7f;
        PlayerCombat.score = 0;
        PlayerCombat.laserUpgrade = false;
        playerHealth.AddMaxHealth(-25);
        Debug.Log("Player Respawn");
        transform.position = respawnPoint.position;
    }
    
}
