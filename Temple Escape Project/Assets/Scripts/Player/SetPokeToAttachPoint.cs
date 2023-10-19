using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetPoke : MonoBehaviour
{
    [Header("Attach Point")]
    public Transform pokeAttachPoint;

    [Header("Interactor")]
    private XRPokeInteractor _pokeInteractor;

    private void Start()
    {
        _pokeInteractor = transform.parent.parent.GetComponentInChildren<XRPokeInteractor>();
    }

    private void SetAttachPoint()
    {
        if (pokeAttachPoint == null)
        {
            Debug.Log("Poke Attach Point is null");

            return;
        }

        if (_pokeInteractor == null)
        {
            Debug.Log("XR Poke Interactor is null");

            return;
        }

        _pokeInteractor.attachTransform = pokeAttachPoint;
    }
}
