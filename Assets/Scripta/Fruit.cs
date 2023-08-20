using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject whole;
    public GameObject sliced;

    private Rigidbody fruitRigidBody;
    private Collider fruitCollider;
    private ParticleSystem juiceParticleEffect;

    public int point = 1;

    private void Awake()
    {
        fruitCollider = GetComponent<Collider>();
        fruitRigidBody = GetComponent<Rigidbody>();
        juiceParticleEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void Slice(Vector3 direction, Vector3 position, float force)
    {
        FindObjectOfType<GameManager>().IncreaseScore(point);

        whole.SetActive(false);
        sliced.SetActive(true);

        fruitCollider.enabled = false;
        juiceParticleEffect.Play();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody slice in slices)
        {
            slice.velocity = fruitRigidBody.velocity;
            slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
        }


    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Blade blade = other.GetComponent<Blade>();
            blade.bladeSound.Play();
            Slice(blade.direction, blade.transform.position, blade.sliceForce);

        }
    }



}
