using UnityEngine;
using UnityEngine.XR.Management;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;

public class SafeXRInit : MonoBehaviour
{
    IEnumerator Start()
    {
        if (XRGeneralSettings.Instance == null || XRGeneralSettings.Instance.Manager == null)
        {
            Debug.LogWarning("XRGeneralSettings not configured. Running in non-VR mode.");
            yield break;
        }

        var xrManager = XRGeneralSettings.Instance.Manager;

        // Wait briefly for Unity XR auto-load to finish on slower devices.
        float timeout = Time.realtimeSinceStartup + 5f;
        while (!xrManager.isInitializationComplete && Time.realtimeSinceStartup < timeout)
            yield return null;

        if (xrManager.activeLoader == null)
        {
            Debug.LogWarning("SafeXRInit: XR not initialized yet. Verify Cardboard loader is enabled for Android.");
            yield break;
        }

        // Ensure XR subsystems are running; if not, start them once.
        if (!IsAnyDisplayRunning())
        {
            Debug.Log("SafeXRInit: XR loader present but display not running, starting XR subsystems...");
            try
            {
                xrManager.StartSubsystems();
            }
            catch (System.Exception e)
            {
                Debug.LogError("SafeXRInit: Failed to start XR subsystems: " + e.Message + "\n" + e.StackTrace);
                yield break;
            }
        }

        // One more short wait and validation pass.
        yield return new WaitForSeconds(0.25f);

        if (IsAnyDisplayRunning())
            Debug.Log("SafeXRInit: XR display running. Cardboard stereo should be active.");
        else
            Debug.LogWarning("SafeXRInit: XR display still not running. Check Cardboard loader and Android graphics API settings.");
    }

    private static bool IsAnyDisplayRunning()
    {
        var displays = new List<XRDisplaySubsystem>();
        SubsystemManager.GetSubsystems(displays);
        for (int i = 0; i < displays.Count; i++)
        {
            if (displays[i] != null && displays[i].running)
                return true;
        }

        return false;
    }

    void OnDestroy()
    {
        // Intentionally no manual XR shutdown. Let Unity XR Management own lifecycle.
    }
}
