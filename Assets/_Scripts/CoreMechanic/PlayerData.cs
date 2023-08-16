using UnityEngine;

public class PlayerData
{
    public Vector3 Origin;
    public Vector3 ViewAngles;
    public Vector3 Velocity;
    public float SurfaceFriction = 1f;
    public float GravityFactor = 1f;
    public float WalkFactor = 1f;
    public GameObject GroundObject;

    public bool IsGrounded()
    {
        return GroundObject != null;
    }
    
}
