using System;
using System.Collections;
using System.Collections.Generic;
using Code.Logic;
using UnityEngine;

public class HeroAnimator : MonoBehaviour
{
    private static readonly int Run = Animator.StringToHash("Run");
    //private static readonly int TIPAngle = Animator.StringToHash("TIPAngle");
    // private static readonly int AttackMeleeHash = Animator.StringToHash("AttackMelee");
    // private static readonly int HitHash = Animator.StringToHash("Hit");
    // private static readonly int DieHash = Animator.StringToHash("Die");
    
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
}
