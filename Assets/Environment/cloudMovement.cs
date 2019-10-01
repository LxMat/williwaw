using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudMovement : MonoBehaviour
{

    private Vector3 target;
    private float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        updateTarget();
        StartCoroutine(moveCloud());
        speed = Random.Range(0.2f, 0.5f);
    }
    void updateTarget()
    {
        Vector3 pos = transform.position;
        target = new Vector3(pos.x, pos.y, 1100);

    }
    IEnumerator moveCloud()
    {
        while (true)
        {
            float dist = Vector3.Distance(transform.position, target);
            if (Vector3.Distance(transform.position, target) < 10f)
            {
                transform.Translate(new Vector3(0, 0, -1100), Space.Self);
                updateTarget();
                speed = Random.Range(0.2f, 0.5f);
            }
            transform.Translate(new Vector3(0, 0, 1)*speed,Space.Self);
            yield return null;
        }
    }
}
