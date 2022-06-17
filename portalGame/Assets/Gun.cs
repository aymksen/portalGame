using UnityEngine;
using TMPro;
public class Gun : MonoBehaviour
{
    //public float damage = 10f;
    //public float range = 100f;
    public Camera fpsCam;
    public ParticleSystem muzzleflash;
    public TextMeshProUGUI ammuniyionDisplay;
    public GameObject bullet;
    public float shootForce, upwardForce;
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    public GameObject explosion;
    int bulletsLeft, bulletsShot;

    bool shooting, readyToShoot, reloading;
    public Transform attackPoint;
    public bool allowInvoke = true;

    public Rigidbody PlayerRb;
    public float recoilForce;

    [SerializeField] Animator deg_Animator;

    private void Awake()
    {
        //deg_Animator = GetComponent<Animator>();
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
 
    void Update()
    {
        MyInput();
        if (ammuniyionDisplay!=null)
        {
            ammuniyionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        }

    }
    private void MyInput()
    {
        if (allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
            
        }
        if (readyToShoot && !shooting &&  bulletsLeft == 0 )
        {
            Reload();
        }
        if(readyToShoot && shooting && !reloading && bulletsLeft >0 )
        {
            bulletsShot = 0;
            shoot();
        }
    }


    void shoot()
    {
        muzzleflash.Play();
        readyToShoot = false;
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        GameObject curretBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        curretBullet.transform.forward = directionWithSpread.normalized;
        curretBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        curretBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        if (hit.rigidbody!=null)
        {
            hit.rigidbody.AddForce(-hit.normal * 100f);
        }

        GameObject impactGO  = Instantiate(explosion, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impactGO, 0.5f);


        /*if (muzzleflash !=null)
        {
            Instantiate(muzzleflash, attackPoint.position, Quaternion.identity);
        }*/

        bulletsLeft--;
        bulletsShot++; 

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
            PlayerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);

        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("shoot", timeBetweenShooting);
    }
    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }
    private void Reload()
    {
        deg_Animator.SetBool("Reloading", true);
        reloading = true;
        readyToShoot = false;
        Invoke("ReloadFinished", reloadTime);
       
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
        deg_Animator.SetBool("Reloading", false);
        readyToShoot = true;

    }


}
