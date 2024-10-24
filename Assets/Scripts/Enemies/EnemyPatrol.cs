using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement parameters")]
    [SerializeField] private float speed;

    private Vector2 moveDirection;
    private int currentPointIndex = 0;
    private bool goingForward = true;  // Controla la dirección de ida y vuelta

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;

    void Start()
    {
        SetMoveDirection();
    }

    void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        if (waypoints.Length == 0) return;

        // Mover al enemigo en la dirección establecida
        transform.Translate(moveDirection * (speed * Time.deltaTime));

        // Si está lo suficientemente cerca del waypoint actual, cambiar al siguiente
        if (Vector2.Distance(transform.position, waypoints[currentPointIndex].position) < 0.1f)
        {
            // Cambiar de dirección si alcanzó el último waypoint o el primero
            if (goingForward && currentPointIndex == waypoints.Length - 1)
            {
                goingForward = false;
            }
            else if (!goingForward && currentPointIndex == 0)
            {
                goingForward = true;
            }

            // Actualizar el índice de waypoint de acuerdo a la dirección
            currentPointIndex = goingForward ? currentPointIndex + 1 : currentPointIndex - 1;

            // Establecer la nueva dirección de movimiento
            SetMoveDirection();
        }
    }

    private void SetMoveDirection()
    {
        Vector2 targetPoint = waypoints[currentPointIndex].position;
        moveDirection = (targetPoint - (Vector2)transform.position).normalized;
    }

    // Gizmos para visualizar los waypoints en la escena
    private void OnDrawGizmos()
    {
        if (waypoints != null)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(waypoints[i].position, 0.2f);

                if (i + 1 < waypoints.Length)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                }
            }
        }
    }
}
