using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace INab.Dissolve { 

public class PrefabsPlacer : MonoBehaviour
{
    public GameObject prefab;
    public int numberOfPlaces = 10;
    public float xOffset = 1f;
    public List<Material> materialsToSet;

#if UNITY_EDITOR
    [ContextMenu("Place Prefabs")]
    public void PlacePrefabs()
    {
        // Clear existing prefabs
        ClearPrefabs();

        // Place new prefabs
        for (int i = 0; i < numberOfPlaces; i++)
        {
            Vector3 position = transform.position + new Vector3(i * xOffset, 0f, 0f);
            GameObject instantiatedPrefab = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            instantiatedPrefab.transform.position = position;
            instantiatedPrefab.transform.parent = transform;

            instantiatedPrefab.GetComponent<Renderer>().material = materialsToSet[i];
            instantiatedPrefab.GetComponent<INab.Dissolve.Dissolver>().FindMaterials();
        }
    }

    [ContextMenu("Clear Prefabs")]
    public void ClearPrefabs()
    {
        // Remove all existing prefabs
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(PrefabsPlacer))]
public class PrefabPlacerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PrefabsPlacer prefabPlacer = (PrefabsPlacer)target;

        if (GUILayout.Button("Place Prefabs"))
        {
            prefabPlacer.PlacePrefabs();
        }

        if (GUILayout.Button("Clear Prefabs"))
        {
            prefabPlacer.ClearPrefabs();
        }
    }
}
#endif
}
