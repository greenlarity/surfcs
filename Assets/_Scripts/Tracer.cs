using _Scripts;
using UnityEngine;

public class Tracer
{
    public static Trace TraceCollider(Collider collider, Vector3 origin, Vector3 end, int layerMask)
    {
        if (collider is BoxCollider)
        {
            return TraceBox(origin, end, collider.bounds.extents, collider.contactOffset, layerMask);
        }
        else if (collider is CapsuleCollider capc)
        {
            SurfPhysics.GetCapsulePoints(capc, origin, out Vector3 point1, out Vector3 point2);
            return TraceCapsule(point1, point2, capc.radius, origin, end, capc.contactOffset, layerMask);
        }

        throw new System.NotImplementedException("Trace missing for collider: " + collider.GetType());
    }

    public static Trace TraceCapsule(Vector3 point1, Vector3 point2, float radius, Vector3 start, Vector3 destination,
        float contactOffset, int layerMask)
    {
        var result = new Trace()
        {
            StartPos = start,
            EndPos = destination
        };

        var longSide = Mathf.Sqrt(contactOffset * contactOffset + contactOffset * contactOffset);
        radius *= (1f - contactOffset);
        var direction = (destination - start).normalized;
        var maxDistance = Vector3.Distance(start, destination) + longSide;

        if (Physics.CapsuleCast(
                point1: point1,
                point2: point2,
                radius: radius,
                direction: direction,
                hitInfo: out RaycastHit hit,
                maxDistance: maxDistance,
                layerMask: layerMask))
        {
            result.Fraction = hit.distance / maxDistance;
            result.HitCollider = hit.collider;
            result.HitPoint = hit.point;
            result.PlaneNormal = hit.normal;
        }
        else
        {
            result.Fraction = 1;
        }

        return result;
    }

    public static Trace TraceBox(Vector3 start, Vector3 destination, Vector3 extents, float contactOffset,
        int layerMask)
    {
        var result = new Trace()
        {
            StartPos = start,
            EndPos = destination
        };

        var longSide = Mathf.Sqrt(contactOffset * contactOffset + contactOffset * contactOffset);
        var direction = (destination - start).normalized;
        var maxDistance = Vector3.Distance(start, destination) + longSide;
        extents *= (1f - contactOffset);

        if (Physics.BoxCast(center: start,
                halfExtents: extents,
                direction: direction,
                orientation: Quaternion.identity,
                maxDistance: maxDistance,
                hitInfo: out RaycastHit hit,
                layerMask: layerMask))
        {
            result.Fraction = hit.distance / maxDistance;
            result.HitCollider = hit.collider;
            result.HitPoint = hit.point;
            result.PlaneNormal = hit.normal;
        }
        else
        {
            result.Fraction = 1;
        }

        return result;
    }
}