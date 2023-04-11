using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public GameObject player;
    public int counter;
    public float moveSpeed;
    public Seeker seeker;
    public PlayerController playerCon;
    public bool gotStag;
    public bool isLaunching;

    public void Punched()
    {
        seeker.maxSpeed = 0;
        Debug.Log(player.GetComponent<PlayerController>().attackCounter);
        if(player.GetComponent<PlayerController>().attackCounter == 1 || player.GetComponent<PlayerController>().attackCounter == 2)
        {
            StartCoroutine(Staggered());
            Invoke("FeelingBetter", 0.2f);
        } else if (player.GetComponent<PlayerController>().attackCounter > 2)
        {
            StartCoroutine(Pow());
            isLaunching = true;
            Invoke("FeelingBetter", 0.3f);
        }
        health--;
    } 

    public void FeelingBetter()
    {
        if(health <= 0)
        {
            Destroy(this.gameObject);
        } else if(gotStag == false)
        {
            isLaunching = false;
            seeker.maxSpeed = 2f;
        }
    }

    IEnumerator Staggered()
    {
        for (float alpha = 1f; alpha >= 0; alpha -= 0.2f)
        {
            transform.Translate(player.transform.forward / 12, Space.World);

            yield return new WaitForSeconds(.02f);
        }
    }

    IEnumerator Pow()
    {
        for (float alpha = 1f; alpha >= 0; alpha -= 0.2f)
        {
            transform.Translate(player.transform.forward / 1.5f, Space.World);

            yield return new WaitForSeconds(.02f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerCon.GotHit();
        }
        if (other.tag == "Wall" && isLaunching == true)
        {
            Debug.Log("STUN!");
            gotStag = true;
            isLaunching = false;
            Invoke("NoStag", 0.3f);
        }
        else if (other.tag == "DeathWall" && isLaunching == true)
        {
            health = 0;
            FeelingBetter();
        }
    }

    public void NoStag()
    {
        gotStag = false;
        FeelingBetter();
    }
}
