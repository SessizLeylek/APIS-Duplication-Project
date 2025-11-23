using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    public Text creditsText;
    public Image whiteRadiance;
    public GameObject view;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void EndingStart()
    {
        animator.Play("ElevatorEnding");
        StartCoroutine(endingEnum());
    }

    IEnumerator endingEnum()
    {
        yield return new WaitForSeconds(6);

        float startTime = Time.time;
        while(Time.time - startTime < 6)
        {
            creditsText.color = new Color(1, 1, 1, (Time.time - startTime) / 6);
            yield return null;
        }

        yield return new WaitForSeconds(30);

        startTime = Time.time;
        while (Time.time - startTime < 6)
        {
            creditsText.color = new Color(1, 1, 1, (1 - Time.time + startTime) / 6);
            yield return null;
        }

        yield return new WaitForSeconds(6);

        startTime = Time.time;
        while (Time.time - startTime < 1)
        {
            whiteRadiance.color = new Color(1, 1, 1, (Time.time - startTime));
            yield return null;
        }

        Application.Quit();
    }

    public void ActivateView()
    {
        view.SetActive(true);
    }
}
