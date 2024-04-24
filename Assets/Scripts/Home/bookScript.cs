using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bookScript : MonoBehaviour
{
    public Animator bookAnimator;
    public GameObject closedBook;
    public GameObject pageOne;
    public GameObject Collection;
    public GameObject pageTwo;


    public void startAnim()
    {
        bookAnimator.SetBool("toOpen",true);
    }

    public void openPageOne()
    {
        closedBook.SetActive(false);
        pageOne.SetActive(true);
        Collection.SetActive(true);
    }

    public void closeBookScreen()
    {
        bookAnimator.SetBool("toOpen",false);
    }

}
