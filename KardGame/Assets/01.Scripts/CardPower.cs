using UnityEngine;

[CreateAssetMenu(fileName = "NewCardPower", menuName = "AfterSchool/CardGame/CardPower")]
public class CardPower : ScriptableObject
{
    public Sprite illust;
    public string cardName;
    public string cardDescription;

    public string seqOnUse;
    public string seqOnDraw;
    public string seqOnDrop;
    public string seqTurnEnd;

    public void Init(Sprite _illust, string _name, string _description,
        string _seqOnUse, string _seqOnDraw, string _seqOnDrop, string _seqTurnEnd)
    {
        this.illust = _illust;
        this.cardName = _name;
        this.cardDescription = _description;
        this.seqOnUse = _seqOnUse;
        this.seqOnDraw = _seqOnDraw;
        this.seqOnDrop = _seqOnDrop;
        this.seqTurnEnd = _seqTurnEnd;
    }
}
