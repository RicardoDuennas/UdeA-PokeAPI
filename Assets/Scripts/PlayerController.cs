using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("References")]
    private CharacterController controller;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    private PlayerInputActions playerInputActions;
    [SerializeField]  private float gravity = -3.5f;

    [Header("Input")]
    private Vector2 moveInput;



    private void OnEnable(){
        playerInputActions.Player_Map.Enable();
    }
    private void OnDisable(){
        playerInputActions.Player_Map.Disable();
    }

    void Awake(){
        playerInputActions = new PlayerInputActions();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();    
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        InputManagement();
        PlayerRotation();
        PlayerMovement();
    }

    private void InputManagement(){
        moveInput = playerInputActions.Player_Map.Move.ReadValue<Vector2>();
    }

    private void PlayerMovement(){

        // Calculate movement according to rotation and WASD keys
        Vector3 tempRot = this.transform.eulerAngles;
        Vector3 move =  Quaternion.Euler(0, tempRot.y, 0)
            * new Vector3(0, 0, moveInput.y)
            * walkSpeed * Time.deltaTime;
        
        // Apply gravity
        if (!controller.isGrounded){
            move += new Vector3(0, gravity, 0);
        }

        // Move controller
        controller.Move(move);
    }
    private void PlayerRotation(){

        // Apply player's rotation according to the mouse input 
        var rotateDirection = playerInputActions.Player_Map.Rotate.ReadValue<float>();

        Vector3 rot = Vector3.up * Time.deltaTime * 30f * rotateDirection;
        this.transform.Rotate(rot);    
    }
}
