using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    public float airMultiplier;

    Vector3 moveDirection;

    public Transform orientation;
    private Rigidbody rb;
    public int moveSpeed;
    public float rotationSpeed;

    public float JumpPower;
    public ParticleSystem Dust;

    public Transform playerObj;
    public Transform cam;
    float horizontalInput;
    float verticalInput;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
    public float groundDrag;

    public Animator anim;
    public float TimePerDust = 0.2f;
    public bool dusted;

    public enum State
    {
        OnGround,
        InAir,
        Dead
    }

    public State currentState;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        rb = gameObject.GetComponent<Rigidbody>();
        currentState = State.OnGround;
    }

    private void FixedUpdate()
    {
        Movement();
        Look();

    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        
        if (verticalInput > 0 && dusted == false && grounded)
        {
            StartCoroutine(LoadDust());
        }

        else if (horizontalInput > 0 && dusted == false && grounded)
        {
            StartCoroutine(LoadDust());
        }

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (grounded)
        {
           anim.SetBool("Jump", false);
           rb.drag = groundDrag;
        }
        else
        {
            anim.SetBool("Jump", true);
           rb.drag = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(LoadDustFaster());
            moveSpeed += 2;
            dusted = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Dust.Play();
            moveSpeed -= 2;
            dusted = false;
        }

        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            anim.SetBool("Jump", true);
            rb.velocity = new Vector3(rb.velocity.x, JumpPower, rb.velocity.z);
        }
    }

    void Look()
    {
        Vector3 direction = new Vector3(horizontalInput, 0f ,verticalInput).normalized;
        if (direction != Vector3.zero)
        {
        playerObj.forward = Vector3.Slerp(playerObj.forward, direction.normalized, Time.deltaTime * rotationSpeed);
        }

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

    }

    void Movement()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        

        // on ground
        if(grounded)
        {
            currentState = State.OnGround;
            rb.AddForce(moveDirection * moveSpeed * 5f, ForceMode.Force);
        }

        // in air
        else if(!grounded)
        {
            currentState = State.InAir;
            rb.AddForce(moveDirection * moveSpeed * 5f * airMultiplier, ForceMode.Force);
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            anim.SetBool("Jump", true);
            rb.velocity = new Vector3(rb.velocity.x, JumpPower, rb.velocity.z);
        }
                    
    }

    private IEnumerator LoadDust()
    {
        Dust.Play();
        dusted = true;
        yield return new WaitForSeconds(TimePerDust / 4); 
        dusted = false;
        Dust.Play();
    }

    private IEnumerator LoadDustFaster()
    {
        Dust.Play();
        yield return new WaitForSeconds(TimePerDust / 8); 
        Dust.Play();
    }
}
