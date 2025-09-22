using UnityEngine;
using UnityEngine.InputSystem.XR;
using static DaynNite;

public class EnemyFollow : MonoBehaviour
{
   public float speed = 3f; // Movement speed of the enemy
   private Transform player;
    public float playerDistance;
    public float minDistance,maxDistance;
    public CharacterController controller;

    public DaynNite daynNiteScript;

   void Start()
        {
            // Find the player by tag
            player = GameObject.FindGameObjectWithTag("Player").transform;

        
        }
   void Update()
    {

        playerDistance = (this.transform.position - player.position).magnitude;
        if (playerDistance <= minDistance )
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
            // Calculate direction towards the player
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            direction = direction.normalized;
            // Move enemy towards the player
            //transform.position += direction * speed * Time.deltaTime;
            controller.Move(direction * speed * Time.deltaTime);

            Debug.Log("Following");
        }
    }

}
