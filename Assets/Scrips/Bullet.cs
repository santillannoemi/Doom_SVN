using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed =20f;
    [SerializeField]
    private GameObject enemyHitParticles;
    [SerializeField]
    private GameObject wallHitParticles;
    protected  float damage = 10f;
    public float Damage {set{damage = value;}}
    private Rigidbody rb;
    private void Awake()
    {
        rb=GetComponent<Rigidbody>();
    }
    void OnEnable()
    {
        rb.angularVelocity =Vector3.zero;
        rb.linearVelocity = Vector3.zero;
        rb.linearVelocity = transform.forward * speed;
    }
    public virtual void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Enemy")
        {
            SoundManager.instance.Play("buttet_hit_enemy");
            PoolManager.Instance.GetObject(enemyHitParticles, transform.position);
        }
        else
        {
            SoundManager.instance.Play("buttet_hit_wall");
            PoolManager.Instance.GetObject(wallHitParticles, transform.position);
        }
        gameObject.SetActive(false);
    }
}
