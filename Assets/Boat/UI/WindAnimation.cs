using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WindAnimation : MonoBehaviour
{
    private Image windIcon;
    private bool imageFilled;
    public float animationSpeed = 3;
    // Start is called before the first frame update
    void Start()
    {
        windIcon = gameObject.GetComponent<Image>();
        windIcon.fillAmount = 1;
        imageFilled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (imageFilled)
        {
            windIcon.fillAmount = 0;
            imageFilled = false;
        }else
        {
            windIcon.fillAmount += 1 / animationSpeed * Time.deltaTime;
        }
        if( windIcon.fillAmount >= 1)
        {
            imageFilled = true;
        }
        
    }
}
