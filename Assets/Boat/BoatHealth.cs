using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class BoatHealth : NetworkBehaviour
{
    public int health = 1;

    private void KillPlayer()
    {
        Destroy(gameObject);
        Debug.Log("You are dead");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Enemy")
        {
            health--;
            Debug.Log("Health left: " + health);
            if (health == 0)
            {
                KillPlayer();
            }
        }
    }
}