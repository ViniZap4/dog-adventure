using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class StaminaProgressBar : MonoBehaviour
{
    public Image progess;
    public TextMeshProUGUI percentage;
    public PlayerController playerRef;

    private float MaxProgress;
    private float progressNumber;

    private float staminaControll;

    // Start is called before the first frame update
    void Start()
    {
        MaxProgress = (float)playerRef.maxStamina;

        UpdateProgressBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (staminaControll != playerRef.stamina)
        {
            UpdateProgressBar();
        }
            

    }

    void UpdateProgressBar()
    {
        staminaControll = playerRef.stamina;
        progressNumber = (MaxProgress * playerRef.stamina) / 100f;
        progess.fillAmount = progressNumber / 100;
        percentage.text = ($"{(int)progressNumber}%");
    }

}