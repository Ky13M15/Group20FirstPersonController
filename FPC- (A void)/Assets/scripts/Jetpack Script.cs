using UnityEngine;
using UnityEngine.InputSystem;

public class JetpackScript : MonoBehaviour
{
    [Header("Jetpack Settings")]
    public float thrustForce = 15f;

    [Header("input settings")]
    public InputActionAsset inputActions;
    private InputAction jetpackAction;

    [Header("effects")]
    public ParticleSystem exhaustparticles;

    private Rigidbody playerRb;
    private bool isActive =false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        if (playerRb == null)
        {
            Debug.LogError("No Rigidbody found on parent(Player");
        }
        if(inputActions == null)
        {
            Debug.LogError("assign an input Action Asset to the input actions field!");
            return;
        }
        jetpackAction == inputActions.FindActionMap("Player").FindAction("Jetpack");
        if (jetpackAction == null)
        {
            Debug.LogError("Could not find 'Jetpack'in 'player' map. ");
            return;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
