//Tutorial Used: https://www.youtube.com/watch?v=Dn_BUIVdAPg

using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [Header("Settings")]
    public int selectedWeapon = 0;

    [Header("References")]
    MouseLook mouseLook;
    JaydenAgent jaydenAgent;
    PlayerAgent playerAgent;

    GameObject bulletCasing;
    GameObject impact;

    void Start()
    {
        SelectWeapon();
        foreach (Transform weapon in transform)
        {
            if (weapon.name != "Hands")
            {
                Gun gun = weapon.GetComponent<Gun>();
                GameObject objectParent = new("ObjectPool");
                for (int i = 0; i < gun.maxAmmo * 3; i++)
                {
                    bulletCasing = Instantiate(gun.bullet, Vector3.zero, Quaternion.identity);
                    impact = Instantiate(gun.impactEffect, Vector3.zero, Quaternion.identity);
                    bulletCasing.transform.SetParent(objectParent.transform);
                    impact.transform.SetParent(objectParent.transform);
                    bulletCasing.SetActive(false);
                    impact.SetActive(false);
                    gun.bulletCasings.Add(bulletCasing);
                    gun.impacts.Add(impact);
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
                mouseLook.gun = weapon.GetComponent<Gun>();
                jaydenAgent.gun = weapon.GetComponent<Gun>();
                playerAgent.gun = weapon.GetComponent<Gun>();
            }
            else if (weapon.name != "Hands")
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
