using TextRPG02.Character;
using TextRPG02.Class.Interface;
using TextRPG02.Class;
using TextRPG02.Item.Interface;
using TextRPG02.Race.Interface;
using TextRPG02.Race;
using TextRPG02.Item;
using System;

namespace TextRPG02
{

    internal class Program
    {
        public static void Inventory(Player player)
        {
            if (player.Inventory.Count == 0)
            {
                Console.WriteLine("인벤토리가 비어 있습니다.");
                return;
            }
            Console.WriteLine();
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("인벤토리 아이템 목록:");
            for (int i = 0; i < player.Inventory.Count; i++)
            {
                IItem item = player.Inventory[i];
                string itemType = item is Weapon ? "무기" : item is Armor ? "방어구" : "아이템";

                // 아이템이 장착된 상태인지 확인
                bool isEquipped = false;
                if (item == player.EquippedWeapon || item == player.EquippedArmor)
                {
                    isEquipped = true;
                }
                // 장착된 아이템 앞에 [E] 추가
                string equippedIndicator = isEquipped ? "[E] " : "" ;
                

                Console.WriteLine($"{i + 1}. {equippedIndicator}[{itemType}] {item.Name}");
            }
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine();

            Console.WriteLine("a. 아이템 장착(사용) ");
            Console.WriteLine("b. 아이템 장착해제 ");
            string Choice = Console.ReadLine();

            if (Choice == "a")
            {
                UseItem(player);
            }
            else if (Choice == "b") 
            {
                UnequipItem(player);
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                return;
            }

        }
        public static void UseItem(Player player)
        {
            

            Console.WriteLine("사용하거나 장착할 아이템의 번호를 입력하세요:");
            int choice = int.Parse(Console.ReadLine());

            if (choice > 0 && choice <= player.Inventory.Count)
            {
                IItem item = player.Inventory[choice - 1];

                if (item is IConsumable consumable)
                {
                    consumable.Consume(player);
                    player.Inventory.Remove(item);
                }
                else if (item is IEquippable equippable)
                {
                    equippable.Equip(player);
                }
                else
                {
                    Console.WriteLine("이 아이템은 사용할 수 없습니다.");
                }
            }
            else
            {
                Console.WriteLine("잘못된 선택입니다.");
            }
        }

