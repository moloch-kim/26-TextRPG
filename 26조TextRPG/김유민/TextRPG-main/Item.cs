public class Item
{
    public int num { get; set; }
    public string name { get; set; }
    public string powerType { get; set; }
    public int powerValue { get; set; }
    public string explanation { get; set; }
    public int gold { get; set; }
    public string goldStr { get; set; }
    public bool having { get; set; }
    public string equip { get; set; }

    public Item(int _num, string _name, string _powerType, int _powerValue, string _explanation, int _gold)
    {
        num = _num;                 // 생성 시 num을 지정해주는 거 말고 num이 1씩 증가하도록 구현해볼 것
        name = _name;
        powerType = _powerType;
        powerValue = _powerValue;
        explanation = _explanation;
        gold = _gold;
        goldStr = gold.ToString() + " G";
        having = false;
        equip = "";
    }

    public Item DeepCopy()
    {
        Item newItem = new Item(num, name, powerType, powerValue, explanation, gold);
        
        return newItem;
    }
}