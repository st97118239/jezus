public class EnemyToSpawn
{
    public EnemyType type;
    public float health;
    public int damage;

    public EnemyToSpawn(EnemyType type, float health, int damage)
    {
        this.type = type;
        this.health = health;
        this.damage = damage;
    }
}