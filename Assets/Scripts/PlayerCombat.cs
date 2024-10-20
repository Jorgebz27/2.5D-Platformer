using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerCombat : MonoBehaviour
{
    public float meleeRange = 0.5f;
    public int meleeDamage = 20;
    public Transform meleeHit;
    public LayerMask enemyLayer;
    public float meleeCooldown = 0.5f;
    private bool _canAttack = false;
    private bool _isAiming = false;

    void Update()
    {
        // 0 es principal y 1 es secundario
        if (Input.GetMouseButton(1))
        {
            _isAiming = true;
            //Aim();
        }
        else
        {
            _isAiming = false;
            //StopAim();
        }

        if (_isAiming && Input.GetMouseButtonDown(0) &&_canAttack)
        {
            //StartCoroutine(LaserAttack());
        }
        else if (!_isAiming && Input.GetMouseButtonDown(0) && _canAttack)
        {
            //StartCoroutine(MeleeAttack());
        }
    }
}
