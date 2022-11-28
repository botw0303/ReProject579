using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class PopEvent : MonoBehaviour
{
    [Header("¿É¼Ç")]
    public RectTransform upperBlock;
    public RectTransform underBlock;
    public RectTransform optionsPanel;
    public GameObject optionPanel;
    public GameObject optionButtonPanel;
    public RectTransform optionButtonPanelRect;
    public RectTransform textRect;
    [Header("ÀÎº¥Åä¸®")]
    public GameObject inventoryPanel;
    public bool isPopUpTheInventoryPanel = false;
    public bool boss1Clear;
    public bool boss2Clear;
    public bool boss3Clear;
    public GameObject boss1X;
    public GameObject boss2X;
    public GameObject boss3X;
    public GameObject creditButton;
    [Header("NPC UI")]
    public RectTransform npcUpperBlock;
    public RectTransform npcUnderBlock;
    public RectTransform rectNPCPanel;
    public GameObject npcPanel;
    public TextMeshProUGUI npcText;
    [Header("¿ø¼Ò È¹µæ")]
    public RectTransform elementUI;
    public TextMeshProUGUI elementText;
    public GameObject elementPanel;
    [Header("¿£µù Å©·¹µ÷")]
    public GameObject creditPanel;
    public TextMeshProUGUI creditText1;
    public float creditDuration = 5f;
    public bool gameisEnd = false;
    public GameObject buttonImage;
    

    bool lif;

    

    SliderController sliderController;

    public float duration = 0.2f;

    //private Sequence sequence;

    private void Awake()
    {
        sliderController = FindObjectOfType<SliderController>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DoPopUp();
        }

        if (!isPopUpTheInventoryPanel && Input.GetKeyDown(KeyCode.E))
        {
            OnInventoryPanel();
            isPopUpTheInventoryPanel = true;
        }
        else if(isPopUpTheInventoryPanel && Input.GetKeyDown(KeyCode.E))
        {
            OffInventoryPanel();
            isPopUpTheInventoryPanel = false;
        }
        if (gameisEnd)
        {
            DownEndingCredit();
        }
    }

    //15 -15 85 -85 
    public void DoPopDown()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(upperBlock.DOLocalMoveY(15f, duration).SetEase(Ease.InCubic));
        seq.Join(underBlock.DOLocalMoveY(-15f, duration).SetEase(Ease.InCubic));
        seq.Join(optionsPanel.DOScaleY(0, duration).SetEase(Ease.InCubic));
        seq.AppendCallback(() => {
            optionPanel.SetActive(false);
            seq.Kill();
        });
    }

    public void DoPopUp()
    {
        Sequence seq = DOTween.Sequence();

        optionPanel.SetActive(true);
        seq.Append(upperBlock.DOLocalMoveY(250f, duration).SetEase(Ease.InCubic));
        seq.Join(underBlock.DOLocalMoveY(-250f, duration).SetEase(Ease.InCubic));
        seq.Join(optionsPanel.DOScaleY(1, duration).SetEase(Ease.InCubic));
        seq.AppendCallback(() => {
            seq.Kill();
        });
    }

    public void DoOptionPopUp()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(upperBlock.DOLocalMoveY(15f, duration).SetEase(Ease.InCubic));
        seq.Join(underBlock.DOLocalMoveY(-15f, duration).SetEase(Ease.InCubic));
        seq.Join(optionsPanel.DOScaleY(0, duration).SetEase(Ease.InCubic));

        optionButtonPanel.SetActive(true);
        seq.Append(upperBlock.DOLocalMoveY(250f, duration).SetEase(Ease.InCubic));
        seq.Join(underBlock.DOLocalMoveY(-250f, duration).SetEase(Ease.InCubic));
        seq.Join(optionButtonPanelRect.DOScaleY(1, duration).SetEase(Ease.InCubic));
        seq.Join(textRect.DOLocalMoveY(310f, duration).SetEase(Ease.InCubic));
        seq.AppendCallback(() =>
        {
            seq.Kill();
        });
    }

    public void DoOptionPopDown()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(upperBlock.DOLocalMoveY(15f, duration).SetEase(Ease.InCubic));
        seq.Join(underBlock.DOLocalMoveY(-15f, duration).SetEase(Ease.InCubic));
        seq.Join(optionButtonPanelRect.DOScaleY(0, duration).SetEase(Ease.InCubic));

        seq.Append(upperBlock.DOLocalMoveY(250f, duration).SetEase(Ease.InCubic));
        seq.Join(underBlock.DOLocalMoveY(-250f, duration).SetEase(Ease.InCubic));
        seq.Join(optionsPanel.DOScaleY(1, duration).SetEase(Ease.InCubic));
        seq.AppendCallback(() =>
        {
            seq.Kill();
        });
    }

    public void OnInventoryPanel()
    {
        inventoryPanel.SetActive(true);

        if (boss1Clear)
        {
            boss1X.SetActive(true);
        }
        if (boss2Clear)
        {
            boss2X.SetActive(true);
        }
        if (boss3Clear)
        {
            boss3X.SetActive(true);
        }

        if(boss1Clear && boss2Clear && boss3Clear)
        {
            Destroy(buttonImage);
            creditButton.SetActive(true);
        }
    }

    public void OffInventoryPanel()
    {
        inventoryPanel.SetActive(false);
    }

    public void PopUpTheNPCUI()
    {
        Sequence seq = DOTween.Sequence();
        Debug.Log("PopUpTheNPCUI ½ÇÇàµÊ");
        npcPanel.SetActive(true);
        seq.Append(npcUpperBlock.DOLocalMoveY(255f, duration).SetEase(Ease.InCubic));
        seq.Join(npcUnderBlock.DOLocalMoveY(-220f, duration).SetEase(Ease.InCubic));
        seq.Join(rectNPCPanel.DOScaleY(1, duration).SetEase(Ease.InCubic));
        seq.AppendCallback(() => {
            seq.Kill();
        });
    }

    public void PopDownTheNPCUI()
    {
        Sequence seq = DOTween.Sequence();
        Debug.Log("PopDownTheNPCUI ½ÇÇàµÊ");
        seq.Append(npcUpperBlock.DOLocalMoveY(50f, duration).SetEase(Ease.InCubic));
        seq.Join(npcUnderBlock.DOLocalMoveY(-50f, duration).SetEase(Ease.InCubic));
        seq.Join(rectNPCPanel.DOScaleY(0, duration).SetEase(Ease.InCubic));
        seq.AppendCallback(() => {
            npcPanel.SetActive(false);
            seq.Kill();
        });
    }

    public void PopUpTheElementUI()
    {
        Sequence seq = DOTween.Sequence();

        elementPanel.SetActive(true);
        seq.Append(elementUI.DOScaleY(1, duration).SetEase(Ease.InCubic));
        seq.AppendCallback(() => {
            seq.Kill();
        });
    }
    public void PopDownTheElementUI()
    {
        Sequence seq = DOTween.Sequence();

        elementPanel.SetActive(false);
        seq.Append(elementUI.DOScaleY(0, duration).SetEase(Ease.InCubic));
        seq.AppendCallback(() => {
            seq.Kill();
        });
    }

    public void UpEndingCredit()
    {
        Sequence seq = DOTween.Sequence();

        creditPanel.SetActive(true);
        seq.Append(creditText1.DOFade(1, creditDuration));
        seq.AppendCallback(() =>
        {
            gameisEnd = true;
            seq.Kill();
        });
    }

    public void DownEndingCredit()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(creditText1.DOFade(0, creditDuration));
        seq.AppendCallback(() =>
        {
            gameObject.SetActive(false);
            QuitToManu();
            seq.Kill();
        });
    }

    public void QuitToManu()
    {
        SceneManager.LoadScene(1);
    }
}
