﻿using UnityEngine;

namespace UnityStandardAssets.Utility
{


	public class SmoothFollow : MonoBehaviour
	{

		// The target we are following
		[SerializeField]
		public Transform target;
		// The distance in the x-z plane to the target
		[SerializeField]
		public float distance = 10.0f;
		// the height we want the camera to be above the target
		[SerializeField]
		public float height = 5.0f;

		[SerializeField]
		private float rotationDamping;
		[SerializeField]
		private float heightDamping = 0.5f;

        
       

		// Use this for initialization
		void Start() {

        }

		// Update is called once per frame
		void LateUpdate()
		{
			// Early out if we don't have a target
			if (!target)
				return;

			// Calculate the current rotation angles
			var wantedRotationAngle = target.eulerAngles.y;
            var wantedHeightRotationAngle = target.eulerAngles.x;
			var wantedHeight = target.position.y + height;


			var currentRotationAngle = transform.eulerAngles.y;
            var currentHeightRotationAngle = transform.eulerAngles.x;
			var currentHeight = transform.position.y;

			// Damp the rotation around the y-axis
			currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
            currentHeightRotationAngle = Mathf.LerpAngle(currentHeightRotationAngle, wantedHeightRotationAngle, rotationDamping * Time.deltaTime);
            // Damp the height
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

			// Convert the angle into a rotation
			var currentRotation = Quaternion.Euler(currentHeightRotationAngle, currentRotationAngle, 0);
    
            // Set the position of the camera on the x-z plane to:
            // distance meters behind the target
            transform.position = target.position;
			transform.position -= currentRotation * Vector3.forward * distance;

			// Set the height of the camera
			transform.position = new Vector3(transform.position.x ,currentHeight , transform.position.z);

			// Always look at the target
			transform.LookAt(target);
		}
	}
}