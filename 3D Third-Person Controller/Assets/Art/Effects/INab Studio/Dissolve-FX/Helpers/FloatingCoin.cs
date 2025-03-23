using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INab.Dissolve
{
    public class FloatingCoin : MonoBehaviour
    {
        public float rotationSpeed = 100f;
        public float floatSpeed = 0.5f;
        public float floatHeight = 0.5f;

        private Vector3 startPosition;

        private void Start()
        {
            startPosition = transform.position;
        }

        private void Update()
        {
            // Rotate the coin
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

            // Float the coin up and down
            Vector3 newPosition = startPosition + new Vector3(0f, Mathf.Sin(Time.time * floatSpeed) * floatHeight, 0f);
            transform.position = newPosition;
        }
    }
}