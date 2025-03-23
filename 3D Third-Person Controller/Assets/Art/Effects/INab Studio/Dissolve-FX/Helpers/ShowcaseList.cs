using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INab.Dissolve
{

    public class ShowcaseList : MonoBehaviour
    {
        [System.Serializable]
        public class Asset
        {
            public GameObject obj;
            public float time = 2;
            public bool materialize = false;
        }

        public List<Asset> list = new List<Asset>();

        private int currentID = 0;

        private void Start()
        {
            foreach (var item in list)
            {
                item.obj.SetActive(false);
            }

            StartCoroutine(PlayShowcase());
        }

        private IEnumerator PlayShowcase()
        {
            while (true)
            {
                Asset currentAsset = list[currentID];

                // Activate the object
                currentAsset.obj.SetActive(true);

                var dissolver = currentAsset.obj.GetComponent<INab.Dissolve.Dissolver>();
                dissolver.Dissolve();

                yield return new WaitForSeconds(currentAsset.time);

                if (currentAsset.materialize)
                {
                    dissolver.Materialize();

                    yield return new WaitForSeconds(currentAsset.time);
                }

                // Deactivate the object
                currentAsset.obj.SetActive(false);

                // Move to the next asset
                currentID = (currentID + 1) % list.Count;
            }
        }
    }
}