using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LaunchingController : MonoBehaviour
{
    [SerializeField] private PanelManager panelManager;
    [SerializeField] private Text countdownText;
    private IEnumerator _launchCoroutine;
    private AsyncOperation _questSceneAsync;

    // Now just with arena quest type
    public void StartLaunching()
    {
        panelManager.ClosePanel();
        panelManager.OpenPanel(8);
        _launchCoroutine = LaunchingRoutine();
        StartCoroutine(_launchCoroutine);
    }

    public void StopLaunching()
    {
        StopCoroutine(_launchCoroutine);
        SceneManager.UnloadSceneAsync("FlightScene");
        panelManager.ClosePanel();
    }

    private IEnumerator LaunchingRoutine()
    {
        if (_questSceneAsync == null)
        {
            _questSceneAsync = SceneManager.LoadSceneAsync("FlightScene", LoadSceneMode.Single);
            _questSceneAsync.allowSceneActivation = false;
        }

        var countdown = 10;

        while (countdown > 0)
        {
            countdownText.text = countdown + "";
            yield return new WaitForSeconds(1);
            countdown--;
        }

        print("Scene launching!");
        _questSceneAsync.allowSceneActivation = true;
    }
}