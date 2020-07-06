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
        [SerializeField] float FadeOutTime = 1f;
        [SerializeField] float FadeInTime = 0.5f;
        [SerializeField] float fadeWaitTime = 1f;

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

            //Fade out 
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(FadeOutTime);

            //Save current level
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save();

            //Load new scene
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            //Load current level
            wrapper.Load();

            //Set player position
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            wrapper.Save();

            //Fade In
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(FadeInTime);

            Destroy(this.gameObject);
        }

        private static void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
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


