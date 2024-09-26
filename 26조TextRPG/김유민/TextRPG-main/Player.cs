public class Player
{
    public int level;
    public string name;
    public string chad;
    public int attack;
    public int defense;
    public int attackUpdate;
    public int defenseUpdate;
    public int hp;
    public int gold;

    public Player(int _level, string _name, string _chad, int _attack, int _defense, int _hp, int _gold)
    {
        level = _level;
        name = _name;
        chad = _chad;
        attackUpdate = 0;
        defenseUpdate = 0;
        attack = _attack + attackUpdate;
        defense = _defense + defenseUpdate;
        hp = _hp;
        gold = _gold;
    }
}