using PlayerLoopProfiles;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static Example;

// A simple class to capsule the normal profile completely.
public static class ProfileNormal 
{
    // Method gets called in Example.cs to register the profile.
    public static PlayerLoopProfile GetProfile()
    {
        // Use the PlayerLoopProfileBuilder to get a new profile.
        PlayerLoopProfile profile = new PlayerLoopProfileBuilder()

            // Set the timeout callback action.
            // This gets invoked once there hasn't been an interaction from any of the registered action maps for the time of 'TimeoutDuration'
            .TimeoutCallback(Timeout)

            // Set the timeout period to 0.1 seconds.
            .TimeoutDuration(0.1f)

            // The first parameter is the type of the given UI element.
            // The second parameter is a delegate which gets called with the selected UI element.
            // In this delegate you can cast the passed component to the type you specified in this call.
            // If the delegate returns true the timeout of the profile gets reset. 
            // If it returns false the currently selected element is discarded.
            // In short this call prevents the timeout callback, so the profile change, whilst a TextMeshPro InputField is focused.
            .ActiveUiEvaluation(typeof(TMP_InputField), CallbackTextMeshPro)

            // Same as above, but for 'Fosuscable' from the UI Toolkit classes
            // In short this call prevents the timeout callback, so the profile change, whilst a UI Toolkit TextField is focused.
            .ActiveUiEvaluation(typeof(TextField), CallbackUiToolkit)

            // Finally build the profile
            .Build();
        return profile;
    }

    // This gets called if the currently selected object has a 'TMP_InputField' component attached.
    private static bool CallbackTextMeshPro(Component pTextField)
    {
        // Return true to reset the timeout.
        // Resturns true and thus resets the timeout whilst a TMP_InputField is focused.
        return ((TMP_InputField)pTextField).isFocused;
    }

    // This gets called if the currently focused object from the UI Toolkit focus controller is of the type 'TextField'.
    private static bool CallbackUiToolkit(Focusable pTextField)
    {
        // Return true to reset the timeout.
        // Resturns true and thus resets the timeout whilst a UI Toolkit TextField is focused .
        return true;
    }

    // This gets called when this profile is active and the user hasn't interacted with any action of the registered action maps for 0.1 seconds.
    private static void Timeout()
    {
        // If there hasn't been any interaction lately, activate the idle profile, as it discards most of the PlayerLoop.
        PlayerLoopManager.SetActiveProfile(Profile.IDLE);
    }
}
