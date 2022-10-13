using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HealthProgressBar : MonoBehaviour
{
    public Image progess;
    public TextMeshProUGUI percentage;
    public PlayerController playerRef;

    private float MaxProgress;
    private float progressNumber;


    private int HPControll;

    // Start is called before the first frame update
    void Start()
    {
        MaxProgress = (float)playerRef.maxHP;

        UpdateProgressBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (HPControll != playerRef.HP)
        {
            UpdateProgressBar();
        }


    }

    void UpdateProgressBar()
    {
        HPControll = playerRef.HP;
        progressNumber = (MaxProgress * (float)playerRef.HP) / 100f;
        progess.fillAmount = progressNumber / 100;
        percentage.text = ($"{(int)progressNumber}%");
    }

}