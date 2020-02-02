using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderManager : MonoBehaviour
{
    Quaternion plus = Quaternion.Euler(90f, 0, 0);
    Quaternion plusSmaller = Quaternion.Euler(90.00001f, 0, 0);
    Quaternion minus = Quaternion.Euler(-90f, 0, 0);
    
    public List<GameObject> numbers;
    float defaultSpeed = 0.5f;
    private int currentNumber = 0;

    public int rotateToNumber = 7; 

    private void Start()
    {
        initClocks();
    }

    private void initClocks()
    { 
        foreach (GameObject n in numbers)
        {
            n.transform.localRotation = minus;
            n.SetActive(false);
        }
        numbers[0].SetActive(true);
        numbers[1].SetActive(true);
        currentNumber = 0;
    }

    [MyBox.ButtonMethod]
    public void RotateNext()
    {
        RotateNext(defaultSpeed);
    }

    public void RotateNext(float speed)
    {
        switch(currentNumber)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:        
                {
                    numbers[currentNumber+2].transform.localRotation = minus;
                    numbers[currentNumber + 2].SetActive(true);
                    StartCoroutine(RotateFromTo(numbers[currentNumber + 1], minus, plus, numbers[currentNumber], speed));
                    currentNumber = (currentNumber + 1)%10;
                    
                }

                break;
            case 8:
                {
                    numbers[0].transform.localRotation = plusSmaller;
                    numbers[0].SetActive(true);
                    StartCoroutine(RotateFromTo(numbers[9], minus, plus, numbers[currentNumber], speed));
                    currentNumber = 9;

                }
                break;
            case 9:
                {
                    numbers[1].transform.localRotation = minus;
                    numbers[1].SetActive(true);
                    numbers[0].transform.localRotation = plusSmaller;
                    StartCoroutine(RotateFromTo(numbers[0], plusSmaller, minus, numbers[currentNumber], speed));
                    currentNumber = 0;

                }
                break;

            default:
                break;
        }
    }



    private IEnumerator RotateFromTo(GameObject go,Quaternion from, Quaternion to, GameObject objectToDisable, float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            go.transform.localRotation = Quaternion.Lerp(from, to, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            if ((elapsedTime / time) > 0.98f)
            {

                go.transform.localRotation = to;
                objectToDisable.SetActive(false);
               
            }

            yield return null;
        }
    }
    [MyBox.ButtonMethod]
   public void RotateToTargetNumber()
    {
        RotateToTargetNumber(rotateToNumber);
    }

    public void RotateToTargetNumber(int number)
    {
        if (currentNumber != number)
            StartCoroutine(rotate(number));
    }

    private IEnumerator rotate(int number)
    {
        RotateNext(0.4f);
         yield return new WaitForSeconds(0.5f);

        if(currentNumber != number)
        StartCoroutine(rotate(number));
        
    }





}
