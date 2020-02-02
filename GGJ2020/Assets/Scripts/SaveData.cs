using UnityEngine;

public static class SaveData {
    const string GameCompleteKey = "IS_GAME_COMPLETE";

    public static bool IsGameComplete() {
        var gameComplete = PlayerPrefs.GetInt(GameCompleteKey, 0);
        return gameComplete == 1;
    }

    public static void CompleteGame() => PlayerPrefs.SetInt(GameCompleteKey, 1);
}