using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputManager
{
    public static InputDevice player1Device;
    public static InputDevice player2Device;
    
    public static List<InputDevice> GetDevices()
    {
        List<InputDevice> devices = new();
        foreach (InputDevice device in InputSystem.devices.ToList())
        {
            if (!device.name.Contains("Mouse"))
                devices.Add(device);
        }
        return devices;
    }
    
    public static InputDevice GetMouse()
    {
        return InputSystem.devices.ToList().Find(device => device.name.Contains("Mouse"));
    }
}
