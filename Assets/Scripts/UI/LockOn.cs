using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{
    [SerializeField] Transform cameraTransform; 

    void OnEnable(){
        
        if (cameraTransform == null) {
            cameraTransform = Camera.main.transform;
        }
        
        StartCoroutine(LookAtTarget());
    }

    private IEnumerator LookAtTarget(){
        while(this.gameObject.activeInHierarchy){
            if (cameraTransform != null) {
                Vector3 directionToCamera = cameraTransform.position - transform.position;
                if (directionToCamera != Vector3.zero) {
                    transform.rotation = Quaternion.LookRotation(directionToCamera);
                }
            }

            yield return null;
        }
    }
}
