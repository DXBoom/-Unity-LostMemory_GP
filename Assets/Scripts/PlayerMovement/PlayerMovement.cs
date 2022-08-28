using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [Header("Movement")]
    public float MoveSpeed;
    public bool LockMovement;

    [Header("Ground")]
    public LayerMask GroundMask;
    public float GroundDrag;
    [HideInInspector] public bool IsGround;

    [Header("Jumping")]
    public KeyCode JumpKey = KeyCode.Space;
    public float JumpForce;
    public float JumpCooldown;
    public float AirMultiplier;
    [HideInInspector] public bool ReadyToJump;

    public Transform Orientation;

    [HideInInspector] public float HorizontalInput;
    [HideInInspector] public float VerticalInput;

    [HideInInspector] public Vector3 MoveDirection;

    [HideInInspector] public Rigidbody PlayerRigidBody;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayerRigidBody = GetComponent<Rigidbody>();
        PlayerRigidBody.freezeRotation = true;

        ReadyToJump = true;
    }

    private void Update()
    {
        IsGround = Physics.CheckSphere(transform.position, 1f, GroundMask);

        InputFunction();

        SpeedControl();

        if (IsGround)
            PlayerRigidBody.drag = GroundDrag;
        else
            PlayerRigidBody.drag = 0;
    }

    private void FixedUpdate()
    {
        if (!LockMovement)
            MovePlayer();
    }

    private void InputFunction()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(JumpKey) && ReadyToJump && IsGround && !LockMovement)
        {
            ReadyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), JumpCooldown);
        }
    }

    private void MovePlayer()
    {
        if (PlayerClimbing.Instance.ExitingWall && PlayerClimbing.Instance.ClimbAvalialble)
            return;

        MoveDirection = Orientation.forward * VerticalInput + Orientation.right * HorizontalInput;

        if (IsGround)
            PlayerRigidBody.AddForce(MoveDirection.normalized * MoveSpeed * 10f, ForceMode.Force);
        else if (!IsGround)
            PlayerRigidBody.AddForce(MoveDirection.normalized * MoveSpeed * 10f * AirMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(PlayerRigidBody.velocity.x, 0f, PlayerRigidBody.velocity.z);

        if (flatVelocity.magnitude > MoveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * MoveSpeed;
            PlayerRigidBody.velocity = new Vector3(limitedVelocity.x, PlayerRigidBody.velocity.y, limitedVelocity.z);
        }
    }

    private void Jump()
    {
        PlayerRigidBody.velocity = new Vector3(PlayerRigidBody.velocity.x, 0f, PlayerRigidBody.velocity.z);

        PlayerRigidBody.AddForce(transform.up * JumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        ReadyToJump = true;
    }
}
