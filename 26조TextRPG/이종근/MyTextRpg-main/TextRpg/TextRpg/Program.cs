using System;
using System.IO;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using Newtonsoft.Json;


namespace TextRpg
{
    public class Item
    {
        //아이템쪽 변수선언
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public int Type { get; set; } // 0: 방어구, 1: 무기
        public float Stat { get; set; }
        public bool IsEquipped { get; set; } // 장착 여부

        public Item(string name, int price, string description, int type, float stat)
        {
            Name = name;
            Price = price;
            Description = description;
            Type = type;
            Stat = stat;
            IsEquipped = false; // 기본값은 장착되지 않음
        }
    }
    public class GameData
    {
        public int level { get; set; }
        public int exp { get; set; }
        public int liveHp { get; set; }
        public int gold { get; set; }
        public Item[] Inventory { get; set; } = new Item[0];
        //리스트를 배열로 저장하는부분 따라하긴했지만 이해하지못함 (공부필요)
    }

    public class SaveLoadSystem
    {
        //게임 데이터가 저장될 파일 경로를 지정
        private static string filePath = "gameData.json";

        //데이터 저장용 메서드
        //저장을 위해선 ? GameData를 해당 메서드에 전달해줘야 그 내용을 저장할 수 있음
        public static void SaveGame(GameData data)
        {
            // 데이터를 JSON형식으로 직렬화 하는 코드
            //Formatting.Indented 이부분은 들여쓰기를 적용하는 "옵션"
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            //위에서 직렬화 한 JSON데이터를 파일로 저장
            File.WriteAllText(filePath, json);
            // 출력 및 슬립을 줘서 사용자가 저장중인것을 인지시킴
            Console.WriteLine("진행상황을 저장중입니다...");
            Thread.Sleep(3000);
        }

        //데이터 불러오기용 메서드
        public static GameData LoadGame()
        {
            //데이터 파일이 있는지 확인하고
            if (File.Exists(filePath))
            {
                //JSON데이터를 문자열로 변환
                string json = File.ReadAllText(filePath);
                //JSON형식의 문자열을 GameData객체로 변환해주는 코드 (압축과 압축풀기로 이해하면 매우 쉬움)
                GameData data = JsonConvert.DeserializeObject<GameData>(json);
                Console.WriteLine("진행상황을 불러오는 중입니다...");
                Thread.Sleep(3000);
                //불러온 변환한 데이터를 반환해서 게임내에 반영
                return data;
            }
            else
            {
                Console.WriteLine("불러올 파일이 없습니다.");
                Thread.Sleep(3000);
                return null;
            }
        }
    }
    public class Program
    {
        private static int dKeyPressCount = 0;
        static bool defenseMode = false; // 방어태세
        private static System.Timers.Timer timer;

        static int level = (exp / 100);
        static int exp = 100;

        int job = 1; //직업 1: 전사
        float baseStr = 9.5f + (level * 0.5f); // 공격력
        float baseDef = 4 + (level * 1f); // 방어력

        static int maxHp = 100; // 최대 체력
        static int hp = maxHp - liveHp;// 현재 체력
        static int liveHp; // 깎인 체력

        int gold = 1500; // 돈

        public List<Item> inventory = new List<Item>();//인벤토리용 아이템 리스트
        public List<Item> shopItems = new List<Item>();//상점용 아이템 리스트

        //전투매커니즘 만들기
        static Random random = new Random(); //랜덤값용
        public Program()
        {
            // 상점에 아이템 추가 이름, 금액, 설명, 아이템종류(방어구:0 무기:1), 증가수치
            shopItems.Add(new Item("가죽 갑옷", 300, "방어력을 5 증가시킵니다.", 0, 5f));
            shopItems.Add(new Item("철 갑옷", 800, "방어력을 15 증가시킵니다.", 0, 15f));
            shopItems.Add(new Item("다이아몬드 갑옷", 2000, "방어력을 40 증가시킵니다.", 0, 40f));

            shopItems.Add(new Item("강철 검", 500, "공격력을 10 증가시킵니다.", 1, 10f));
            shopItems.Add(new Item("은 검", 1000, "공격력을 20 증가시킵니다.", 1, 20f));
            shopItems.Add(new Item("황금 검", 1500, "공격력을 30 증가시킵니다.", 1, 30f));
        }

        public static void Main()
        {
            Program game = new Program();
            game.Run();
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("MyTextRpg에 오신것을 환영합니다.");
                Console.WriteLine("MyTextRpg를 제대로 즐기기 위해 F11을 눌러 전체화면으로 플레이 해주시길 바랍니다.");
                Console.WriteLine("");
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 던전 입장 (던전 입장시 꼭 F11을 눌러 전체화면으로 입장해주세요.) ");
                Console.WriteLine("5. 휴식하기");
                Console.WriteLine("6. 진행상황 저장하기");
                Console.WriteLine("7. 진행상황 불러오기");
                Console.WriteLine("0. 종료");
                Console.WriteLine("");
                Console.WriteLine("원하시는 행동을 입력해주세요.");

                Console.Write(">> ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Stat();
                        break;
                    case "2":
                        Inventory();
                        break;
                    case "3":
                        Shop();
                        break;
                    case "4":
                        Dungeon();
                        break;
                    case "0":
                        return;
                    case "5":
                        Rest();
                        break;
                    case "6":
                        Save();
                        break;
                    case "7":
                        Load();
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }
        }

