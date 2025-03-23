using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace INab.Dissolve
{
    [CustomEditor(typeof(Dissolver))]
    [CanEditMultipleObjects]
    public class DissolverEditor : Editor
    {
        SerializedProperty manualControl;
        SerializedProperty updateValues;
        SerializedProperty MaterialsDissolveValue;
        SerializedProperty MaterialsInvertedDissolveValue;


        SerializedProperty curveSettings;
        SerializedProperty dissolveCurve;
        SerializedProperty materializeCurve;
        SerializedProperty duration;
        SerializedProperty initialState;
        SerializedProperty currentState;
        SerializedProperty materials;
        SerializedProperty materialsInverted;
        SerializedProperty keywordsFlags;
        SerializedProperty useAutomaticKeywords;

        Coroutine dissolveLoop;
        Coroutine materializeLoop;

        void OnEnable()
        {
            manualControl = serializedObject.FindProperty("manualControl");
            updateValues = serializedObject.FindProperty("updateValues");
            MaterialsDissolveValue = serializedObject.FindProperty("MaterialsDissolveValue");
            MaterialsInvertedDissolveValue = serializedObject.FindProperty("MaterialsInvertedDissolveValue");

            curveSettings = serializedObject.FindProperty("curveSettings");
            dissolveCurve = serializedObject.FindProperty("dissolveCurve");
            materializeCurve = serializedObject.FindProperty("materializeCurve");
            duration = serializedObject.FindProperty("duration");
            initialState = serializedObject.FindProperty("initialState");
            currentState = serializedObject.FindProperty("currentState");
            materials = serializedObject.FindProperty("materials");
            materialsInverted = serializedObject.FindProperty("materialsInverted");
            keywordsFlags = serializedObject.FindProperty("keywordsFlags");
            useAutomaticKeywords = serializedObject.FindProperty("useAutomaticKeywords");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Dissolver dissolver = (Dissolver)target;

            EditorGUILayout.PropertyField(manualControl);

            if(manualControl.boolValue)
            {
                EditorGUILayout.PropertyField(updateValues);
                EditorGUILayout.Space();

                //EditorGUILayout.PropertyField(initialState);
                //
                //GUI.enabled = false;
                //EditorGUILayout.PropertyField(currentState);
                //GUI.enabled = true;
                //EditorGUILayout.Space();

                EditorGUILayout.PropertyField(MaterialsDissolveValue);
                EditorGUILayout.PropertyField(MaterialsInvertedDissolveValue);
                EditorGUILayout.Space();
            }
            else
            {

                EditorGUILayout.PropertyField(curveSettings);
                Dissolver.CurveSettings cs = (Dissolver.CurveSettings)curveSettings.intValue;

                if (cs == Dissolver.CurveSettings.TwoCurves)
                {
                    EditorGUILayout.PropertyField(dissolveCurve);
                    EditorGUILayout.PropertyField(materializeCurve);
                }
                else
                {
                    EditorGUILayout.PropertyField(dissolveCurve);
                }

                EditorGUILayout.PropertyField(duration);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(initialState);

                GUI.enabled = false;
                EditorGUILayout.PropertyField(currentState);
                GUI.enabled = true;

                EditorGUILayout.Space();

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(useAutomaticKeywords);
                EditorGUILayout.PropertyField(keywordsFlags);
                EditorGUILayout.Space();
            }

            EditorGUILayout.PropertyField(materials);
            EditorGUILayout.PropertyField(materialsInverted);
            EditorGUILayout.Space();


            if (GUILayout.Button("Find materials"))
            {
                dissolver.FindMaterials();
            }
            if (GUILayout.Button("Find materials in children"))
            {
                dissolver.FindMaterialsInChildren();
            }


            EditorGUILayout.Space();

            if (!manualControl.boolValue)
            {
                using (var verticalLayout = new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    EditorGUILayout.LabelField("Action Test", EditorStyles.boldLabel);
                    if (GUILayout.Button("Dissolve"))
                    {
                        if (dissolver.currentState == Dissolver.DissolveState.Dissolved)
                        {
                            dissolver.currentState = Dissolver.DissolveState.Materialized;
                        }

                        dissolver.Dissolve();
                    }
                    if (GUILayout.Button("Materialize"))
                    {
                        if (dissolver.currentState == Dissolver.DissolveState.Materialized)
                        {
                            dissolver.currentState = Dissolver.DissolveState.Dissolved;
                        }

                        dissolver.Materialize();
                    }

                }

                using (var verticalLayout = new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    EditorGUILayout.LabelField("Action Loop Test", EditorStyles.boldLabel);

                    Color oldGUIColor = GUI.backgroundColor;

                    if (dissolveLoop == null)
                    {
                        if (GUILayout.Button("Start Dissolve Loop"))
                        {
                            dissolveLoop = dissolver.StartCoroutine(dissolver.AutomaticDissolveCoroutine());
                        }
                    }
                    else
                    {
                        GUI.backgroundColor = Color.red;

                        if (GUILayout.Button("Stop Dissolve Loop"))
                        {
                            if (dissolveLoop != null) dissolver.StopCoroutine(dissolveLoop);
                            dissolveLoop = null;
                        }

                        GUI.backgroundColor = oldGUIColor;
                    }

                    EditorGUILayout.Space();

                    if (materializeLoop == null)
                    {
                        if (GUILayout.Button("Start Materialize Loop"))
                        {
                            materializeLoop = dissolver.StartCoroutine(dissolver.AutomaticMaterializeCoroutine());
                        }
                    }
                    else
                    {
                        GUI.backgroundColor = Color.red;

                        if (GUILayout.Button("Stop Materialize Loop"))
                        {
                            if (materializeLoop != null) dissolver.StopCoroutine(materializeLoop);
                            materializeLoop = null;
                        }

                        GUI.backgroundColor = oldGUIColor;
                    }

                }

                using (var verticalLayout = new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    EditorGUILayout.LabelField("Other", EditorStyles.boldLabel);

                    if (GUILayout.Button("Stop All Coroutines"))
                    {
                        dissolver.StopAllCoroutines();
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }


    }

    
}