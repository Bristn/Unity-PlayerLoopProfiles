package com.bristn.lowpowerplugin;

import android.view.Window;

import com.unity3d.player.UnityPlayer;

public class PluginInstance {

    private static Window window;
    private static Window.Callback originalCallback;
    private static Window.Callback customCallback;

    private static InteractionCallback interactionCallback;

    public static void setup(InteractionCallback pCallback) {
        interactionCallback = pCallback;

        window = UnityPlayer.currentActivity.getWindow();
        originalCallback = window.getCallback();
        customCallback = new PluginWindowCallback(originalCallback, interactionCallback);
        window.setCallback(customCallback);
    }

    public static void pause() {
        window.setCallback(originalCallback);
    }

    public static void resume() {
        window.setCallback(customCallback);
    }

    public static void clean() {
        window.setCallback(originalCallback);
    }

}
