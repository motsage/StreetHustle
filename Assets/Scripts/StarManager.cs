using UnityEngine;
using UnityEngine.UI;

public class StarManager : MonoBehaviour
{
    [Header("Star Setup")]
    public Image[] stars;
    public Sprite emptyStar;
    public Sprite filledStar;

    [Header("Star Logic")]
    public int maxStars = 3;

    public void ResetStars()
    {
        foreach (Image star in stars)
        {
            star.sprite = emptyStar;
        }
    }

    public void ShowStars(int earnedStars)
    {
        earnedStars = Mathf.Clamp(earnedStars, 0, maxStars);

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].sprite = (i < earnedStars) ? filledStar : emptyStar;
        }
    }
}