        public void Save()
        {
            // GameData객체를 생성하고 현재 가지고있는 저장해야할 변수들을 넣어줌
            GameData gameData = new GameData
            {
                level = level,
                exp = exp,
                liveHp = liveHp,
                gold = gold,
                Inventory = inventory.ToArray() // 리스트를 배열로 변환하여 저장 (리스트그대로 JSON화가 힘들다고 함)
            };
            //JSON화 하여 파일로 저장하는 메서드를 호출하고 GameData객체를 넣어줌
            SaveLoadSystem.SaveGame(gameData);
        }


        public void Load()
        {
            //saveLoadSystem을 통해 저장된 데이터를 불러옴
            GameData loadedData = SaveLoadSystem.LoadGame();
            // 데이터가 들어있으면
            if (loadedData != null)
            {// loadedData안에 있는 값들을 변수들에 넣어줌
                level = loadedData.level;
                exp = loadedData.exp;
                liveHp = loadedData.liveHp;
                gold = loadedData.gold;
                inventory = new List<Item>(loadedData.Inventory); // 배열을 리스트로 변환
            }
        }


        public void Stat()
        {
            // 공격력 및 방어력 계산용 변수
            float totalStr = baseStr;
            float totalDef = baseDef;
            // 포리치를 통해 아이템 리스트에서 불값으로 장착중인지 확인하여 공, 방 계산
            foreach (var item in inventory)
            {
                if (item.IsEquipped)
                {
                    if (item.Type == 0) // 장착중이면 더해줌 (방어력)
                        totalDef += item.Stat;
                    else if (item.Type == 1) // (공격력)
                        totalStr += item.Stat;
                }
            }

            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            Console.WriteLine("");
            Console.WriteLine("Lv. {0}", level);
            Console.WriteLine("Exp {0}/100%", exp % 100);
            Console.WriteLine("직업: {0}", job == 1 ? "전사" : "기타");
            Console.WriteLine("(기본 공격력 : {0}) (무기 공격력 : {1}) 총 공격력 : {2}", baseStr, totalStr - baseStr, totalStr);
            Console.WriteLine("(기본 방어력 : {0}) (방어구 방어력 : {1}) 총 방어력 : {2}", baseDef, totalDef - baseDef, totalDef);
            Console.WriteLine("체력: {0}", hp);
            Console.WriteLine("Gold: {0} G", gold);
            Console.WriteLine("");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">>");
            Console.ReadLine();
        }

        public void Dungeon()
        {
            Console.Clear();
            Console.WriteLine("던전에 입장합니다...");

            // 던전 난이도 랜덤 결정
            int dungeonLevel = random.Next(1, 4); // 1: 쉬움, 2: 보통, 3: 어려움
            //난이도별 골드
            int baseGold = dungeonLevel == 1 ? 1000 : dungeonLevel == 2 ? 1700 : 2500;
            // 난이도출력
            Console.WriteLine(dungeonLevel == 1 ? "쉬운 던전" : dungeonLevel == 2 ? "보통 던전" : "어려운 던전");

            // 보스 체력 설정
            int bossHp = dungeonLevel == 1 ? 50 : dungeonLevel == 2 ? 100 : 150;
            //둘중 하나가 죽을때까지 배틀
            while (bossHp > 0 && hp > 0)
            {
                Battle(ref bossHp, dungeonLevel);
            }

            if (hp <= 0)
            {
                Console.WriteLine("캐릭터가 사망했습니다. 게임 오버.");
                // 캐릭터 정보 초기화
            }
            else
            {
                // 보상지급
                float attackBonus = baseStr * ((float)random.Next(10, 21) / 100); // 공격력 보너스
                int totalGold = baseGold + (int)attackBonus;
                Console.WriteLine($"던전을 클리어했습니다! 보상: {totalGold} G, 경험치: 100 XP");
                gold += totalGold;
                exp += 100;

                Console.WriteLine("0. 나가기");
                Console.ReadLine();
            }
        }

        public void Battle(ref int bossHp, int dungeonLevel)
        {

            Console.Clear();
            Console.WriteLine("보스와 전투 시작!");

            int attackCounter = 0; // 10번채우면 공격

            while (bossHp > 0 && hp > 0)
            {
                Console.Clear();
                DisplayBattleField();
                Console.WriteLine($"플레이어 HP: {hp} | 보스 HP: {bossHp}");
                Console.WriteLine("");
                Console.WriteLine("보스와 플레이어는 서로 자신의 공격차례에만 공격할 수 있습니다.");
                Console.WriteLine("");
                Console.WriteLine("                공격 : A            방어 : D          ");
                Console.WriteLine("");
                Console.WriteLine("                      지금은 서로 대치중입니다 !");
                Console.WriteLine("");
                Console.WriteLine("대치상황에서는 S를 눌러 다음턴으로 넘어갈 수 있습니다 S를 계속 눌러주세요 !!");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (random.Next(1, 10) == 9)
                {
                    Console.WriteLine("당신이 공격할 수 있는 기회입니다 ! 공격하려면 A를 3초안에 10번 누르세요 !!!");
                    timer = new System.Timers.Timer(3000);
                    timer.AutoReset = false;
                    timer.Start();//3초 타이머 시작
                    // 키 입력 이벤트
                    while (timer.Enabled)//타이머 진행되는동안 반복문
                    {
                        if (Console.KeyAvailable)
                        {
                            keyInfo = Console.ReadKey(true);
                            if (keyInfo.Key == ConsoleKey.A)
                            {
                                attackCounter++;
                            }
                        }
                    }// 반복문이 돌아가는동안 D키를 10번이상 눌렀다면
                    if (attackCounter >= 10)
                    {
                        
                        attackCounter = 0;
                        bossHp -= (int)baseStr;
                        PlayAttackAnimation();//공격
                    }
                    else
                    {
                        Console.WriteLine("실패! A 키를 10번 누르지 못했습니다.");
                    }
                }
                // 몬스터 공격 (랜덤)
                if (random.Next(1, 13) == 12)
                {
                    defenseMode = false; //방어태세가 되어있을수 있으니 false로 바꿔줌
                    Console.WriteLine("보스가 공격을 준비합니다! 방어하려면 D를 3초안에 10번 누르세요 !!!");

                    //타이머설정
                    timer = new System.Timers.Timer(3000);
                    timer.AutoReset = false;
                    timer.Start();//3초 타이머 시작
                    // 키 입력 이벤트
                    while (timer.Enabled)//타이머 진행되는동안 반복문
                    {
                        if (Console.KeyAvailable)
                        {
                            keyInfo = Console.ReadKey(true);
                            if (keyInfo.Key == ConsoleKey.D)
                            {
                                dKeyPressCount++;
                            }
                        }
                    }// 반복문이 돌아가는동안 D키를 10번이상 눌렀다면
                    if (dKeyPressCount >= 10)
                    {
                        Console.WriteLine("성공! D 키를 10번 눌렀습니다.");
                        Console.Write("방어 준비!");
                        defenseMode = true;// 방어모드 켜주고
                    }
                    else
                    {
                        
                    }

                    // 방어모드라면
                    if (defenseMode == true)
                    {// 방어성공 출력 후
                        defenseMode = false;// 방어모드 꺼주고
                        PlayDefenseAnimation();//방어 보여주기
                    }//실패라면
                    else
                    {
                        Console.WriteLine("방어 실패! 보스의 공격!");
                        int damage = (int)(baseDef - (dungeonLevel * 10));
                        damage = damage <= 0 ? 10 : damage;// 데미지가 0혹은 음수면 최소데미지인 10를 줌
                        liveHp += damage;
                        PlayHitAnimation();// 맞기
                    }
                }

                Thread.Sleep(500);
            }
        }


