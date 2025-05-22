using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour
{
    LineRenderer aimLR;
    Transform bullet;
    Rigidbody2D bulletRB;
    Vector2 dir;
    Transform target;
    float shotDelay;
    Coroutine shotDelayCoroutine;
    float bulletSpeed;


    void Awake()
    {
        bullet = transform.GetChild(0);
        bulletRB = bullet.GetComponent<Rigidbody2D>();
        aimLR = GetComponent<LineRenderer>();
    }

    void Update()
    {
        AimLineToTarget();
    }

    void AimLineToTarget()
    {
        aimLR.SetPosition(1, target.position - transform.position);
    }

    public void ReadyShot(Vector2 pos, float shotDelay, Transform target, float bulletSpeed)
    {
        transform.position = pos;
        this.shotDelay = shotDelay;
        this.target = target;
        this.bulletSpeed = bulletSpeed;

        AimLineToTarget();
        aimLR.enabled = true;

        bullet.localPosition = Vector2.zero;
        bulletRB.linearVelocity = Vector2.zero;
        StartCoroutine(DelayedShot());
    }

    private IEnumerator DelayedShot()
    {
        yield return new WaitForSeconds(shotDelay);

        aimLR.enabled = false;
        AimBulletToTarget();
        bulletRB.AddForce(bullet.right * bulletSpeed, ForceMode2D.Impulse);
        bullet.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);
    }

    void AimBulletToTarget()
    {
        // Calculate direction from shot position to target
        Vector2 direction = (target.position - bullet.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void Reset()
    {
        StopAllCoroutines();

        aimLR.enabled = false;
        bullet.localPosition = Vector2.zero;
        bulletRB.linearVelocity = Vector2.zero;

        target = null;
    }
}
