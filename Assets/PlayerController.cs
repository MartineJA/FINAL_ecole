using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    enum PlayerState
    {
        walking,
        sprinting,
        crouching,
        jumping
    }

    private PlayerState currentState;

    #region VARIABLES

    private Animator animator;
    private Vector2 animationBlend;
    private Vector2 animationVelocity;
    public float animationSmoothTime = 0.1f;


    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private Transform camera;
    public Transform scale;

    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction hitAction;


    private InputAction lookAction;
    private InputAction jumpAction;

    private InputAction sprintStart;
    private InputAction sprintEnd;

    private InputAction crouchStart;
    private InputAction crouchEnd;

    [SerializeField]
    private float moveSpeed = 2.0f;

    [SerializeField]
    private float walkSpeed = 2.0f;

    [SerializeField]
    private float sprintSpeed = 10.0f;

    [SerializeField]
    private float rotationSpeed = 2.0f;

    [SerializeField]
    private float jumpHeight = 1.0f;

    [SerializeField]
    private float gravityValue = -9.81f;
    #endregion


    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        camera = Camera.main.transform;

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        hitAction = playerInput.actions["Hit"];
        lookAction = playerInput.actions["Look"];
        //jumpAction = playerInput.actions["Jump"];
        //sprintStart = playerInput.actions["SprintStart"];
        //sprintEnd= playerInput.actions["SprintEnd"];

        //crouchStart = playerInput.actions["CrouchStart"];
        //crouchEnd = playerInput.actions["CrouchEnd"];

        animator = GetComponent<Animator>();


    }

    void Update()
    {
        //StateHandler();

        // Marche Basique
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }


        Vector2 input = moveAction.ReadValue<Vector2>();
        animationBlend = Vector2.SmoothDamp(animationBlend, input, ref animationVelocity, animationSmoothTime);

        Vector3 move = new Vector3(animationBlend.x, 0, animationBlend.y);

        // Caméra
        move = move.x * camera.transform.right.normalized + move.z * camera.transform.forward.normalized;
        move.y = 0f;

        controller.Move(move * Time.deltaTime * moveSpeed);

        // Animator
        animator.SetFloat("moveX", animationBlend.x);
        animator.SetFloat("moveZ", animationBlend.y);

        /*
        // Saut
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.CrossFade("Jump", 0.2f);
        }*/

        if (hitAction.triggered && groundedPlayer)
        {
            
            animator.SetTrigger("Hit");
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        /*
        if (sprintStart.triggered && groundedPlayer)
        {
            currentState = PlayerState.sprinting;
            moveSpeed = 15;
            controller.Move(move * Time.deltaTime * moveSpeed);
            Debug.Log(moveSpeed);
            animator.SetTrigger("Run");
        }


        if (sprintStart.WasReleasedThisFrame())
        {
            currentState = PlayerState.walking;
            moveSpeed = 10;
            controller.Move(move * Time.deltaTime * moveSpeed);
            Debug.Log(moveSpeed);
        }

        // Accroupi
        if (crouchStart.triggered && groundedPlayer)
        {
            Debug.Log("accroupi");
            moveSpeed = 5;
            Vector3 newScale = new Vector3(1f, 0.5f, 1f);
            controller.Move(move * Time.deltaTime * moveSpeed);
            scale.localScale = newScale;

        }
        if (crouchEnd.triggered && groundedPlayer)
        {
            Debug.Log("pas accroupi");
            moveSpeed = 10;
            Vector3 newScale = new Vector3(1f, 1f, 1f);
            controller.Move(move * Time.deltaTime * moveSpeed);
            scale.localScale = newScale;
        }*/




        // Rotation
        float targetAngles = camera.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngles, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


        // Animation




    }


    private void StateHandler()
    {

        if (groundedPlayer)
        {
            currentState = PlayerState.walking;
            moveSpeed = walkSpeed;
        }
        else
        {
            currentState = PlayerState.jumping;
        }


        if (sprintStart.triggered && groundedPlayer)
        {
            currentState = PlayerState.sprinting;
            moveSpeed = sprintSpeed;
            Debug.Log(moveSpeed);
        }


        if (sprintEnd.triggered && groundedPlayer)
        {
            currentState = PlayerState.walking;
            moveSpeed = walkSpeed;
            Debug.Log(moveSpeed);
        }

    }






}