        public void DisplayBattleField()
        {// 대치
            Console.Clear();
            Console.WriteLine("                                                                            ,-            ");
            Console.WriteLine("                                                         @!                           @@@#.         ");
            Console.WriteLine("                                                       .@@@                         .$@$@@#.        ");
            Console.WriteLine("                                                      -@@@@*                 .*@@@@#=@@. *@:        ");
            Console.WriteLine("                                                     .@@@@@@               .;@@@$$#@@@@   @;        ");
            Console.WriteLine("                                                    .#@@@@@#              ,$@@$:  .~$@;   @;        ");
            Console.WriteLine("                                                    @@@#@@@.            ,!@@=~      =@!. :@;        ");
            Console.WriteLine("                                                  .#@@#-@@$            ;#@#~        !@@=!@@:        ");
            Console.WriteLine("                                                 ,#@@$.$@@:           ,#@!,         .-=@@#-         ");
            Console.WriteLine("                                                .#@@#.-@@#            !@@.            .~~,          ");
            Console.WriteLine("                                                @@@#  =@@~            #@=                            ");
            Console.WriteLine("                                               #@@@.  @@@~            #=                             ");
            Console.WriteLine("                                             :@@@@,  :@@@            .@=                             ");
            Console.WriteLine("                                            ~@@@@,   $@@.            *@*                             ");
            Console.WriteLine("                         ,@@@*             -@@@@.   ,@@$            .@@@#.                           ");
            Console.WriteLine("                        ,@@@@@$           ~@@@@     #@@            :@@@@@@                           ");
            Console.WriteLine("                       ,@@@@@@@$         :@@@;     .@@@           ~@@$.~@@.                          ");
            Console.WriteLine("                       ,@@#-;@@@        =#@@=.     :@@*           :@!. ,$@$~                         ");
            Console.WriteLine("                       ,@@#-;@@@       .@@@@       @@@,           :@,   ,=@#~                        ");
            Console.WriteLine("                 !!.   .$@@@@@@:        $@@@,     :@@*            !@,     ;@@~                       ");
            Console.WriteLine("                ;@@@;:  .$@@@@#          $@@@;    @@@,           :@@.      :@@                       ");
            Console.WriteLine("                ~@@@@@#:~.=@@*.         ~@@@@@#~ -@@@           .#@:        $@-                      ");
            Console.WriteLine("                 -==@@@@@=;@@*----    .$@@@=@@@@~=@@~         ,;$@=         -@*                      ");
            Console.WriteLine("                    *#@@@@@@@@@@@@~~~~!@@@# !@@@@@@@         -=@@#-         .@@-                     ");
            Console.WriteLine("                      ~#@@@@@@@@@@@@@@@@@:   ;@@@@@=       ~#@@@;  .         $@$,                    ");
            Console.WriteLine("                        ..~@@@@@@@@@@@@@-     .@@*@      ,$@@@@@~,#@#         $@@@##=~               ");
            Console.WriteLine("                          ~@@!    .~~~~                 ,#@@= :@@@@*,        ,=@@@@@@@@=,           ");
            Console.WriteLine("                          ~@@!                          #@!   ~@@@=          *@@@$@@!;@@#           ");
            Console.WriteLine("                          ~@@!                         ,@@    @@=@@:           ,#@@:   *@;          ");
            Console.WriteLine("                          ~@@!                         ,@~    =: ~@=          ,#@@@~   -@@          ");
            Console.WriteLine("                          ~@@!                         ,@=        :~          :@$=@@!   @@          ");
            Console.WriteLine("                          ~@@!                          #@-        .--------.  ,. ;=;   @@          ");
            Console.WriteLine("                          ~@@!                          ;@!        *@@@@@@@@$===*.      @@          ");
            Console.WriteLine("                          ~@@!                           ##~       ,:::::::;#####-     ~@@          ");
            Console.WriteLine("                         -*@@$                           ;@@*;-                        !@-          ");
            Console.WriteLine("                       .:@@@@@$                           ;@@@@=~~-                   :@@           ");
            Console.WriteLine("                       !@@@@@@@!                          .:;*@@@@$!;.              ,;#@*           ");
            Console.WriteLine("                     -@@@@# :@@@;                             ,**$@@@@=~~~,        ~$@#!            ");
            Console.WriteLine("                    !@@@@:   ~@@@@                                 .$@@@@@@@@@@@@@@@@@.              ");
            Console.WriteLine("                    !@@$.     -@@@@                                   ...;@@@@@@@@@#-                ");
            Console.WriteLine("                     ,~        -@@@                                                                 ");
            Console.WriteLine("                                ~;~                                                             ");

        }

