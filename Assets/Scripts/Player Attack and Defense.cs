using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class PlayerAttackandDefense : MonoBehaviour
{
    private static float attackCooldown = 0.6f;
    private float cooldown = 0f;
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        cooldown -= Time.deltaTime;

        if (Input.GetButtonDown("Attack") && cooldown <= 0)
            Attack();
        else if (Input.GetButtonDown("Block"))
            Block();
        else if (Input.GetButtonUp("Block"))
            _animator.SetBool("IdleBlock", false);

    }
    private void Block()
    {
        _animator.SetTrigger("Block");
        _animator.SetBool("IdleBlock", true);
    }
    private void Attack()
    {
        int random = Random.Range(1, 4);

        for (int i = 1; i < 4; i++)
        {
            _animator.ResetTrigger("Attack" + i);
        }

        _animator.SetTrigger("Attack" + random);

        cooldown = attackCooldown;
    }
}
