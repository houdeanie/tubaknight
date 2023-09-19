using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    int currHealth;
    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //void FixedUpdate()
    //{

    //    // movement
    //    rb.MovePosition(rb.position + movement * activeMoveSpeed * Time.fixedDeltaTime);
    //}

    public void TakeDamage(int damage)
    {
        currHealth -= damage;

        if (currHealth <= 0) {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");
    }
}
