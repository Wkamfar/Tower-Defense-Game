using UnityEngine;

/// <summary>
/// TowerActionScript tracks and shoots an enemy
/// </summary>
public class TowerActionScript : MonoBehaviour
{
    protected virtual void Update()
    {
    }
    protected virtual bool HasTarget()
    {
        return false;
    }
    protected virtual void TrackEnemy() //add aiming ahead and predicting movement later for projectile bullets
    {
    }
    protected virtual void Shoot()
    {
    }
    protected virtual bool CanShoot()
    {
        return false;
    }
}
