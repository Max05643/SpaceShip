using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [SerializeField]
    Image leftHealth, rightHealth, leftSpeed, rightSpeed;

    [SerializeField]
    TextMeshProUGUI healthText, coinsText, endGameText;

    [SerializeField]
    CanvasGroup mainUI, endGameUI;

    Sequence currentAnimation = null;
    void Start()
    {
        currentAnimation = DOTween.Sequence();
        currentAnimation.Append(mainUI.DOFade(1, 1).SetDelay(0.5f));
    }

    public void DisplayCoinsCount(int? coinsCount)
    {
        coinsText.text = coinsCount.HasValue ? coinsCount.ToString() : string.Empty;
    }

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

    public void ShowGameOver(string message)
    {
        currentAnimation?.Kill(true);
        currentAnimation = DOTween.Sequence();
        currentAnimation.Append(mainUI.DOFade(0, 1));
        currentAnimation.Append(endGameUI.DOFade(1, 1));

        endGameText.text = message;
    }
}
