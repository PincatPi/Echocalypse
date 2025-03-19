using System;
using UnityEngine;

public class EnemySwapWeapon : MonoBehaviour
{
    [SerializeField] private WeaponConfig[] weapons;
    [SerializeField] private WeaponConfig currentActiveWeapon;

    void Start()
    {
        //默认手上拿的是Katana
        currentActiveWeapon = weapons[0];
    }
    
    public void EnemySwapWeapons(string weaponName)
    {
        currentActiveWeapon.weaponInHand.SetActive(false); //当前手上的武器隐藏
        currentActiveWeapon.weaponOnBack.SetActive(true); //背上的武器显示
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].weaponName == weaponName)
            {
                currentActiveWeapon = weapons[i];
                break;
            }
        }
        currentActiveWeapon.weaponOnBack.SetActive(false); //手上的武器显示
        currentActiveWeapon.weaponInHand.SetActive(true); //背上的武器隐藏
    }
}

[Serializable]
public struct WeaponConfig
{
    public string weaponName;
    public GameObject weaponInHand;
    public GameObject weaponOnBack;
}
