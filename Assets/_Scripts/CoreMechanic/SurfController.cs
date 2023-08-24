using _Scripts;
using UnityEngine;

public class SurfController
{
    private ISurfControllable _surfControllable;
    private MovementConfig _movementConfig;
    private float _deltaTime;

    public void ProcessMovement(ISurfControllable surfControllable, MovementConfig movementConfig, float deltaTime)
    {
        _surfControllable = surfControllable;
        _movementConfig = movementConfig;
        _deltaTime = deltaTime;

        if (_surfControllable.PlayerData.GroundObject == null)
        {
            _surfControllable.PlayerData.Velocity.y -=
                _surfControllable.PlayerData.GravityFactor * _movementConfig.Gravity * _deltaTime;
            _surfControllable.PlayerData.Velocity.y += _surfControllable.BaseVelocity.y * _deltaTime;
        }

        CheckGrounded();
        CalculateMovementVelocity();

        _surfControllable.PlayerData.Origin += _surfControllable.PlayerData.Velocity * _deltaTime;
        SurfPhysics.ResolveCollisions(_surfControllable.CapsuleCollider, ref _surfControllable.PlayerData.Origin,
            ref _surfControllable.PlayerData.Velocity);
        _surfControllable = null;
    }

    private void CalculateMovementVelocity()
    {
        if (_surfControllable.PlayerData.GroundObject == null)
        {
            // apply movement from input
            _surfControllable.PlayerData.Velocity += AirInputMovement();

            // let the magic happen
            SurfPhysics.Reflect(ref _surfControllable.PlayerData.Velocity, _surfControllable.CapsuleCollider, _surfControllable.PlayerData.Origin, _deltaTime);
        }
        else
        {
            // apply movement from input
            _surfControllable.PlayerData.Velocity += GroundInputMovement();

            // jump/friction
            if (_surfControllable.InputData.JumpPressed)
            {
                Jump();
            }
            else
            {
                var friction = _surfControllable.PlayerData.SurfaceFriction * _movementConfig.Friction;
                var stopSpeed = _movementConfig.StopSpeed;
                SurfPhysics.Friction(ref _surfControllable.PlayerData.Velocity, stopSpeed, friction, _deltaTime);
            }
        }
    }

    private bool CheckGrounded()
    {
        _surfControllable.PlayerData.SurfaceFriction = 1f;
        var movingUp = _surfControllable.PlayerData.Velocity.y > 0f;
        var trace = TraceToFloor();
        
        if (trace.HitCollider == null
            || trace.HitCollider.gameObject.layer == LayerMask.NameToLayer("Trigger")
            || trace.PlaneNormal.y < 0.7f
            || movingUp)
        {
            SetGround(null);
            if (movingUp)
                _surfControllable.PlayerData.SurfaceFriction = _movementConfig.AirFriction;
            return false;
        }
        else
        {
            SetGround(trace.HitCollider.gameObject);
            return true;
        }
    }

    private Trace TraceToFloor()
    {
        var down = _surfControllable.PlayerData.Origin;
        down.y -= 0.1f;
        return Tracer.TraceCollider(_surfControllable.CapsuleCollider, _surfControllable.PlayerData.Origin, down,
            SurfPhysics.GroundLayerMask);
    }
    
    private Vector3 GroundInputMovement()
    {
        GetWishValues(out var wishVel, out var wishDir, out var wishSpeed);

        if ((wishSpeed != 0.0f) && (wishSpeed > _movementConfig.MaxSpeed))
        {
            wishVel *= _movementConfig.MaxSpeed / wishSpeed;
            wishSpeed = _movementConfig.MaxSpeed;
        }

        wishSpeed *= _surfControllable.PlayerData.WalkFactor;

        return SurfPhysics.Accelerate(_surfControllable.PlayerData.Velocity, wishDir,
            wishSpeed, _movementConfig.Accel, _deltaTime, _surfControllable.PlayerData.SurfaceFriction);
    }

    private Vector3 AirInputMovement()
    {
        GetWishValues(out Vector3 wishVel, out Vector3 wishDir, out float wishSpeed);

        if (_movementConfig.ClampAirSpeed && (wishSpeed != 0f && (wishSpeed > _movementConfig.MaxSpeed)))
        {
            wishVel = wishVel * (_movementConfig.MaxSpeed / wishSpeed);
            wishSpeed = _movementConfig.MaxSpeed;
        }

        return SurfPhysics.AirAccelerate(_surfControllable.PlayerData.Velocity, wishDir,
            wishSpeed, _movementConfig.AirAccel, _movementConfig.AirCap, _deltaTime);
    }
    
    private void GetWishValues(out Vector3 wishVel, out Vector3 wishDir, out float wishSpeed)
    {
        wishVel = Vector3.zero;
        wishDir = Vector3.zero;
        wishSpeed = 0f;

        Vector3 forward = _surfControllable.FpsCamera.transform.forward,
            right = _surfControllable.FpsCamera.transform.right;

        forward[1] = 0;
        right[1] = 0;
        forward.Normalize();
        right.Normalize();

        for (var i = 0; i < 3; i++)
            wishVel[i] = forward[i] * _surfControllable.InputData.ForwardMove + right[i] * _surfControllable.InputData.SideMove;
        wishVel[1] = 0;

        wishSpeed = wishVel.magnitude;
        wishDir = wishVel.normalized;
    }
    
    private void Jump()
    {
        if (!_movementConfig.AutoBhop && _surfControllable.InputData.JumpPressedLastUpdate)
            return;

        _surfControllable.PlayerData.Velocity.y += _movementConfig.JumpPower;
    }
    
    private void SetGround(GameObject obj)
    {
        if (obj != null)
        {
            _surfControllable.PlayerData.GroundObject = obj;
            _surfControllable.PlayerData.Velocity.y = 0;
        }
        else
        {
            _surfControllable.PlayerData.GroundObject = null;
        }
    }
}