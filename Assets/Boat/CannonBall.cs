using UnityEngine;

public class CannonBall : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -2)
        {
            Destroy(gameObject);
        }
    }
}
