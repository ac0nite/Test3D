using System;
using UnityEngine;

public interface IRocketCollision
{
    event Action OnThereIsContact;
}
public class RocketCollision : MonoBehaviour, IRocketCollision
{
    private void OnCollisionEnter(UnityEngine.Collision other)
    {
        OnThereIsContact?.Invoke();
    }

    public event Action OnThereIsContact;
}