using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishData : MonoBehaviour
{
    [SerializeField] private Sprite[] verySmallFishSprites;
    [SerializeField] private Sprite[] smallFishSprites;
    [SerializeField] private Sprite[] middleSizeFishSprites;
    [SerializeField] private Sprite[] bigFishSprites;
    [SerializeField] private Sprite[] veryBigFishSprites;
    [SerializeField] private Sprite[] veryVeryBigFishSprites;

    public Sprite[] VerySmallFishSprites => this.verySmallFishSprites;
    public Sprite[] SmallFishSprites => this.smallFishSprites;
    public Sprite[] MiddleSizeFishSprites => this.middleSizeFishSprites;
    public Sprite[] BigFishSprites => this.bigFishSprites;
    public Sprite[] VeryBigFishSprites => this.veryBigFishSprites;
    public Sprite[] VeryVeryBigFishSprites => this.veryVeryBigFishSprites;

    public static FishData Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new System.Exception();
        }
    }
}
