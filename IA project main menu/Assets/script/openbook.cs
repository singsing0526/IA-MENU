using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class openbook : MonoBehaviour
{
    [SerializeField] Button openBtn = null;
    [SerializeField] Button closeBtn = null;

    [SerializeField] GameObject openedBook = null;
    [SerializeField] GameObject insideBackCover = null;

    [SerializeField] AudioSource audioSourse = null;
    [SerializeField] AudioClip openBook = null;

    private Vector3 rotationVector;

    private bool isOpenClicked;
    private bool isCloseClicked;

    private DateTime startTime;
    private DateTime endTime;

    private Quaternion startRotation = new Quaternion();

    void Start()
    {

        startRotation = transform.rotation;

        if (openBtn != null)
            openBtn.onClick.AddListener(() => openBtn_Click());

        AppEvent.CloseBook += new EventHandler(closeBook_Click);
    }

    void Update()
    {
        if (isOpenClicked || isCloseClicked)
        {
            transform.Rotate(rotationVector * Time.deltaTime);
            endTime = DateTime.Now;

            if (isOpenClicked)
            {

                if ((endTime - startTime).TotalSeconds >= 1)
                {

                    isOpenClicked = false;
                    gameObject.SetActive(false);
                    insideBackCover.SetActive(false);
                    openedBook.SetActive(true);

                    AppEvent.OpenBookFunction();

                    Vector3 newRotation = new Vector3(startRotation.x, 180, startRotation.z);
                    transform.rotation = Quaternion.Euler(newRotation);

                }
            }
            if (isCloseClicked)
            {
                if ((endTime - startTime).TotalSeconds >= 1)
                {
                    isCloseClicked = false;

                    Vector3 newRotation = new Vector3(startRotation.x, 0, startRotation.z);
                    transform.rotation = Quaternion.Euler(newRotation);
                }
            }
        }
    }
    private void openBtn_Click()
    {
        isOpenClicked = true;
        startTime = DateTime.Now;

        rotationVector = new Vector3(0, 180, 0);

        playSound();
    }
    private void playSound()
    {
        if((audioSourse != null)&&(openBook != null))
        {
            audioSourse.PlayOneShot(openBook);
        }
    }

    private void closeBook_Click(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
        openedBook.SetActive(false);
        insideBackCover.SetActive(true);

        isCloseClicked = true;
        startTime = DateTime.Now;
        rotationVector = new Vector3(0, -180, 0);

        playSound();
    }
}
