using UnityEngine;

namespace MiddleMast.GameplayFramework.DogsAndBillboards
{
    public class BillboardComponent : MonoBehaviour
    {
        private void LateUpdate()
        {
            Camera mainCamera = Camera.main;
            Vector3 lookDirection = (mainCamera.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }
}
