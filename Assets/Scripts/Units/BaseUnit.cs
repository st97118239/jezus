using UnityEngine;
using UnityEngine.AI;

public abstract class BaseUnit : MonoBehaviour
{
    public UnitType type; // 0 = Spearman, 1 = Knight, 2 = DaVinci Tank
    public BarracksTower tower;
    public NavMeshAgent agent;
    public BoxCollider boxCollider;
    public Range rangeObject;
    public MeshRenderer rangeRenderer;
    public AudioSource hitSound;
    public Vector3 destination;
    public bool atDestination;
    public bool hasReachedDestination;
    public bool canMove = true;
    public int damage;
    public int price;
    public int maxUnits;
    public float health;
    public float speed;
    public float range;
    public float destinationRange;
    public float extraDistanceToFindEnemiesIn;

    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackTimer;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected bool isAttacking;

    protected OutlineSelection outlineSelection;
    protected Main main;
    protected EnemySpawner es;

    protected bool isSelected;

    private void Awake()
    {
        canMove = true;
        outlineSelection = FindObjectOfType<OutlineSelection>();
        main = FindObjectOfType<Main>();
        es = FindObjectOfType<EnemySpawner>();
        agent.speed = speed;
        attackTimer = attackSpeed;

        OnStart();
    }

    protected virtual void OnStart()
    {
    }
    
    public void Select(bool extraFunctions)
    {
        if (extraFunctions)
        {
            main.up.Activate(type, (int)health);
            tower.Selected(false);
            outlineSelection.AddNewSelection(tower.transform);
            RedrawRange();
        }

        outlineSelection.AddNewSelection(transform);
        isSelected = true;
        outlineSelection.ChangeLayerOfAllDescendants(transform, 9);
    }

    public void Deselect(bool extraFunctions)
    {
        if (extraFunctions)
        {
            main.up.Deactivate();
            tower.Deselected(false);
            rangeRenderer.enabled = false;
        }

        isSelected = false;
        outlineSelection.ChangeLayerOfAllDescendants(transform, 10);
    }
    
    public void RedrawRange()
    {
        rangeObject.transform.position = transform.position;
        rangeObject.transform.localScale = new Vector3(range * 2, 0.1f, range * 2);
        rangeRenderer.enabled = true;
    }
    
    public void Remove()
    {
        outlineSelection.selections.Remove(transform);
        tower.spawnedUnits.Remove(this);
        Destroy(gameObject);
    }
    protected abstract void Attack();
    
    public void IsInRange()
    {
        agent.isStopped = true;
        atDestination = true;
        hasReachedDestination = true;

        if (type == UnitType.DaVinciTank)
            Attack();
    }

    public void NewDestination(Vector3 position)
    {
        atDestination = false;
        isAttacking = false;
        destination = position;
    }

    public virtual void NewDestinationPoint(Vector3 position)
    {
        atDestination = false;
        hasReachedDestination = false;
        isAttacking = false;
        destination = position;
    }

    public void DisableUnit()
    {
        canMove = false;
        agent.isStopped = true;
    }
}