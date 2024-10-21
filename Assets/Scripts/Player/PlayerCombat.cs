using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerCombat : MonoBehaviour
{
    // Ataque melee
    public float meleeRange = 0.5f;
    public int meleeDamage = 20;
    public Transform meleePoint;
    public LayerMask enemyLayer;

    // Ataque láser
    public float laserRange = 10f;
    public LineRenderer laserRenderer;
    public float laserDuration = 0.5f;
    public int laserDamage = 50;

    // Cooldowns
    public float meleeCooldown = 0.3f;
    public float laserCooldown = 1f;
    private bool canAttack = true;
    private bool isAiming = false;
    
    void Update()
    {
        if (Input.GetMouseButton(1)) // Botón secundario del ratón
        {
            isAiming = true;
            StartAiming();
        }
        else
        {
            isAiming = false;
            StopAiming();
        }
        
        if (isAiming && Input.GetMouseButtonDown(0) && canAttack) // Botón secundario
        {
            StartCoroutine(LaserBeamAttack());
        }
        else if (!isAiming && Input.GetMouseButtonDown(0) && canAttack)
        {
            StartCoroutine(MeleeAttack());
        }
    }

    // Ataque melee
    private IEnumerator MeleeAttack()
    {
        canAttack = false;

        // Aquí podrías poner animaciones de ataque melee
        Debug.Log("Ataque Melee");

        // Detectar enemigos en el área del ataque
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleePoint.position, meleeRange, enemyLayer);

        // Daño a los enemigos detectados
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Enemigo golpeado: " + enemy.name);
        }

        // Cooldown para el ataque melee
        yield return new WaitForSeconds(meleeCooldown);
        canAttack = true;
    }

    // Dibuja el rango del ataque melee en la vista de escena
    private void OnDrawGizmosSelected()
    {
        if (meleePoint == null)
            return;

        Gizmos.DrawWireSphere(meleePoint.position, meleeRange);
    }

    // ATAQUE LÁSER
    private IEnumerator LaserBeamAttack()
    {
        canAttack = false;

        // Activar el LineRenderer para mostrar el láser
        laserRenderer.enabled = true;

        // Definir el punto de inicio y final del láser
        Vector3 laserEndPoint = transform.position + transform.right * laserRange;
        laserRenderer.SetPosition(0, transform.position);  // Punto inicial
        laserRenderer.SetPosition(1, laserEndPoint);       // Punto final

        // Detectar colisión con enemigos en la línea del rayo
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, laserRange, enemyLayer);
        if (hit)
        {
            Debug.Log("Láser golpeó a: " + hit.collider.name);
        }

        // Esperar la duración del láser antes de desactivarlo
        yield return new WaitForSeconds(laserDuration);
        laserRenderer.enabled = false;

        // Esperar el cooldown del láser
        yield return new WaitForSeconds(laserCooldown);
        canAttack = true;
    }

    // Método que se activa cuando comienza a apuntar
    private void StartAiming()
    {
        // Podrías activar algún indicador visual de apuntado o cambiar el cursor
        Debug.Log("Apuntando con láser");
    }

    // Método que se activa cuando deja de apuntar
    private void StopAiming()
    {
        // Desactiva cualquier indicador visual de apuntado
        Debug.Log("Dejó de apuntar");
    }
}
