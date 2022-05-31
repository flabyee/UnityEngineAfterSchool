using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{

    public MeshRenderer headRenderer;
    public SkinnedMeshRenderer bodyRenderer;
    private bool invincible;
    private Color originColor;

    void Start()
    {
        originColor = headRenderer.material.color;
    }

    public void StartInvincible()
    {
        invincible = true;
        StartCoroutine(InvincibleCoroutine());
    }

    private IEnumerator InvincibleCoroutine()
    {
        Color minAlphaColor = originColor;
        minAlphaColor.a = 0.3f;

        float duration = 0.3f;
        float elapsedTime = 0f;
        bool fadeOut = true;

        while (invincible)
        {
            Debug.Log(fadeOut);
            //TODO: Change Alpha
            if (fadeOut)
            {
                elapsedTime += Time.deltaTime;
                // elpasedTim++
                // elapsedTime == duration;
                // elapsedTime / duration = 1; => originColor == minAlphaColor

                ChangeSkinColor(Color.Lerp(originColor, minAlphaColor, elapsedTime / duration));

                if (elapsedTime > duration)
                {
                    fadeOut = false;
                }
                yield return null;
            }
            else
            {
                elapsedTime -= Time.deltaTime;

                ChangeSkinColor(Color.Lerp(minAlphaColor, originColor, (duration - elapsedTime) / duration));

                if (elapsedTime < 0)
                {
                    fadeOut = true;
                }
                yield return null;
            }
        }

    }

    public void StopInvincible()
    {
        ChangeSkinColor(originColor);

        invincible = false;
    }

    private void ChangeSkinColor(Color color)
    {
        bodyRenderer.material.color = color;
        headRenderer.material.color = color;
    }

    public void SetEnemyColor()
    {
        ChangeSkinColor(Color.red);
    }
}