        public void PlayAttackAnimation()// 공격
        {
            Console.Clear();
            Console.WriteLine("                                                                                      .=#!-.        ");
            Console.WriteLine("                                                                                      ;@@@@*.       ");
            Console.WriteLine("                                                                              .-::::~ @@!;#@*       ");
            Console.WriteLine("                                                                            .~$@@@@@@#@#. -#$       ");
            Console.WriteLine("                                                                           ,*@@@$;;=@@@$   #$       ");
            Console.WriteLine("                                                                          ~#@@$~     @@:   #$       ");
            Console.WriteLine("                                                                        :$@@*,       @@=, ;@$       ");
            Console.WriteLine("                                                                       *@@#-         =@@$*@@=       ");
            Console.WriteLine("                                                                      ,@@;            .$@@@:        ");
            Console.WriteLine("                                                                      =@#.             .--,         ");
            Console.WriteLine("                                                                     .@@:                            ");
            Console.WriteLine("                                                                     .@$-                            ");
            Console.WriteLine("                                                                     .@*                             ");
            Console.WriteLine("                                                                     !@*                             ");
            Console.WriteLine("                      ....                                          ,@@;                             ");
            Console.WriteLine("                     ;@@@@:                                         !@@@#-                           ");
            Console.WriteLine("                    ;@@@@@@-                .!*@@=~~~~~,          ,#@@=#@=                           ");
            Console.WriteLine("                   ;@@@@@@@@-               @$.@@@@@@@@@#####:    #@@! ~@$.                          ");
            Console.WriteLine("                   !@@=,.#@@~               @@@@@@@@@@@@@@@@@@@@@@@@*!!=@@@;                         ");
            Console.WriteLine("                   !@@@!~@@@~               @@@- -$$$$$#@@@@@@@@@@@@@@@@@@@@@@@=,,,                  ");
            Console.WriteLine("            *#     .#@@@@@@@                @@@-        .....*@@@@@@@@@@@@@@@@@@@$!#                 ");
            Console.WriteLine("           .@@@=;-  ,$@@@@@;                @@@-                -$@$;;;;;#@@@@@@@*~@                 ");
            Console.WriteLine("           .@@@@@@$...#@@@           .....@@@@@-               .*@#. ..:@@@@@@@@@@#,                 ");
            Console.WriteLine("            ,;@@@@@@#*;@@@          =@@@@@@@@@@*              ~$@@===@@@@@@@@@@@--                   ");
            Console.WriteLine("               *@@@@@@@@@@@@@@;    ;@@@@@@@@@@@$             !@@@@@@@@@@@@@@@@@=                     ");
            Console.WriteLine("                .$@@@@@@@@@@@@@@@@@@@@@@@@##@@@$          .,$@@@@@@@@@@@##~ .=@$                     ");
            Console.WriteLine("                  ,-##*@@@@@@@@@@@@@@@~---  -@@$       -==@@@@@@@@@@@--,     ~@@#=-                  ");
            Console.WriteLine("                      ;@@@!!!!!#@@@@*:      -@@$  -::@@@@@@@@@@@#@@@:         :#@@@@@#!~.            ");
            Console.WriteLine("                      ;@@@                  -@@@@@@@@@@@@@@@@=@@@@#.         ,$@@@@@@@@@@:          ");
            Console.WriteLine("                      ;@@@                  -@@@@@@@@@@@@:   ;@@@@~          ~@@@@#@#~:#@@~         ");
            Console.WriteLine("                      ;@@@                  ,#@$@@$** =@$   -@@=#@#.            !@@@-   !@=.        ");
            Console.WriteLine("                      ;@@@                            $@    .=: -$@.          .;@@@@,   ,#@.        ");
            Console.WriteLine("                      ;@@@                            $@*        ,:           ,=@*=@@*   =@.        ");
            Console.WriteLine("                      ;@@@                            ~@@,         ,,,,,,,,,.  .. .=$;   =@.        ");
            Console.WriteLine("                      ;@@@                             =@~        ,#@@@@@@@@$====:       =@.        ");
            Console.WriteLine("                      ;@@@                             ~@=.        ;!!!!!!!*@@@@@#      -#@.        ");
            Console.WriteLine("                     .!@@@.                             #@$~,.              ,,,,,.      :@=.        ");
            Console.WriteLine("                    ,#@@@@$                             ,#@@@$;                        ,$@:         ");
            Console.WriteLine("                   ;#@@@@@@$                             ~@@@@@##$-                   .*@#-         ");
            Console.WriteLine("                 .~@@@@##@@@:                               :#@@@@@@=,.              :@@@;          ");
            Console.WriteLine("                .$@@@@!  $@@@!                                ,,~$@@@@#$$$:        ,$@@=,           ");
            Console.WriteLine("                @@@@$,    =@@@=.                                   ,;=@@@@@@@@@@@@@@@@*.            ");
            Console.WriteLine("                #@@:       #@@@=                                         :##########=                ");
            Console.WriteLine("                 ,,         =@@=                                                                 ");
            Console.WriteLine("성공! A 키를 10번 눌렀습니다.");
            Console.WriteLine("공격 성공!"); ;
            Thread.Sleep(2000);
        }

