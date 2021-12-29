using System;
using Code.CameraLogic;
using Code.Infrastructure;
using Code.Services.Input;
using UnityEngine;

namespace Code.Hero
{
    public class HeroMove : MonoBehaviour
    {
        public float MovementSpeed;
        
        private CharacterController _characterController;
        private IInputService _inputService;
        private Camera _camera;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _inputService = Game.InputService;
        }

        private void Start()
        {
            _camera = Camera.main;
            CameraFollow();
        }

        private void Update()
        {
            Vector3 movementVector = Vector3.zero;

            if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
            {
                movementVector = new Vector3(_inputService.Axis.x, 0, _inputService.Axis.y);
                movementVector.Normalize();

                transform.forward = movementVector;
            }

            movementVector += Physics.gravity;
            
            _characterController.Move(MovementSpeed * movementVector * Time.deltaTime);
        }

        private void CameraFollow() => 
            _camera.GetComponent<CameraFollow>().Follow(gameObject);
    }
}