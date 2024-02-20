using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputManager
{
    public static InputDevice player1Device;
    public static InputDevice player2Device;
    
    private static readonly List<(InputDevice, string)> deviceSchemaPairs = new();
    
    public static List<InputDevice> GetDevices()
    {
        deviceSchemaPairs.Clear();
        deviceSchemaPairs.Add((null, "null"));
        List<InputDevice> devices = new();
        foreach (InputDevice device in InputSystem.devices.ToList())
        {
            if (!device.name.Contains("Mouse"))
                devices.Add(device);
            
            if (device.name.Contains("Keyboard"))
                deviceSchemaPairs.Add((device, "Keyboard"));
            
            if (device.name.Contains("Controller"))
                deviceSchemaPairs.Add((device, "Gamepad"));
        }
        return devices;
    }

    public static string GetSchemaByDevice(InputDevice device)
    {
        return deviceSchemaPairs.Find(pair => pair.Item1 == device).Item2;
    }
    
    public static InputDevice GetMouse()
    {
        return InputSystem.devices.ToList().Find(device => device.name.Contains("Mouse"));
    }
}
