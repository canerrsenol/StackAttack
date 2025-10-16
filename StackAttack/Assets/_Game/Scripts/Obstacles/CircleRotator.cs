using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CircleRotator : MonoBehaviour
{
    [SerializeField] private float r = 1f;
    [SerializeField] private int numberOfTargets = 6;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private HexaTarget hexaTarget;
    [SerializeField, HideInInspector] private List<HexaTarget> hexaTargets = new List<HexaTarget>();

    void OnValidate()
    {
        if (!gameObject.scene.IsValid())
        {
            return;
        }

        RebuildTargets();
    }

    public void RebuildTargets()
    {
        numberOfTargets = Mathf.Max(0, numberOfTargets);
        r = Mathf.Max(0f, r);

        EnsureListMatchesChildren();

        if (numberOfTargets == 0)
        {
            RemoveAllTargets();
            return;
        }

        RemoveExcessTargets();

        if (hexaTarget == null)
        {
            ArrangeTargetsOnCircle();
            return;
        }

        SpawnMissingTargets();
        ArrangeTargetsOnCircle();
    }

    void Update()
    {
        transform.Rotate(-Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void EnsureListMatchesChildren()
    {
        for (int i = hexaTargets.Count - 1; i >= 0; i--)
        {
            var target = hexaTargets[i];
            if (target == null || target.transform.parent != transform)
            {
                hexaTargets.RemoveAt(i);
            }
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var target = child.GetComponent<HexaTarget>();
            if (target != null && !hexaTargets.Contains(target))
            {
                hexaTargets.Add(target);
            }
        }

        hexaTargets.Sort((a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));
    }

    private void RemoveAllTargets()
    {
        for (int i = hexaTargets.Count - 1; i >= 0; i--)
        {
            DestroyTarget(hexaTargets[i]);
            hexaTargets.RemoveAt(i);
        }
    }

    private void RemoveExcessTargets()
    {
        for (int i = hexaTargets.Count - 1; i >= numberOfTargets; i--)
        {
            DestroyTarget(hexaTargets[i]);
            hexaTargets.RemoveAt(i);
        }
    }

    private void SpawnMissingTargets()
    {
        while (hexaTargets.Count < numberOfTargets)
        {
            var spawned = SpawnTarget();
            if (spawned == null)
            {
                break;
            }

            hexaTargets.Add(spawned);
        }
    }

    private void ArrangeTargetsOnCircle()
    {
        if (hexaTargets.Count == 0)
        {
            return;
        }

        float angleStep = Mathf.PI * 2f / hexaTargets.Count;

        for (int i = 0; i < hexaTargets.Count; i++)
        {
            var target = hexaTargets[i];
            if (target == null)
            {
                continue;
            }

            float angle = angleStep * i;
            Vector3 localPosition = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * r;
            target.transform.SetParent(transform);
            target.transform.localPosition = localPosition;

            if (localPosition.sqrMagnitude > 0f)
            {
                target.transform.localRotation = Quaternion.LookRotation(-localPosition.normalized, Vector3.up);
            }
            else
            {
                target.transform.localRotation = Quaternion.identity;
            }
        }
    }

    private HexaTarget SpawnTarget()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            var source = hexaTarget;
            if (source == null)
            {
                return null;
            }

            GameObject editorInstance;
            var prefab = source.gameObject;
            if (prefab == null)
            {
                return null;
            }

            var prefabType = PrefabUtility.GetPrefabAssetType(prefab);
            if (prefabType == PrefabAssetType.NotAPrefab)
            {
                var instancedComponent = Instantiate(source, transform);
                editorInstance = instancedComponent != null ? instancedComponent.gameObject : null;
            }
            else
            {
                editorInstance = PrefabUtility.InstantiatePrefab(prefab, transform) as GameObject;
            }

            if (editorInstance == null)
            {
                return null;
            }

            var target = editorInstance.GetComponent<HexaTarget>();
            if (target == null)
            {
                Undo.DestroyObjectImmediate(editorInstance);
                return null;
            }

            Undo.RegisterCreatedObjectUndo(editorInstance, "Spawn Hexa Target");
            return target;
        }
#endif

        return Instantiate(hexaTarget, transform);
    }

    private void DestroyTarget(HexaTarget target)
    {
        if (target == null)
        {
            return;
        }

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            Undo.DestroyObjectImmediate(target.gameObject);
            return;
        }
#endif

        Destroy(target.gameObject);
    }
}
