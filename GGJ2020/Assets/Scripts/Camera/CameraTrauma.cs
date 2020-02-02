using UnityEngine;

[CreateAssetMenu(menuName = "LWG/PersistentGameData/PersistentCameraShake")]
public class CameraTrauma : ScriptableObject {

    [SerializeField] float timeFromMaxToMin = 1.0f;
    public float TimeFromMaxToMin => timeFromMaxToMin;

    public float Trauma {get; private set;}
    public float Shake => Trauma * Trauma;

    // Use this for initialization
    float traumaDrainRate;
    void OnEnable () => traumaDrainRate = 1f / timeFromMaxToMin;
    public void RemoveAllTrauma() => Trauma = 0.0f;

    public void UpdateTrauma(float deltaTime) {
        Trauma -= traumaDrainRate * deltaTime;
        Trauma = Mathf.Max(Trauma, 0.0f);
    }

    public void AddTrauma(float amnt) {
        Trauma += amnt;
        Trauma = Mathf.Min(Trauma, 1.0f);
    }
}
