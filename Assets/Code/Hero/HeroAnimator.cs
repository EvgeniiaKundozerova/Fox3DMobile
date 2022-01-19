using System;
using System.Collections;
using System.Collections.Generic;
using Code.Logic;
using UnityEngine;

public class HeroAnimator : MonoBehaviour
{
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int AttackMelee = Animator.StringToHash("AttackMelee");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Death = Animator.StringToHash("Death");
    
    private Animator _animator;
    private CharacterController _characterController;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _animator.SetFloat(Run, _characterController.velocity.magnitude, 0.1f, Time.deltaTime);
    }

    public void PlayHit() => _animator.SetTrigger(Hit);
    public void PlayAttack() => _animator.SetTrigger(AttackMelee);
    public void PlayDeath() => _animator.SetTrigger(Death);
}
