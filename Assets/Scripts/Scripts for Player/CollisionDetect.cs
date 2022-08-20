using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CollisionDetect : MonoBehaviour
{
    bool DetecCollision = false;

    public PlayerController PC;

    string otherMat;

    private void Start()
    {
        if (gameObject.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();

            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        StartCoroutine(WaitToDetect());   // WaitToDetect()
    }

    // FOr Obstacles
    private void OnTriggerEnter(Collider other)
    {
        if (DetecCollision == false || other.transform.CompareTag("Player"))
        {
            return;
        }
        else if(other.transform.CompareTag("Boost"))
        {
            TunnelScript.Instance.StopCooldown();

            if (TunnelScript.Instance.TunnelSpeed == TunnelScript.Instance.tunnelRealSpeed)
            {
                TunnelScript.Instance.TunnelSpeed = TunnelScript.Instance.tunnelRealSpeed * LevelManager.Instance.LevelSettings[LevelManager.PlayerLvl()].BoostMultiplier;
                ;
                AudioManager.Instance.BG_MusicSpeed(true);
            }

            return;
        }
        else if(other.transform.CompareTag("Obstacle"))
        {
            AudioManager.Instance.PlaySoundEffect(Sounds.ObstacleCollide);
            PlayerController.Instance.StartGame = false;
            StartCoroutine(WaitToRestartLevel());
            return;
        }

        if (other.transform.CompareTag("ColorObj"))
        {
            AudioManager.Instance.PlaySoundEffect(Sounds.GoodCollide);

            //  get text object
            if (other.transform.childCount > 0)
            {
                Transform canvas = other.transform.GetChild(0);
                Text TextGO = canvas.GetChild(0).GetComponent<Text>();

                otherMat = TextGO.text.Substring(0, 1);
            }
            else
            {
                // or get block
                MeshRenderer OtherMR = other.gameObject.GetComponent<MeshRenderer>();
                otherMat = OtherMR.material.name.Substring(0, 1);
            }

            PC.InsertBalls(otherMat);


            // move obstacle behind the view so Obstacle Handler can use it
            other.transform.parent.position = new Vector3(0, 0, -20);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Boost"))
        {
            TunnelScript.Instance.StartCooldown();
        }
    }

    IEnumerator WaitToDetect()
    {
        yield return new WaitForSeconds(1);

        DetecCollision = true;
    }
    IEnumerator WaitToRestartLevel()
    {
        AudioManager.Instance.BG_MusicSpeed(false);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(0);
    }
}
