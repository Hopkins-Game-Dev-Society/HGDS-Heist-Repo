using UnityEngine;
using UnityEngine.Rendering.Universal;

/*
 * Author: Shawn Guo
 * Updated: 1/6/2026
 */
public class Guard : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField, Min(0.01f)] private float radius = 3.5f;
    [SerializeField] private string playerTag = "Player";

    [Header("UI")]
    [Tooltip("A GameObject (typically a Canvas) to toggle when the player is detected.")]
    [SerializeField] private GameObject detectedCanvas;

    [Header("Light (2D URP)")]
    [SerializeField] private Light2D spotLight;
    [SerializeField, Range(0f, 360f)] private float spotOuterAngle = 90f;
    [SerializeField, Range(0f, 360f)] private float spotInnerAngle = 60f;
    [SerializeField] private float innerRadiusFactor = 0.6f;

    [Header("Detector")]
    [Tooltip("Optional existing detector child. If null, one will be created.")]
    [SerializeField] private Transform detectorRoot;

    private CircleCollider2D detectorCollider;

    private void Reset()
    {
        radius = 3.5f;
        playerTag = "Player";
        spotOuterAngle = 90f;
        spotInnerAngle = 60f;
        innerRadiusFactor = 0.6f;
    }

    private void Awake()
    {
        EnsureSpotLight();
        EnsureDetector();
        ApplyRadius(radius);

        if (detectedCanvas != null)
            detectedCanvas.SetActive(false);
    }

    private void OnValidate()
    {
        if (radius < 0.01f) radius = 0.01f;
        if (innerRadiusFactor < 0f) innerRadiusFactor = 0f;

        // Keep things updated in-editor where possible.
        if (Application.isPlaying) return;

        if (spotLight == null) spotLight = GetComponentInChildren<Light2D>(true);
        if (detectorRoot == null)
        {
            var t = transform.Find("Detector2D");
            if (t != null) detectorRoot = t;
        }

        if (detectorRoot != null)
            detectorCollider = detectorRoot.GetComponent<CircleCollider2D>();

        if (spotLight != null || detectorCollider != null)
            ApplyRadius(radius);
    }
    
    public void SetRadius(float newRadius)
    {
        radius = Mathf.Max(0.01f, newRadius);
        ApplyRadius(radius);
    }

    private void ApplyRadius(float r)
    {
        if (spotLight != null)
        {
            spotLight.lightType = Light2D.LightType.Point;

            spotLight.pointLightOuterRadius = r;
            spotLight.pointLightInnerRadius = Mathf.Clamp01(innerRadiusFactor) * r;

            spotLight.pointLightOuterAngle = spotOuterAngle;
            spotLight.pointLightInnerAngle = Mathf.Min(spotInnerAngle, spotOuterAngle);
        }

        if (detectorCollider != null)
        {
            detectorCollider.radius = r;
        }
    }

    private void EnsureSpotLight()
    {
        if (spotLight != null) return;

        // Try to find an existing child light first.
        spotLight = GetComponentInChildren<Light2D>(true);
        if (spotLight != null) return;

        // Create one, I think
        var go = new GameObject("SpotLight2D");
        go.transform.SetParent(transform, false);
        go.transform.localPosition = Vector3.zero;

        spotLight = go.AddComponent<Light2D>();
        spotLight.lightType = Light2D.LightType.Point;
        
        spotLight.intensity = 1f;
        spotLight.color = Color.white;
    }

    private void EnsureDetector()
    {
        if (detectorRoot == null)
        {
            var existing = transform.Find("Detector2D");
            if (existing != null)
                detectorRoot = existing;
        }

        if (detectorRoot == null)
        {
            var go = new GameObject("Detector2D");
            go.transform.SetParent(transform, false);
            go.transform.localPosition = Vector3.zero;
            detectorRoot = go.transform;
        }

        detectorCollider = detectorRoot.GetComponent<CircleCollider2D>();
        if (detectorCollider == null)
            detectorCollider = detectorRoot.gameObject.AddComponent<CircleCollider2D>();

        detectorCollider.isTrigger = true;

        var rb = detectorRoot.GetComponent<Rigidbody2D>();
        if (rb == null) rb = detectorRoot.gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;

        var relay = detectorRoot.GetComponent<TriggerRelay2D>();
        if (relay == null) relay = detectorRoot.gameObject.AddComponent<TriggerRelay2D>();
        relay.Init(this);
    }

    private void SetDetected(bool detected)
    {
        if (detectedCanvas != null)
            detectedCanvas.SetActive(detected);
    }

    private void HandleTriggerEnter(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        SetDetected(true);
    }

    private void HandleTriggerExit(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        // I actually don't want this yet
        // SetDetected(false);
    }
    
    private sealed class TriggerRelay2D : MonoBehaviour
    {
        private Guard owner;

        public void Init(Guard guard) => owner = guard;

        private void OnTriggerEnter2D(Collider2D other)
        {
            owner?.HandleTriggerEnter(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            owner?.HandleTriggerExit(other);
        }
    }
}
