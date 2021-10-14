using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Moving
{
    public int damage;
    public AudioClip attackSound1;                      
    public AudioClip attackSound2;                      

    Animator animator;
    Transform target;
    bool skipMove;
    // Start is called before the first frame update
    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = FindObjectOfType<Player>().transform;
        base.Start();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
       if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);
        skipMove = true;
    }


   public void Move()
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            yDir = target.position.y > transform.position.y ? 1 : -1;
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player>(xDir, yDir);
    }


    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;
        hitPlayer.LoseFood(damage);
        animator.SetTrigger("Attack");
        SoundManager.instance.RandomizeSfx(attackSound1, attackSound2);
    }
}
