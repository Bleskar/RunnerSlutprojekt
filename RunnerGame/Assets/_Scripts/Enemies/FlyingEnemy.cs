public class FlyingEnemy : EnemyBase
{
    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        
    }

    public override void Kill()
    {
        Destroy(gameObject);
    }
}
