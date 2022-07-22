package com.bristn.lowpowerplugin;

import android.os.Build;
import android.view.ActionMode;
import android.view.KeyEvent;
import android.view.Menu;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.view.SearchEvent;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.view.accessibility.AccessibilityEvent;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.annotation.RequiresApi;

public class PluginWindowCallback implements Window.Callback {

    private final Window.Callback originalCallback;
    private final InteractionCallback interactionCallback;

    public PluginWindowCallback(Window.Callback pWindowCallback, InteractionCallback pInteractionCallback) {
        originalCallback = pWindowCallback;
        interactionCallback = pInteractionCallback;
    }


    @Override
    public boolean dispatchKeyEvent(KeyEvent keyEvent) {
        return originalCallback.dispatchKeyEvent(keyEvent);
    }

    @RequiresApi(api = Build.VERSION_CODES.HONEYCOMB)
    @Override
    public boolean dispatchKeyShortcutEvent(KeyEvent keyEvent) {
        return originalCallback.dispatchKeyShortcutEvent(keyEvent);
    }

    @Override
    public boolean dispatchTouchEvent(MotionEvent motionEvent) {
        int action = motionEvent.getActionMasked();
        if (action == MotionEvent.ACTION_DOWN) {
            interactionCallback.onTouchEvent(true);

        } else if (action == MotionEvent.ACTION_UP) {
            interactionCallback.onTouchEvent(false);
        }

        return originalCallback.dispatchTouchEvent(motionEvent);
    }

    @Override
    public boolean dispatchTrackballEvent(MotionEvent motionEvent) {
        return originalCallback.dispatchTrackballEvent(motionEvent);
    }

    @RequiresApi(api = Build.VERSION_CODES.HONEYCOMB_MR1)
    @Override
    public boolean dispatchGenericMotionEvent(MotionEvent motionEvent) {
        return originalCallback.dispatchGenericMotionEvent(motionEvent);
    }

    @Override
    public boolean dispatchPopulateAccessibilityEvent(AccessibilityEvent accessibilityEvent) {
        return originalCallback.dispatchPopulateAccessibilityEvent(accessibilityEvent);
    }

    @Nullable
    @Override
    public View onCreatePanelView(int i) {
        return originalCallback.onCreatePanelView(i);
    }

    @Override
    public boolean onCreatePanelMenu(int i, @NonNull Menu menu) {
        return originalCallback.onCreatePanelMenu(i, menu);
    }

    @Override
    public boolean onPreparePanel(int i, @Nullable View view, @NonNull Menu menu) {
        return originalCallback.onPreparePanel(i, view, menu);
    }

    @Override
    public boolean onMenuOpened(int i, @NonNull Menu menu) {
        return originalCallback.onMenuOpened(i, menu);
    }

    @Override
    public boolean onMenuItemSelected(int i, @NonNull MenuItem menuItem) {
        return originalCallback.onMenuItemSelected(i, menuItem);
    }

    @Override
    public void onWindowAttributesChanged(WindowManager.LayoutParams layoutParams) {
        originalCallback.onWindowAttributesChanged(layoutParams);
    }

    @Override
    public void onContentChanged() {
        originalCallback.onContentChanged();
    }

    @Override
    public void onWindowFocusChanged(boolean b) {
        originalCallback.onWindowFocusChanged(b);
    }

    @Override
    public void onAttachedToWindow() {
        originalCallback.onAttachedToWindow();
    }

    @Override
    public void onDetachedFromWindow() {
        originalCallback.onDetachedFromWindow();
    }

    @Override
    public void onPanelClosed(int i, @NonNull Menu menu) {
        originalCallback.onPanelClosed(i,menu);
    }

    @Override
    public boolean onSearchRequested() {
        return originalCallback.onSearchRequested();
    }

    @RequiresApi(api = Build.VERSION_CODES.M)
    @Override
    public boolean onSearchRequested(SearchEvent searchEvent) {
        return originalCallback.onSearchRequested(searchEvent);
    }

    @RequiresApi(api = Build.VERSION_CODES.HONEYCOMB)
    @Nullable
    @Override
    public ActionMode onWindowStartingActionMode(ActionMode.Callback callback) {
        return originalCallback.onWindowStartingActionMode(callback);
    }

    @RequiresApi(api = Build.VERSION_CODES.M)
    @Nullable
    @Override
    public ActionMode onWindowStartingActionMode(ActionMode.Callback callback, int i) {
        return originalCallback.onWindowStartingActionMode(callback, i);
    }

    @RequiresApi(api = Build.VERSION_CODES.HONEYCOMB)
    @Override
    public void onActionModeStarted(ActionMode actionMode) {
        originalCallback.onActionModeStarted(actionMode);
    }

    @RequiresApi(api = Build.VERSION_CODES.HONEYCOMB)
    @Override
    public void onActionModeFinished(ActionMode actionMode) {
        originalCallback.onActionModeFinished(actionMode);
    }
}
