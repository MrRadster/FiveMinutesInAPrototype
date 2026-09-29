using UnityEngine;

public class Animatronic : MonoBehaviour
{
    [Header("Info")]
    public string animatronicName = "Animatronic";

    [Header("Movement")]
    public Transform[] path;
    public float moveInterval = 5f;
    [Range(0f, 1f)]
    public float moveChance = 0.4f;

    [Header("Stare Settings (Red only)")]
    public bool isStareType = false;
    public float stareTimeNeeded = 1.5f;

    [Header("Door Blocking")]
    public FNAFDoor blockingDoor;
    public float doorBlockSendBackTime = 2.5f;   
    [Header("Attack")]
    public float attackGraceTime = 2.5f;         
    public GameObject jumpscareObject;
    public AudioSource jumpscareSound;

    private int currentStage = 0;
    private float moveTimer;
    private float stareTimer = 0f;
    private float doorBlockTimer = 0f;
    private float attackTimer = 0f;
    private bool isBeingWatched = false;
    private bool hasAttacked = false;
    private bool isInAttackStage = false;

    void Start()
    {
        moveTimer = moveInterval;

        if (path != null && path.Length > 0)
        {
            transform.position = path[0].position;
            currentStage = 0;
        }
    }

    void Update()
    {
        if (hasAttacked) return;

        if (isStareType)
        {
            if (isBeingWatched)
            {
                stareTimer += Time.deltaTime;
                if (stareTimer >= stareTimeNeeded)
                {
                    if (currentStage > 0)
                    {
                        currentStage--;
                        transform.position = path[currentStage].position;
                        Debug.Log(animatronicName + " was pushed back by staring!");
                    }
                    stareTimer = 0f;
                }
            }
            else
            {
                stareTimer = 0f;
            }
        }

        if (blockingDoor != null && currentStage == path.Length - 2)
        {
            if (!blockingDoor.IsOpen())
            {
                doorBlockTimer += Time.deltaTime;
                if (doorBlockTimer >= doorBlockSendBackTime)
                {
                    currentStage = Mathf.Max(0, currentStage - 1);
                    transform.position = path[currentStage].position;
                    doorBlockTimer = 0f;
                    Debug.Log(animatronicName + " was sent back by the door!");
                }
            }
            else
            {
                doorBlockTimer = 0f;
            }
        }

        if (isInAttackStage)
        {
            attackTimer += Time.deltaTime;

            if (blockingDoor != null && !blockingDoor.IsOpen())
            {
                currentStage = Mathf.Max(0, currentStage - 1);
                transform.position = path[currentStage].position;
                isInAttackStage = false;
                attackTimer = 0f;
                Debug.Log(animatronicName + " was blocked at the door and sent back!");
                return;
            }

            if (attackTimer >= attackGraceTime)
            {
                Attack();
            }
            return; 
        }

        if (isStareType && isBeingWatched) return; 
        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0f)
        {
            moveTimer = moveInterval;
            TryMoveForward();
        }
    }

    void TryMoveForward()
    {
        if (path == null || path.Length == 0) return;
        if (currentStage >= path.Length - 1) return;

        if (currentStage == path.Length - 2)
        {
            if (blockingDoor != null && !blockingDoor.IsOpen())
            {
                return;
            }
        }

        if (Random.value <= moveChance)
        {
            currentStage++;
            transform.position = path[currentStage].position;
            Debug.Log(animatronicName + " moved to stage " + currentStage);

            if (currentStage >= path.Length - 1)
            {
                isInAttackStage = true;
                attackTimer = 0f;
                Debug.Log(animatronicName + " is at the door! You have a few seconds!");
            }
        }
    }

    void Attack()
    {
        if (hasAttacked) return;
        hasAttacked = true;

        Debug.Log(animatronicName + " is attacking!");

        if (jumpscareSound != null)
            jumpscareSound.Play();

        if (jumpscareObject != null)
            jumpscareObject.SetActive(true);

        GameOverManager gameOver = FindObjectOfType<GameOverManager>();
        if (gameOver != null)
            gameOver.ShowGameOver();
    }

    public void SetBeingWatched(bool watched)
    {
        isBeingWatched = watched;
    }

    public int GetCurrentStage()
    {
        return currentStage;
    }
}