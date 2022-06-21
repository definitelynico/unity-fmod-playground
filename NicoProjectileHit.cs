using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class NicoProjectileHit : MonoBehaviour
{

    [EventRef] public string projectileHit;
    EventInstance projectileHitInstance;


    public void PlayProjectileHitNonDamagable(GameObject projectile)
    {
        projectileHitInstance = RuntimeManager.CreateInstance(projectileHit);
        RuntimeManager.AttachInstanceToGameObject(projectileHitInstance, projectile.transform, projectile.GetComponent<Rigidbody2D>());

        projectileHitInstance.setParameterByName("Damagable", 0);
        projectileHitInstance.start();
        projectileHitInstance.release();
    }

    public void PlayProjectileHitDamagable(GameObject projectile)
    {
        projectileHitInstance = RuntimeManager.CreateInstance(projectileHit);
        RuntimeManager.AttachInstanceToGameObject(projectileHitInstance, projectile.transform, projectile.GetComponent<Rigidbody2D>());

        projectileHitInstance.setParameterByName("Damagable", 1);
        projectileHitInstance.start();
        projectileHitInstance.release();
    }
}
