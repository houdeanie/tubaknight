using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public Rigidbody2D rb;

    Vector2 movement;
    Vector2 previousMovement;
    private float lastFacing;

    private float activeMoveSpeed;
    public float dashSpeed = 10f;

    public float dashLength = .5f, dashCooldown = 3f;
    private float dashCounter;
    private float dashCoolCounter;

    private bool canMove;

    public Text stamina;
    public Text Health;

    public Animator animator;

    public Transform attackPointLeft;
    public Transform attackPointRight;
    public Transform attackPointDown;
    public Transform attackPointUp;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public int attackDamage = 45;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    public bool isFlipped = false;

    public BoxCollider2D bc;

    //private bool canMove;
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        activeMoveSpeed = movementSpeed;
        lastFacing = 1;
        dashCoolCounter = 0;
        dashCounter = 0;
        stamina.text = "0";
        Health.text = "0";
        isFlipped = false;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveX, moveY).normalized;

        previousMovement = movement;
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveX, moveY).normalized;

        //rb.velocity = movement * activeMoveSpeed;
        stamina.text = dashCoolCounter.ToString();
        Health.text = "dashCounter: " + dashCounter.ToString();

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        

        if (previousMovement.sqrMagnitude > 0)
        {
            lastFacing = previousMovement.x;
            animator.SetFloat("LastFacing", lastFacing);
        }

        previousMovement = movement;

        // FlipPlayer();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dashCoolCounter <= 0 && dashCounter <= 0)
            {
                animator.SetTrigger("Dash");
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
            }
        }

        // change speed to default while on cooldown
        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;

            if (dashCounter <= 0)
            {
                activeMoveSpeed = movementSpeed;
                dashCoolCounter = dashCooldown;
            }
        }

        // cooldown till next dash
        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;

        }

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        
    }


    // updated per frame, e.g. updated 30 frames per second
    void FixedUpdate()
    {
        
        // movement
        rb.MovePosition(rb.position + movement * activeMoveSpeed * Time.fixedDeltaTime);
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        //Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPointLeft.position, attackRange, enemyLayers);
        //foreach (Collider2D enemy in hitEnemies)
        //{
        //    Debug.Log("We hit " + enemy.name);
        //    enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        //}
        if (lastFacing < 0)
        {
            //Debug.Log("attacking left");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPointLeft.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("We hit " + enemy.name);
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
        }
        else
        {
            //Debug.Log("attacking right");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPointRight.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("We hit " + enemy.name);
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
        }

    }
    
    // flip the player hit boxes when the character is moving left and right
    void FlipPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;
        Debug.Log("flip?");
        if (lastFacing < 0 && isFlipped)
        {
            Debug.Log("Facing left, flipped right");
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (lastFacing > 0 && !isFlipped)
        {
            Debug.Log("Facing right, flip right");
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPointRight == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPointRight.position, attackRange);

        if (attackPointLeft == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPointLeft.position, attackRange);

        if (attackPointUp == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPointUp.position, attackRange);

        if (attackPointDown == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPointDown.position, attackRange);
    }
}
