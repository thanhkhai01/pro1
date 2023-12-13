using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControllerNetwork : NetworkBehaviour
{
    [SerializeField] float walkSpeed;

    [SerializeField] NetworkCharacterControllerPrototype characterControllerPrototype;
    [SerializeField] MyPlayerInput myPlayerInput;
    [SerializeField] MyCameraController myCameraController;
    //[SerializeField] UI_GameFestivalScene gameFestivalScene_UI;
    //[SerializeField] PlayerStatus playerStatus;

    [Space]
    [SerializeField] Transform cameraLookAt;
    [SerializeField] Vector3 moveDir;

    Vector3 _verlocity = Vector3.zero;



    [SerializeField] float graphity = -15f;
    [SerializeField] Vector3 verticalVelocity = Vector3.zero;

    public bool isGround;
    [SerializeField] Transform groundCheckPosition;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundMask;

    [SerializeField] float jumpHeight = 10;

    [SerializeField] float currentSpeed;
    //[SerializeField] float moveSpeed;
    [SerializeField] bool isMove;
    float blendSpeed;

    [Space]
    [SerializeField] PlayerAnimationControllerNetwork playerAnimationControllerNetwork;

    [SerializeField] Vector3 offset;

    public Transform lookAtTarget;
    [SerializeField] LayerMask ignorLayer;

    //[SerializeField] CharacterController characterController;

    public bool canInput;
    [SerializeField] float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private void OnEnable()
    {
        myPlayerInput.Enable();
    }

    private void OnDisable()
    {
        myPlayerInput.Disable();
    }

    private void Awake()
    {
        //characterController = GetComponent<CharacterController>(); 
        characterControllerPrototype = GetComponent<NetworkCharacterControllerPrototype>();
        myPlayerInput = new MyPlayerInput();
        //gameFestivalScene_UI = FindAnyObjectByType<UI_GameFestivalScene>();
        //playerStatus = GetComponent<PlayerStatus>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!HasInputAuthority) { return; }
        //playerStatus.Init();
        //Move();

        PlayerInput();
        PlayerAnim();
    }

    private void FixedUpdate()
    {
        //Debug.Log("Test");
        //if (isMove)
        //{
        //    characterController.Move(new Vector3(moveDir.x, 0f, moveDir.y) * moveSpeed);
        //    Debug.Log("Player moving");
        //}
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasInputAuthority) { return; }

        //MoveTest();
        //Move();
        GroundCheckAndGravity();
        Movement();
    }

    void Init()
    {
        //moveSpeed = playerData.walkSpeed;
        if (!HasInputAuthority) { return; }
        myCameraController = FindAnyObjectByType<MyCameraController>();
        canInput = true;
        currentSpeed = 0;

        /*if(gameFestivalScene_UI.appMongoLaucher != null)
        {
            gameFestivalScene_UI.appMongoLaucher.GetUserInfo();
        }*/

        myCameraController.Init(transform, cameraLookAt);

    }

    void PlayerInput()
    {
        //if(!canInput) { return; }
        //Get Move direction from input
        moveDir.Set(myPlayerInput.Player.Move.ReadValue<Vector2>().x, 0, myPlayerInput.Player.Move.ReadValue<Vector2>().y);
        moveDir.Normalize();

        //Change moveDir follow camera
        moveDir = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, Vector3.up) * moveDir;


        if (moveDir != Vector3.zero)
        {
            currentSpeed = walkSpeed;
        }
        else
        {
            currentSpeed = 0;
        }

        //Jump Input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGround)
            {
                Jump();

                //Neu co Animation thi phai dung animation event de goi ham jump 
                //playerAnimationController.JumpAnim();
            }

        }

        //Attack
        //if (bow != null)
        //{
        //    Attack();
        //}

        Aim();

        //Lock_UnLock_Cursor
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;

        }
        else
        {
            if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;

            }
        }
    }
    void GroundCheckAndGravity()
    {
        //Code GroundCheck
        //Vector3 groundCheckPosition = new Vector3(transform.position.x, transform.position.y - groundCheckOffset, transform.position.z);
        isGround = Physics.CheckSphere(groundCheckPosition.position, groundCheckRadius, groundMask);

        //Code graphity
        if (isGround)
        {
            if (verticalVelocity.y < 0)
            {
                verticalVelocity.y = 0;
            }
        }
        else
        {
            if (verticalVelocity.y > graphity)
            {
                verticalVelocity.y += graphity;
            }
        }
    }
    void Movement()
    {
        _verlocity = new Vector3(moveDir.x * walkSpeed, 0f, moveDir.z * walkSpeed);
        //characterController.Move(_verlocity);
        characterControllerPrototype.Move(_verlocity * Runner.DeltaTime);

        //playerAnimationController.MoveAnim(currentSpeed);
    }

    public void Jump()
    {
        verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * graphity);
        //_verlocity.y = verticalVelocity.y;
    }

    void PlayerAnim()
    {
        if (playerAnimationControllerNetwork != null)
        {
            playerAnimationControllerNetwork.MovemantAnimation(currentSpeed);
        }
    }

    void Aim()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, ~ignorLayer))
        {
            if (lookAtTarget != null)
            {
                lookAtTarget.position = hit.point;

            }            
        }

        //Debug.Log(hit.transform.name);
    }


    public void PickUpCallback()
    {

    }


    #region
    void Move()
    {
        //if (!HasInputAuthority) { return; }

        Vector3 moveInputDirection = new Vector3(myPlayerInput.Player.Move.ReadValue<Vector2>().x, 0, myPlayerInput.Player.Move.ReadValue<Vector2>().y).normalized;
        currentSpeed = moveInputDirection.magnitude;

        if (moveInputDirection.magnitude >= 0.1f)
        {
            //float targetAngle = Mathf.Atan2(moveInputDirection.x,moveInputDirection.z) * Mathf.Rad2Deg ;
            float targetAngle = Mathf.Atan2(moveInputDirection.x, moveInputDirection.z) * Mathf.Rad2Deg * Camera.main.transform.eulerAngles.y;
            //float smoothRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothTime, turnSmoothTime);
            //transform.rotation = Quaternion.Euler(0, smoothRotation, 0);

            //moveDir = Quaternion.Euler(0f,targetAngle,0f) * Vector3.forward ;
            moveDir = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, Vector3.up) * moveInputDirection.normalized;

            characterControllerPrototype.Move(moveDir * Runner.DeltaTime * walkSpeed);

        }

        //Animation
        if (playerAnimationControllerNetwork != null)
        {
            playerAnimationControllerNetwork.MovemantAnimation(currentSpeed);
        }
    }

    void MoveTest()
    {
        if (!HasInputAuthority) { return; }
        if (myPlayerInput.Player.Move.IsPressed() || isMove)
        {
            if (!isMove)
            {
                isMove = true;
            }
            moveDir.Set(myPlayerInput.Player.Move.ReadValue<Vector2>().x, 0, myPlayerInput.Player.Move.ReadValue<Vector2>().y);
            //moveDir = myPlayerInput.Player.Move.ReadValue<Vector2>();
            moveDir.Normalize();

            currentSpeed = moveDir.magnitude;

            characterControllerPrototype.Move(moveDir * Runner.DeltaTime * walkSpeed);

            if (moveDir.magnitude <= 0.1)
            {
                Debug.Log("Player Stop move");
                isMove = false;
            }
        }

        //blendSpeed = Mathf.Lerp(blendSpeed, currentSpeed, Time.deltaTime * 10);


        if (playerAnimationControllerNetwork != null)
        {
            playerAnimationControllerNetwork.MovemantAnimation(currentSpeed);
        }
    }

    #endregion

}
