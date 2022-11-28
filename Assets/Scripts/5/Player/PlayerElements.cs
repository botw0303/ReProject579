using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElements : MonoBehaviour
{
    [SerializeField] Transform elementsPosition;
    [SerializeField] SpriteRenderer groundPet;
    [SerializeField] SpriteRenderer waterPet;
    [SerializeField] SpriteRenderer windPet;
    [SerializeField] SpriteRenderer firePet;
    [SerializeField] ParticleSystem groundEffect;
    [SerializeField] ParticleSystem waterEffect;
    [SerializeField] ParticleSystem windEffect;
    [SerializeField] ParticleSystem fireEffect;

    public bool ElementWind { get; set; }
    public bool ElementFire { get; set; }
    public bool ElementGround { get; set; }
    public bool ElementWater { get; set; }
    private PlayerSkill playerSkill;
    private int elementsCount = 0;

    [SerializeField] AudioSource elementSource;
    [SerializeField] AudioClip windClip;
    [SerializeField] AudioClip fireClip;
    [SerializeField] AudioClip waterClip;
    [SerializeField] AudioClip earthClip;

    private void Start()
    {
        playerSkill = GetComponentInParent<PlayerSkill>();
        waterPet.color = new Color(1, 1, 1, 0);
        firePet.color = new Color(1, 1, 1, 0);
        groundPet.color = new Color(1, 1, 1, 0);
        windPet.color = new Color(1, 1, 1, 0);
    }
    void Update()
    {
        groundEffect.transform.position = elementsPosition.position;
        waterEffect.transform.position = elementsPosition.position;
        windEffect.transform.position = elementsPosition.position;
        fireEffect.transform.position = elementsPosition.position;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(elementsCount != 4)
            elementsCount++;
            else
            {
                elementsCount = 1;
            }
        }
        switch (elementsCount)
        {
            case 1:
                if (playerSkill.GetWind)
                {
                    fireEffect.Play();
                    waterPet.color = new Color(1, 1, 1, 0);
                    firePet.color = new Color(1, 1, 1, 0);
                    groundPet.color = new Color(1, 1, 1, 0);
                    windPet.color = new Color(1, 1, 1, 1);
                    ElementFire = false;
                    ElementGround = false;
                    ElementWater = false;
                    ElementWind = true;
                }
                break;
            case 2:
                if (playerSkill.GetFire)
                {
                    groundEffect.Play();
                    windPet.color = new Color(1, 1, 1, 0);
                    groundPet.color = new Color(1, 1, 1, 0);
                    waterPet.color = new Color(1, 1, 1, 0);
                    firePet.color = new Color(1, 1, 1, 1);
                    ElementWind = false;
                    ElementGround = false;
                    ElementWater = false;
                    ElementFire = true;
                }
                break;
            case 3:
                if (playerSkill.GetGround)
                {
                    waterEffect.Play();
                    firePet.color = new Color(1, 1, 1, 0);
                    waterPet.color = new Color(1, 1, 1, 0);
                    windPet.color = new Color(1, 1, 1, 0);
                    groundPet.color = new Color(1, 1, 1, 1);
                    ElementFire = false;
                    ElementWater = false;
                    ElementWind = false;
                    ElementGround = true;
                }
                break;
            case 4:
                if (playerSkill.GetWater)
                {
                    windEffect.Play();
                    groundPet.color = new Color(1, 1, 1, 0);
                    firePet.color = new Color(1, 1, 1, 0);
                    windPet.color = new Color(1, 1, 1, 0);
                    waterPet.color = new Color(1, 1, 1, 1);
                    ElementWind = false;
                    ElementGround = false;
                    ElementFire = false;
                    ElementWater = true;
                }
                break;
            
        }
    }
}
