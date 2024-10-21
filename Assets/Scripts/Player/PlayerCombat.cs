using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerCombat : MonoBehaviour
{
    public Transform meleePoint;
    public float attackRange = 1f; // Rango del ataque
    public int attackDamage = 10; // Daño del ataque
    public LayerMask enemyLayers; // Capas que representan enemigos

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Botón izquierdo del mouse
        {
            MeleeAttack();
        }
    }

    void MeleeAttack()
    {
        // Realiza un ataque en forma de área
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleePoint.position, attackRange, enemyLayers);

        // Recorre todos los enemigos en el rango y aplica daño
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Health>().TakeDamage(attackDamage);
            Debug.Log("hit");
        }

        // Opcional: Puedes agregar efectos visuales o sonoros aquí
        Debug.Log("Ataque melee realizado.");
    }

    private void OnDrawGizmosSelected()
    {
        // Muestra el rango de ataque en el editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(meleePoint.position, attackRange);
    }
}
