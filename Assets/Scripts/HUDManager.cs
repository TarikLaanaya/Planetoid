using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("Images")]
    public Image MinigunImg;
    public Image ChargeGunImg;
    public Image MissileImg;

    [SerializeField] private float missileCooldownDuration = 4f;
    private Coroutine missileCooldownCoroutine;

    void Start()
    {
        MinigunImg.color = Color.white;
        ChargeGunImg.color = new Color(1f, 1f, 1f, 0.4627f);
        MissileImg.fillAmount = 1f;
        missileCooldownCoroutine = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            MinigunImg.color = Color.white;
            ChargeGunImg.color = new Color(1f, 1f, 1f, 0.4627f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChargeGunImg.color = Color.white;
            MinigunImg.color = new Color(1f, 1f, 1f, 0.4627f);
        }
    }

    public void MissileCooldown()
    {
        if (missileCooldownCoroutine != null)
        {
            Debug.Log("Missile on Cooldown");
        }
        else
        {
            missileCooldownCoroutine = StartCoroutine(MissileCooldownRoutine());
        }

            
    }

    private IEnumerator MissileCooldownRoutine()
    {
        float timer = 0f;
        while (timer < missileCooldownDuration)
        {
            timer += Time.deltaTime;
            MissileImg.fillAmount = timer / missileCooldownDuration;
            yield return null;
        }
        MissileImg.fillAmount = 1f;
        missileCooldownCoroutine = null;
    }

}

