using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace csci485.World
{
    public class CrystalPickups : MonoBehaviour
    {
        [SerializeField] private float respawnTime = 120f;
        [SerializeField] private int minAmount = 5; 
        [SerializeField] private int maxAmount = 21;
        private Vector2Int id;
        Planets.PlanetData.CrytalClump clump;

        private void Awake()
        {
            id = GetComponentInParent<Area>().GetCoordinates();
            if (Planets.PlanetData.CrystalClumps.TryGetValue(id, out clump))
            {
                if((clump.lastPickupTime + respawnTime) > Time.time)
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                clump = new Planets.PlanetData.CrytalClump();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag != "Player") return;

            Character.ResourceManagement player = collision.GetComponent<Character.ResourceManagement>();
            if(player != null)
            {
                player.addCrystal(Random.Range(minAmount, maxAmount));
                clump.lastPickupTime = Time.time;
                Planets.PlanetData.CrystalClumps[id] = clump;
                gameObject.SetActive(false);
            }
        }

    }
}