        public void PlayDefenseAnimation()
        {
            Console.Clear();
            Console.WriteLine("                                                                                     -$#*:.         ");
            Console.WriteLine("                                 .~==============================~                  .*@@@@=,        ");
            Console.WriteLine("                                 ~@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@           .:;;;;-~@@;;#@=        ");
            Console.WriteLine("                                 -$@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@$:       ,!$@@@@@##@$  ~##.       ");
            Console.WriteLine("                                  -$$$$$$$$$$$$$$$$$$$$$$$$$$$$$#@@@      ,=@@@#$$@@@@=   $#.       ");
            Console.WriteLine("                                 *=.                            .@@@    .!@@@$-   .-@@~   $#.       ");
            Console.WriteLine("                                !@@@                            .@@@  .:#@@=-       @@:  -##.       ");
            Console.WriteLine("                                !@@@                            .@@@ -#@@$,         @@@*:@@#.       ");
            Console.WriteLine("                                !@@@                            .@@@ #@#;.          ~*@@@@=~        ");
            Console.WriteLine("                                !@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@~@@;              :@@=.         ");
            Console.WriteLine("                                !@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@$.                             ");
            Console.WriteLine("                                !@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@:                              ");
            Console.WriteLine("                                !@@@*****************************@@@@@                               ");
            Console.WriteLine("                                !@@@                            .@@@@@                               ");
            Console.WriteLine("                                !@@@                            .@@@@$                               ");
            Console.WriteLine("                 .!===:         ;@@@.                           .@@@@#!-                             ");
            Console.WriteLine("                 *@@@@@:        .#@@!                           .@@@@@@$.                            ");
            Console.WriteLine("               .*@@@@@@@:       .#@@!                           ;@@@~:@@-                            ");
            Console.WriteLine("               ~@@@@$@@@@       .#@@*,,,,,,,,,,,,,,,            @@@@  @@;.                           ");
            Console.WriteLine("               ~@@#- -@@@       .#@@@@@@@@@@@@@@@@@@$$$$$$$$$$$$@@@@  *@@#-                          ");
            Console.WriteLine("               ~@@@@~@@@@       .#@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@   ;$@$-                         ");
            Console.WriteLine("        =#.     *@@@@@@@~       .#@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@    ,$@#-                        ");
            Console.WriteLine("       ,@@@$;~   *@@@@@#.       .#@@!               ;;;;;;;;;;=@@@@@     ,=@*                        ");
            Console.WriteLine("       ,@@@@@@@,  *@@@:         .#@@!                        .*@@@@@      ~@@-                       ");
            Console.WriteLine("        -!@@@@@@@*~@@@:         .#@@$                       ~=@@$@@@       =@;                       ");
            Console.WriteLine("           :#@@@@@@@@@@@@@@~....*@@@@#                    ,=@@@=.@@@       !@@.                      ");
            Console.WriteLine("            .;$@@@@@@@@@@@@@####@@@@$.                  ,;$@@$;,.@@@       ~@@!                      ");
            Console.WriteLine("              ,~*$!@@@@@@@@@@@@@@@@@*,,,,,,.          .~$@@@#~ ,*@@@       .*@@=!-,,.                ");
            Console.WriteLine("                  ,@@@=;;;;*@@@@@@@@@@@@@@@#;;;;;;;;;;=@@@##@*:$@@@@        .!@@@@@@$!~.            ");
            Console.WriteLine("                  ,@@@:     ----~#@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@,       ;$@@@@@@@@@=:           ");
            Console.WriteLine("                  ,@@@:         .#@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@*      .=@@@@#@$:!#@#~          ");
            Console.WriteLine("                  ,@@@:         .#@@*,,,,,,,:*******@@=***$@@@@@@@@@:       .,-#@@#,  ,=@*.         ");
            Console.WriteLine("                  ,@@@:         .#@@!               @@.   -$!~;#@@@@         .*@@@$    ~@@,         ");
            Console.WriteLine("                  ,@@@:         .#@@!               @@.    .   ~@@@@         ;@@@@@=,   $@,         ");
            Console.WriteLine("                  ,@@@:         .#@@!               #@!         .@@@         ,!; *@@!   $@,         ");
            Console.WriteLine("                  ,@@@:         .#@@!               :@@         .@@@@@@@@@*.            $@,         ");
            Console.WriteLine("                  ,@@@:          *@@$               .@@,        .@@@@@@@@@@@@@@@*       $@,         ");
            Console.WriteLine("                  ,@@@:          ~@@@......          ;@@-       .@@@      !#####;      :@#.         ");
            Console.WriteLine("                 -=@@@$          ~#~$@@@@@@$**********@@@=*~    .@@@                   !@;          ");
            Console.WriteLine("               .~@@@@@@#         ~#~=@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@#=                 :@@:          ");
            Console.WriteLine("               !@@@@@@@@!        ~@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@#.$.              ,;#@=.          ");
            Console.WriteLine("             -=@@@@$-$@@@!       ~@@@------:$$$$$$$$$$$$$$$$@@@@@@@=@-,,.         .-$@@=,           ");
            Console.WriteLine("            ;#@@@$:   $@@@!.     ~@@@                      .;;;;!@@@@@@@#!;;;;;;;;*@@#:              ");
            Console.WriteLine("            @@@#*,    .$@@@=.    ~@@@                           .@@@===$@@@@@@@@@@@#*~               ");
            Console.WriteLine("            :$#~        $@@@;    ~@@@                           .@@@   .:::::::::::-                ");
            Console.WriteLine("             ..         .=@@~    !@@@$$$$$:~~~~~~~~~~~~~~~,     .@@@                                 ");
            Console.WriteLine("                         .~-    .#@@@@@@@@@@@@@@@@@@@@@@@@#******@@@:                                ");
            Console.WriteLine("                                .#@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                                ");
            Console.WriteLine("                                 ,!!!!!!!!!#@@@@@@@@@@@@@@@@@@@@@@@@@.                               ");
            Console.WriteLine("방어 성공 !");
            // 방어
            Thread.Sleep(2000);
        }

