using TMPro;
using UnityEngine;

public class ScoreboardController : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;

    public void Start()
    {
        textMeshPro.text = string.Empty;
    }

    private void OnEnable()
    {
        GameManager.SetGameEnabled += OnSetGameEnabled;
        GameManager.SetGameScore += OnSetGameScore;
    }

    private void OnDisable()
    {
        GameManager.SetGameEnabled -= OnSetGameEnabled;
        GameManager.SetGameScore -= OnSetGameScore;
    }

    private void OnSetGameEnabled(bool value)
    {
        if (value)
        {
            textMeshPro.text = "0";
        }
    }

    private void OnSetGameScore(int score)
    {
        textMeshPro.text = score.ToString();
    }
}
