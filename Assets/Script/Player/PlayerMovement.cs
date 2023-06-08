using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //public bl_Joystic js;

    public PlayerInput playerInput;
    public Rigidbody playerRigid;
    public PlayerThrow playerThrow;

    [Header("Movement Option")]
    [Tooltip("이동 속도")]
    public float moveSpeed = 1.0f;
    [Tooltip("회전 속도")]
    public float rotSpeed = 180f;

    public Animator anim;

    private Vector3 moveVec;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigid = GetComponent<Rigidbody>();
        playerThrow = GetComponent<PlayerThrow>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.getInstance().getGameState())
            return;
        Move();
        Rotate();

    }

    public void Rotate()
    {
        //float turn = playerInput.rotate * rotSpeed * Time.deltaTime;
        //playerRigid.rotation = playerRigid.rotation * Quaternion.Euler(0, -turn, 0 );
        if (moveVec.sqrMagnitude == 0)
        {
            playerRigid.velocity = Vector3.zero;
            playerRigid.angularVelocity = Vector3.zero;
            playerRigid.Sleep();
            return;
        }
        Quaternion dirQuat = Quaternion.LookRotation(moveVec);
        Quaternion moveQuat = Quaternion.Slerp(playerRigid.rotation, dirQuat, 0.3f);
        playerRigid.MoveRotation(moveQuat);

        //playerRigid.rotation = playerRigid.rotation * moveVec;
        //playerRigid.rotation = playerRigid.rotation * Quaternion.Euler(-90, 0,0);

    }
    public void Move()
    {
        moveVec = new Vector3(playerInput.rotate,0 , playerInput.move) * moveSpeed * Time.deltaTime;

        playerRigid.MovePosition(playerRigid.position + moveVec);
        if (playerInput.move >= 0)
            anim.SetFloat("move", playerInput.move);
        else if(playerInput.move <= 0)
            anim.SetFloat("move", playerInput.move * -1);   
        //anim.SetFloat("rotate", playerInput.rotate);
        //Debug.Log("move: " + playerInput.move.ToString());

    }
}
