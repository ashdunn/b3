using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    private Animator anim;
    private Rigidbody rb;
    CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;

    public float MoveSpeed = 3;
    public float RotateSpeed = 1;
    public float JumpSpeed = 2;
    //public float JumpHeight = 10;

    int nrOfAlowedDJumps = 1; // New vairable
    int dJumpCounter = 0;

    bool canJump = false;

    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (anim == null)
            return;

        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        Move(x, y);
    }

    private void Move(float x, float y)
    {
        anim.SetFloat("velx", x);
        anim.SetFloat("vely", y);

        moveDirection = new Vector3(0.0f, 0.0f, Input.GetAxis("Vertical"));
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //print("fdjslkfjakfj");
            moveDirection.y = JumpSpeed * Time.deltaTime;
            anim.SetFloat("Jump4Blend", 1);
            rb.AddForce(0, 3, 0);
        }
        else anim.SetFloat("Jump4Blend", 0);
        
        if (moveDirection != Vector3.zero)
            transform.TransformDirection(Vector3.forward);
        transform.Rotate(0, x * RotateSpeed, 0);

        

        /*
        if (timer > 0.1f)
        {
            timer = 0;
            canJump = false;
            anim.SetFloat("Jump4Blend", 0);
        }
        
        if (Input.GetButtonDown("Jump") && rb.position.y < .2)
        {
            moveDirection.y = JumpSpeed * Time.deltaTime;
            anim.SetFloat("Jump4Blend", 1);
            rb.AddForce(0, 10000, 0);
            timer = 0;
            canJump = true;
        }
        else if(timer < 0.1f)
        {
            timer += Time.deltaTime;
            rb.AddForce(0, 10000, 0);
        }*/
           
        moveDirection.y -= 20.0f * Time.deltaTime;


        controller.Move(moveDirection * Time.deltaTime);


        /*
        //transform.position += Vector3.forward * MaxSpeed * y * Time.deltaTime;

        Vector3 forward = transform.TransformDirection(Vector3.forward);

        float curSpeed = MaxSpeed * y;
        controller.Move(forward * curSpeed);
        transform.Rotate(0, x * RotateSpeed, 0);

        anim.SetFloat("AngSpeed", x * RotateSpeed);*/


    }
}
