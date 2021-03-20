using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brain : MonoBehaviour
{
    [Header("Debug. Перед релизом уберём эту парашу")]
    public GameObject movementPointer;
    public GameObject movementPointer2;
    public bool logBrain = true;

    [Header("Technical stuff")]
    [Tooltip("Put EnemyMovement script here.")]
    public EnemyMovement enemyMovement;

    [Tooltip("Put TotalCommander object here.")]
    public TotalCommander totalCommander;
    [Tooltip("Put AttackChooser script here.")]
    public AttackChooser attackChooser;
    [Tooltip("Collider of this vampire. If anyone wants to sue you and take your shirt, let him have your coat also. MF 5:40")]
    public Collider2D thisCollider;
    [Tooltip("Character animator")]
    public Animator animator;
    [Tooltip("Same animator object")]
    public Transform transformToRotate;
    [Tooltip("Yeah baby, give me your collider")]
    public Collider2D myLegsCollider;


    [Header("Дима, эту хуйню пока не трогай")]
    [Tooltip("This value describes the distance vampire want to keep between itself and the player. Recomendation: 0 <= value <= 2")]
    public float ApproachThePlayer;
    [Tooltip("DO NOT READ THIS TEXT, IT IS NOT TRUE!! \nHigher value -> more direct(forward) path to the player. Less value -> more curve. Should be > 0.")]
    public float PathStraightforwardness;
    [Tooltip("Moving engine can generate random moves. This value describes the force of DECISION of moving in random direction. Higher value -> more random.")]
    public float RandomMovingForce;

    [Tooltip("Higher value -> longer distance between obstacles and vampire")]
    public float AvoidObstacles;

    [Tooltip("Number of FRAMES when the vampire can't roll again.")]
    public int DodgeCooldown;
    [Tooltip("Multiplier of speed when dodging")]
    public float DodgeMovementSpeed;
    [Header("Fighting sytle.")]

   
    private Vector2 movementDirection; // The direction in which the vampire will move

    // values for dodge
    private int dodgeFrame = 0;
    private float speedBefore;
    private int realCooldown;
    private const int numberOfDodgeFrames = 30;
    private bool doingDodge = false;
    private Collider2D bulletWeAreAvoiding;

    // animation
    private AIAnimatorController aIAnimatorController;

    void Start()
    {
        realCooldown = DodgeCooldown;
        aIAnimatorController = new AIAnimatorController(animator, transformToRotate);

        movementPointer.SetActive(false);
        movementPointer2.SetActive(false);
    }

    void Update()
    {
        DebugShit();

        if (!weAreInTheMIddleOfAttack())
        {
            // Attacking
            if (!getIdea())
            {
                // Movement 
                if (needToDodge() || doingDodge)
                    calculateDodge();
                else
                    calculateMovement();

                move();
            }
        }
        else
        {
            stopMoving();
        }
    }

    private bool weAreInTheMIddleOfAttack()
    {
        return attackChooser.isAttacking();
    }

    private void DebugShit()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (movementPointer.activeSelf == false)
            {
                movementPointer.SetActive(true);
                movementPointer2.SetActive(true);
            }
            else
            {
                movementPointer.SetActive(false);
                movementPointer2.SetActive(false);
            }
        }
        //Debug.Log(ShitUtilities.getVectorLength(-(transform.position - totalCommander.player.transform.position)).ToString());
    }

    private void log(string msg, bool important=false)
    {
        if (logBrain || important)
            Debug.Log("Vampire brain: " + msg);
    }


    #region Attack
    // Think about e2-e4 move
    private bool getIdea() // NOT Intellij Idea
    {
        attackChooser.setRatings(getVectorFromBossToPlayer());
        if (attackChooser.anyAttacksAvaliable())
        {
            attackChooser.setDirection(getVectorFromBossToPlayer());
            
            return attackChooser.useBestAttack();
        }
        return false;
    }

    private void AttackIfPossible()
    {
        attackChooser.setDirection(getVectorFromBossToPlayer());
        attackChooser.doAnyAttack();
    }

    #endregion


    #region Dodge
    // But I say to you, do not resist an evil person; but whoever slaps you on your right cheek, turn the other to him also. MF 5:39
    private bool needToDodge()
    {
    // Dodge coolDown
    --realCooldown;
    if (realCooldown < 0)
        realCooldown = 0;

    // count dodge frames
    if (doingDodge)
        ++dodgeFrame;

    if (dodgeFrame == numberOfDodgeFrames)
    {
        // Do only when end dodging
        enemyMovement.speed = speedBefore;
        realCooldown = DodgeCooldown;
        doingDodge = false;
        dodgeFrame = 0;
        log("End dodge");
    }

    // Are we in danger?
    if (realCooldown == 0)
    {
        foreach (Collider2D bullet in totalCommander.fakeBullets)
        {
            if (bullet != null && bullet.Distance(thisCollider).isOverlapped)
            {
                // Yes, we are!
                bulletWeAreAvoiding = bullet;
                return true;
            }
        }
    }

    return false;
}

