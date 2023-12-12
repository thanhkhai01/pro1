using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput PlayerInput;
    InputAction moveAction;

    Rigidbody rb;

    [SerializeField] float speed = 5;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        PlayerInput = GetComponent<PlayerInput>();
        moveAction = PlayerInput.actions.FindAction("Move");

    }

    private void Update()
    {
        MovementPlayer();
    }
    public void MovementPlayer()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(direction.x, 0, direction.y) * speed*  Time.deltaTime;
    }
}
