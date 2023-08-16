using UnityEngine;

public interface ISurfControllable
{
    PlayerData PlayerData { get; }
    InputData InputData { get; }
    CapsuleCollider CapsuleCollider { get; }
    Vector3 BaseVelocity { get; }
    Camera FpsCamera { get; }
}
