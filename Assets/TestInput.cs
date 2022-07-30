using Assets.Scripts.LowPower;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using static Assets.Scripts.Example;
using static UnityEngine.InputSystem.HID.HID;
using static UnityEngine.InputSystem.InputAction;

public class TestInput : MonoBehaviour
{
    [SerializeField] private PlayerInput input;

    [SerializeField] private InputActionAsset _inputMap;
    private InputAction position;
    private InputAction click;

    public enum ActionType
    {
        NAVIGATE,
        POINT,
        RIGHT_CLICK,
        MIDDLE_CLICK,
        CLICK,
        SCROLL_WHEEL,
        MOVE,
        SUBMIT,
        CANCEL,
        TRACKED_DEVICE_POSITION,
        TRACKED_DEVICE_ORIENTATION,
    }

    private List<string> actionNames = new string[]
    {
        "Navigate", 
        "Point", 
        "RightClick", 
        "MiddleClick",
        "Click", 
        "ScrollWheel", 
        "Move", 
        "Submit", 
        "Cancel", 
        "TrackedDevicePosition",
        "TrackedDeviceOrientation"
    }.ToList();

    private Dictionary<string, UnityAction<CallbackContext>> actions = new Dictionary<string, UnityAction<CallbackContext>>();


    private Mouse virtualMouse;
    private void Start()
    {
        return;
        _inputMap.Enable();
        virtualMouse = InputSystem.AddDevice<Mouse>();

        actions.Add(actionNames[(int)ActionType.NAVIGATE], Navigate);
        actions.Add(actionNames[(int)ActionType.POINT], Point);
        actions.Add(actionNames[(int)ActionType.RIGHT_CLICK], RightClick);
        actions.Add(actionNames[(int)ActionType.MIDDLE_CLICK], MiddleClick);
        actions.Add(actionNames[(int)ActionType.CLICK], Click);
        actions.Add(actionNames[(int)ActionType.SCROLL_WHEEL], ScrollWheel);
        actions.Add(actionNames[(int)ActionType.MOVE], Move);
        actions.Add(actionNames[(int)ActionType.SUBMIT], Submit);
        actions.Add(actionNames[(int)ActionType.CANCEL], Cancel);
        actions.Add(actionNames[(int)ActionType.TRACKED_DEVICE_POSITION], TrackedDevicePosition);
        actions.Add(actionNames[(int)ActionType.TRACKED_DEVICE_ORIENTATION], TrackedDeviceOrientation);

        string prefix = "UI/";
        foreach (PlayerInput.ActionEvent element in input.actionEvents)
        {
            if (!element.actionName.StartsWith("UI/"))
            {
                continue;
            }

            string name = element.actionName;
            name = name.Substring(name.IndexOf(prefix) + prefix.Length);
            if (name.Contains("["))
            {
                name = name.Substring(0, name.IndexOf("["));
            }

            if (actions.TryGetValue(name, out UnityAction<CallbackContext> result))
            {
                element.AddListener(result);
            }
        }

        position = _inputMap.FindAction("Point");
        click = _inputMap.FindAction("Click");

        // InputSystem.onEvent += EventListener;

    }

    int valueTest = 0;
    private void EventListener(InputEventPtr pPointer, InputDevice pDevice)
    {
        if (!pPointer.IsA<StateEvent>() && !pPointer.IsA<DeltaStateEvent>())
        {
            return;
        }

        var mouse = pDevice as Mouse;
        if (mouse == null)
        {
            return;
        }

        if (valueTest >= 5)
        {
            return;
        }

        foreach (InputControl control in pPointer.EnumerateChangedControls())
        {
            if (control.ToString().Contains("position"))
            {
                Debug.Log($"Control {control} changed value to {control.ReadValueFromEventAsObject(pPointer)}");

                Test(control);
                //valueTest++;
                break;
            } 
        }


        
        // InputSystem.onEvent -= EventListener;
    }

    private void Test(InputControl pControl)
    {
        Debug.Log("Queue " + position.ReadValue<Vector2>());
        // InputSystem.QueueEvent(pPointer);

        /*
        var mouse = InputSystem.AddDevice<Mouse>();
        using (StateEvent.From(mouse, out var eventPtr))
        {
            // mouse.leftButton.WriteValueIntoEvent(true, eventPtr); // Fails on this line
            mouse.leftButton.WriteValueIntoEvent(1f, eventPtr);
            InputSystem.QueueEvent(eventPtr);
        }
        */

        /*

        // Update position of current mouse.
        InputSystem.QueueDeltaStateEvent(Mouse.current.position, new Vector2(123, 234));

        // Create a fake mouse and change its position.
        var myMouse = InputSystem.AddDevice<Mouse>();
        InputSystem.QueueDeltaStateEvent(myMouse.position, new Vector2(234, 345));

        // Update the entire state of the mouse.
        // InputSystem.QueueStateEvent(myMouse, new MouseState { });

        // Grab some arbitrary device of whatever type and update its "firstButton" and "dpad/up" to be pressed.
        var device = InputSystem.devices[1];
        InputEventPtr eventPtr;
        using (StateEvent.From(device, out eventPtr))
        {
            // device["firstButton"].WriteValueIntoEvent(1, eventPtr);
            // device["dpad/up"].WriteValueIntoEvent(1, eventPtr);
            InputSystem.QueueEvent(eventPtr);
        }
        */
    }


    public void Navigate(CallbackContext pContext)
    {
        Debug.Log("Navigate");
    }

    public void Point(CallbackContext pContext)
    {
        Debug.Log("Point");
    }

    public void Click(CallbackContext pContext)
    {
        MouseState state = new MouseState();
        state.position = position.ReadValue<Vector2>();

        float leftClick = click.ReadValue<float>();
        state.WithButton(MouseButton.Left, leftClick == 1f);


        LowPowerManager.Instance.playerLoopManager.SetActiveProfile(Profile.NORMAL);
        // LowPowerDispatcher.Instance.DispatchEvent(SendClick, state);
    }

    public void SendClick(MouseState pState)
    {
        InputSystem.QueueStateEvent(virtualMouse, pState);
    }

    public void MiddleClick(CallbackContext pContext)
    {
        Debug.Log("MiddleClick");
    }

    public void RightClick(CallbackContext pContext)
    {
        Debug.Log("RightClick");
    }

    public void ScrollWheel(CallbackContext pContext)
    {
        Debug.Log("ScrollWheel");
    }

    public void Move(CallbackContext pContext)
    {
        Debug.Log("Move");
    }

    public void Submit(CallbackContext pContext)
    {
        Debug.Log("Submit");
    }

    public void Cancel(CallbackContext pContext)
    {
        Debug.Log("Cancel");
    }

    public void TrackedDevicePosition(CallbackContext pContext)
    {
        Debug.Log("TrackedDevicePosition");
    }

    public void TrackedDeviceOrientation(CallbackContext pContext)
    {
        Debug.Log("TrackedDeviceOrientation");
    }




    public void Moved(CallbackContext pContext)
    {
        Mouse mouse = InputSystem.GetDevice<Mouse>();

        MouseState stateA = new MouseState();
        MouseState stateB = new MouseState();

        stateA.position = new Vector2(850, 80);
        stateA = stateA.WithButton(MouseButton.Left, true);
        stateB.position = new Vector2(850, 80);
        stateB = stateB.WithButton(MouseButton.Left, false);

        InputSystem.QueueStateEvent(mouse, stateA);
        InputSystem.QueueStateEvent(mouse, stateB);

    }
}
