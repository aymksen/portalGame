
using UnityEngine;

public class CostumeBullet1 : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;
    public float damage = 34f;
    [Range(0f, 1f)] public float bounciness;
    public bool useGravity;

    public int explosionDamage;
    public float explotionRange;

    public int maxCollision;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicMaterial physics_mat;

    private void Start()
    {
        Setup();
    }
    private void Update()
    {
        if (collisions > maxCollision) Explode();

        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0) Explode();
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.collider.CompareTag("Bullet")) return;

        collisions++;
        if (collision.collider.CompareTag("Enemy") && explodeOnTouch)
        { Explode(); }
    }
    private void Explode ()
    {
        if (explosion != null)
        {
            //Instantiate(explosion, transform.position, Quaternion.LookRotation(transform.position.normalized));
        }
        Collider[] enemies = Physics.OverlapSphere(transform.position, explotionRange, whatIsEnemies);
        for (int i=0; i<enemies.Length;i++)
        {
            enemies[i].GetComponent<Target>().TakeDamage(damage);
            
            
        }
        Destroy(gameObject);
        Invoke("Delay", 0.05f);
    }
    private void Delay()
    {
        Destroy(gameObject);
    }
    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explotionRange);
    }*/
    private void Setup()
    {
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;

        GetComponent<SphereCollider>().material = physics_mat;

        rb.useGravity = useGravity;
    }



}
