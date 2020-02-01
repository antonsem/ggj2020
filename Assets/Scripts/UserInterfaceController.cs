using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserInterfaceController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private TextMeshProUGUI _Items;
    [SerializeField] private TextMeshProUGUI _health;

    private void Update()
    {
        UpdateTimer();
        UpdateItems();
        UpdateHealth();
    }

    private void UpdateTimer()
    {
        var gameTime = Mathf.RoundToInt(gameManager.GameTime).ToString();

        if (gameTime.Length < 2)
        {
            gameTime = "0" + gameTime;
        }

        _timer.text = gameTime;
    }

    private void UpdateItems()
    {
        var breakablesCount = gameManager.BreakablesCount;
        var breakablesCountAlive = breakablesCount - gameManager.InstanceDeathCount;

        var text = breakablesCountAlive + "/" + breakablesCount;

        _Items.text = text;
    }

    private void UpdateHealth()
    {
        var health = gameManager.Health;

        var text = string.Empty;
        text += health >= .2f ? "*" : "-";
        text += health >= .4f ? "*" : "-";
        text += health >= .6f ? "*" : "-";
        text += health >= .8f ? "*" : "-";
        text += health >= 1f ? "*" : "-";

        _health.text = text;
    }
}
