using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudMovement : MonoBehaviour
{

    private Vector3 target;
    private WindController windController;
    private float speed = 1f;
    private Vector3 dir;
    private float pwr;
    // Start is called before the first frame update
    void Start()
    {
        windController = GameObject.Find("Wind").GetComponent<WindController>();
        updateTarget();

        StartCoroutine(moveCloud());
        speed = Random.Range(0.2f, 0.5f);
    }
    private void Update()
    {
        dir = windController.direction;
        pwr = windController.power;
        updateTarget();
    }
    void updateTarget()
    {
        target = new Vector3(dir.x * 1100, transform.position.y, dir.z * 1100);

    }
    IEnumerator moveCloud()
    {
        while (true)
        {
            float dist = Vector3.Distance(transform.position, target);

            //Magnus: I commented out this as in my mind clouds are no longer supposed to respawn back where they started. H
            //if (Vector3.Distance(transform.position, target) < 10f)
            //{
            //    transform.Translate(new Vector3(0, 0, -1100), Space.Self);
            //    updateTarget();
            //    speed = Random.Range(0.2f, 0.5f);
            //}

            if (transform.position.x < 0 ||
                transform.position.z < 0 ||
                transform.position.x > 1100 ||
                transform.position.z > 1100)
            {
                Destroy(gameObject);
            }


            transform.Translate(dir * speed * pwr, Space.Self);
            yield return null;
        }
    }
}
