using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Traffic_Light : MonoBehaviour
{
    [Header("Refer to the lights that should change")]
    public GameObject Red;
    public GameObject Orange;
    public GameObject Green;

    [Header("Time for lights")]
    public float greenTime = 7f; // Duration for green light
    public float orangeTime = 5f; // Duration for orange light
    public float redTime = 12f; // Duration for red light
    public float redPause = 2f; // Duration for red pause (red still emitting)

    [Header("Renderer or Material for Emission")]
    [Tooltip("Assign Renderer (e.g., MeshRenderer) or leave empty if using Material directly")]
    public Renderer greenRen;
    public Renderer orangeRen;
    public Renderer redRen;

    [Tooltip("Assign Material directly if not using Renderer")]
    public Material greenMat;
    public Material orangeMat;
    public Material redMat;

    [Header("Sequence Control")]
    [Tooltip("If true, start with red (12s + 2s pause); if false, start with red (2s)")]
    public bool startWithRed = false;

    private Material[] materials = new Material[3];
    private readonly Color greenEmission = Color.green;
    private readonly Color orangeEmission = new Color(1f, 0.5f, 0f); // RGB for orange
    private readonly Color redEmission = Color.red;
    private readonly Color noEmission = Color.black;

    // Static list to track all Traffic_Light instances for synchronization
    private static readonly List<Traffic_Light> instances = new List<Traffic_Light>();
    private bool isOnRed = false; // Tracks if this instance is in red phase or pause

    private void OnEnable()
    {
        // Add this instance to the list
        instances.Add(this);
    }

    private void OnDisable()
    {
        // Remove this instance from the list
        instances.Remove(this);
    }

    private void Start()
    {
        // Initialize materials array based on Renderer or Material assignments
        materials[0] = GetMaterial(0, Green, greenRen, greenMat, "Green");
        materials[1] = GetMaterial(1, Orange, orangeRen, orangeMat, "Orange");
        materials[2] = GetMaterial(2, Red, redRen, redMat, "Red");

        // Validate materials and enable emission
        bool isSetupValid = true;
        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i] == null)
            {
                Debug.LogError($"Material for {GetLightName(i)} light is missing!");
                isSetupValid = false;
                continue;
            }

            if (!materials[i].IsKeywordEnabled("_EMISSION"))
            {
                Debug.Log($"Enabling emission keyword for {GetLightName(i)} light");
                materials[i].EnableKeyword("_EMISSION");
            }

            // Start with emission disabled
            materials[i].SetColor("_EmissionColor", noEmission);
        }

        if (!isSetupValid)
        {
            Debug.LogError("Emission sequence will not start due to setup errors.");
            return;
        }

        // Start the emission sequence coroutine
        StartCoroutine(EmissionSequence());
    }

    private Material GetMaterial(int index, GameObject obj, Renderer ren, Material mat, string lightName)
    {
        if (mat != null)
        {
            Debug.Log($"Using directly assigned material for {lightName} light");
            return mat;
        }

        if (ren != null)
        {
            if (ren.material != null)
            {
                Debug.Log($"Using material from assigned Renderer for {lightName} light");
                return ren.material;
            }
            Debug.LogError($"{lightName} Renderer has no material assigned!");
            return null;
        }

        if (obj != null)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null && renderer.material != null)
            {
                Debug.Log($"Using material from GameObject {obj.name} for {lightName} light");
                return renderer.material;
            }
            Debug.LogError($"{lightName} GameObject ({obj.name}) has no Renderer or material!");
            return null;
        }

        Debug.LogError($"{lightName} light has no GameObject, Renderer, or Material assigned!");
        return null;
    }

    private string GetLightName(int index)
    {
        return index switch
        {
            0 => "Green",
            1 => "Orange",
            2 => "Red",
            _ => "Unknown"
        };
    }

    private bool IsOtherOnRed()
    {
        // Check if any other instance is on red
        foreach (var instance in instances)
        {
            if (instance != this && instance.isOnRed)
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator WaitForOtherRed()
    {
        // Wait until another instance is on red
        while (!IsOtherOnRed())
        {
            yield return null; // Wait for the next frame
        }
    }

    IEnumerator EmissionSequence()
    {
        // Define the sequence order: Green (0), Orange (1), Red (2)
        (int index, Color color, float duration)[] sequence = new[]
        {
            (0, greenEmission, greenTime), // Green
            (1, orangeEmission, orangeTime), // Orange
            (2, redEmission, redTime) // Red
        };

        if (startWithRed)
        {
            // Checked: Start with red (redTime + redPause)
            Debug.Log("Starting with red phase (checked)");
            isOnRed = true;
            SetEmissionForOne(2, redEmission);
            yield return new WaitForSeconds(redTime);
            // Wait for unchecked to be on red before starting pause
            if (!IsOtherOnRed())
            {
                Debug.Log("Waiting for other sequence to reach red (checked)");
                yield return StartCoroutine(WaitForOtherRed());
            }
            Debug.Log("Starting red pause (checked)");
            SetEmissionForOne(2, redEmission);
            yield return new WaitForSeconds(redPause);
            isOnRed = false;
        }
        else
        {
            // Unchecked: Start with red pause (2s)
            Debug.Log("Starting with initial red pause (unchecked)");
            isOnRed = true;
            SetEmissionForOne(2, redEmission);
            // Wait for checked to be on red before starting pause
            if (!IsOtherOnRed())
            {
                Debug.Log("Waiting for other sequence to reach red (unchecked)");
                yield return StartCoroutine(WaitForOtherRed());
            }
            yield return new WaitForSeconds(redPause);
            isOnRed = false;
        }

        while (true)
        {
            // Run the sequence: Green -> Orange -> Red
            for (int i = 0; i < sequence.Length; i++)
            {
                var (index, color, duration) = sequence[i];
                Debug.Log($"Enabling {GetLightName(index)} emission");
                isOnRed = (index == 2);
                SetEmissionForOne(index, color);
                yield return new WaitForSeconds(duration);

                // After red phase, wait for other sequence to be on red, then pause
                if (index == 2)
                {
                    if (!IsOtherOnRed())
                    {
                        Debug.Log("Waiting for other sequence to reach red before pause");
                        yield return StartCoroutine(WaitForOtherRed());
                    }
                    Debug.Log("Adding red pause after red phase");
                    SetEmissionForOne(2, redEmission);
                    yield return new WaitForSeconds(redPause);
                    isOnRed = false;
                }
            }
        }
    }

    void SetEmissionForOne(int activeIndex, Color emissionColor)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i] != null)
            {
                materials[i].SetColor("_EmissionColor", i == activeIndex ? emissionColor : noEmission);
            }
            else
            {
                Debug.LogWarning($"Material for {GetLightName(i)} light is null, skipping emission change.");
            }
        }
    }

    private void OnDestroy()
    {
        foreach (Material mat in materials)
        {
            if (mat != null)
            {
                mat.SetColor("_EmissionColor", noEmission);
                Debug.Log("Disabled emission for all lights");
            }
        }
    }
}