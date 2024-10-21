using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyProjectile :  EnemyDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;

    private float lifetime;
    private Animator anim;
    private BoxCollider2D coll;


    private bool hit;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        coll.enabled = true;
    }
    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(0, movementSpeed, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        base.OnTriggerEnter2D(collision); 
        coll.enabled = false;

        if (anim != null)
            anim.SetTrigger("explode"); //creo que el player usa fireballs o algo asi entonces le meti esto
        else
            gameObject.SetActive(false);
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}