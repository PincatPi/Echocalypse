using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthAndEndurance : MonoBehaviour
{
    [SerializeField] private EnemyBase enemyParameter;
    
    private float maxHealth; // 最大生命值
    private float maxEndurance; // 最大耐力值
    
    [SerializeField] private TextMeshProUGUI bossNameText;
    [SerializeField] private Image bossHealthFill;
    [SerializeField] private Image bossHealthBackGround;
    [SerializeField] private Image bossEnduranceFill;
    [SerializeField] private Image bossEnduranceBackGround;
    
    [SerializeField] private float barAppearTime;
    [SerializeField] private float barDisappearTime;
    
    
    [SerializeField] private Image textBackGround;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private float textAppearTime;
    [SerializeField] private float textDisappearTime;
    [SerializeField] private float textTime;
    
    private bool isAppearing = false;
    private bool isDisappearing = false;

    private void Start()
    {
        maxHealth = enemyParameter.health;
        maxEndurance = enemyParameter.endurance;
        
        DisappearBar();
        text.enabled = false;
        textBackGround.enabled = false;
    }
    
    private void Update()
    {
        // 每帧更新血条和耐力条
        UpdateHealthBar();
        UpdateEnduranceBar();
    }

    private void UpdateHealthBar()
    {
        // 计算当前生命值的比例
        float healthRatio = enemyParameter.health / maxHealth;

        // 更新血条的填充比例
        bossHealthFill.fillAmount = healthRatio;
    }

    private void UpdateEnduranceBar()
    {
        // 计算当前耐力值的比例
        float enduranceRatio = enemyParameter.endurance / maxEndurance;

        // 更新耐力条的填充比例
        bossEnduranceFill.fillAmount = enduranceRatio;
    }
    

    public void AppearText()
    {
        if (isDisappearing)
        {
            StopCoroutine(DisappearTextCoroutine());
            isDisappearing = false;
        }
        StartCoroutine(AppearTextCoroutine());
    }

    private void DisappearText()
    {
        if (isAppearing)
        {
            StopCoroutine(AppearTextCoroutine());
            isAppearing = false;
        }
        StartCoroutine(DisappearTextCoroutine());
    }
    
    private IEnumerator AppearTextCoroutine()
    {
        Debug.Log("Appear Text");
        isAppearing = true;
        
        textBackGround.enabled = true;
        text.enabled = true;
        
        float elapsedTime = 0f;
        Color startColor = new Color(text.color.r, text.color.g, text.color.b, 0f);
        Color endColor = new Color(text.color.r, text.color.g, text.color.b, 1f);
        Color bgStartColor = new Color(textBackGround.color.r, textBackGround.color.g, textBackGround.color.b, 0f);
        Color bgEndColor = new Color(textBackGround.color.r, textBackGround.color.g, textBackGround.color.b, 1f);

        textBackGround.color = bgStartColor;
        text.color = startColor;
        
        while (elapsedTime < textAppearTime)
        {
            float alpha = elapsedTime / textAppearTime;
            text.color = Color.Lerp(startColor, endColor, alpha);
            textBackGround.color = Color.Lerp(bgStartColor, bgEndColor, alpha);
            
            //TEST: 测试代码
            Debug.Log(text.color + "   " + textBackGround.color);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        text.color = endColor;
        textBackGround.color = bgEndColor;
        isAppearing = false;

        Invoke(nameof(DisappearText), textTime);
    }
    
    private IEnumerator DisappearTextCoroutine()
    {
        isDisappearing = true;
        float elapsedTime = 0f;
        Color startColor = new Color(text.color.r, text.color.g, text.color.b, 1f);
        Color endColor = new Color(text.color.r, text.color.g, text.color.b, 0f);
        Color bgStartColor = new Color(textBackGround.color.r, textBackGround.color.g, textBackGround.color.b, 1f);
        Color bgEndColor = new Color(textBackGround.color.r, textBackGround.color.g, textBackGround.color.b, 0f);

        while (elapsedTime < textDisappearTime)
        {
            float alpha = elapsedTime / textDisappearTime;
            text.color = Color.Lerp(startColor, endColor, alpha);
            textBackGround.color = Color.Lerp(bgStartColor, bgEndColor, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        text.color = endColor;
        textBackGround.color = bgEndColor;
        
        text.enabled = false;
        textBackGround.enabled = false;
        
        isDisappearing = false;
    }

    public void AppearBar()
    {
        //更新血条和耐力条
        UpdateHealthBar();
        UpdateEnduranceBar();

        bossNameText.enabled = true;
        bossHealthFill.enabled = true;
        bossHealthBackGround.enabled = true;
        bossEnduranceFill.enabled = true;
        bossEnduranceBackGround.enabled = true;
    }
    
    public void DisappearBar()
    {
        bossNameText.enabled = false;
        bossHealthFill.enabled = false;
        bossHealthBackGround.enabled = false;
        bossEnduranceFill.enabled = false;
        bossEnduranceBackGround.enabled = false;
    }
}
