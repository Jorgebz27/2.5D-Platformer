using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                SceneManager.LoadScene(1);
            }
    }

    public void Respawn()
    {
        Stats();
        transform.position = respawnPoint.position;
    }
    
    public void Stats()
    {
        PlayerMovement.mSpeed = 7f;
        PlayerCombat.score = 0;
        PlayerCombat.laserUpgrade = false;
        playerHealth.AddMaxHealth(-25);
        Debug.Log("Player Respawn");
    }
}
