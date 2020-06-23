using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIndex
        {
            A, B, C, D, E, F
        }


        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIndex destination;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set!");
                yield break;
            }

            DontDestroyOnLoad(this.gameObject);

            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            print("New Scene Loaded");

            GameObject player = GameObject.FindGameObjectWithTag("Player");

            Portal otherPortal = GetOtherPortal();

            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            //playerPosition.position = otherPortal.spawnPoint.position;

            player.transform.rotation = otherPortal.spawnPoint.rotation;

            Destroy(this.gameObject);
        }
        
        private Portal GetOtherPortal()
        {
            Portal[] portals = GameObject.FindObjectsOfType<Portal>();
            foreach (Portal portal in portals)
            {
                if (portal == this)
                {
                    continue;
                }
                else if(destination == portal.destination)
                {
                    return portal;
                }
                
            }
            return null;
        }
    }
}


