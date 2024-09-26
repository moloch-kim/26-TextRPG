using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TextRPG02.Character;
using TextRPG02.Class.Interface;
using TextRPG02.Race.Interface;
using TextRPG02.Race.NonPlayerRace;
using TextRPG02.Item;
using TextRPG02.Item.Interface;
using TextRPG02.Race;
using TextRPG02.Class.NonPlayerClass;

namespace TextRPG02
{
    public class Stage
    {
        public int StageNumber { get; set; }

        public int Progress { get; set; }
        public int MaxProgress { get; set; }
        public bool StairFound { get; set; }
        public bool ShopFound {  get; set; }
        public bool isCompleted {  get; set; }

        

        private Random random;
        private int stairposition;




        public Stage(int stageNumber)
        {
            StageNumber = stageNumber;
            Progress = 0;
            MaxProgress = 95 + (stageNumber * 5);
            StairFound = false;
            ShopFound = false;
            isCompleted = false;

            random = new Random();

            stairposition = random.Next(20, MaxProgress);
            Console.Clear();
            Console.Write("당신은 던전에 입장했습니다."); Thread.Sleep(500); Console.Write("."); Thread.Sleep(500); Console.Write("."); Thread.Sleep(500); Console.Write("."); Thread.Sleep(500);
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------------------");
            Console.WriteLine($"[ 던전 {StageNumber}층 ]");
            Console.WriteLine();
        }

        public void Explore(Player player)
        {
            if (isCompleted)
            {
                Console.WriteLine("더 탐험할게 없습니다.");
                return;
            }
            else
            {
                Progress += 10;
                int randomtext = random.Next(1, 4);
                switch (randomtext)
                {
                    case 1:
                        Console.Write("당신은 어두운 방과 던전 사이를 걷습니다.");
                        break;
                    case 2:
                        Console.Write("당신의 길은 어둠과 미지로 가득차있습니다.");
                        break;
                    case 3:
                        Console.Write($"[{player.Name}] : 온통 어둠과 먼지 뿐이군.");
                        break;
                }

                Thread.Sleep(500); Console.Write("."); Thread.Sleep(500); Console.Write("."); Thread.Sleep(500); Console.Write("."); Thread.Sleep(500); Console.WriteLine(".");
            }
            
            Console.WriteLine($"진행도 : {Progress}/{MaxProgress}");
            Thread.Sleep(200);

            FindStair(player);
            TriggerEvent(player);

            if(Progress >= MaxProgress)
            {
                if (!StairFound)
                {
                    Console.WriteLine("무언가 잘못되었습니다. 스테이지를 다시 시작합니다.");
                    //리스타트
                }
                else
                {
                    Console.WriteLine($"{StageNumber}층 탐험이 완료되었습니다!");
                }
            }
        }
        public void FindStair(Player player)
        {
            if (Progress >= stairposition && !StairFound)
            {
                StairFound = true;
                Console.WriteLine("계단을 발견했습니다! 다음 스테이지로 이동 가능합니다.");

                // 다음 스테이지 진행 선택지 활성화 로직

            }
            else
            {
                return;
            }
        }
        public void TriggerEvent(Player player)
        {
            int eventChance = random.Next(1, 101);

            if (eventChance <= 50)
            {
                EncounterEnemy(player);
            }
            else if (eventChance <= 70)
            {
                FindShop(player);
            }
            else if (eventChance <= 90)
            {
                FindItem(player);
            }
            else
            {
                Console.WriteLine("아무일도 일어나지 않았습니다...");
            }

        }

        public void RandomEnemy(out Enemy enemy)
        {
            IRace Erace = null;
            IClass Eclass = null;
            Random random = new Random();

            int Racenum = random.Next(1,3);
            switch (Racenum)
            {
                case 1: 
                    Erace = new NpGoblin();
                    break;
                case 2:
                    Erace = new NpLizardman();
                    break;
                default:
                    Erace = new Human();
                    break;
            }
            int enemyClass = random.Next(1, 5);
            switch (enemyClass)
            {
                case 1:
                    Eclass = new NpRaider();
                    break;
                case 2:
                    Eclass = new NpScoundrel();
                    break;
                case 3:
                    Eclass = new NpWeakling();
                    break;
                case 4:
                        Eclass = new NpShaman();
                    break;
                default:
                    Eclass = new NpRaider(); ;
                    break;
            }

            enemy = new Enemy("적",Erace,Eclass,StageNumber);


        }

        private void EncounterEnemy(Player player)
        {
            Console.WriteLine("어둠속에서 무언가가 움직입니다....!!");
            Thread.Sleep(800);
            Console.WriteLine();
            int encountertext = random.Next(1, 3);

            switch(encountertext)
            {
                case 1: 
                    Console.WriteLine("역시 적입니다! 전투 준비!");
                        break;
                case 2: 
                    Console.WriteLine("적을 만났습니다! 전투에 들어갑니다!");
                        break;
                case 3:
                    Console.WriteLine($"[{player.Name}] : 덤벼라!! 너같은 애송이가 내 길을 막게 두지 않겠다!");
                        break;
            }
            Thread.Sleep(800);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("계속__(아무키나 입력해 진행)");
            Console.ReadLine();
            Console.Clear();

            Combat combat = new();
            //Enemy enemy = null;
            RandomEnemy(out Enemy enemy);

            combat.combat(player, enemy);

            // 전투 로직 구현

        }

        private void FindShop(Player player)
        {
            Console.WriteLine("상점을 발견했습니다!");
            Thread.Sleep(500);
            int encountertext = random.Next(1, 3);

            switch (encountertext)
            {
                case 1:
                    Console.WriteLine("이런곳에 상점이라니 기이하군요.");
                    break;
                case 2:
                    Console.WriteLine("이곳에서 물건을 사거나 휴식할수 있습니다.");
                    break;
                case 3:
                    Console.WriteLine($"{player.Name} : 이런곳에 상점이라니?");
                    break;
            }
            ShopFound = true;
            //상점 선택지 활성화 로직
        }

        private void FindItem(Player player)
        {
            Console.WriteLine("무언가 반짝이는게 보입니다!");
            Thread.Sleep(500);
            int eventChance = random.Next(1, 101);
            if (eventChance <= 50)
            {
                int randomGold = random.Next(5 + (StageNumber * 2), 500 + (StageNumber * 5));
                Console.WriteLine("금화를 발견했습니다!");
                player.Gold += randomGold;
            }
            else
            {
                Console.WriteLine("포션을 발견했습니다!");
                player.Inventory.Add(ItemRepository.GetRandomPotion());
                //인벤토리에 포션 추가

            }
            Thread.Sleep(500);
            int encountertext = random.Next(1, 3);
            switch (encountertext)
            {
                case 1:
                    Console.WriteLine("횡재로군요.");
                    break;
                case 2:
                    Console.WriteLine("앞으로의 여정에 도움이 될겁니다.");
                    break;
                case 3:
                    Console.WriteLine($"{player.Name} : 횡재로군.");
                    break;
            }
        }

    }
}
