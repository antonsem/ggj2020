using UnityEngine;

public class ScoreBoard : MonoBehaviour
{


    public int score = 123;

    public CylinderManager first;
    public CylinderManager second;
    public CylinderManager third;
    public CylinderManager fourth;


    private void OnEnable()
    {
        GameManager.SetGameScore += setScore;
        GameManager.SetGameEnabled += SetPlaying;
    }

    private void OnDisable()
    {
        GameManager.SetGameScore -= setScore;
        GameManager.SetGameEnabled -= SetPlaying;
    }

    public void SetPlaying(bool state)
    {
        if (state)
            setScore(0);
    }

    public void setScore(int score)
    {
        Debug.Log(score % 10 + " second: " + (score / 10) % 10 + " third " + (score / 100) % 10 + " fourth " + (score / 1000) % 10);
        fourth.RotateToTargetNumber(score % 10);
        third.RotateToTargetNumber((score / 10) % 10);
        second.RotateToTargetNumber((score / 100) % 10);
        first.RotateToTargetNumber((score / 1000) % 10);
    }
    [MyBox.ButtonMethod]
    public void setScore()
    {
        setScore(score);
    }

}
