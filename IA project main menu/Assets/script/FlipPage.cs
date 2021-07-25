using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FlipPage : MonoBehaviour
{
    private enum ButtonType
    {
        NextButton,
        PrevButton
    }

    [SerializeField] Button nextBtn = null;
    [SerializeField] Button prevBtn = null;
    [SerializeField] Button closeBtn = null;

    [SerializeField] AudioSource audioSourse = null;
    [SerializeField] AudioClip openBook = null;

    [SerializeField] Text headerText1_1;
    [SerializeField] Text headerText1_2;
    [SerializeField] Text headerText2_1;
    [SerializeField] Text headerText2_2;

    [SerializeField] Text bodyText1_1;
    [SerializeField] Text bodyText1_2;
    [SerializeField] Text bodyText2_1;
    [SerializeField] Text bodyText2_2;

    [SerializeField] Text footerText1_1;
    [SerializeField] Text footerText1_2;
    [SerializeField] Text footerText2_1;
    [SerializeField] Text footerText2_2;

    private Vector3 rotationVector;
    private Vector3 startPosition;

    private Quaternion startRotation;

    private bool isClicked;

    private DateTime startTime;
    private DateTime endTime;

    // Start is called before the first frame update
    private void Start()
    {
        startRotation = transform.rotation;
        startPosition = transform.position;

        if (nextBtn != null)
            nextBtn.onClick.AddListener(() => turnOnePageBtn_Click(ButtonType.NextButton));

        if (prevBtn != null)
            prevBtn.onClick.AddListener(() => turnOnePageBtn_Click(ButtonType.PrevButton));

        if (closeBtn != null)
            closeBtn.onClick.AddListener(() => closeBookBtn_Click());
    }

    //Awake
    private void Awake()
    {
        AppEvent.OpenBook += new EventHandler(openBookBtn_Click);

    }

    // Update is called once per frame
    private void Update()
    {
        if (isClicked)
        {
            transform.Rotate(rotationVector * Time.deltaTime);

            endTime = DateTime.Now;

            if((endTime - startTime).TotalSeconds >= 1)
            {
                isClicked = false;
                transform.rotation = startRotation;
                transform.position = startPosition;

                SetVisibleText();
            }
        }
    }

    private void openBookBtn_Click(object sender, EventArgs e)
    {
        Page pge = Page.GetRandomPage();
        Page.CurrentPage1 = 0;
        Page.CurrentPage2 = 1;

        prevBtn.gameObject.SetActive(true);
        nextBtn.gameObject.SetActive(true);

        if (pge.Pages.Count > 2)
            nextBtn.gameObject.SetActive(true);

        SetVisibleText();

    }

    private void SetVisibleText()
    {
        Page pge = Page.RandomPage;

        string footer1 = "";
        string footer2 = "";

        string body1 = "";
        string body2 = "";

        string header1 = "";
        string header2 = "";

        if(Page.CurrentPage1 < pge.Pages.Count)
        {
            footer1 = String.Format("Page {0} of {1}", Page.CurrentPage1 + 1, pge.Pages.Count);
            body1 = pge.Pages[Page.CurrentPage1];
            header1 = pge.Title;
        }
        if (Page.CurrentPage2 < pge.Pages.Count)
        {
            footer2 = String.Format("Page {0} of {1}", Page.CurrentPage2 + 1 , pge.Pages.Count);
            body2 = pge.Pages[Page.CurrentPage2];
            header2 = pge.Title;
        }

        headerText1_1.text = header1;
        headerText2_1.text = header2;

        footerText1_1.text = footer1;
        footerText2_1.text = footer2;

        bodyText1_1.text = body1;
        bodyText2_1.text = body2;

    }

    private void SetFlipPageText(int leftPage, int rightPage)
    {
        Page pge = Page.RandomPage;

        string footerR = "";
        string footerL = "";

        string bodyR = "";
        string bodyL = "";

        string headerR = "";
        string headerL = "";

        if (rightPage < pge.Pages.Count)
        {
            footerR = String.Format("Page {0} of {1}", rightPage + 1, pge.Pages.Count);
            bodyR = pge.Pages[rightPage];
            headerR = pge.Title;
        }
        if (leftPage < pge.Pages.Count)
        {
            footerL = String.Format("Page {0} of {1}", leftPage + 1, pge.Pages.Count);
            bodyL = pge.Pages[leftPage];
            headerL = pge.Title;
        }

        headerText1_2.text = headerL;
        headerText2_2.text = headerR;

        footerText1_2.text = footerL;
        footerText2_2.text = footerR;

        bodyText1_2.text = bodyL;
        bodyText2_2.text = bodyR;
    }

    private void ClearVisibleText()
    {
        Page pge = Page.RandomPage;

        string footer1 = "";
        string footer2 = "";

        string body1 = "";
        string body2 = "";

        string header1 = "";
        string header2 = "";
        headerText1_1.text = header1;
        headerText2_1.text = header2;

        footerText1_1.text = footer1;
        footerText2_1.text = footer2;

        bodyText1_1.text = body1;
        bodyText2_1.text = body2;

    }

    private void turnOnePageBtn_Click(ButtonType type)
    {
        isClicked = true;
        startTime = DateTime.Now;
        nextBtn.gameObject.SetActive(true);
        prevBtn.gameObject.SetActive(true);

        if (type == ButtonType.NextButton)
        {
            rotationVector = new Vector3(0, 180, 0);

            SetFlipPageText(Page.CurrentPage2, Page.CurrentPage2 + 1);

            Page.CurrentPage1 += 2;
            Page.CurrentPage2 += 2;
            Page pge = Page.RandomPage;

            //SetVisibleText();
            ClearVisibleText();

            if ((Page.CurrentPage2 >= pge.Pages.Count) || (Page.CurrentPage1 >= pge.Pages.Count))
            {
                nextBtn.gameObject.SetActive(false);
            }
        }
        else if (type == ButtonType.PrevButton)
        {
            Vector3 NewRotation = new Vector3(startRotation.x, 180, startRotation.z);
            transform.rotation = Quaternion.Euler(NewRotation);

            rotationVector = new Vector3(0, -180, 0);

            SetFlipPageText(Page.CurrentPage2, Page.CurrentPage2 + 1);

            Page.CurrentPage1 -= 2;
            Page.CurrentPage2 -= 2;

            //SetVisibleText();

            if ((Page.CurrentPage2 <= 0) || (Page.CurrentPage1 <= 0))
            {
                prevBtn.gameObject.SetActive(false);
            }
        }
        playSound();
    }
    private void playSound()
    {
        if ((audioSourse != null) && (openBook != null))
        {
            audioSourse.PlayOneShot(openBook);
        }
    }
    private void closeBookBtn_Click()
    {
        AppEvent.CloseBookFunction();
    }
}
