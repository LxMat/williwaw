using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogWarning("No animation set for "+ gameObject.name);
        }

    }
    public void enable()
    {
        anim.enabled = true;
    }
    public void disable()
    {
        anim.enabled = false;
    }
    public void boatDead()
    {
        enable();
        anim.SetTrigger("deathTrigger");
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Active triggered");
            boatDead();
        }
    }
}
