using System.Collections;
using System.Collections.Generic;


public class Page
{
    public string Title { get; set; }
    public string Text{get; set;}
    public List<string> Pages { get; set; }

    private static List<Page> _pageList = null;

    public static Page RandomPage;


    public static int CurrentPage1 = 0;
    public static int CurrentPage2 = 1;

    public static Page GetRandomPage()
    {
        List<Page> pageList = Page.PageList;

        int num = UnityEngine.Random.Range(0, pageList.Count);
        Page pge = pageList[num];
        pge.Pages = new List<string>();

        string[] words = pge.Text.Split(' ');
        //put 7 words on each page
        string page = "";
        int wordCnt = 0;

        foreach(string word in words)
        {
            wordCnt++;
            if(wordCnt > 110)
            {
                pge.Pages.Add(page);
                page = "";
                wordCnt = 0;
            }
            page += string.Format("{0} ", word);
        }
        pge.Pages.Add(page);

        RandomPage = pge;

        return pge;
    }

    public static List<Page> PageList
    {
        get
        {
            if (_pageList == null)
            {
                _pageList = new List<Page>();
                _pageList.Add(new Page
                {
                    //1
                    Title = "Story",
                    Text = "Once upon a time, there was a kingdom which is prosperous and peaceful. There was a holy sword named Excalibur which protect the kingdom from evil monsters.One day, a god cursed the sword because the king angered the god. Since then, monsters start coming out from the sword. The kingdom became ruined. " +
                    "This day was called the Doomsday. Upon the Doomsday, many people died, and some escaped. The residents of the kingdom relocated themselves in the Espoir village. To reclaim the lost kingdom, they have to fight way to the lost kingdom. If someone could reclaim the lost kingdom, then he would be the new king. Under this incentive, many people tried their best to reclaim the lost kingdom. Our main character was one of them."
                });
            }
            return _pageList;
        }
    }
}