private void calculateDodge()
{
    if (dodgeFrame == 0)
    {
        // Do only when start dodging
        speedBefore = enemyMovement.speed;
        doingDodge = true;
        log("Do dodge");

        // Find the perpendicular vector
        Vector2 vectorToBullet = (transform.position - bulletWeAreAvoiding.gameObject.transform.position).normalized;
        movementDirection = ShitUtilities.getPerpendicular(vectorToBullet);

        enemyMovement.speed = enemyMovement.speed * DodgeMovementSpeed;
    }
}
    #endregion
    #region Movement Calculation
    // Thinking where to move
    private void stopMoving()
    {
        movementDirection = new Vector2(0, 0);
        enemyMovement.setTargetVector(movementDirection);
    }
    private const bool AIM_PLAYER = true;
    private const bool AIM_POINT = false;
    private bool aim = AIM_PLAYER;
    
    private bool isCloseTo0(float x)   {   return Mathf.Abs(x) < 0.1f;   } 
    private List<Point> points = new List<Point>();
    void calculateMovement()
    {
        float minLen = float.MaxValue;
        Collider2D closestObstacle = new Collider2D();
        Vector2 directionToClosestObstacle = new Vector2();

            // Moving towards the player
        Vector2 directionToPlayer = getVectorFromBossToPlayer().normalized;
        float sizeToPlayer = ShitUtilities.getVectorLength(directionToPlayer);


        float playerMovementM = (sizeToPlayer - ApproachThePlayer) * PathStraightforwardness;

        // Moving around obstacles
        foreach (Collider2D obstacle in totalCommander.obstacles)
        {
            if (obstacle != null)
            {
                Vector2 directionToObstacle;
                float sizeToObstacle = myLegsCollider.Distance(obstacle).distance;
                ColliderDistance2D tmpCD = myLegsCollider.Distance(obstacle);
                directionToObstacle = tmpCD.normal;

                if (sizeToObstacle < minLen)
                {
                    minLen = sizeToObstacle;
                    closestObstacle = obstacle;
                    directionToClosestObstacle = directionToObstacle;
                }
                // Give to him who asks of you, and do not turn away from him who wants to borrow from you. MF 5:42
            }
        }


        if (minLen < 0.1f)
            minLen = 0.1f;

        float obstacleMovementM = (AvoidObstacles / minLen);

        float theDifference // Between An Amature And A Professional Is That You Write Your Own Compiler
            = Mathf.Abs((directionToPlayer + directionToClosestObstacle).x) + Mathf.Abs((directionToPlayer + directionToClosestObstacle).y);

        if (theDifference < 0.1f && aim == AIM_PLAYER && minLen < 2f)
        {
            //log("My aim has changed. I am moving towards PATH");

            int mode = 0;
            float shortX = directionToPlayer.x;
            float shortY = directionToPlayer.y;

            if (isCloseTo0(shortX + 1) && isCloseTo0(shortY))
                mode = MovementPointsCalculator.CODE_RIGHT; // Right
            if (isCloseTo0(shortX-1) && isCloseTo0(shortY))
                mode = MovementPointsCalculator.CODE_LEFT; // Left
            if (isCloseTo0(shortX) && isCloseTo0(shortY-1))
                mode = MovementPointsCalculator.CODE_BOTTOM; // Bottom
            if (isCloseTo0(shortX) && isCloseTo0(shortY + 1))
                mode = MovementPointsCalculator.CODE_TOP; // Top


            aim = AIM_POINT;
            List<Point> path = MovementPointsCalculator.GetRandomPathForOurSituation(mode, closestObstacle, myLegsCollider);
            foreach (Point p in path)
                points.Add(p);
        }

        if (aim == AIM_POINT)
        {
            Vector2 directionToAim = -(transform.position - points[0].getPosition());
            float distToPoint = ShitUtilities.getVectorLength(directionToAim);


            if (distToPoint < 0.05f)
            {
                //log("I've reached the POINT");
                points.RemoveAt(0);
                if (points.Count == 0)
                {
                    //log("My aim has changed. I am moving towards PLAYER");
                    aim = AIM_PLAYER;
                }
                else
                {
                    //log("New POINT is at: " + points[0].x + " " + points[0].y);
                }
            }

            if (attackChooser.isNoObstaclesToPlayer())
            {
                points.Clear();
                aim = AIM_PLAYER;
            }
        }

        Vector2 mainVector = new Vector2();
        switch (aim)
        {
            case AIM_PLAYER:
                mainVector = directionToPlayer * playerMovementM;
                mainVector += directionToClosestObstacle * obstacleMovementM;
                break;

            case AIM_POINT:
                Vector2 directionToAim = -(transform.position - points[0].getPosition()).normalized;
                if (points.Count > 0)
                    mainVector = directionToAim * playerMovementM;
                else
                {
                    log("WARNING! I lost point to go.", true);
                    aim = AIM_PLAYER;
                }
                break;
        }

        

        movementDirection = mainVector;
        //movementDirection += -perpDirection * obstacleMovementM;

        Vector2 difference = directionToClosestObstacle; //-directionToClosestObstacle;
        difference.Normalize();
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        if (rotationZ == 0f)
        {
            rotationZ = 0.000001f;
        }
        movementPointer.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

        Vector2 difference2 = -mainVector;
        difference2.Normalize();
        float rotationZ2 = Mathf.Atan2(difference2.y, difference2.x) * Mathf.Rad2Deg;
        if (rotationZ2 == 0f)
        {
            rotationZ2 = 0.000001f;
        }
        movementPointer2.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ2);
        //Debug.Log(closestObstacle.name.ToString() + " " + minLen.ToString());
        
        movementDirection = movementDirection.normalized;
    }

    private void move()
    {
        // And whosoever shall compel thee to go a mile, go with him twain. MF 5:41
        enemyMovement.setTargetVector(movementDirection);
        aIAnimatorController.Run(movementDirection);
    }
    #endregion

    private Vector2 getVectorFromBossToPlayer()
    {
        return -(transform.position - totalCommander.player.transform.position);
    }
}
class ShitUtilities
{
    public static float getVectorLength(Vector2 vec)
    {
        //Pythagorean theorem
        return Mathf.Sqrt(vec.x * vec.x + vec.y * vec.y);
    }

