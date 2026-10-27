using System;
using UnityEngine;

public static class NoiseSystem
{
    public static event Action<Vector3, float> OnNoise;

    public static void Emit(Vector3 position, float radius)
    {
        if (OnNoise != null)
            OnNoise(position, radius);
    }
}
