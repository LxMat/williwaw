using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICooldownController : MonoBehaviour
{
    public Image ImageCooldown;
    public float CooldownTime = 2.0f; //cooldowntime is assigned in Cannon script in Boat prefab, 
    private bool isCooldown;

    public void startCooldown()
    {
        //if cooldown is still running
        if (isCooldown)
        {
            return;
        }
        Debug.Log("start cooldown is called");
        isCooldown = true;
        ImageCooldown.fillAmount = 1;
    }

    void Update()
    {
        if (!isCooldown)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                ImageCooldown.fillAmount = 1;
                isCooldown = true;
            }
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    ImageCooldown.fillAmount = 1;
                    isCooldown = true;
                }
            }
        }


        if (isCooldown)
        {
            ImageCooldown.fillAmount -= 1 / CooldownTime * Time.deltaTime;
            Debug.Log(ImageCooldown.fillAmount);
        }
        isCooldown &= ImageCooldown.fillAmount != 0;
    }

}
