using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbing : MonoBehaviour
{
    public static PlayerClimbing Instance;

    public bool ClimbAvalialble;

    [Header("References")]
    public Transform Orientation;
    public Rigidbody PlayerRigidBody;
    public LayerMask WallLayer;

    [Header("Climbing")]
    public float ClimbSpeed;
    public float MaxClimbTime;
    private float ClimbTimer;

    private bool IsClimbing;

    [Header("Exiting")]
    public bool ExitingWall;
    public float ExitWallTime;
    private float ExitWallTimer;

    [Header("ClimbJump")]
    public float ClimbJumpUpForce;
    public float ClimbJumpBackForce;


    [Header("Detection")]
    public float DetectionLenght;
    public float SphereCastRadius;
    public float MaxWallLookAngle;
    private float WallLookAngle;

    private RaycastHit FrontWallHit;
    private bool IsWall;

    private void Awake()
    {
        Instance = this;
        ClimbAvalialble = false;
    }

    private void Update()
    {
        if (ClimbAvalialble)
        {
            WallCheck();
            StateMachine();

            if (IsClimbing && !ExitingWall)
                ClimbingMovement();
        }
    }

    private void StateMachine()
    {
        if (IsWall && Input.GetKey(KeyCode.W) && WallLookAngle < MaxWallLookAngle && !ExitingWall)
        {
            if (!IsClimbing && ClimbTimer > 0)
                StartClimbing();

            if (ClimbTimer > 0)
                ClimbTimer -= Time.deltaTime;

            if (ClimbTimer < 0)
                StopClimbing();
        }    

        else if(ExitingWall)
        {
            if (IsClimbing)
                StopClimbing();

            if (ExitWallTimer > 0)
                ExitWallTimer -= Time.deltaTime;
            if (ExitWallTimer < 0)
                ExitingWall = false;
        }

        else
        {
            if (IsClimbing)
                StopClimbing();
        }

        if (IsWall && Input.GetKeyDown(PlayerMovement.Instance.JumpKey))
            ClimbJump();
    }

    private void WallCheck()
    {
        var getAllDirection = Physics.OverlapSphere(transform.position, SphereCastRadius, WallLayer);
        IsWall = getAllDirection.Length > 0;

        WallLookAngle = Vector3.Angle(Orientation.forward, -FrontWallHit.normal);

        if (PlayerMovement.Instance.IsGround)
        {
            ClimbTimer = MaxClimbTime;
        }
    }

    private void StartClimbing()
    {
        IsClimbing = true;


    }

    private void ClimbingMovement()
    {
        PlayerRigidBody.velocity = new Vector3(PlayerRigidBody.velocity.x, ClimbSpeed, PlayerRigidBody.velocity.z);
    }

    private void StopClimbing()
    {
        IsClimbing = false;
    }

    private void ClimbJump()
    {
        ExitingWall = true;
        ExitWallTimer = ExitWallTime;

        Vector3 forceToApply = transform.up * ClimbJumpUpForce + FrontWallHit.normal * ClimbJumpBackForce;

        PlayerRigidBody.velocity = new Vector3(PlayerRigidBody.velocity.x, 0f, PlayerRigidBody.velocity.z);
        PlayerRigidBody.AddForce(forceToApply, ForceMode.Impulse);
    }
}