        public static void UnequipItem(Player player)
        {
            Console.WriteLine("해제할 장비를 선택하세요:");
            Console.WriteLine("1. 무기 해제");
            Console.WriteLine("2. 방어구 해제");
            Console.WriteLine("3. 취소");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    if (player.EquippedWeapon != null)
                    {
                        player.EquippedWeapon.Unequip(player);
                    }
                    else
                    {
                        Console.WriteLine("장착된 무기가 없습니다.");
                    }
                    break;
                case "2":
                    if (player.EquippedArmor != null)
                    {
                        player.EquippedArmor.Unequip(player);
                    }
                    else
                    {
                        Console.WriteLine("장착된 방어구가 없습니다.");
                    }
                    break;
                case "3":
                    Console.WriteLine("취소되었습니다.");
                    break;
                default:
                    Console.WriteLine("잘못된 선택입니다.");
                    break;
            }
        }
        static void Main(string[] args)
        {

            //이름 입력
            Console.WriteLine("캐릭터의 이름을 입력하세요:");
            string characterName = Console.ReadLine() ?? "";

            // 종족 선택
            IRace selectedRace = null;
            while (selectedRace == null)
            {
                Console.WriteLine("캐릭터의 종족을 선택하세요: ");
                Console.WriteLine("1. 인간, 2. 엘프, 3. 드워프, 4. 오거, 5. 스프리건");
                string raceChoice = Console.ReadLine() ?? "";

                switch (raceChoice)
                {
                    case "1":
                        selectedRace = new Human();
                        break;
                    case "2":
                        selectedRace = new Elf();
                        break;
                    case "3":
                        selectedRace = new Dwarf();
                        break;
                    case "4":
                        selectedRace = new Ogre();
                        break;
                    case "5":
                        selectedRace = new Spriggan();
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다. 다시 시도하세요.");
                        break;
                }
            }

            // 클래스 선택
            IClass selectedClass = null;
            while (selectedClass == null)
            {
                Console.WriteLine("캐릭터의 클래스를 선택하세요: ");
                Console.WriteLine("1. 파이터, 2. 로그, 3. 위저드, 4. 워락, 5. 바드, 6. 레인저");
                string classChoice = Console.ReadLine() ?? "";    

                switch (classChoice)
                {
                    case "1":
                        selectedClass = new Fighter();
                        break;
                    case "2":
                        selectedClass = new Rogue();
                        break;
                    case "3":
                        selectedClass = new Wizard();
                        break;
                    case "4":
                        selectedClass = new Warlock();
                        break;
                    case "5":
                        selectedClass = new Bard();
                        break;
                    case "6":
                        selectedClass = new Ranger();
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다. 다시 시도하세요.");
                        break;
                }
            }

            int level = 1;
            int currentStage = 0;
            // 캐릭터 생성
            Player player = new Player(characterName, selectedRace, selectedClass, level);
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------------------");
            Console.WriteLine("부귀영화를 누리기위해? 아니면 그저 모험심이 뛰어난 탓일까요?");
            Console.WriteLine("어떤이유이던지간에, 당신은 던전으로 모험을 떠납니다.");
            Console.WriteLine("--------------------------------------------------------------------");
            Console.WriteLine();
            Shop shop = new Shop(eShop.Startshop);

            bool isIntro = true;
            while (isIntro)
            {

                Console.WriteLine("무엇을 하시겠습니까?");
                Console.WriteLine("a. 상점 방문");
                Console.WriteLine("i. 인벤토리 확인 및 아이템 사용");
                Console.WriteLine("c. 캐릭터 정보 보기");
                Console.WriteLine("d. 던전 입장");
                Console.WriteLine("E. 게임 종료");
                string input = Console.ReadLine() ?? "";

                switch (input)
                {
                    case "a":
                        shop.BuyItem(player);
                        break;
                    case "i":
                        Inventory(player);
                        break;
                    case "c":
                        player.DisplayCharacterInfo();
                        break;
                    case "d":
                        currentStage = 1;
                        isIntro = false;
                        break;
                    case "E":
                        isIntro = false;
                        Console.WriteLine("게임을 종료합니다.");
                        break;
                    case "e":
                        Console.WriteLine("대문자를 입력해주세요.");
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }

            while (currentStage <= 20 || currentStage >= 1)
            {
                Stage stage = new(currentStage);
                shop = null;
                while (!stage.isCompleted)
                {
                    Console.WriteLine("--------------------------------------------------------------------");
                    Console.WriteLine("무엇을 하시겠습니까?");
                    Console.WriteLine();
                    Console.WriteLine("a. 탐험하기");
                    Console.WriteLine("c. 캐릭터 정보 보기");
                    Console.WriteLine("i. 인벤토리 확인 및 아이템 사용");
                    if (stage.ShopFound)
                    {
                        Random random = new Random();
                        int randomshop = random.Next(1, 3);
                        switch (randomshop)
                        {
                            case 1:
                                shop = new Shop(eShop.Armor);
                                break;
                            case 2:
                                shop = new Shop(eShop.Consum);
                                break;
                            case 3:
                                shop = new Shop(eShop.Weapon);
                                break;
                        }
                        Console.WriteLine("s. 상점 입장");
                    }
                    if (stage.StairFound)
                    {
                        Console.WriteLine("n. 다음 스테이지로");
                    }
                    Console.WriteLine("E. 게임 종료");
                    Console.WriteLine("--------------------------------------------------------------------");

                    string input = Console.ReadLine() ??"";
                    switch (input)
                    {
                        case "a":
                            stage.Explore(player);
                            break;
                        case "c":
                            player.DisplayCharacterInfo();
                            break;
                        case "i":
                            Inventory(player);
                            break;
                        case "s":
                            if (!stage.ShopFound)
                            {
                                Console.WriteLine("잘못된 입력입니다.");
                            }
                            else
                            {
                                Console.WriteLine("당신은 상점으로 향합니다.");
                                shop.BuyItem(player);
                            }
                                break;
                        case "n":
                            if (!stage.StairFound)
                            {
                                Console.WriteLine("잘못된 입력입니다.");
                            }
                            else
                            {
                                stage.isCompleted = true;
                            }
                            break;
                        case "E":
                            Console.WriteLine("게임을 종료합니다.");
                            return;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            break;
                    }
                    
                    if (player.Health <= 0)
                    {
                        Console.WriteLine("죽었습니다...........");
                        return;
                    }

                }

                currentStage++;

            }

        }
    }
}