        public void PlayHitAnimation()//보스공격
        {
            Console.Clear();
            Console.WriteLine("                                                                                     -@@@#-         ");
            Console.WriteLine("                                                                                    ,=@#@@#-        ");
            Console.WriteLine("                          .,,,,,                                             -*@@@@*;@@- =@@        ");
            Console.WriteLine("                        -$#@@@@@$=                                         ~$@@@@@@@@@@   @@        ");
            Console.WriteLine("                      .:=@@@@@@@@@!.                                      :$@@#!::*#@@=   @@        ");
            Console.WriteLine("                     .$@@@@@@@@@@@@@=.                                  ,*@@#;,     #@.   @@        ");
            Console.WriteLine("                    ~#@@@#!-----$@@@@*.                               -*#@#;,       #@=-.!@@        ");
            Console.WriteLine("                    @@@@!,       !$@@@;                              ,$@@=.         *@@$*@@$        ");
            Console.WriteLine("                   *@@@:          .$@@#.                             !@@~            ,$@@@:.        ");
            Console.WriteLine("                   #@@*    ,!!.    .#@@:                            ,#@=              ,::-          ");
            Console.WriteLine("                  .@@@-    =@@@~-   :@@*                            ;@#.                             ");
            Console.WriteLine("                  .@@@-    =@@@@@*, :@@*                            !@;                              ");
            Console.WriteLine("                  .@@@~    ,=@@@@@@#*@@*                            !@~                              ");
            Console.WriteLine("                   =@@=      .:@@@@@@@@,                           ,#@~                              ");
            Console.WriteLine("                   ~@@@*        -!@@@@@@*-                         !@@;,                             ");
            Console.WriteLine("                   =@@@@@:       @@@@@@@@@@                       ,$@@@@,                            ");
            Console.WriteLine("                  =@@@@@@@#*****@@@@@*@@@@@@@*                   !@@$;#@;                            ");
            Console.WriteLine("                 ;@@@@@@@@@@@@@@@@@@;  ~@@@@@@@;                ~@@#- *@=                            ");
            Console.WriteLine("                 ;@@@;-@@@@@@@@@@@$.    ,$@@@@@@$,              ~@$,  !@@$,                          ");
            Console.WriteLine("                 ;@@#~,$@@@#####:~        ~;#@@@@@=!,           ~@!   .!@@$,                         ");
            Console.WriteLine("          ,-     ~@@@@@@@@:                  ,=@@@@@@!,         :@!     ,#@$.                        ");
            Console.WriteLine("         -#@$:.   ~@@@@@@$                     -#@@@@@#*..     -$@;      ,$@$                        ");
            Console.WriteLine("         ~@@@@@$:  :@@@@*,                       :!=@@@@@@:.  .@@$,       ,@@~                       ");
            Console.WriteLine("          !@@@@@@@#.~@@@.         .@!.              -#@@@@@@*.*@@~         !@;                       ");
            Console.WriteLine("           ,:=@@@@@@=@@@*!!!;     #@@@=.              ~$@@@@@@@@*.         -@#~                      ");
            Console.WriteLine("               #@@@@@@@@@@@@@@@@@@@@@#:                  ;@@@@#;.          .@@*.                    ");
            Console.WriteLine("                --@@;@@@@@@@@@@@@@@@*                   :#@@@!. ~*;         !@@$*,                   ");
            Console.WriteLine("                    -@@@=****#@@@@=!                  ,$@@$=@@~;#@@,         !#@@@@@$;-              ");
            Console.WriteLine("                    -@@@.                            ,$@@$  *@@@@*          -#@@@@@@@@@#~           ");
            Console.WriteLine("                    -@@@.                            !@$,  .=@@@$,          !@@@@#@#;;@@@#@!        ");
            Console.WriteLine("                    -@@@.                           -@@~   *@@#@@!             *@@#,   $@@@@        ");
            Console.WriteLine("                    -@@@.                           -@=    :#, !@#           .;@@@$. -=@@@@~        ");
            Console.WriteLine("                    -@@@.                           -@=    ,;  -$=           -=@@@#;:!@@@@#         ");
            Console.WriteLine("                    -@@@.                           -@@~                     -=$-*@@@@@@@@          ");
            Console.WriteLine("                    -@@@.                            *@=         ,!!!!!!!!!,     ~@@@@@=@@          ");
            Console.WriteLine("                    -@@@.                            -##.        !@@@@@@@@@@@@@@@@@@@@. @@          ");
            Console.WriteLine("                    -@@@.                             *@*.        ,,,,,,,,!@@@@@@@@@~. :@@          ");
            Console.WriteLine("                   -!@@@~                             -#@$:~.               ~=@@@@*-   !@!          ");
            Console.WriteLine("                  .@@@@@@~                             ~@@@@@:             ~@@@@@!    -@@-          ");
            Console.WriteLine("                 $@@@@@@@@~                             -::#@@@@#*!-      #@@@@$.   -!@@=           ");
            Console.WriteLine("               :@@@@@:,#@@@~                                ;$$#@@@@#:,,!@@@@#~   .~#@@*            ");
            Console.WriteLine("              *@@@@:.  ,#@@@=                                  .,-=@@@@@@@@@@#$$$$#@@#,             ");
            Console.WriteLine("              #@@$;     ,#@@@!                                                                      ");
            Console.WriteLine("               @@        ,@@@@                                                                      ");
            Console.WriteLine("                          ,#@!                                                                      ");
            Console.WriteLine("실패! D 키를 10번 누르지 못했습니다.");

            //보스가 공격
            Thread.Sleep(2000);
        }
        public void Inventory()
        {
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");
            //아이템이 없으면
            if (inventory.Count == 0)
            {
                Console.WriteLine("인벤토리에 아이템이 없습니다.");
            }
            else // 있으면
            {// 0부터 가지고 있는만큼
                for (int i = 0; i < inventory.Count; i++)
                {
                    var item = inventory[i];
                    string equippedText = item.IsEquipped ? "[E] " : "";// 장착중이면 표시하기위해
                                                                        //가지고 있는 아이템들 모두 출력
                    Console.WriteLine("{0}{1} - {2}", equippedText, item.Name, item.Description);
                }
            }
            Console.WriteLine("");
            Console.WriteLine("1. 장착 관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">>");
            string choice = Console.ReadLine();
            if (choice == "1")
            {
                Equip();
            }
        }

        public void Equip()
        {
            Console.Clear();
            Console.WriteLine("인벤토리 - 장착 관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("");
            //아이템이 없으면
            if (inventory.Count == 0)
            {
                Console.WriteLine("장착할 아이템이 없습니다.");
                Console.WriteLine("");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
            }
            else
            {//있으면 모두 표시
                for (int i = 0; i < inventory.Count; i++)
                {
                    var item = inventory[i];
                    string equippedText = item.IsEquipped ? "[E] " : "";
                    Console.WriteLine("{0}. {1}{2} - {3}", i + 1, equippedText, item.Name, item.Description);
                }
                Console.WriteLine("");
                Console.WriteLine("장착또는 해제할 아이템 번호를 선택하세요");
                Console.WriteLine("");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int itemIndex) && itemIndex > 0 && itemIndex <= inventory.Count)
                {
                    Item selectedItem = inventory[itemIndex - 1];
                    //장착중인 아이템 선택시 장착 해제
                    if (selectedItem.IsEquipped)
                    {
                        selectedItem.IsEquipped = false;
                        Console.WriteLine("{0}을(를) {1}했습니다.", selectedItem.Name, selectedItem.IsEquipped ? "장착" : "해제");
                        Console.WriteLine("");
                        Console.WriteLine("0. 나가기");
                        Console.WriteLine("");
                        Console.WriteLine("원하시는 행동을 입력해주세요.");
                        Console.Write(">>");
                    }
                    //장착중이지 않다면
                    else if (!selectedItem.IsEquipped)
                    {
                        //선택한 아이템이 방어구라면
                        if (selectedItem.Type == 0)
                        {   //true : 장착중인게있음 false : 없음
                            bool check = false;
                            //인벤토리에 아이템을 하나씩체크
                            for (int i = 0; i < inventory.Count; i++)
                            {
                                var item = inventory[i];
                                if (selectedItem.Type == 0)
                                {//방어구중 장착한 아이템이 있는지 확인하고
                                    check = item.IsEquipped ? true : false;// 장착중이면 표시하기위해
                                    item.IsEquipped = false; // 장착해제해주고
                                }
                            }
                            if (check) //이미 장착중인 방어구가 있다면
                            {
                                Console.WriteLine("");
                                selectedItem.IsEquipped = true;
                                Console.WriteLine("장착중이던 장비를 해제하고");
                                Console.WriteLine("{0}을(를) {1}했습니다.", selectedItem.Name, selectedItem.IsEquipped ? "장착" : "해제");
                                Console.WriteLine("");
                                Console.WriteLine("0. 나가기");
                                Console.WriteLine("");
                                Console.WriteLine("원하시는 행동을 입력해주세요.");
                                Console.Write(">>");
                            }
                            else
                            {
                                selectedItem.IsEquipped = true;
                                Console.WriteLine("{0}을(를) {1}했습니다.", selectedItem.Name, selectedItem.IsEquipped ? "장착" : "해제");
                                Console.WriteLine("");
                                Console.WriteLine("0. 나가기");
                                Console.WriteLine("");
                                Console.WriteLine("원하시는 행동을 입력해주세요.");
                                Console.Write(">>");
                            }
                        }
                        else
                        {
                            bool check = false;

                            for (int i = 0; i < inventory.Count; i++)
                            {
                                var item = inventory[i];
                                if (selectedItem.Type == 1)
                                {//무기중 장착한 아이템이 있는지 확인하고
                                    check = item.IsEquipped ? true : false;// 장착중이면 표시하기위해
                                    item.IsEquipped = false; // 장착해제
                                }
                            }
                            if (check) //이미 장착중인 무기가 있다면
                            {
                                Console.WriteLine("");
                                selectedItem.IsEquipped = true;
                                Console.WriteLine("장착중이던 장비를 해제하고");
                                Console.WriteLine("{0}을(를) {1}했습니다.", selectedItem.Name, selectedItem.IsEquipped ? "장착" : "해제");
                                Console.WriteLine("");
                                Console.WriteLine("0. 나가기");
                                Console.WriteLine("");
                                Console.WriteLine("원하시는 행동을 입력해주세요.");
                                Console.Write(">>");
                            }
                            else
                            {
                                selectedItem.IsEquipped = true;
                                Console.WriteLine("{0}을(를) {1}했습니다.", selectedItem.Name, selectedItem.IsEquipped ? "장착" : "해제");
                                Console.WriteLine("");
                                Console.WriteLine("0. 나가기");
                                Console.WriteLine("");
                                Console.WriteLine("원하시는 행동을 입력해주세요.");
                                Console.Write(">>");
                            }
                        }
                    }
                }
            }

            Console.ReadLine();
        }



