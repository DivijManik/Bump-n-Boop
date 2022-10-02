﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class PauseButtonAction : MonoBehaviour
{
    [SerializeField]
    GameObject StartButton;

    public void PauseButtonClick()
    {
        PlayerController.Instance.StartGame = false;
        //postProcess.weight = 1;
        //DOTween.To(() => postProcess.weight, x => postProcess.weight = x, 1, 0.5f);

        //ScriptHolderPANEL.gameObject.SetActive(true);
        transform.gameObject.SetActive(false);
        StartButton.transform.parent.gameObject.SetActive(true);
    }
}
