using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHP : MonoBehaviour
{
    void Start()
    {
        BossHPInit();
    }
    /*
     * Если вы попросите ветерана-программиста дать один дельный совет о программировании, то после некоторого раздумья он ответит: 
     * «Избегайте использования глобальных переменных!». И, частично, он будет прав. Глобальные переменные являются одними из самых злоупотребляемых
     * объектов в языке C++. Хоть они и выглядят безвредными в небольших программах, использование их в крупных проектах зачастую чрезвычайно проблематично.
     * Новички часто используют огромное количество глобальных переменных, потому что с ними легко работать, особенно когда задействовано много функций.
     * Это плохая идея. Многие разработчики считают, что неконстантные глобальные переменные вообще не следует использовать!
     * Но прежде, чем мы разберемся с вопросом «Почему?», нужно кое-что уточнить. Когда разработчики говорят, что глобальные переменные — это зло,
     * они НЕ подразумевают полностью ВСЕ глобальные переменные. Они говорят о неконстантных глобальных переменных.*/
    public int HP;
    public Slider BossHPSlider;
    private int arrowDamage,knifeDamage;
    private Collider2D bossCollider;
    void BossHPInit()
    {
        arrowDamage = FindObjectOfType<PlayerControll>().arrowDamage;
        knifeDamage = FindObjectOfType<PlayerControll>().meleeDamage;
        BossHPSlider.maxValue = HP;
        BossHPSlider.minValue = 0;
        BossHPSlider.value = HP;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //bossCollider
        if (collision.tag == "arrow" && HP > 0)
        {
            HP -= arrowDamage;
            BossHPSlider.value = HP;
            Destroy(collision.gameObject);
            if (HP <= 0) Destroy(transform.gameObject);
        }
        if (collision.tag == "knife" && HP > 0)
        {
            HP -= knifeDamage;
            BossHPSlider.value = HP;
            if (HP <= 0) Destroy(transform.gameObject);
            //Destroy(collision.gameObject);
        }
    }
}
