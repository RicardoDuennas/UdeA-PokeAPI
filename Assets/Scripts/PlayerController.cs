using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("References")]
    private CharacterController controller;

    [Header("Movement Settings")]
    [SerializeField] 
    private float walkSpeed = 5f;
    private PlayerInputActions playerInputActions;

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

        Vector3 tempRot = this.transform.eulerAngles;
        Vector3 move =  Quaternion.Euler(0, tempRot.y,0)
            * new Vector3(0, 0, moveInput.y)
            * walkSpeed * Time.deltaTime;
        
        controller.Move(move);
    }
    private void PlayerRotation(){

        var rotateDirection = playerInputActions.Player_Map.Rotate.ReadValue<float>();

        Vector3 rot = Vector3.up * Time.deltaTime * 30f * rotateDirection;
        this.transform.Rotate(rot);    
    }
}
