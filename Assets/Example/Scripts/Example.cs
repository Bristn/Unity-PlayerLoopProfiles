using PlayerLoopProfiles;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Example : MonoBehaviour
{
    [SerializeField] private GameObject Cube;
    [SerializeField] private TextMeshProUGUI Frames;
    [SerializeField] private InputActionAsset InputAsset;

    private int frameCount;

    public enum Profile
    {
        IDLE,
        NORMAL,
    }

    public enum InteractionType
    {
        NAVIGATE,
        POINT,
        RIGHT_CLICK,
        MIDDLE_CLICK,
        CLICK,
        SCROLL_WHEEL,
        SUBMIT,
        CANCEL,
        TRACKED_DEVICE_POSITION,
        TRACKED_DEVICE_ORIENTATION,
    }

    public static List<string> actionNames = new string[]
    {
        "Navigate",
        "Point",
        "RightClick",
        "MiddleClick",
        "Click",
        "ScrollWheel",
        "Submit",
        "Cancel",
        "TrackedDevicePosition",
        "TrackedDeviceOrientation"
    }.ToList();

    void Start()
    {
        // Limit framerate
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        PlayerLoopInteraction.AddActionMap(InputAsset.FindActionMap("UI"));

        PlayerLoopManager.AddProfile(Profile.IDLE, ProfileIdle.GetProfile());
        PlayerLoopManager.AddProfile(Profile.NORMAL, ProfileNormal.GetProfile());
        PlayerLoopManager.SetActiveProfile(Profile.IDLE);
    }

    void Update()
    {
        frameCount++;
        Cube.transform.RotateAround(Vector3.zero, Vector3.forward, 1);
        Frames.SetText("Frame count: " + frameCount);
    }
}