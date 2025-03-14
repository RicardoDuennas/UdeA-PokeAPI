using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    [SerializeField] private PokemonAPIManager _pokeAPIManager;
    [SerializeField] private InfoSideBarManager _infoSideBar;

    [Header("Movement Settings")]
    [SerializeField] private float _walkSpeed = 5f;
    private PlayerInputActions _playerInputActions;
    [SerializeField]  private float _gravity = -3.5f;

    [Header("Input")]
    private Vector2 moveInput;

    private bool isPaused = false;
    public AudioSource audioSource;

    // Methods to enable and disable input system accordingly
    private void OnEnable()
    {
        _playerInputActions.Player_Map.Enable();
    }
    private void OnDisable()
    {
        _playerInputActions.Player_Map.Disable();
    }

    void Awake(){
        _playerInputActions = new PlayerInputActions();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();        
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        InputManagement();
        PlayerRotation();
        PlayerMovement();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) 
    {
        string objectTag = hit.gameObject.tag;
        
        if (objectTag == "Pokemon")
        {
            string objectName = hit.gameObject.name;
            int id = hit.gameObject.GetComponent<PokemonObject>().id;
            
            PokemonObject pokemonObject = hit.gameObject.GetComponent<PokemonObject>();
            PokemonPool.Instance.ReleasePokemon(pokemonObject);
            string name = _pokeAPIManager.AddPokemonById(id);
            _infoSideBar.AddPokemonToList(name);
            GameManager.Instance.SendDataToSave();
            audioSource.Play();
        }   
    }

    private void InputManagement()
    {
        moveInput = _playerInputActions.Player_Map.Move.ReadValue<Vector2>();
    }

    private void PlayerMovement()
    {

        // Calculate movement according to rotation and WASD keys
        Vector3 tempRot = this.transform.eulerAngles;
        Vector3 move =  Quaternion.Euler(0, tempRot.y, 0)
            * new Vector3(0, 0, moveInput.y)
            * _walkSpeed * Time.deltaTime;
        
        // Apply gravity
        if (!controller.isGrounded)
        {
            move += new Vector3(0, _gravity, 0);
        }

        // Move controller
        controller.Move(move);
    }
    private void PlayerRotation()
    {

        // Apply player's rotation according to the mouse input 
        var rotateDirection = _playerInputActions.Player_Map.Rotate.ReadValue<float>();

        Vector3 rot = Vector3.up * Time.deltaTime * 30f * rotateDirection;
        this.transform.Rotate(rot);    
    }

    public void PauseGame()
    {
        isPaused = true;
        _playerInputActions.Player_Map.Disable();
        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        _playerInputActions.Player_Map.Enable();
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public bool IsGamePaused()
    {
        return isPaused;
    }
}
