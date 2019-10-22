using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class Cannon : NetworkBehaviour
{
    public float cooldown = 2f;
    private float nextAttack;
    public float firePower;
    public float fireAngle;
    public GameObject projectile;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && nextAttack < Time.time)
        {
            FireCannon();
        }
    }

    private void FireCannon()
    {
        nextAttack = Time.time + cooldown;
        CmdFire();
        Debug.Log("BOOM");
    }

    [Command]
    void CmdFire()
    {
        float up = Mathf.Cos(fireAngle) * firePower;
        float forward = Mathf.Sin(fireAngle) * firePower;
        Debug.Log("up" + up);
        Debug.Log("forward" + forward);
        GameObject cannonBall = Instantiate(projectile, transform.position + transform.right * -10, transform.rotation);
        cannonBall.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(- firePower, firePower/10, 0));
        NetworkServer.Spawn(cannonBall);
        Debug.Log("BOOM");
    }


}
