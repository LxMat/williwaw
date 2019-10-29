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
    private float up;
    private float forward;
    private float fireAngleRad;

    private Transform smallExplosion;
    private ParticleSystem explosion;

    // Update is called once per frame
    private void Start()
    {
        fireAngleRad = (fireAngle * Mathf.PI) / 180;
        up = Mathf.Sin(fireAngleRad) * firePower;
        forward = Mathf.Cos(fireAngleRad) * firePower;
        Debug.Log("up" + up);
        Debug.Log("forward" + forward);

        smallExplosion = transform.GetChild(8);
        explosion = smallExplosion.GetComponent<ParticleSystem>();
        explosion.Pause(true);

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            FireCannon();
        }

        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; ++i)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {

                    FireCannon();
                }
            }
        }
    }

    private void FireCannon()
    {
        if (nextAttack < Time.time)
        {
            nextAttack = Time.time + cooldown;
            CmdFire();
            explosion.Play();
            Debug.Log("BOOM");
        }
    }

    [Command]
    void CmdFire()
    {
        GameObject cannonBall = Instantiate(projectile, transform.position + transform.right * -10, transform.rotation);
        cannonBall.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(-forward, up, 0));
        NetworkServer.Spawn(cannonBall);
        Debug.Log("BOOM");
    }
}
