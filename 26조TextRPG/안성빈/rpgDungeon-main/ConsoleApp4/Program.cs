using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApp4
{

    class Program
    {
        static void Main()
        {
            // 플레이어 캐릭터 초기 설정
            Character player = new Character("Chad", 100, 15, 5, 10, 2500);
            player.Inventory = new Inventory();
            Shop shop = new Shop(player);
            Inn inn = new Inn(player);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 던전 탐험");
                Console.WriteLine("5. 여관");
                Console.WriteLine("6. 저장하기");
                Console.WriteLine("7. 불러오기");
                Console.WriteLine();
                Console.Write("원하시는 행동을 입력해주세요: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        player.DisplayInfo();
                        break;
                    case "2":
                        player.Inventory.ManageInventory(player);
                        break;
                    case "3":
                        shop.ManageShop();
                        break;
                    case "4":
                        Dungeon dungeon = new Dungeon(player);
                        dungeon.Start();
                        break;
                    case "5":
                        inn.Rest();
                        break;
                    case "6":
                        SaveGame(player);
                        break;
                    case "7":
                        LoadGame(ref player);
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private static void SaveGame(Character player)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "savegame.json");
            player.SaveCharacter(filePath);
            Console.WriteLine("게임이 저장되었습니다.");
            Console.WriteLine(Directory.GetCurrentDirectory());
            Console.ReadLine(); // 입력 대기
        }

        private static void LoadGame(ref Character player)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "savegame.json");
            Console.WriteLine(Directory.GetCurrentDirectory());
            player.LoadCharacter(filePath);
            Console.WriteLine("게임이 불러와졌습니다.");
            Console.ReadLine(); // 입력 대기
        }
    }


    // 캐릭터 클래스
    public class Character
    {
        public string Name { get; private set; }
        public int Health { get; set; }
        public int MaxHealth { get; private set; }
        public int BaseAttackPower { get; private set; }
        public int BaseDefensePower { get; private set; }
        public int Speed { get; private set; }
        public int Gold { get; private set; }
        public int Level { get; private set; }
        public int Experience { get; set; }
        public int ExperienceToNextLevel { get; private set; }
        public int StatPoints { get; private set; }
        public int ActionGauge { get; private set; }

        public Inventory Inventory { get; set; } // 인벤토리
        public Item EquippedWeapon { get; private set; } // 장착된 무기
        public Item EquippedArmor { get; private set; }  // 장착된 방어구

        public bool IsDefending { get; private set; } // 방어 여부를 확인하는 변수

        public int TotalAttackPower => BaseAttackPower + (EquippedWeapon?.Attack ?? 0); // 무기 공격력 포함
        public int TotalDefensePower => BaseDefensePower + (EquippedArmor?.Defense ?? 0); // 방어구 방어력 포함

        public Character(string name, int health, int attackPower, int DefensePower, int speed, int gold)
        {
            Name = name;
            Health = MaxHealth = health;
            BaseAttackPower = attackPower;
            BaseDefensePower = DefensePower;
            Speed = speed;
            Gold = gold;
            Level = 1;
            Experience = 0;
            ExperienceToNextLevel = 100;
            StatPoints = 0;
            ActionGauge = 0;
            Inventory = new Inventory();
            IsDefending = false;
        }

        public List<Item> EquippedItems
        {
            get
            {
                List<Item> equippedItems = new List<Item>();
                if (EquippedWeapon != null) equippedItems.Add(EquippedWeapon);
                if (EquippedArmor != null) equippedItems.Add(EquippedArmor);
                return equippedItems;
            }
        }

        // 무기 장착 메소드
        public void EquipWeapon(Item weapon)
        {
            if (weapon.Type == ItemType.Weapon)
            {
                EquippedWeapon = weapon;
                Console.WriteLine($"{weapon.Name}을(를) 장착했습니다.");
            }
            else
            {
                Console.WriteLine("이 아이템은 무기가 아닙니다.");
            }
        }

        // 방어구 장착 메소드
        public void EquipArmor(Item armor)
        {
            if (armor.Type == ItemType.Armor)
            {
                EquippedArmor = armor;
                Console.WriteLine($"{armor.Name}을(를) 장착했습니다.");
            }
            else
            {
                Console.WriteLine("이 아이템은 방어구가 아닙니다.");
            }
        }
    

        public void DisplayInfo()
        {
            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine($"이름: {Name}");
            Console.WriteLine($"체력: {Health}/{MaxHealth}");
            Console.WriteLine($"공격력: {TotalAttackPower}"); 
            Console.WriteLine($"방어력: {TotalDefensePower}");
            Console.WriteLine($"속도: {Speed}");
            Console.WriteLine($"레벨: {Level}");
            Console.WriteLine($"경험치: {Experience}/{ExperienceToNextLevel}");
            Console.WriteLine($"스탯 포인트: {StatPoints}");
            Console.WriteLine($"골드: {Gold}");
            Console.WriteLine($"인벤토리 아이템 수: {EquippedItems.Count}");
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.Write("원하시는 행동을 입력해주세요: ");
            string input = Console.ReadLine();
            if (input == "0") return;
        }

        public void IncreaseActionGauge()
        {
            ActionGauge += Speed;
            if (ActionGauge > 100)
            {
                ActionGauge = 100;
            }
            Console.WriteLine($"행동 게이지가 {Speed}% 증가하여 현재 {ActionGauge}%입니다.");
        }

        public bool CanAct()
        {
            return ActionGauge >= 100;
        }

        public void ResetActionGauge()
        {
            ActionGauge = 0;
        }

        public void GainExperience(int amount)
        {
            Experience += amount;
            Console.WriteLine($"{amount} 경험치를 획득했습니다.");
            if (Experience >= ExperienceToNextLevel)
            {
                LevelUp();
            }
        }

        public void LevelUp()
        {
            Experience -= ExperienceToNextLevel;
            Level++;
            ExperienceToNextLevel = (int)(ExperienceToNextLevel * 1.5);
            StatPoints += 5;
            Console.WriteLine("레벨업! 스탯 포인트 5점을 획득했습니다.");
            DistributeStatPoints();
        }

        public void DistributeStatPoints()
        {
            while (StatPoints > 0)
            {
                Console.Clear();
                Console.WriteLine("스탯 포인트를 분배하세요.");
                Console.WriteLine($"남은 스탯 포인트: {StatPoints}");
                Console.WriteLine("1. 공격력");
                Console.WriteLine("2. 방어력");
                Console.WriteLine("3. 체력");
                Console.WriteLine("4. 속도");
                Console.Write("원하는 스탯을 선택하세요: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        BaseAttackPower++;
                        Console.WriteLine("공격력이 증가했습니다.");
                        break;
                    case "2":
                        BaseDefensePower++;
                        Console.WriteLine("방어력이 증가했습니다.");
                        break;
                    case "3":
                        MaxHealth += 10;
                        Health = MaxHealth;
                        Console.WriteLine("최대 체력이 증가했습니다.");
                        break;
                    case "4":
                        Speed++;
                        Console.WriteLine("속도가 증가했습니다.");
                        break;
                    default:
                        Console.WriteLine("잘못된 선택입니다.");
                        continue;
                }
                StatPoints--;
            }
        }

        public void SpendGold(int amount)
        {
            Gold -= amount;
        }

        public void EarnGold(int amount)
        {
            Gold += amount;
            Console.WriteLine($"{amount} 골드를 획득했습니다.");
        }

        public void Heal(int amount)
        {
            Health += amount;
            if (Health > MaxHealth) Health = MaxHealth;
            Console.WriteLine($"체력이 {amount}만큼 회복되었습니다.");
        }

        public void Attack(Enemy enemy)
        {
            int damage = TotalAttackPower - enemy.DefensePower;
            if (damage < 0) damage = 0;
            enemy.Health -= damage;
            Console.WriteLine($"{Name}이(가) {enemy.Name}에게 {damage}만큼의 피해를 입혔습니다.");
        }

        public void Defend()
        {
            IsDefending = true;
            Console.WriteLine($"{Name}이(가) 방어 태세를 취했습니다.");
        }

        public void ResetDefense()
        {
            IsDefending = false;
        }

        public void SaveCharacter(string filePath)
        {
            var characterData = new
            {
                Name,
                Health,
                MaxHealth,
                BaseAttackPower,
                BaseDefensePower,
                Speed,
                Gold,
                Level,
                Experience,
                ExperienceToNextLevel,
                StatPoints,
                InventoryItems = Inventory.GetItems(),
                EquippedWeapon = EquippedWeapon?.Name, // 장비의 이름을 저장
                EquippedArmor = EquippedArmor?.Name // 장비의 이름을 저장
            };

            string json = JsonConvert.SerializeObject(characterData, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }


        public void LoadCharacter(string filePath)
        {
            var jsonData = File.ReadAllText(filePath);
            dynamic characterData = JsonConvert.DeserializeObject(jsonData);

            Name = characterData.Name;
            Health = characterData.Health;
            MaxHealth = characterData.MaxHealth;
            BaseAttackPower = characterData.BaseAttackPower;
            BaseDefensePower = characterData.BaseDefensePower;
            Speed = characterData.Speed;
            Gold = characterData.Gold;
            Level = characterData.Level;
            Experience = characterData.Experience;
            ExperienceToNextLevel = characterData.ExperienceToNextLevel;
            StatPoints = characterData.StatPoints;

            // 인벤토리 아이템 불러오기
            foreach (var item in characterData.InventoryItems)
            {
                Inventory.AddItem(new Item(
                    item.Name.ToString(),
                    (int)item.Attack,
                    (int)item.Defense,
                    item.Description.ToString(),
                    (ItemType)Enum.Parse(typeof(ItemType), item.Type.ToString()),
                    (int)item.Price,
                    (bool)item.IsPurchased
                ));
            }

            if (characterData.EquippedWeapon != null)
            {
                string equippedWeaponName = characterData.EquippedWeapon.Name.ToString();
                EquippedWeapon = Inventory.GetItems().FirstOrDefault(i => i.Name == equippedWeaponName);
                if (EquippedWeapon != null) EquippedWeapon.Purchase(); // 장착된 것으로 표시
            }

            if (characterData.EquippedArmor != null)
            {
                string equippedArmorName = characterData.EquippedArmor.Name.ToString();
                EquippedArmor = Inventory.GetItems().FirstOrDefault(i => i.Name == equippedArmorName);
                if (EquippedArmor != null) EquippedArmor.Purchase(); // 장착된 것으로 표시
            }
        }






    }


    // 여관 클래스
    public class Inn
    {
        private Character player;

        public Inn(Character player)
        {
            this.player = player;
        }

        public void Rest()
        {
            int cost = 50;
            if (player.Gold >= cost)
            {
                player.SpendGold(cost);
                player.Heal(player.MaxHealth);
                Console.WriteLine("여관에서 휴식을 취하여 체력을 모두 회복했습니다.");
            }
            else
            {
                Console.WriteLine("골드가 부족합니다.");
            }
            Console.ReadLine();
        }
    }

    // 던전 클래스
    public class Dungeon
    {
        private Character player;

        public Dungeon(Character player)
        {
            this.player = player;
        }

        public void Start()
        {
            List<Enemy> enemies = new List<Enemy>
        {
            new Enemy("고블린", 50, 10, 2, 8, 20, 50),
            new Enemy("오크", 80, 12, 4, 6, 40, 100),
            new Enemy("트롤", 100, 8, 6, 5, 60, 150)
        };

            Random random = new Random();
            Enemy enemy = enemies[random.Next(enemies.Count)];

            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== 당신은 몬스터를 조우했다. ====");
                Console.WriteLine($"{player.Name} VS {enemy.Name}");
                Console.WriteLine();

                player.IncreaseActionGauge();
                enemy.IncreaseActionGauge();

                DisplayStatus(player, enemy);

                if (player.CanAct())
                {
                    PlayerTurn(enemy);
                }

                if (enemy.EnemyCanAct())
                {
                    EnemyTurn(enemy);
                }

                if (player.Health <= 0)
                {
                    Console.WriteLine("당신은 패배했습니다....");
                    Console.WriteLine("아무키나 누르세요...");
                    Console.ReadKey();
                    break;
                }
                else if (enemy.Health <= 0)
                {
                    Console.WriteLine($"당신은 {enemy.Name}을 물리쳤습니다.");
                    player.GainExperience(enemy.ExperienceReward);
                    player.EarnGold(enemy.GoldReward);
                    Console.WriteLine("아무키나 누르세요...");
                    Console.ReadKey();
                    break;
                }

                Console.WriteLine("아무키나 누르세요...");
                Console.ReadKey();
            }

            ReturnToTown();
        }

        private void ReturnToTown()
        {
            Console.WriteLine("마을로 돌아갑니다...");
            Console.WriteLine("아무키나 누르세요...");
            Console.ReadKey();
        }

        private void DisplayStatus(Character player, Enemy enemy)
        {
            Console.WriteLine($"{player.Name}: Health = {player.Health}/{player.MaxHealth}, Action Gauge = {player.ActionGauge}%");
            Console.WriteLine($"{enemy.Name}: Health = {enemy.Health}, Action Gauge = {enemy.ActionGauge}%");
        }

        private void PlayerTurn(Enemy enemy)
        {
            player.ResetDefense();
            Console.WriteLine("당신의 턴입니다. 할 행동을 선택하십시오.");
            Console.WriteLine("1. 공격");
            Console.WriteLine("2. 방어");

            bool validInput = false;
            while (!validInput)
            {
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    player.Attack(enemy);
                    validInput = true;
                }
                else if (choice == "2")
                {
                    player.Defend();
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 선택하세요.");
                }
            }

            player.ResetActionGauge();
        }

        private void EnemyTurn(Enemy enemy)
        {
            Console.WriteLine("적의 턴입니다....");
            enemy.Attack(player);
            enemy.EnemyResetActionGauge();
        }
    }

    // 몬스터 클래스
    public class Enemy
    {
        public string Name { get; }
        public int Health { get; set; }
        public int MaxHealth { get; }
        public int AttackPower { get; }
        public int DefensePower { get; }
        public int Speed { get; }
        public int ExperienceReward { get; }
        public int GoldReward { get; }
        public int ActionGauge { get; private set; }  // 행동 게이지 추가

        public Enemy(string name, int health, int attackPower, int defensePower, int speed, int experienceReward, int goldReward)
        {
            Name = name;
            Health = MaxHealth = health;
            AttackPower = attackPower;
            DefensePower = defensePower;
            Speed = speed;
            ExperienceReward = experienceReward;
            GoldReward = goldReward;
            ActionGauge = 0; // 초기화
        }

        public bool IsAlive()
        {
            return Health > 0;
        }

        public void Attack(Character character)
        {
            int defense = character.TotalDefensePower;

            if (character.IsDefending)
            {
                defense *= 2;
                Console.WriteLine($"{character.Name}이(가) 방어 태세로 데미지를 줄입니다.");
            }

            int damage = AttackPower - defense;
            if (damage < 0) damage = 0;
            character.Health -= damage;
            Console.WriteLine($"{Name}이(가) {character.Name}에게 {damage}만큼의 피해를 입혔습니다.");
        }

        public void IncreaseActionGauge()
        {
            ActionGauge += Speed;
            if (ActionGauge > 100) ActionGauge = 100;
        }

        public bool EnemyCanAct()
        {
            return ActionGauge >= 100; // 행동 게이지가 100 이상일 때 행동 가능
        }

        public void EnemyResetActionGauge()
        {
            ActionGauge = 0; // 행동 후 게이지 리셋
        }
    }

    // 상점 클래스
    public class Shop
    {
        private Character player;
        private List<Item> shopItems;

        public Shop(Character player)
        {
            this.player = player;
            shopItems = new List<Item>
            {
                new Item("수련자 갑옷", 0, 5, "수련에 도움을 주는 갑옷입니다.", ItemType.Armor, 1000),
                new Item("무쇠갑옷", 0, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", ItemType.Armor, 1500),
                new Item("스파르타의 갑옷", 0, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", ItemType.Armor, 3500),
                new Item("낡은 검", 2, 0, "쉽게 볼 수 있는 낡은 검입니다.", ItemType.Weapon, 600),
                new Item("청동 도끼", 5, 0, "어디선가 사용됐던 도끼입니다.", ItemType.Weapon, 1500),
                new Item("스파르타의 창", 7, 0, "스파르타의 전사들이 사용했다는 전설의 창입니다.", ItemType.Weapon, 2500)
            };
        }

        public void ManageShop()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("상점");
                Console.WriteLine("1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기");
                Console.Write("원하는 옵션의 번호를 입력하세요: ");

                string input = Console.ReadLine();
                if (input == "0") break;

                if (input == "1")
                {
                    BuyItems();
                }
                else if (input == "2")
                {
                    SellItems();
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadLine(); // 입력 대기
                }
            }
        }

        private void BuyItems()
        {
            Console.Clear();
            Console.WriteLine("구매할 아이템을 선택하세요.");

            for (int i = 0; i < shopItems.Count; i++)
            {
                var item = shopItems[i];
                string purchasedMark = item.IsPurchased ? "[구매됨] " : "";
                Console.WriteLine($"{i + 1}. {purchasedMark}{item.Name} | 방어력 +{item.Defense} | 공격력 +{item.Attack} | 가격: {item.Price} G");
            }

            Console.WriteLine("0. 나가기");
            Console.Write("구매할 아이템의 번호를 입력해주세요: ");

            string input = Console.ReadLine();
            if (input == "0") return; // 구매 메뉴로 돌아감

            if (int.TryParse(input, out int index) && index > 0 && index <= shopItems.Count)
            {
                var item = shopItems[index - 1];

                // 장착된 아이템인지 확인
                if (player.Inventory.HasEquippedItem(item, player))
                {
                    Console.WriteLine("장착된 아이템은 판매할 수 없습니다.");
                    Console.ReadLine(); // 입력 대기
                    return;
                }

                if (!item.IsPurchased && player.Gold >= item.Price)
                {
                    player.SpendGold(item.Price);
                    item.Purchase();
                    player.Inventory.AddItem(item);
                    Console.WriteLine($"{item.Name}을(를) 구매하셨습니다.");
                }
                else if (item.IsPurchased)
                {
                    Console.WriteLine("이미 구매한 아이템입니다.");
                }
                else
                {
                    Console.WriteLine("골드가 부족합니다.");
                }
                Console.ReadLine(); // 입력 대기
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadLine(); // 입력 대기
            }
        }

        private void SellItems()
        {
            Console.Clear();
            Console.WriteLine("판매할 아이템을 선택하세요.");

            var itemsToSell = player.Inventory.GetItems(); // 플레이어의 인벤토리에서 아이템을 가져오는 메소드
            for (int i = 0; i < itemsToSell.Count; i++)
            {
                var item = itemsToSell[i];
                Console.WriteLine($"{i + 1}. {item.Name} | 방어력 +{item.Defense} | 공격력 +{item.Attack} | 가격: {item.Price * 3 / 4} G");
            }

            Console.WriteLine("0. 나가기");
            Console.Write("판매할 아이템의 번호를 입력해주세요: ");

            string input = Console.ReadLine();
            if (input == "0") return; // 판매 메뉴로 돌아감

            if (int.TryParse(input, out int index) && index > 0 && index <= itemsToSell.Count)
            {
                var item = itemsToSell[index - 1];

                // 장착된 아이템은 판매할 수 없음
                if (player.Inventory.HasEquippedItem(item, player))
                {
                    Console.WriteLine("장착된 아이템은 판매할 수 없습니다.");
                    Console.ReadLine(); // 입력 대기
                    return;
                }

                player.Inventory.RemoveItem(item); // 아이템을 인벤토리에서 제거
                player.EarnGold(item.Price * 3 / 4); // 아이템의 가격만큼 골드 추가
                Console.WriteLine($"{item.Name}을(를) 판매하셨습니다.");
                Console.ReadLine(); // 입력 대기
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadLine(); // 입력 대기
            }
        }

        
    }

    //인벤토리 클래스
    public class Inventory
    {
        private List<Item> items;

        public Inventory()
        {
            items = new List<Item>();
        }

        public void AddItem(Item item)
        {
            items.Add(item);
        }

        public void RemoveItem(Item item)
        {
            if (items.Contains(item))
            {
                items.Remove(item);
            }
        }

        // 아이템 목록 반환 메소드
        public List<Item> GetItems()
        {
            return new List<Item>(items); // 인벤토리에 있는 아이템 목록 반환
        }

        public int ItemCount() => items.Count;

        public void ManageInventory(Character player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("인벤토리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine("[아이템 목록]");

                for (int i = 0; i < items.Count; i++)
                {
                    string equippedMark = player.EquippedItems.Contains(items[i]) ? "[E] " : "";
                    Console.WriteLine($"{i + 1}. {equippedMark}{items[i].Name} | 공격력 +{items[i].Attack} | 방어력 +{items[i].Defense} | {items[i].Description}");
                }

                Console.WriteLine("1. 장착 관리");
                Console.WriteLine("0. 나가기");
                Console.Write("원하시는 행동을 입력해주세요: ");

                string input = Console.ReadLine();
                if (input == "0") break;

                if (input == "1")
                {
                    ManageEquipment(player);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadLine(); // 입력 대기
                }
            }
        }

        private void ManageEquipment(Character player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("장착 관리");
                Console.WriteLine("보유 중인 아이템을 장착하거나 해제할 수 있습니다.");
                Console.WriteLine("[아이템 목록]");

                for (int i = 0; i < items.Count; i++)
                {
                    string equippedMark = "";
                    if (player.EquippedWeapon == items[i])
                    {
                        equippedMark = "[무기 장착됨] ";
                    }
                    else if (player.EquippedArmor == items[i])
                    {
                        equippedMark = "[방어구 장착됨] ";
                    }

                    Console.WriteLine($"{i + 1}. {equippedMark}{items[i].Name} | 공격력 +{items[i].Attack} | 방어력 +{items[i].Defense} | {items[i].Description}");
                }

                Console.WriteLine("0. 나가기");
                Console.Write("원하시는 행동을 입력해주세요: ");

                string input = Console.ReadLine();
                if (input == "0") break;

                if (int.TryParse(input, out int index) && index > 0 && index <= items.Count)
                {
                    var item = items[index - 1];
                    if (item.Type == ItemType.Weapon)
                    {
                        player.EquipWeapon(item); // 무기 장착
                    }
                    else if (item.Type == ItemType.Armor)
                    {
                        player.EquipArmor(item); // 방어구 장착
                    }
                    Console.ReadLine(); // 입력 대기
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadLine(); // 입력 대기
                }
            }
        }

        public bool HasEquippedItem(Item item, Character character)
        {
            // 아이템이 장착된 경우 true 반환
            List<Item> equippedItems = character.EquippedItems;
            return equippedItems.Contains(item);
        }

    }

    //아이템 클래스
    public enum ItemType
    {
        Weapon,
        Armor
    }

    public class Item
    {
        public string Name { get; }
        public int Attack { get; }
        public int Defense { get; }
        public string Description { get; }
        public int Price { get; }
        public bool IsPurchased { get; private set; }
        public ItemType Type { get; } // 아이템 타입 추가

        public Item(string name, int attack, int defense, string description, ItemType type, int price = 0, bool isPurchased = false)
        {
            Name = name;
            Attack = attack;
            Defense = defense;
            Description = description;
            Type = type; // 무기인지 방어구인지 타입을 설정
            Price = price;
            IsPurchased = isPurchased;
        }

        public void Purchase()
        {
            IsPurchased = true;
        }

    }

}


