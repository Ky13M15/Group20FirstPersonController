using UnityEngine;
using UnityEngine.InputSystem.XR;
using static DaynNite;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 3f;
    public float playerDistance;
    public float minDistance, maxDistance;
    public CharacterController controller;
    public DaynNite daynNiteScript;

    private Transform player;


    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;


    }
    void Update()
    {

        playerDistance = (this.transform.position - player.position).magnitude;
        if (playerDistance <= minDistance)
        {
            PlayerFollow();
        }
        else if (playerDistance >= maxDistance)
        {
            Debug.Log("Player is too far");
        }

        
    }

    public void PlayerFollow()
    {

        if ((player != null) && (daynNiteScript.timeOfDay == TimeOfDay.Nite))
        {

            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            direction = direction.normalized;

            controller.Move(direction * speed * Time.deltaTime);

            Debug.Log("Following");
        }

    }


}
