using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerCombat : MonoBehaviour
{
    //Melee
    public Transform meleePoint;
    public float attackRange = 1f;
    public int attackDamage = 10; 
    public LayerMask enemyLayers; 

    //Laser
    public Transform laserFirePoint;
    public LineRenderer laserLineRenderer;
    //public LineRenderer upgradedLaserLineRenderer;
    public float laserRange = 10f;
    public float upgradedLaserRange = 15f;
    public int laserDamage = 20;
    public int UpgradedLaserDamage = 25;
    public float laserDamageRate = 0.5f;

    private bool _isLaserActive = false;
    private float _nextLaserDamageTime = 0f;

    //progresion
    public static float score;
    public bool stage0 = true;
    public static bool healthUpgrade;
    public static bool laserUpgrade;
    public static bool movementUpgrade;



    private void Awake()
    {
        healthUpgrade = false;
        laserUpgrade = false;
        movementUpgrade = false;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            MeleeAttack();
        }
        
        if (Input.GetMouseButton(1))
        {
            if (!_isLaserActive)
            {
                _isLaserActive = true;
                laserLineRenderer.enabled = true;
            }
            ShootLaser();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            _isLaserActive = false;
            laserLineRenderer.enabled = false;
        }
    }

    void MeleeAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleePoint.position, attackRange, enemyLayers);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Health>().TakeDamage(attackDamage);
            if (enemy.GetComponent<Health>().dead == true)
            {
                score += 50;
            }
            Debug.Log("hit");
        }
        
        Debug.Log("Ataque melee realizado.");
    }

    void ShootLaser()
    {
        //if (laserUpgrade == false)
        //{
        laserLineRenderer.SetPosition(0, laserFirePoint.position);
        if (laserUpgrade == false)
        {
            laserLineRenderer.startColor = Color.cyan;
            laserLineRenderer.endColor = Color.cyan;
        }
        if (laserUpgrade == true)
        {
            laserLineRenderer.startColor = Color.red;
            laserLineRenderer.endColor = Color.red;
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)laserFirePoint.position).normalized;
        RaycastHit2D hitInfo = Physics2D.Raycast(laserFirePoint.position, direction, laserRange, enemyLayers);
        if (hitInfo)
        {
            laserLineRenderer.SetPosition(1, hitInfo.point);

            Health enemy = hitInfo.collider.GetComponent<Health>();
            if (enemy != null && Time.time >= _nextLaserDamageTime)
            {
                if (laserUpgrade == false)
                {
                    enemy.TakeDamage(laserDamage);
                }
                if (laserUpgrade == true)
                {
                    enemy.TakeDamage(UpgradedLaserDamage);
                }
                _nextLaserDamageTime = Time.time + laserDamageRate;
                if (enemy.GetComponent<Health>().dead == true)
                {
                    score += 50;
                    Debug.Log(score);

                }
                Debug.Log("Daño continuo con láser.");
            }
        }
        else
        {
            laserLineRenderer.SetPosition(1, (Vector2)laserFirePoint.position + direction * laserRange);
        }
        //}
        //if(laserUpgrade == true) 
        //{
        //    upgradedLaserLineRenderer.SetPosition(0, laserFirePoint.position);

        //    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Vector2 direction = (mousePosition - (Vector2)laserFirePoint.position).normalized;

        //    RaycastHit2D hitInfo = Physics2D.Raycast(laserFirePoint.position, direction, laserRange+5, enemyLayers);
        //    if (hitInfo)
        //    {
        //        upgradedLaserLineRenderer.SetPosition(1, hitInfo.point);

        //        Health enemy = hitInfo.collider.GetComponent<Health>();
        //        if (enemy != null && Time.time >= _nextLaserDamageTime)
        //        {
        //            enemy.TakeDamage(laserDamage+5);
        //            _nextLaserDamageTime = Time.time + laserDamageRate;
        //            if (enemy.GetComponent<Health>().dead == true)
        //            {
        //                score += 50;
        //                Debug.Log(score);

        //            }
        //            Debug.Log("Daño continuo con láser.");
        //        }
        //    }
        //    else
        //    {
        //        upgradedLaserLineRenderer.SetPosition(1, (Vector2)laserFirePoint.position + direction * laserRange);
        //    }
        //}
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(meleePoint.position, attackRange);
    }
}
