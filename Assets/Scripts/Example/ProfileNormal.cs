using PlayerLoopProfiles;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static Example;

public static class ProfileNormal 
{
    public static PlayerLoopProfile GetProfile()
    {
        PlayerLoopProfile profile = new PlayerLoopProfileBuilder()
           .TimeoutCallback(Timeout)
           .TimeoutDuration(0.1f)
           .UI(typeof(TMP_InputField), CallbackTextMeshPro)
           .UI(typeof(TextField), CallbackUiToolkit)
           .Build();
        return profile;
    }

    private static bool CallbackTextMeshPro(Component pTextField)
    {
        return ((TMP_InputField)pTextField).isFocused;
    }

    private static bool CallbackUiToolkit(Focusable pTextField)
    {
        return true;
    }

    private static void Timeout()
    {
        PlayerLoopManager.SetActiveProfile(Profile.IDLE);
    }
}
