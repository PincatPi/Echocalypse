using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwapWeapon : MonoBehaviour
{
    [SerializeField] private WeaponConfig[] weapons;
    [SerializeField] private WeaponConfig currentActiveWeapon;
    [SerializeField] private EnemyAttackDetection enemyAttackDetection;
    
    void Start()
    {
        //默认手上拿的是Katana
        currentActiveWeapon = weapons[0];
        currentActiveWeapon.weaponDetection.enabled = true;

        enemyAttackDetection = GetComponent<EnemyAttackDetection>();
    }
    
    public void EnemySwapWeapons(string weaponName)
    {
        enemyAttackDetection.weaponType = Enum.Parse<E_WeaponType>(weaponName); //切换武器类型枚举
            
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

    #region 公共接口

    public WeaponConfig GetCurrentActiveWeapon() => currentActiveWeapon;

    #endregion
}

[Serializable]
public struct WeaponConfig
{
    public string weaponName;
    public E_WeaponType weaponType;
    public GameObject weaponInHand;
    public GameObject weaponOnBack;
    public Collider weaponDetection;
}
