using System.ComponentModel.Design;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Movement", menuName = "Scriptable Objects/Player Movement")]
public class PlayerMovementSO : ScriptableObject
{
    [Header("Gravity")]
    [HideInInspector] public float gravityStrength; //Downwards force (gravity) needed for the desired jumpHeight and jumpTimeToApex.
    [HideInInspector] public float gravityScale; //Strength of the player's gravity as a multiplier of gravity (set in ProjectSettings/Physics2D).
                                                 //Also the value the player's rigidbody2D.gravityScale is set to.
    [Space(5)]
    public float plungingGravityMult=4.5f; // Plunge Attack
    public float fallGravityMult=1.5f; //Multiplier to the player's gravityScale when falling.
    public float maxFallSpeed=45f; //Maximum fall speed (terminal velocity) of the player when falling.
    [Space(5)]


    [Space(20)]

    [Header("Run")]
    public float maxMoveSpeed = 9f; //Target speed we want the player to reach.

    [Header("Sprint")]
    public float maxSprintSpeed=12f; //Target speed we want the player to reach while sprinting.
  

    [Space(20)]

    [Header("Jump")]
    public float jumpHeight=6.5f; //Height of the player's jump
    public float jumpTimeToApex=0.5f; //Time between applying the jump force and reaching the desired jump height. These values also control the player's gravity and jump force.
    [HideInInspector] public float jumpForce; //The actual force applied (upwards) to the player when they jump.

    [Header("Both Jumps")]
    public float jumpCutGravityMult =2.5f; //Multiplier to increase gravity if the player releases thje jump button while still jumping

    [Space(20)]

    [Header("Assists")]
    [Range(0.01f, 0.5f)] public float coyoteTime=0.2f; //Grace period after falling off a platform, where you can still jump
    [Range(0.01f, 0.5f)] public float jumpInputBufferTime=0.2f; //Grace period after pressing jump where a jump will be automatically performed once the requirements (eg. being grounded) are met.

    [Space(20)]

    [Header("Dash")]
    
   [HideInInspector] public float dashSpeed;
    public float dashDistance = 4f;
    public float dashTime=0.2f;
    [Space(5)]
    public float dashCooldownTime=0.5f;
    [Space(5)]
    [Range(0.01f, 0.5f)] public float dashInputBufferTime=0.2f;

    //Unity Callback, called when the inspector updates
    private void OnValidate()
    {
        //Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

        //Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
        gravityScale = gravityStrength / Physics2D.gravity.y;
        //Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;

        dashSpeed = dashDistance / dashTime;

       
    }
}