    public static Vector2 getPerpendicular(Vector2 vec)
    {
        // Find the perpendicular vector
        // https://zaochnik.com/spravochnik/matematika/vektory/nahozhdenie-vektora-perpendikuljarnogo-dannomu-vek/
        // a(x) * b(x) + a(y) * b(y) = 0,   a = vec
        vec = vec.normalized;

        float aX = vec.x;
        float aY = vec.y;
        //bX = 1f;
        float bY = -aX / aY;

        return new Vector2(1f, bY).normalized; // bX = 1
    }
}








/*switch (mode)
{
    case 0:
        float nextX = closestObstacle.bounds.max.x + myLegsCollider.bounds.extents.x * 2;
        float nextY = closestObstacle.bounds.max.y + myLegsCollider.bounds.extents.y * 7;
        Point point = new Point(nextX, nextY);
        log("New POINT is at: " + point.x + " " + point.y);
        points.Add(point);

        nextX = closestObstacle.bounds.min.x - myLegsCollider.bounds.extents.x * 2;
        nextY = closestObstacle.bounds.max.y + myLegsCollider.bounds.extents.y * 7;
        point = new Point(nextX, nextY);
        points.Add(point);
        break;
    case 1:
        nextX = closestObstacle.bounds.min.x - myLegsCollider.bounds.extents.x * 2;
        nextY = closestObstacle.bounds.max.y + myLegsCollider.bounds.extents.y * 7;
        point = new Point(nextX, nextY);
        log("New POINT is at: " + point.x + " " + point.y);
        points.Add(point);

        nextX = closestObstacle.bounds.min.x + myLegsCollider.bounds.extents.x * 2;
        nextY = closestObstacle.bounds.max.y + myLegsCollider.bounds.extents.y * 7;
        point = new Point(nextX, nextY);
        points.Add(point);
        break;
    case 2:
        nextX = closestObstacle.bounds.max.x + myLegsCollider.bounds.extents.x * 2;
        nextY = closestObstacle.bounds.min.y - myLegsCollider.bounds.extents.y * 7;
        point = new Point(nextX, nextY);
        log("New POINT is at: " + point.x + " " + point.y);
        points.Add(point);

        nextX = closestObstacle.bounds.max.x + myLegsCollider.bounds.extents.x * 2;
        nextY = closestObstacle.bounds.max.y + myLegsCollider.bounds.extents.y * 7;
        point = new Point(nextX, nextY);
        points.Add(point);
        break;
    case 3:
        nextX = closestObstacle.bounds.max.x + myLegsCollider.bounds.extents.x * 2;
        nextY = closestObstacle.bounds.max.y + myLegsCollider.bounds.extents.y * 7;
        point = new Point(nextX, nextY);
        log("New POINT is at: " + point.x + " " + point.y);
        points.Add(point);

        nextX = closestObstacle.bounds.max.x + myLegsCollider.bounds.extents.x * 2;
        nextY = closestObstacle.bounds.min.y - myLegsCollider.bounds.extents.y * 7;
        point = new Point(nextX, nextY);
        points.Add(point);
        break;
} */
