using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    Image leftHealth, rightHealth, leftSpeed, rightSpeed;

    [SerializeField]
    TextMeshProUGUI healthText;


    public void UpdateHealth(float valueNormalized)
    {
        healthText.text = valueNormalized.ToString("P0");
        leftHealth.fillAmount = valueNormalized;
        rightHealth.fillAmount = valueNormalized;
    }

    public void UpdateSpeed(float valueNormalized)
    {
        leftSpeed.fillAmount = valueNormalized;
        rightSpeed.fillAmount = valueNormalized;
    }
}
