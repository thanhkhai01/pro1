using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public CameraControl MCC;
    public float rotSpeed = 600f;
    Quaternion requiredRotation;
    bool playerControl = true;

    public Animator animator;
    public CharacterController cc;
    public float groundCheckRadius = 0.3f;
    public Vector3 groundCheckOffset;
    public LayerMask groundlayer;
    bool grounded;
    [SerializeField] float fallingSpeed;
    [SerializeField] Vector3 moveDir;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        PlayerMove();
        if (!playerControl)
            return;
        if (grounded)
        {
            fallingSpeed = 0f;
        }
        else
        {
            fallingSpeed += Physics.gravity.y * Time.deltaTime;
        }
        var verlocity = moveDir * movementSpeed;
        verlocity.y = fallingSpeed;

        
        SurfaceCheck();
        Debug.Log("Player on ground " + grounded);
    }
    void PlayerMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float movementAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        var movementInput = (new Vector3(horizontal, 0, vertical)).normalized;
        var movementDirection = MCC.flatRotation * movementInput;

        cc.Move(movementDirection * movementSpeed * Time.deltaTime);
        if (movementAmount > 0)
        {
            requiredRotation = Quaternion.LookRotation(movementDirection);
        }

        movementDirection = moveDir;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, requiredRotation, rotSpeed * Time.deltaTime);

        animator.SetFloat("Blend", movementAmount,0.2f,Time.deltaTime);
    }
    void SurfaceCheck()
    {
        grounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundlayer);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }
    public void SetControl(bool hasControl)
    {
        this.playerControl=hasControl;
        cc.enabled = hasControl;
        if (!hasControl)
        animator.SetFloat("Blend", 0f);
        requiredRotation = transform.rotation;
    }
}
