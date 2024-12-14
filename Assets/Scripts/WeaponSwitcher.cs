//Tutorial Used: https://www.youtube.com/watch?v=Dn_BUIVdAPg

using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [Header("Settings")]
    public int selectedWeapon = 0;

    void Start()
    {
        SelectWeapon();
        foreach (Transform weapon in transform)
        {
            if (weapon.name != "Hands")
            {
                Gun gun = weapon.GetComponent<Gun>();
                GameObject parent = new(weapon.name + "BulletCasings");
                for (int i = 0; i < gun.maxAmmo * 2; i++)
                {
                    GameObject bulletCasing = Instantiate(gun.bullet, Vector3.zero, Quaternion.identity);
                    bulletCasing.transform.SetParent(parent.transform);
                    bulletCasing.SetActive(false);
                    gun.bulletCasings.Add(bulletCasing);
                }
            }
        }
    }

    public void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else if (weapon.name != "Hands")
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
