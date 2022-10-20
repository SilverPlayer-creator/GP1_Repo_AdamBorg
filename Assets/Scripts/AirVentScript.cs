using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVentScript : MonoBehaviour
{
    [SerializeField] private Transform _blowOrigin;
    [SerializeField] [Range(0, 1)] private float _sphereRadius;
    [SerializeField] [Range(0, 10)] private float _hitDistance;
    [SerializeField] [Range(0, 10)] private float _pushForce;
    [SerializeField] private LayerMask _mask;

    void Update()
    {
        RaycastHit hit;

        if (Physics.SphereCast(_blowOrigin.position, _sphereRadius, this.transform.forward, out hit, _hitDistance, _mask))
        {
            if (hit.collider.GetComponent<CharacterController>() != null)
            {
                Push(hit.collider.GetComponent<CharacterController>());
            }
        }

    }

    private void Push(CharacterController controller)
    {
        controller.Move(this.transform.forward * _pushForce * Time.deltaTime);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_blowOrigin.position, _sphereRadius);
    }
}
