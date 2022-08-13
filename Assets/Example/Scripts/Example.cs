using PlayerLoopProfiles;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Example : MonoBehaviour
{
    [SerializeField] private GameObject Cube;
    [SerializeField] private TextMeshProUGUI Frames;

    private int frameCount;

    public enum Profile
    {
        IDLE,
        NORMAL,
    }

    void Start()
    {
        // Limit framerate
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

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