        public void Shop()
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine("{0} G", gold);
            Console.WriteLine("");

            for (int i = 0; i < shopItems.Count; i++)
            {
                Item item = shopItems[i];
                if (inventory.Contains(item))
                {
                    Console.WriteLine("{0} - 구매 완료", item.Name);
                }
                else
                {
                    Console.WriteLine("{0} - {1} G", item.Name, item.Price);
                }
            }

            Console.WriteLine("");
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">>");
            int choice = int.Parse(Console.ReadLine());
            //아래는 아이템 번호에 따른 아이템 구매
            if (choice == 1)
            {
                ShopBuy();
            }
            else if (choice == 2)
            {
                ShopSell();
            }
        }



        //구매용상점
        public void ShopBuy()
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine("{0} G", gold);
            Console.WriteLine("");

            for (int i = 0; i < shopItems.Count; i++)
            {
                Item item = shopItems[i];
                if (inventory.Contains(item))
                {
                    Console.WriteLine("{0}. {1} - 구매 완료", i + 1, item.Name);
                }
                else
                {
                    Console.WriteLine("{0}. {1} - {2} G", i + 1, item.Name, item.Price);
                }
            }

            Console.WriteLine("");
            Console.WriteLine("구매를 원하는 아이템 번호를 입력해주세요.");
            Console.WriteLine("");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">>");
            string choice = Console.ReadLine();
            //아래는 아이템 번호에 따른 아이템 구매
            if (int.TryParse(choice, out int selectedItem) && selectedItem > 0 && selectedItem <= shopItems.Count)
            {
                BuyItem(selectedItem - 1);
            }
        }



        //판매용상점
        public void ShopSell()
        {
            Console.Clear();
            Console.WriteLine("판매 상점");
            Console.WriteLine("필요없는 아이템을 판매할 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine("{0} G", gold);
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            if (inventory.Count == 0)
            {
                Console.WriteLine("보유중인 아이템이 없습니다.");
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
            }
            else
            {
                // 인벤토리 목록을 출력
                for (int i = 0; i < inventory.Count; i++)
                {
                    var item = inventory[i];
                    string equippedText = item.IsEquipped ? "[E] " : "";
                    Console.WriteLine("{0}. {1}{2} - {3} - 판매가격 : {4} G",
                        i + 1, equippedText, item.Name, item.Description, (item.Price * 0.85));
                }

                Console.WriteLine();
                Console.WriteLine("판매할 아이템 번호를 선택하세요");
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                string input = Console.ReadLine();
                if (int.TryParse(input, out int itemIndex) && itemIndex > 0 && itemIndex <= inventory.Count)
                {
                    Item selectedItem = inventory[itemIndex - 1];

                    if (selectedItem.IsEquipped) // 장착중인 아이템이면 해제
                    {
                        selectedItem.IsEquipped = false;
                        Console.WriteLine("{0}의 장착을 해제했습니다.", selectedItem.Name);
                    }

                    // 아이템 판매
                    double sellPrice = selectedItem.Price * 0.85;
                    gold += (int)sellPrice; // 판매 후 골드 추가
                    Console.WriteLine("{0}을(를) {1} G에 판매했습니다.", selectedItem.Name, (int)sellPrice);
                    inventory.Remove(selectedItem); // 인벤토리에서 아이템 제거
                    Console.WriteLine();
                    Console.WriteLine("0. 나가기");
                    Console.WriteLine();
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    Console.Write(">>");
                }
                else if (itemIndex == 0)
                {
                    return; // 나가기 선택 시 함수 종료
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 선택입니다.");
                    Console.WriteLine();
                    Console.WriteLine("0. 나가기");
                    Console.WriteLine();
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    Console.Write(">>");
                }
            }

            Console.ReadLine();
        }


        public void BuyItem(int index)
        {
            Item item = shopItems[index];

            if (inventory.Contains(item))
            {
                Console.WriteLine("이미 구매한 아이템입니다.");
                Console.WriteLine("");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
            }
            else if (gold >= item.Price)
            {
                gold -= item.Price;
                inventory.Add(item);
                Console.WriteLine("{0}을(를) 구매하였습니다!", item.Name);
                Console.WriteLine("");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
            }
            else
            {
                Console.WriteLine("골드가 부족합니다.");
                Console.WriteLine("");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
            }

            Console.ReadLine();
        }

        public void Rest()
        {
            Console.Clear();
            Console.WriteLine("휴식하기");
            Console.WriteLine("현재 체력 : {0}", hp);
            Console.WriteLine("500 G를 내면 체력을 회복할 수 있습니다. (보유 골드 : {0})", gold);
            Console.WriteLine("");
            Console.WriteLine("1. 휴식하기");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">>");
            int choice = int.Parse(Console.ReadLine());
            if (choice == 1)
            {
                if (gold >= 500)
                {
                    gold -= 500;
                    liveHp = 0;
                    Console.WriteLine("");
                    Console.WriteLine("체력이 모두 회복되었습니다.");
                    Console.WriteLine("");
                    Console.WriteLine("0. 나가기");
                    Console.WriteLine("");
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    Console.Write(">>");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("골드가 부족합니다.");
                    Console.WriteLine("");
                    Console.WriteLine("0. 나가기");
                    Console.WriteLine("");
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    Console.Write(">>");
                    Console.ReadLine();
                }
            }


        }
    }
}
