using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class Explosions : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
            GameObject.Destroy(Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Asset/Models/Imported Models/Enemies/Boss2/Animations/ParticlSystems/ExplosionPS.prefab", typeof(GameObject)), collisionEvents[i].intersection, Quaternion.LookRotation(transform.up)), 8f);
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
