using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UICooldownController : MonoBehaviour
{
    // Start is called before the first frame update
    public Image ImageCooldown;
    public float CooldownTime = 2.5f;
    bool isCooldown;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.B) && !isCooldown)
        {
            isCooldown = true;
            ImageCooldown.fillAmount = 1;
        }
        if (isCooldown)
        {
            Debug.Log("test");
            ImageCooldown.fillAmount -= 1 / CooldownTime * Time.deltaTime;
        }
        if(ImageCooldown.fillAmount == 0)
        {
            isCooldown = false;
        }
    }

}
