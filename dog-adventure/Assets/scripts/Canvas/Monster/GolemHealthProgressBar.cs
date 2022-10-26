using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GolemHealthProgressBar : MonoBehaviour
{
    public Image progess;
    public TextMeshProUGUI percentage;
    public Golem MonsterRef;

    private float MaxProgress;
    private float progressNumber;


    private int HPControll;

    // Start is called before the first frame update
    void Start()
    {
        MaxProgress = (float)MonsterRef.maxHP;
        UpdateProgressBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (HPControll != MonsterRef.HP)
        {
            UpdateProgressBar();
        }
    }

    void UpdateProgressBar()
    {
        HPControll = MonsterRef.HP;
        progressNumber = ((float)MonsterRef.HP/ MaxProgress) * 100f;
        progess.fillAmount = progressNumber / 100;
        percentage.text = ($"{(int)progressNumber}%");
    }

}