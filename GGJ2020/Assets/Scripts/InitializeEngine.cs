using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public static class InitializeEngine  {

    const int targetFramerate = 60;

    [RuntimeInitializeOnLoadMethod]
    public static void InitializeEngineMethod() {
        Application.targetFrameRate = targetFramerate;
        Time.fixedDeltaTime = 1f / targetFramerate;
    }
}
