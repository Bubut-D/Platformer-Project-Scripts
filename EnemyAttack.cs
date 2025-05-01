using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Collider2D attackColl;
    private EnemyDamageManager damage;
    private Animator animator;

    private int attackAnimIndex = -1;
    private string attackAnimName;
    private int idleAnimIndex = -1;
    private string idleAnimName;
    private float attackDuration;
    private float idleDuration;
    private float[] timeArr = new float[2];
    private string[] nameArr = new string[2];

    // Attack interval in cycles.
    [SerializeField] private int attackIntervalCycle = 3;
    
    void Start()
    {
        attackColl = GetComponentsInChildren<Collider2D>()[1];
        attackColl.enabled = false;
        damage = GetComponent<EnemyDamageManager>();
        animator = GetComponent<Animator>();
        for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
        {
            if (animator.runtimeAnimatorController.animationClips[i].name == name.Replace(" ", "_") + "_Attack")
                attackAnimIndex = i;
            else if (animator.runtimeAnimatorController.animationClips[i].name == name.Replace(" ", "_") + "_")
                idleAnimIndex = i;
            if (attackAnimIndex != -1 && idleAnimIndex != -1)
                break;
        }
        attackDuration = animator.runtimeAnimatorController.animationClips[attackAnimIndex].length;
        attackAnimName = animator.runtimeAnimatorController.animationClips[attackAnimIndex].name;
        idleDuration = animator.runtimeAnimatorController.animationClips[idleAnimIndex].length;
        idleAnimName = animator.runtimeAnimatorController.animationClips[idleAnimIndex].name;

        timeArr[0] = attackIntervalCycle * idleDuration;
        timeArr[1] = attackDuration;
        nameArr[0] = attackAnimName;
        nameArr[1] = idleAnimName;

        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        //while (true)
        //{
        //    yield return new WaitForSeconds(attackIntervalCycle * idleDuration);
        //    if (damage.isDead)
        //        break;
        //    animator.Play(attackAnimName);
        //    damage.isAttacking = true;                                                        //Fonksiyonun açýk hali.

        //    yield return new WaitForSeconds(attackDuration);
        //    if (damage.isDead)
        //        break;
        //    animator.Play(idleAnimName);
        //    damage.isAttacking = false;
        //}
        for (int i = 0; true; i++)
        {
            i %= 2;
            yield return new WaitForSeconds(timeArr[i]);
            if (damage.isDead)
                break;
            animator.Play(nameArr[i]);
            damage.isAttacking = !damage.isAttacking;
        }
    }
    private void ColliderEnabler() 
    {
        attackColl.enabled = true;
    }
    private void ColldierDisabler()
    {
        attackColl.enabled = false;

    }
}
