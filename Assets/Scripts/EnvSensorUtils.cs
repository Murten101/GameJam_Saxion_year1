using UnityEngine;

public static class EnvSensorUtils  
{
    private static Vector2 GetGroundCheckPos(Vector2 origin ,int index, int resolution, float width)
    {
        var pos = width * -0.5f;
        var offset = width / (resolution - 1) * index;
        return new Vector2(origin.x + pos + offset, origin.y);
    }

    public static bool Check(int resolution,float width, Vector2 position, Vector2 direction, LayerMask layerMask, float range = 0.2f, float maxAngleDifference = 45f)
    {
        return Check2(resolution, width, position, direction, layerMask, range, maxAngleDifference).collider != null;
    }
        public static RaycastHit2D Check2(int resolution,float width, Vector2 position, Vector2 direction, LayerMask layerMask, float range = 0.2f, float maxAngleDifference = 45f)
    {
        for (int i = 0; i < resolution; i++)
        {
            var rayOrigin = GetGroundCheckPos(position ,i , resolution, width);
            var hit = Physics2D.Raycast(rayOrigin, direction, range, layerMask);

            if (hit.collider == null)
            {
                Debug.DrawRay(rayOrigin, direction * range, Color.red);
                continue;
            }

            if (Vector2.Angle(-direction, hit.normal) > maxAngleDifference)
            {
                Debug.DrawRay(rayOrigin, direction * range, Color.yellow);
                continue;
            }

            Debug.DrawRay(rayOrigin, direction * range, Color.green);
            return hit;
        }
        return default;
    }
}
