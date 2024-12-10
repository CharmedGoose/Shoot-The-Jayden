//Tutorial Used: https://www.youtube.com/watch?v=Dn_BUIVdAPg

using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    
    public int selectedWeapon = 0;

    void Start()
    {
        SelectWeapon();
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
