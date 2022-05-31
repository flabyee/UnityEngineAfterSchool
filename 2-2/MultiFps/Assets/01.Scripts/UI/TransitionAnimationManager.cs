using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionAnimationManager : MonoBehaviour
{

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        
    }

    public void StartFadeOut()
    {
        Invoke("FadeOut", 2f);
    }

    private void FadeOut()
    {
        animator.SetTrigger("FadeOut");

    }


}
