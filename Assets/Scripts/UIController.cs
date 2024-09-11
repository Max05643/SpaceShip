using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/// <summary>
/// Controls the UI
/// </summary>
public class UIController : MonoBehaviour
{
    [SerializeField]
    Image leftHealth, rightHealth, leftSpeed, rightSpeed;

    [SerializeField]
    TextMeshProUGUI healthText, coinsText, endGameText;

    [SerializeField]
    CanvasGroup mainUI, endGameUI, tutorialUI;

    Sequence currentAnimation = null;
    void Start()
    {
        currentAnimation = DOTween.Sequence();
        currentAnimation.Append(mainUI.DOFade(1, 1).SetDelay(0.5f));
    }

    /// <summary>
    /// Hides the tutorial window
    /// </summary>

    public void HideTutorial()
    {
        tutorialUI.DOFade(0, 0.5f);
    }

    /// <summary>
    /// Displays the coins count
    /// </summary>
    public void DisplayCoinsCount(int? coinsCount)
    {
        coinsText.text = coinsCount.HasValue ? coinsCount.ToString() : string.Empty;
    }

    /// <summary>
    /// Updates the health bar
    /// </summary>
    public void UpdateHealth(float valueNormalized)
    {
        healthText.text = valueNormalized.ToString("P0");
        leftHealth.fillAmount = valueNormalized;
        rightHealth.fillAmount = valueNormalized;
    }


    /// <summary>
    /// Updates the speed bar
    /// </summary>
    public void UpdateSpeed(float valueNormalized)
    {
        leftSpeed.fillAmount = valueNormalized;
        rightSpeed.fillAmount = valueNormalized;
    }

    /// <summary>
    /// Shows the game over screen with specified message
    /// </summary>

    public void ShowGameOver(string message)
    {
        currentAnimation?.Kill(true);
        currentAnimation = DOTween.Sequence();
        currentAnimation.Append(mainUI.DOFade(0, 1));
        currentAnimation.Append(endGameUI.DOFade(1, 1));

        endGameText.text = message;
    }

    void OnDestroy()
    {
        currentAnimation?.Kill();
    }
}
