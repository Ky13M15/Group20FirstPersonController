using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

//Title: How to make a METEOR SHOWER for your game - Unity - Blender - Visual effects
//Author: Stick Wizard
//Date: 26 May 2023
//Availability: https://www.youtube.com/watch?v=VJdGo9tlLAI
public class Explosions : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;


    private void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        int i = 0;

        while (i < numCollisionEvents)
        {
            GameObject.Destroy(Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Asset/Prefabs/Explosions.prefab", typeof(GameObject)), collisionEvents[i].intersection, Quaternion.LookRotation(transform.up)), 8f);
            //FindObjectOfType<AudioManager>().Play("");

            /*Collider[] HitByBomb = Physics.OverlapSphere(collisionEvents[i].intersection, 60f);
            foreach (Collider c in HitByBomb)
            {
                if (c.transform.name == "StickmanWizard" && !Player.Invincible)
                {
                    ObjectInteraction.PlayerTakeDamage();
                }*/
            }

            ++i;
        }
    }
