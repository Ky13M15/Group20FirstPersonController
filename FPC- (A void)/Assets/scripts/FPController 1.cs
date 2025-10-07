using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPController : MonoBehaviour
{
    private InputActionAsset interactAction;
    public Camera playerCamera;
    public float normalFOV = 60f;
    public float aimFOV = 40f;
    public float aimSpeed = 10f;
   
    [SerializeField] private float interactRange = 3f;
    [SerializeField] LayerMask interactLayer;
    
    [SerializeField] float jetpackForce = 10f;
    [SerializeField] float fuel = 100f;

    [SerializeField] int ammoInClip = 10;
    [SerializeField] int maxAmmo = 24;
    [SerializeField] float reloadTime = 1.5f;
    private bool isReloading = false;
    



    private bool isAiming = false;

    //aiming event callback
    public event Action<bool> onAimChanged;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;


    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform gunPoint;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public float crouchSpeed = 2.5f;
    private float originalMoveSpeed;

    [Header("Pickup Settings")]
    public float pickupRange = 3f;
    public Transform holdPoint;
    private PickupObject heldObject;
    public TMP_Text pickupText;


    [Header("Throwing settings")]
    public float throwForce = 10f;
    public float throwUpwardBoost = 1f;

    [Header("Sprint settings")]
    [SerializeField] private float sprintMultiplier;
    private bool isSprinting=false;




    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        originalMoveSpeed = moveSpeed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerInput playerInput = GetComponent<PlayerInput>();
        interactAction = playerInput.actions;
        playerCamera = Camera.main;

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }



    }
    private void Update()
    {
        HandleMovement();
        HandleLook();

        if (heldObject != null)
        {
            heldObject.MoveToHoldPoint(holdPoint.position);
        }
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            PickupObject pickup = hit.collider.GetComponent<PickupObject>();
            if (pickup != null)
            {
                pickupText.text = pickup.gameObject.name;
                return;
            }
        }
        //clear text if not looking at a pickup


        float targetFOV = isAiming ? aimFOV : normalFOV;
        float newFOV = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * aimSpeed);


    }







    public void OnMove(InputAction.CallbackContext context) => moveInput = context.ReadValue<Vector2>();
    public void OnPickup(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (heldObject == null)
        {
            Ray ray = new(cameraTransform.position, cameraTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
            {
                if (hit.collider.GetComponent<IInteractable>() != null)
                {
                    pickupText.text = "Press E to interact";
                }
                else
                {
                    pickupText.text = "";
                }
            }

            {
                PickupObject pickUp = hit.collider.GetComponent<PickupObject>();
                if (pickUp != null)
                {
                    pickUp.Pickup(holdPoint);
                    heldObject = pickUp;

                }
            }
        }
        else
        {
            heldObject.Drop();
            heldObject = null;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

    }
    public void OnThrow(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (heldObject == null) return;

        Vector3 dir = cameraTransform.forward;
        Vector3 implulse = dir * throwForce + Vector3.up * throwUpwardBoost;

        heldObject.Throw(implulse);
        heldObject = null;

    }
    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isAiming = true;

            onAimChanged?.Invoke(true);
        }
        else if (context.canceled)
        {
            isAiming = false;
            onAimChanged?.Invoke(false);
        }


    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed) return;
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
            else
            {
                Debug.Log("interacted with:" + hit.collider.name);
            }
        }

        else
        {
            Debug.Log("Nothing to interact with");
        }
    }


    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Shoot();
        }
    }

    public void OnLook(InputAction.CallbackContext context) => lookInput = context.ReadValue<Vector2>();
    private void Shoot()
    {
        if (bulletPrefab != null && gunPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(gunPoint.forward * 1000f); //Adjust force value as needed
                Destroy(bullet, 3);// Delete the bullet from the scene after 3 seconds
            }

        }
    }
        public void OnSprint(InputAction.CallbackContext context) {
        if (context.performed)
        {
            isSprinting = true;
        }
        else if (context.canceled)
        {
            isSprinting = false;
        }

    }
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            controller.height = crouchHeight;
            moveSpeed = originalMoveSpeed;
        }
        else if (context.canceled)
        {
            controller.height = standHeight;
            moveSpeed = originalMoveSpeed;
        }
    }

    public void HandleMovement()
    {
        float currentSpeed = moveSpeed * (isSprinting ? sprintMultiplier: 1f);
        Vector3 move = transform.right * moveInput.x + transform.forward *
        moveInput.y;
        controller.Move(moveSpeed * Time.deltaTime * move);
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    public void HandleLook()
    {
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit,
        verticalLookLimit);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    public void HandleJetpack()
    {
        if (Keyboard.current.spaceKey.isPressed && fuel > 0)
        {
            controller.Move(Vector3.up * jetpackForce * Time.deltaTime);
            fuel -= Time.deltaTime * 5;
        }
        else
        {
            fuel = Math.Min(fuel + Time.deltaTime * 2, 100);
        }

    }
}
