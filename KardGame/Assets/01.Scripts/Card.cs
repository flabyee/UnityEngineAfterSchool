using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "AfterSchool/CardGame/Card")]
public class Card : ScriptableObject
{
    public string id;
    public string tagString;    // 어따쓰는거지

    public bool usable;
    public bool disposable; // 1회성 카드

    public CardPower power;

    public void Init(string id, string tagString, CardPower defaultCP, bool dispose = false, bool usable = true)
    {
        this.id = id;
        this.tagString = tagString;
        this.disposable = dispose;

        power = defaultCP;
    }

    public Card Clone(bool setDispose = false)
    {
        var card = CreateInstance<Card>();  // ScriptableObject / new랑 비슷
        bool dispose = setDispose || this.disposable;   // 복제 할 때 원래 1회성이 아닌 카드를 1회성으로 만들어 줄 수 있다?
        card.Init(id, tagString, power, dispose);   
        return card;
    }

    public void OnUse()
    {
        Debug.Log("Card Use : " + power.cardName);
    }
    public void OnDraw()
    {
        Debug.Log("Card Draw : " + power.cardName);
    }
    public void OnDrop()
    {
        Debug.Log("Card Drop : " + power.cardName);
    }
    public void OnTurnEnd()
    {
        Debug.Log("TurnEnd : " + power.cardName);
    }
}
