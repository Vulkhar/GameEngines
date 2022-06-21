using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

/// <summary>
/// Dieses Skript wechselt das Target der Camera um zwischen verschiedenen Charakteren wechseln zu k�nnen
/// Zus�tzlich schaltet es das Controller Skript der inaktiven Charakter aus damit sie nicht umher laufen
/// 
/// 1 in der Liste zur�ck
/// 2 in der Liste vor
/// 
/// 3 timescale runter
/// 4 timescale hoch
/// 
/// R Scene neu laden
/// </summary>
public class ShowcaseController : MonoBehaviour
{
    public int currentSpiderIndex = 0;
    public float timeScaleMultiplier = 0.1f;
    public Controller[] spiders;
    public CinemachineFreeLook cinemachineFreeLook;

    private void Start()
    {
        ChangeToSpider(currentSpiderIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeToSpider(currentSpiderIndex - 1);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeToSpider(currentSpiderIndex + 1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Time.timeScale -= timeScaleMultiplier;
            Debug.Log("TimeScale =" + Time.timeScale);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Time.timeScale += timeScaleMultiplier;
            Debug.Log("TimeScale =" + Time.timeScale);
        }

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ChangeToSpider(int index)
    {
        if (index > spiders.Length-1)
            currentSpiderIndex = 0;
        else if (index < 0)
            currentSpiderIndex = spiders.Length-1;
        else
            currentSpiderIndex = index;

        for (int i = 0; i < spiders.Length; i++)
            spiders[i].enabled = i == currentSpiderIndex ? true : false;

        cinemachineFreeLook.Follow = spiders[currentSpiderIndex].gameObject.transform;
        cinemachineFreeLook.LookAt = spiders[currentSpiderIndex].gameObject.transform;
    }
}
