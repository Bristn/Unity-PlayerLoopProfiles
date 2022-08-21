using PlayerLoopProfiles;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Example : MonoBehaviour
{
    // Objects/Variables to better visualize when the player loop is modified
    [SerializeField] private GameObject Cube;
    [SerializeField] private TextMeshProUGUI Frames;
    [SerializeField] private InputActionAsset InputAsset;
    private int frameCount;

    // This enum is used to register and activate Profiles.
    // There is no need to use an enum, a integer value is sufficient.
    public enum Profile
    {
        IDLE,
        NORMAL,
    }

    // The values of this enum correspond to the actions of the default UI action map.
    // Again the enum is not necessary, the interaction callback gets passed the action name as a string.
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

    // The actual string values that get passed into the interaction callback.
    // The enum above is used as an index to this array.
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
        // Limit framerate, not really anything to do with the plugin
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        // Get the UI action map from the InputAsset and add this map.
        // This way all actions from the UI map get registered and used in the interaction callback.
        PlayerLoopInteraction.AddActionMap(InputAsset.FindActionMap("UI"));

        // Create two different profiles and register them.
        // See the profile classes for more detail on how they are created.
        // (It's not required to split them into different classes, but more complex profiles get large rather quickly)
        PlayerLoopManager.AddProfile(Profile.IDLE, ProfileIdle.GetProfile());
        PlayerLoopManager.AddProfile(Profile.NORMAL, ProfileNormal.GetProfile());

        // Lastly activate the normal profile.
        PlayerLoopManager.SetActiveProfile(Profile.NORMAL);
    }

    // Simple update method to show how many frames have elapsed. 
    // Used to better visualize when the profiles change
    void Update()
    {
        frameCount++;
        Cube.transform.RotateAround(Vector3.zero, Vector3.forward, 1);
        Frames.SetText("Frame count: " + frameCount);
    }
}