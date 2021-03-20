using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody2D me; // Vampire body
    public float normalSpeed; // Unit speed without effects

    private float targetX; // Local coordinates it wants to move to
    private float targetY;

    [Header("Do NOT set values below in Inspector")]
    public float speed; // Movement speed in the current frame

    void Start()
    {
        speed = normalSpeed;
    }

    void Update()
    {
        
        Vector2 direction = new Vector2(targetX, targetY).normalized;
        me.velocity = direction*speed;
    }

    public void setTargetVector(Vector2 vec)
    {
        targetX = vec.x;
        targetY = vec.y;
    }
}

public struct Point
{
    public Point(float x, float y) { this.x = x; this.y = y; }
    public float x;
    public float y;
    public Vector3 getPosition() { return new Vector3(x, y, 0); }
}

public static class MovementPointsCalculator
{
    const float Xmarign = 2f;
    const float Ymarign = 7f;

    public const int CODE_TOP = 0;
    public const int CODE_LEFT = 1;
    public const int CODE_RIGHT = 2;
    public const int CODE_BOTTOM = 3;

    public static Point GetTopRightPoint(Collider2D obstacle, Collider2D myLegsCollider)
    {
        float nextX = obstacle.bounds.max.x + myLegsCollider.bounds.extents.x * Xmarign;
        float nextY = obstacle.bounds.max.y + myLegsCollider.bounds.extents.y * Ymarign;
        return new Point(nextX, nextY);
    }

    public static Point GetTopLeftPoint(Collider2D obstacle, Collider2D myLegsCollider)
    {
        float nextX = obstacle.bounds.min.x - myLegsCollider.bounds.extents.x * Xmarign;
        float nextY = obstacle.bounds.max.y + myLegsCollider.bounds.extents.y * Ymarign;
        return new Point(nextX, nextY);
    }

    public static Point GetBottomLeftPoint(Collider2D obstacle, Collider2D myLegsCollider)
    {
        float nextX = obstacle.bounds.min.x - myLegsCollider.bounds.extents.x * Xmarign;
        float nextY = obstacle.bounds.min.y - myLegsCollider.bounds.extents.y * Ymarign;
        return new Point(nextX, nextY);
    }

    public static Point GetBottomRightPoint(Collider2D obstacle, Collider2D myLegsCollider)
    {
        float nextX = obstacle.bounds.max.x + myLegsCollider.bounds.extents.x * Xmarign;
        float nextY = obstacle.bounds.min.y - myLegsCollider.bounds.extents.y * Ymarign;
        return new Point(nextX, nextY);
    }

    public static List<Point> GetRandomTopSolve(Collider2D obstacle, Collider2D legs)
    {
        List<Point> res = new List<Point>();
        if (Random.value >= 0.5f)
        {
            res.Add(GetTopLeftPoint(obstacle, legs));
            res.Add(GetBottomLeftPoint(obstacle, legs));
        }
        else
        {
            res.Add(GetTopRightPoint(obstacle, legs));
            res.Add(GetBottomRightPoint(obstacle, legs));
        }
        return res;
    }

    public static List<Point> GetRandomRightSolve(Collider2D obstacle, Collider2D legs)
    {
        List<Point> res = new List<Point>();
        if (Random.value >= 0.5f)
        {
            res.Add(GetBottomRightPoint(obstacle, legs));
            res.Add(GetBottomLeftPoint(obstacle, legs));
        }
        else
        {
            res.Add(GetTopRightPoint(obstacle, legs));
            res.Add(GetTopLeftPoint(obstacle, legs));
        }
        return res;
    }

    public static List<Point> GetRandomBottomSolve(Collider2D obstacle, Collider2D legs)
    {
        List<Point> res = new List<Point>();
        if (Random.value >= 0.5f)
        {
            res.Add(GetBottomLeftPoint(obstacle, legs));
            res.Add(GetTopLeftPoint(obstacle, legs));
        }
        else
        {
            res.Add(GetBottomRightPoint(obstacle, legs));
            res.Add(GetTopRightPoint(obstacle, legs));
        }
        return res;
    }

    public static List<Point> GetRandomLeftSolve(Collider2D obstacle, Collider2D legs)
    {
        List<Point> res = new List<Point>();
        if (Random.value >= 0.5f)
        {
            res.Add(GetTopLeftPoint(obstacle, legs));
            res.Add(GetTopRightPoint(obstacle, legs));
        }
        else
        {
            res.Add(GetBottomLeftPoint(obstacle, legs));
            res.Add(GetBottomRightPoint(obstacle, legs));
        }
        return res;
    }

    public static List<Point> GetRandomPathForOurSituation(int code, Collider2D obstacle, Collider2D legs)
    {
        List<Point> res = new List<Point>();
        if (obstacle == null || legs == null)
        {
            Debug.Log("Brain sent NULL obstacle or legs collider to MovementCalculator. Path won't be calculated!");
            return res;
        }

        switch (code)
        {
            case CODE_TOP: res = GetRandomTopSolve(obstacle, legs); break;
            case CODE_LEFT: res = GetRandomLeftSolve(obstacle, legs); break;
            case CODE_RIGHT: res = GetRandomRightSolve(obstacle, legs); break;
            case CODE_BOTTOM: res = GetRandomBottomSolve(obstacle, legs); break;
            default: Debug.Log("Brain sent wrong situation code. It is NOT possible to solve ambiguity situation. But error is not critical, we can continue the fight."); break;
        }

        return res;
    }
}
