using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown player1Input;
    [SerializeField] private TMP_Dropdown player2Input;
    
    private void Start()
    {
        SetupInputDropdowns();
    }

    void SetupInputDropdowns()
    {
        List<TMP_Dropdown.OptionData> player1Devices = new ();
        List<TMP_Dropdown.OptionData> player2Devices = new () {
            new TMP_Dropdown.OptionData("No player 2")
        };
        foreach (InputDevice device in InputManager.GetDevices())
        {
            player1Devices.Add(new TMP_Dropdown.OptionData(device.displayName));
            player2Devices.Add(new TMP_Dropdown.OptionData(device.displayName));
        }
        player1Input.options = player1Devices;
        player2Input.options = player2Devices;
        
        InputManager.player1Device = InputManager.GetDevices()[0];
    }
    
    public void OnPlayer1InputChange (int value)
    {
        InputManager.player1Device = InputManager.GetDevices()[value];
    }
    
    public void OnPlayer2InputChange (int value)
    {
        if (value == 0)
            InputManager.player2Device = null;
        else
            InputManager.player2Device = InputManager.GetDevices()[value - 1];
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainHub");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
