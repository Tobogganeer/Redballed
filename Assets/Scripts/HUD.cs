using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HUD : MonoBehaviour
{
    // I love making everything a singleton
    private static HUD instance;
    private void Awake()
    {
        instance = this;
    }

    public TMPro.TMP_Text redBallCounter;
    public GameObject packageSprite;
    public GameObject socialMedia;

    int redBalls;


    private void Start()
    {
        packageSprite.SetActive(false);
        redBallCounter.text = "0";
        socialMedia.SetActive(false);
    }

    public static void IncreaseRedBalls()
    {
        instance.redBalls++;
        instance.redBallCounter.text = instance.redBalls.ToString();
    }

    public static void ShowPackage()
    {
        instance.packageSprite.SetActive(true);
    }

    public static void HidePackage()
    {
        instance.packageSprite.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
            socialMedia.SetActive(!socialMedia.activeSelf);
    }
}
