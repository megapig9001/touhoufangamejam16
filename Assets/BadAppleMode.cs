using System;
using System.Collections;
using UnityEngine;

public class BadAppleMode : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer buttonSpriteRenderer;
    [SerializeField] 
    private Sprite badAppleButton;

    public bool badAppleMode = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (badAppleMode)
            buttonSpriteRenderer.sprite = badAppleButton;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetBadAppleMode()
    {
        return badAppleMode;
    }
}
