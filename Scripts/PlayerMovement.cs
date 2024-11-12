using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    [SerializeField] GameObject Lanterna;
    [SerializeField] GameObject Telecomanda;
    [SerializeField] GameObject Lumina_Auntru;
    [SerializeField] GameObject Lumina_Afara;
    bool hasTelecomanda;
    bool hasLanterna;
    public string textValue;
    public TextMeshProUGUI textElement;

    Rigidbody rb;

    private WalkSoundController walkSoundController;
            
    // Start is called before the first frame update

    private void Awake()
    {
        walkSoundController = GetComponentInChildren<WalkSoundController>();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        textValue = "Watch TV";
        
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");



        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);

        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        if (rb.velocity.x != 0f || rb.velocity.y != 0f)
        {
            walkSoundController.StartWalking();
        }
        else
        {
            walkSoundController.StopWalking();
        }

    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity
        if(flatVel.magnitude > moveSpeed) { 
        Vector3 limitedVel = flatVel.normalized * moveSpeed;
        rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    public void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        textElement.text = "Current Objective: " + textValue;
    }

    // Update is called once per frame
    void Update()
    {
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f);

        MyInput();
        SpeedControl();

        //handle drag

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        // Check if the player is walking or not and call the appropriate methods
        

    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (other.gameObject.tag == "Telc")
            {
                hasTelecomanda = true;
                Telecomanda.gameObject.SetActive(true);
                Destroy(other.gameObject);
                textValue = "Turn On TV";
            }
            if (other.gameObject.tag == "Respawn" && hasTelecomanda)
            {
                textValue = "Get Your Flashlight";
            }
            if (other.gameObject.tag == "Lant" && Lumina_Afara.gameObject.activeSelf)
            {
                hasLanterna = true;
                Lanterna.gameObject.SetActive(true);
                Destroy(other.gameObject);
                Lumina_Afara.gameObject.SetActive(false);
                textValue = "Go to sleep when ready";
            }
            if (other.gameObject.tag == "Pat" && hasLanterna)
            {
                {
                    moveSpeed = 0;
                    Lanterna.gameObject.SetActive (false);
                    Telecomanda.SetActive(false);
                    textValue = "End";
                }
            }
        }
    }
}
