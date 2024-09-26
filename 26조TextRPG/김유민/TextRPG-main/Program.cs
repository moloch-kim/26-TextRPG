using System.Runtime.InteropServices;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace TextRPG;

class Program
{
    public static void selectStartScreen(Player player, List<Item> storeItem, List<Item> inventoryItem)
    {
        Console.WriteLine(@"
            🗡️ 스파르타 마을에 오신 여러분 환영합니다. 
            이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.
            
            0. 게임 종료
            1. 상태 보기
            2. 인벤토리
            3. 상점
            
            원하시는 행동을 입력해주세요.
        ");

        int select = int.Parse(Console.ReadLine());

        if (select == 0)
        {
            Console.WriteLine("게임이 종료됩니다.");
            Environment.Exit(0);
        }
        else if (select == 1)           // 상태 보기
        {
            SelectStateScreen(player, storeItem, inventoryItem);
        }
        else if (select == 2)      // 인벤토리
        {
            SelectInventoryScreen(player, storeItem, inventoryItem);
        }
        else if (select == 3)      // 상점
        {
            SelectStoreScreen(player, storeItem, inventoryItem);
        }
        else
        {
            Console.WriteLine("❗️ 잘못된 입력입니다.");
        }
        selectStartScreen(player, storeItem, inventoryItem);
    }

    public static void SelectStateScreen(Player player, List<Item> storeItem, List<Item> inventoryItem)
    {
        Console.Write($@"
            📍 상태 보기
            캐릭터의 정보가 표시됩니다.
        
            Lv. {player.level}
            Chad ( {player.chad} ) ");

        if (player.attackUpdate == 0)
        {
            Console.Write($@"
            공격력 : {player.attack} ");
        }
        else 
        {
            Console.Write($@"
            공격력 : {player.attack} (+{player.attackUpdate}) ");
        }

        if (player.defenseUpdate == 0)
        {
            Console.Write($@"
            방어력 : {player.defense} ");
        }
        else 
        {
            Console.Write($@"
            방어력 : {player.defense} (+{player.defenseUpdate}) ");
        }

        Console.WriteLine($@"
            체 력 : {player.hp}
            Gold : {player.gold} G

            0. 나가기

            원하시는 행동을 입력해주세요.");

        int select = int.Parse(Console.ReadLine());

        if (select == 0)
        {
            selectStartScreen(player, storeItem, inventoryItem);
        }
        else
        {
            Console.WriteLine("❗️ 잘못된 입력입니다.");
        }
        SelectStateScreen(player, storeItem, inventoryItem);
    }

    public static void SelectInventoryScreen(Player player, List<Item> storeItem, List<Item> inventoryItem)
    {
        Console.Write($@"
            📍 인벤토리
            보유 중인 아이템을 관리할 수 있습니다.
        
            [아이템 목록] ");

        foreach (Item item in inventoryItem)
        {
            Console.Write($@"
            - {item.equip}{item.name} | {item.powerType} +{item.powerValue} | {item.explanation}" );
        }

        Console.WriteLine($@"

            1. 장착 관리
            0. 나가기

            원하시는 행동을 입력해주세요.
        ");

        int select = int.Parse(Console.ReadLine());

        if (select == 0)
        {
            selectStartScreen(player, storeItem, inventoryItem);
        }
        else if (select == 1)
        {
            SelectEquipScreen(player, storeItem, inventoryItem);
        }
        else
        {
            Console.WriteLine("❗️ 잘못된 입력입니다.");
        }
        SelectInventoryScreen(player, storeItem, inventoryItem);
    }

    public static void SelectEquipScreen(Player player, List<Item> storeItem, List<Item> inventoryItem)
    {
        Console.Write($@"
            📍 인벤토리 - 장착 관리
            보유 중인 아이템을 관리할 수 있습니다.
        
            [아이템 목록] ");

        foreach (Item item in inventoryItem)
        {
            Console.Write($@"
            - {item.num} {item.equip}{item.name} | {item.powerType} +{item.powerValue} | {item.explanation}");
        }

         Console.WriteLine($@"

            0. 나가기

            원하시는 행동을 입력해주세요.
        ");

        int select = int.Parse(Console.ReadLine());

        if ( (select <= inventoryItem.Count) && (select >= 1) )
        {
            if (inventoryItem[select-1].equip == "[E]")
            {
                inventoryItem[select - 1].equip = "";
                if (inventoryItem[select - 1].powerType == "공격력")
                {
                    player.attackUpdate -= inventoryItem[select-1].powerValue;
                }
                else if (inventoryItem[select - 1].powerType == "방어력")
                {
                    player.defenseUpdate -= inventoryItem[select - 1].powerValue;
                }
            }
            else
            {
                inventoryItem[select - 1].equip = "[E]";
                if (inventoryItem[select - 1].powerType == "공격력")
                {
                    player.attackUpdate += inventoryItem[select-1].powerValue;
                }
                else if (inventoryItem[select - 1].powerType == "방어력")
                {
                    player.defenseUpdate += inventoryItem[select - 1].powerValue;
                }
            }
        }
        else if (select == 0)
        {
            SelectInventoryScreen(player, storeItem, inventoryItem);
        }
        else
        {
            Console.WriteLine("❗️ 잘못된 입력입니다.");
        }

        SelectEquipScreen(player, storeItem, inventoryItem);
    }
    
    public static void SelectStoreScreen(Player player, List<Item> storeItem, List<Item> inventoryItem)
    {
        Console.WriteLine($@"
            📍 상점
            필요한 아이템을 얻을 수 있는 상점입니다.

            [보유 골드]
            {player.gold} G

            [아이템 목록]
        ");

        foreach (Item item in storeItem)
        {
            Console.WriteLine($"- {item.name} | {item.powerType} +{item.powerValue} | {item.explanation} | {item.goldStr}");
        }

        Console.WriteLine($@"

            1. 아이템 구매
            0. 나가기

            원하시는 행동을 입력해주세요.
        ");

        int select = int.Parse(Console.ReadLine());

        if (select == 0) 
        {
            selectStartScreen(player, storeItem, inventoryItem);
        }
        else if (select == 1)
        {
            SelectBuyScreen(player, storeItem, inventoryItem);
        }
        else 
        {
            Console.WriteLine("❗️ 잘못된 입력입니다.");
        }
        SelectStoreScreen(player, storeItem, inventoryItem);
    }    

    public static void SelectBuyScreen(Player player, List<Item> storeItem, List<Item> inventoryItem) 
    {
        Console.WriteLine($@"
            📍 상점 - 아이템 구매
            필요한 아이템을 얻을 수 있는 상점입니다.

            [보유 골드]
            {player.gold} G

            [아이템 목록]
        ");

        foreach (Item item in storeItem)
        {
            Console.WriteLine($"- {item.num} {item.name} | {item.powerType} +{item.powerValue} | {item.explanation} | {item.goldStr}");
        }

        Console.WriteLine($@"

            0. 나가기

            원하시는 행동을 입력해주세요.
        ");

        int select = int.Parse(Console.ReadLine());


        if (select == 1 || select == 2 || select == 3 || select == 4 || select == 5 || select == 6)
        {
            if (storeItem[select - 1].having == true)
            {
                Console.WriteLine("❗️ 이미 구매한 아이템입니다.");
            }
            else
            {
                if (player.gold >= storeItem[select - 1].gold)
                {
                    Console.WriteLine("⭕️ 구매를 완료했습니다.");
                    player.gold -= storeItem[select - 1].gold;
                    storeItem[select - 1].having = true;
                    Item copyItem = storeItem[select - 1].DeepCopy();
                    inventoryItem.Add(copyItem);
                    inventoryItem[inventoryItem.Count - 1].num = inventoryItem.Count;
                    storeItem[select - 1].goldStr = "구매완료";
                    
                }
                else
                {
                    Console.WriteLine("❗️ Gold가 부족합니다.");
                }
            }
            // inventoryItem.Insert(select - 1, storeItem.Find(item => item.num == select));  // 아이템 번호를 통해 리스트 내의 객체를 가져옴        
        }
        else if (select == 0)
        {
            SelectStoreScreen(player, storeItem, inventoryItem);
        }
        else
        {
            Console.WriteLine("❗️ 잘못된 입력입니다.");
        }
        SelectBuyScreen(player, storeItem, inventoryItem);

    }    
    
    public static void Main(string[] args)
    {
        // 플레이어 정보 저장
        // Player player;
        // player.level = 1;
        // player.name = "";
        // player.chad = "전사";
        // player.attack = 10;
        // player.defense = 5;
        // player.hp = 100;
        // player.gold = 3000;

        Player player = new Player(1, "", "전사", 10, 5, 100, 3000);
        
        // 아이템 목록 저장
        Item item1 = new Item(1, "수련자 갑옷", "방어력", 5, "수련에 도움을 주는 갑옷입니다.", 1000);
        Item item2 = new Item(2, "무쇠갑옷", "방어력", 9 ,"무쇠로 만들어져 튼튼한 갑옷입니다.", 2000);
        Item item3 = new Item(3, "스파르타의 갑옷", "방어력", 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500);
        Item item4 = new Item(4, "낡은 검", "공격력", 2, "쉽게 볼 수 있는 낡은 검 입니다.", 600);
        Item item5 = new Item(5, "청동 도끼", "공격력", 5, "어디선가 사용됐던거 같은 도끼입니다.", 1500);
        Item item6 = new Item(6, "스파르타의 창", "공격력", 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2000);

        // 아이템 목록 리스트에 추가
        List<Item> storeItem = new List<Item>()
        {
            item1, item2, item3, item4, item5, item6
        };

        // List<Item> inventoryItem = new List<Item>(storeItem);
        // List<Item> inventoryItem = storeItem.ConvertAll(item => new Item(item.num, item.name, item.defense, item.explanation, item.gold));
        List<Item> inventoryItem = new List<Item>();

        selectStartScreen(player, storeItem, inventoryItem);

    }
}