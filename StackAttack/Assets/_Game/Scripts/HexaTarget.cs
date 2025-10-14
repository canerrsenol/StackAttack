using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class HexaTarget : MonoBehaviour, IHitable
{
    [SerializeField] private int health = 3;
    [SerializeField] private HexaColors hexaColors;
    [SerializeField] private GameObject hexaVisual;
    [SerializeField] private TextMeshPro healthText;
    [SerializeField] private float yOffset = .3f;
    [SerializeField] private int hexaHealthMultiplier = 5;
    [SerializeField, HideInInspector] private List<MeshRenderer> hexaVisualMeshRenderers = new List<MeshRenderer>();
    [SerializeField] private MaterialsSO materialsSO;

    void Awake()
    {
        if (CanSyncVisuals())
        {
            SyncHealthVisuals();
        }
    }

    void OnValidate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying && !CanSyncVisuals())
        {
            if (healthText != null)
            {
                healthText.text = Mathf.Max(0, health).ToString();
            }

            return;
        }
#endif

        SyncHealthVisuals();
    }

    public void Hit(float damage)
    {
        Debug.Log($"HexaTarget was hit! Damage: {damage}");

        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOPunchScale(Vector3.one * 0.2f, 0.1f, 1, 0.5f));
        sequence.AppendCallback(() =>
        {
            health = Mathf.Max(0, health - (int)damage);
            SyncHealthVisuals();

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        });
    }

    public int GetDamage()
    {
        return health;
    }

    private void SyncHealthVisuals()
    {
        if (healthText != null)
        {
            healthText.text = Mathf.Max(0, health).ToString();
        }

        for (int i = hexaVisualMeshRenderers.Count - 1; i >= 0; i--)
        {
            if (hexaVisualMeshRenderers[i] == null)
            {
                hexaVisualMeshRenderers.RemoveAt(i);
            }
        }

        if (hexaHealthMultiplier <= 0)
        {
            Debug.LogWarning("hexaHealthMultiplier must be greater than zero to spawn visuals.", this);
            return;
        }

        if (hexaVisual == null)
        {
            return;
        }

        int desiredCount = health <= 0
            ? 0
            : Mathf.CeilToInt((float)health / hexaHealthMultiplier);

        // Keep spawning until the visuals match the computed count.
        while (hexaVisualMeshRenderers.Count < desiredCount)
        {
            var visualInstance = SpawnHexaVisual();
            if (visualInstance == null)
            {
                break;
            }

            var meshRenderer = visualInstance.GetComponentInChildren<MeshRenderer>(true);
            if (meshRenderer == null)
            {
                Debug.LogWarning("Spawned hexa visual is missing a MeshRenderer component.", visualInstance);
                DestroySpawnedObject(visualInstance);
                continue;
            }

            hexaVisualMeshRenderers.Add(meshRenderer);
        }

        while (hexaVisualMeshRenderers.Count > desiredCount)
        {
            int lastIndex = hexaVisualMeshRenderers.Count - 1;
            var meshRenderer = hexaVisualMeshRenderers[lastIndex];
            hexaVisualMeshRenderers.RemoveAt(lastIndex);

            if (meshRenderer != null)
            {
                var rootObject = meshRenderer.transform.parent != null
                    ? meshRenderer.transform.parent.gameObject
                    : meshRenderer.gameObject;
                DestroySpawnedObject(rootObject);
            }
        }

        var targetMaterial = GetMaterialForCurrentColor();

        for (int i = 0; i < hexaVisualMeshRenderers.Count; i++)
        {
            var meshRenderer = hexaVisualMeshRenderers[i];
            if (meshRenderer == null)
            {
                continue;
            }

            var meshRoot = meshRenderer.transform.parent != null
                ? meshRenderer.transform.parent
                : meshRenderer.transform;
            meshRoot.localPosition = new Vector3(0f, i * yOffset, 0f);

            if (targetMaterial != null)
            {
                meshRenderer.sharedMaterial = targetMaterial;
            }
        }

        if (healthText != null)
        {
            healthText.transform.localPosition = new Vector3(0f, (hexaVisualMeshRenderers.Count + 1) * yOffset, 0f);
        }
    }

    private bool CanSyncVisuals()
    {
        return hexaVisual != null;
    }

    private Material GetMaterialForCurrentColor()
    {
        if (materialsSO == null || materialsSO.Materials == null)
        {
            return null;
        }

        int colorIndex = (int)hexaColors;
        if (colorIndex < 0 || colorIndex >= materialsSO.Materials.Length)
        {
            Debug.LogWarning($"Material for color {hexaColors} is not defined in MaterialsSO.", this);
            return null;
        }

        return materialsSO.Materials[colorIndex];
    }

    private GameObject SpawnHexaVisual()
    {
        if (hexaVisual == null)
        {
            Debug.LogWarning("Hexa visual prefab reference is missing.", this);
            return null;
        }

#if UNITY_EDITOR
        var prefabInstance = PrefabUtility.InstantiatePrefab(hexaVisual, transform) as GameObject;
        if (prefabInstance != null)
        {
            return prefabInstance;
        }
#endif

        return Instantiate(hexaVisual, transform);
    }

    private void DestroySpawnedObject(GameObject spawnedObject)
    {
        if (spawnedObject == null)
        {
            return;
        }

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            EditorApplication.delayCall += () =>
            {
                if (spawnedObject != null)
                {
                    DestroyImmediate(spawnedObject);
                }
            };

            return;
        }
#endif

        Destroy(spawnedObject);
    }
}
