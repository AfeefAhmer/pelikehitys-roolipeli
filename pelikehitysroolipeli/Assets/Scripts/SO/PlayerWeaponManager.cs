using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public Weapon currentWeapon;

    public void SetWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
    }

    public void Attack(Vector2 direction)
    {
        if (currentWeapon == null)
        {
            Debug.Log("No weapon equipped");
            return;
        }

        currentWeapon.Attack(direction);
    }
}