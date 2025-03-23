using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INab.Dissolve
{
    [ExecuteInEditMode]
    public class Dissolver : MonoBehaviour
    {
        #region Enums
        public enum CurveSettings
        {
            TwoCurves,          // Two separate curves for dissolve and materialize
            OneCurve,           // One curve for both dissolve and materialize
        }

        public enum DissolveState
        {
            Dissolved,      // Object is fully dissolved
            Materialized    // Object is fully materialized
        }

        [System.Flags]
        public enum KeywordsFlags
        {
            UseDissolve = 1,            // Flag for using dissolve keyword in materials
            UseVertexDisplacement = 2,   // Flag for using vertex displacement keyword in materials
        }

        #endregion

        #region ManualControl
        public bool manualControl = false;

        [Tooltip("When turned on, material values will always be updated in the update() function. Turn it off when you want to modify the dissolve amount property in materials. It is automatically turned on in the Start() function and needs to be on during runtime.")]
        public bool updateValues = true;

        [SerializeField, Range(-1, 2)]
        [Tooltip("Value of the dissolve amount property in the materials list.")]
        private float MaterialsDissolveValue = 1f;

        [SerializeField,Range(-1,2)]
        [Tooltip("Value of the dissolve amount property in the inverted materials list.")]
        private float MaterialsInvertedDissolveValue = 0f;

        #endregion

        #region AutomaticControl
        [Tooltip("How to evaluate the curves.")]
        public CurveSettings curveSettings = CurveSettings.OneCurve;

        public AnimationCurve dissolveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public AnimationCurve materializeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

        [Tooltip("Duration of the effect.")]
        public float duration = 2f;

        [Tooltip("Initial state the object will be set on Start().")]
        public DissolveState initialState = DissolveState.Materialized;

        [Tooltip("Current state of the object.")]
        public DissolveState currentState;

        [Tooltip("Indicates which keywords should be automatically enabled and disabled when needed.")]
        public KeywordsFlags keywordsFlags;

        [Tooltip("Whether to use automatic keywords which make sure shader do not render any unnessesery stuff.")]
        public bool useAutomaticKeywords = false;

        #endregion


        [Tooltip("Materials the effect will be performed on.")]
        public List<Material> materials = new List<Material>();

        [Tooltip("Materials the effect will be performed on in inverted manner.")]
        public List<Material> materialsInverted = new List<Material>();


        #region VFXGraph
        // Delegates used with visual effect graph

        public delegate void DissolveAmountChange(float value);
        public event DissolveAmountChange OnPropertyUpdate;

        public delegate void DissolveEvent(bool start, bool materialize);
        public event DissolveEvent OnDissolveStateChange;

        #endregion

        private void OnEnable()
        {
            currentState = initialState;
        }

        public void Start()
        {
            updateValues = true;

            if (materials.Count == 0)
            {
                FindMaterials();
            }

            if (initialState == DissolveState.Dissolved)
            {
                foreach (var material in materials)
                {
                    ChangeDissolveAmount(material, 1);
                }

                foreach (var material in materialsInverted)
                {
                    ChangeDissolveAmount(material, 0);
                }
            }
            else
            {
                foreach (var material in materials)
                {
                    ChangeDissolveAmount(material, 0);
                }

                foreach (var material in materialsInverted)
                {
                    ChangeDissolveAmount(material, 1);
                }
            }

        }

        public void Update()
        {
            if (manualControl && updateValues)
            {
                ManualValuesUpdate();
            }
        }

        #region PublicFunctions

        /// <summary>
        /// Find and initialize the materials to be dissolved/materialized using GetComponentsInChildren
        /// </summary>
        public void FindMaterialsInChildren()
        {
            materials.Clear();
            foreach(var item in GetComponentsInChildren<Renderer>())
            {
                materials.AddRange(item.sharedMaterials);
            }
        }

        /// <summary>
        /// Find and initialize the materials to be dissolved/materialized using GetComponents
        /// </summary>
        public void FindMaterials()
        {
            materials.Clear();
            foreach (var item in GetComponents<Renderer>())
            {
                materials.AddRange(item.sharedMaterials);

            }
        }

        /// <summary>
        /// Materialize the object
        /// </summary>
        public void Materialize()
        {
            EnableKeywords();

            if (materials.Count == 0)
            {
                Debug.LogWarning("There are no materials to materialize in " + name);
                return;
            }

            //if (currentState == DissolveState.Materialized)
            //{
            //    Debug.LogWarning("You are trying to materialize an already materialized object in " + name);
            //    return;
            //}

            StartCoroutine(MaterializeEnumerator());
        }

        /// <summary>
        /// Dissolve the object
        /// </summary>
        public void Dissolve()
        {
            EnableKeywords();

            if (materials.Count == 0)
            {
                Debug.LogWarning("There are no materials to dissolve in " + name);
                return;
            }

            //if (currentState == DissolveState.Dissolved)
            //{
            //    Debug.LogWarning("You are trying to dissolve an already dissolved object in " + name);
            //    return;
            //}

            StartCoroutine(DissolveEnumerator());
        }

        /// <summary>
        /// Update dissolve amount properties in materials and materials inverted.
        /// </summary>
        public void ManualValuesUpdate()
        {
            foreach (var material in materials)
            {
                // Change material value
                material.SetFloat("_DissolveAmount", MaterialsDissolveValue);

                // Call event for visual effect
                if (OnPropertyUpdate != null) OnPropertyUpdate(MaterialsDissolveValue);
            }

            foreach (var material in materialsInverted)
            {
                // Change material value
                // Do not call Dissolver VFX update event
                material.SetFloat("_DissolveAmount", MaterialsInvertedDissolveValue);
            }
        }

        #endregion

        #region PrivateFunctions

        /// <summary>
        /// Update _DissolveAmount property in material and call OnPropertyUpdate() event.
        /// </summary>
        /// <param name="material"></param>
        /// <param name="dissolveAmount"></param>
        private void ChangeDissolveAmount(Material material, float dissolveAmount)
        {
            // Change material value
            material.SetFloat("_DissolveAmount", dissolveAmount);

            // Call event for visual effect
            if (OnPropertyUpdate != null) OnPropertyUpdate(dissolveAmount);
        }


        /// <summary>
        /// Calls delegate that sends vfx graph events.
        /// </summary>
        /// <param name="start">Whether the dissolve starts or not.</param>
        /// <param name="materialize"></param>
        private void ChangeDissolveState(bool start, bool materialize = false)
        {
            // Call event for visual effect
            if (OnDissolveStateChange != null) OnDissolveStateChange(start,materialize);
        }

        // Check if a flag is set in a bitmask
        private static bool HasFlag(KeywordsFlags a, KeywordsFlags b)
        {
            return (a & b) == b;
        }

        // Enable keywords in the materials based on the flags
        private void EnableKeywords()
        {
            if (!useAutomaticKeywords) return;

            if (HasFlag(keywordsFlags, KeywordsFlags.UseDissolve))
            {
                foreach (var material in materials)
                {
                    material.EnableKeyword("_USE_DISSOLVE");
                }

                foreach (var material in materialsInverted)
                {
                    material.EnableKeyword("_USE_DISSOLVE");
                }
            }

            if (HasFlag(keywordsFlags, KeywordsFlags.UseVertexDisplacement))
            {
                foreach (var material in materials)
                {
                    material.EnableKeyword("_USE_VERTEX_DISPLACEMENT");
                }

                foreach (var material in materialsInverted)
                {
                    material.EnableKeyword("_USE_VERTEX_DISPLACEMENT");
                }
            }

        }

        // Disable keywords in the materials based on the flags
        private void DisableKeywords()
        {
            if (!useAutomaticKeywords) return;

            if (HasFlag(keywordsFlags, KeywordsFlags.UseDissolve))
            {
                foreach (var material in materials)
                {
                    material.DisableKeyword("_USE_DISSOLVE");
                }

                foreach (var material in materialsInverted)
                {
                    material.DisableKeyword("_USE_DISSOLVE");
                }
            }

            if (HasFlag(keywordsFlags, KeywordsFlags.UseVertexDisplacement))
            {
                foreach (var material in materials)
                {
                    material.DisableKeyword("_USE_VERTEX_DISPLACEMENT");
                }

                foreach (var material in materialsInverted)
                {
                    material.DisableKeyword("_USE_VERTEX_DISPLACEMENT");
                }
            }
        }

        // Coroutine to gradually materialize the object
        private IEnumerator MaterializeEnumerator()
        {
            float dissolveAmount;
            float elapsedTime = 0f;

            AnimationCurve curve;
            if (curveSettings == CurveSettings.TwoCurves)
            {
                curve = materializeCurve;   // Use the materialize curve for materialization
            }
            else
            {
                curve = dissolveCurve;      // Use the dissolve curve for materialization
            }

            // Called for VFX graph events
            ChangeDissolveState(true,true);

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                if (curveSettings == CurveSettings.OneCurve)
                {
                    dissolveAmount = curve.Evaluate(1 - elapsedTime / duration);   // Evaluate the curve in reverse if it is flipped
                }
                else
                {
                    dissolveAmount = curve.Evaluate(elapsedTime / duration);
                }

                foreach (var material in materials)
                {
                    ChangeDissolveAmount(material, dissolveAmount);
                }

                foreach (var material in materialsInverted)
                {
                    ChangeDissolveAmount(material, 1 - dissolveAmount);
                }

                yield return null;
            }

            currentState = DissolveState.Materialized;
            DisableKeywords();

            // Called for VFX graph events
            ChangeDissolveState(false,true);
        }

        // Coroutine to gradually dissolve the object
        private IEnumerator DissolveEnumerator()
        {
            float dissolveAmount;
            float elapsedTime = 0f;

            AnimationCurve curve = dissolveCurve;   // Use the materialize curve for dissolution

            ChangeDissolveState(true);

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                dissolveAmount = curve.Evaluate(elapsedTime / duration);   // Evaluate the curve in reverse for dissolution

                foreach (var material in materials)
                {
                    ChangeDissolveAmount(material, dissolveAmount);
                }

                foreach (var material in materialsInverted)
                {
                    ChangeDissolveAmount(material, 1 - dissolveAmount);
                }

                yield return null;
            }

            currentState = DissolveState.Dissolved;
            DisableKeywords();

            // Called for VFX graph events
            ChangeDissolveState(false);
        }

        #endregion

        #region DebugAndDev

        private float coroutnieTimeOffset = .2f;

        /// <summary>
        /// Used ONLY for debug purposes and in inspector editor.
        /// </summary>
        /// <param name="dissolver"></param>
        /// <returns></returns>
        public IEnumerator AutomaticDissolveCoroutine()
        {
            float timeLasted = duration;

            currentState = DissolveState.Materialized;
            Dissolve();

            while (true)
            {
                timeLasted -= Time.deltaTime;

                if (timeLasted < -coroutnieTimeOffset)
                {
                    currentState = DissolveState.Materialized;
                    Dissolve();

                    timeLasted = duration;
                }

                yield return null; // Wait for the next frame
            }
        }

        /// <summary>
        /// Used ONLY for debug purposes and in inspector editor.
        /// </summary>
        /// <param name="dissolver"></param>
        /// <returns></returns>
        public IEnumerator AutomaticMaterializeCoroutine()
        {
            float timeLasted = duration;

            currentState = DissolveState.Dissolved;
            Materialize();

            while (true)
            {
                timeLasted -= Time.deltaTime;

                if (timeLasted < -coroutnieTimeOffset)
                {
                    currentState = DissolveState.Dissolved;
                    Materialize();

                    timeLasted = duration;
                }

                yield return null; // Wait for the next frame
            }
        }

        #endregion

    }
}