using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG02.Character;

namespace TextRPG02
{

    public class Combat
    {
        public bool isCombat = false;
        public void Attack(Player player, Enemy enemy)
        {
            int playerAttackRoll = Dice.Roll(20) + player.StrengthModifier;
            Console.WriteLine();
            Console.WriteLine($"당신의 공격 굴림 : {playerAttackRoll}");
            Thread.Sleep(500);
            if (playerAttackRoll == 20)
            {
                Console.WriteLine("공격이 명중했습니다!");
                int damage = Dice.RollMultiple(2, player.ATK) + (player.StrengthModifier * 2);
                enemy.Health -= damage;
                Console.WriteLine("정말 치명적인 일격이였습니다!!");
                Thread.Sleep(500);
                Console.WriteLine($"{enemy.Name}에게 {damage}의 피해를 입혔습니다! (적 체력: {enemy.Health})");
            }
            else if (playerAttackRoll == 1)
            {
                Random random = new Random();
                int num = random.Next(1, 3);
                switch (num)
                {
                    case 1:
                        Console.WriteLine("당신은 형편없이 고꾸라지고 맙니다...");
                        break;
                    case 2:
                        Console.WriteLine("당신은 갑자기 몸이 굳어버렸습니다!!");
                        break;
                    case 3:
                        Console.WriteLine($"{player.Name} : 어... 지금 공격해야 했었던건가?");
                        break;
                }
                Thread.Sleep(500);
                Console.WriteLine("공격이 빗나가고 말았습니다!");

            }
            else if (playerAttackRoll >= enemy.AC)
            {
                Console.WriteLine("공격이 명중했습니다!");
                int damage = Dice.RollMultiple(1, player.ATK) + player.StrengthModifier;
                enemy.Health -= damage;
                Thread.Sleep(500);
                Console.WriteLine($"{enemy.Name}에게 {damage}의 피해를 입혔습니다! (적 체력: {enemy.Health})");
            }
            else
            {
                Console.WriteLine("공격이 빗나가고 말았습니다!");
                Console.WriteLine($"(적 체력: {enemy.Health})");
            }
            Console.WriteLine();
        }
        public void EnemyAttack(Player player , Enemy enemy)
        {

            int enemyAttackRoll = Dice.Roll(20) + enemy.StrengthModifier;
            Console.WriteLine($"적의 공격 굴림 : {enemyAttackRoll}");

            if (enemyAttackRoll == 20)
            {
                Console.WriteLine("적의 공격이 명중했습니다!");
                int damage = Dice.RollMultiple(2, enemy.ATK) + (enemy.StrengthModifier * 2);
                player.Health -= damage;
                Console.WriteLine("정말 치명적인 일격이였습니다!!");
                Console.WriteLine($"당신은 {damage}의 피해를 입혔습니다! (남은 체력: {player.Health})");
            }
            else if (enemyAttackRoll == 1)
            {
                Random random = new Random();
                int num = random.Next(1, 3);
                switch (num)
                {
                    case 1:
                        Console.WriteLine("우습게도 적은 갑자기 고꾸라지고 말았습니다...");
                        break;
                    case 2:
                        Console.WriteLine("적의 몸이 굳어버린것 같습니다!");
                        break;
                    case 3:
                        Console.WriteLine($"{enemy.Name} : 어..말로.. 말로 해결할까??");
                        break;
                }
                Console.WriteLine("적의 공격이 빗나갔습니다!");

            }
            else if (enemyAttackRoll >= player.AC)
            {
                Console.WriteLine("적의 공격이 명중했습니다!");
                int damage = Dice.RollMultiple(1, enemy.ATK) + enemy.StrengthModifier;
                player.Health -= damage;
                Console.WriteLine($"당신은 {damage}의 피해를 입혔습니다! (남은 체력: {player.Health})");
            }
            else
            {
                Console.WriteLine("적의 공격이 빗나갔습니다!");
            }
        }
        public void Flee(Player player, Enemy enemy)
        {
            int fleeRoll = Dice.Roll(20) + player.AgilityModifier;
            int catchRoll = Dice.Roll(20) + enemy.AgilityModifier;
            if (fleeRoll > catchRoll)
            {
                Console.WriteLine("도망쳤습니다!");
                isCombat = false;
            }
            else
            {
                Console.WriteLine("도망치는데 실패했습니다!!");
            }

        }

        public void combat(Player player, Enemy enemy)
        {

            isCombat = true;
            
            while (isCombat)
            {
                Console.WriteLine();
                Console.WriteLine("--------------------------------------------------------------------");
                Console.WriteLine($"체력 : {player.Health}  ,  마력 : {player.Magicka} ");
                Console.WriteLine("--------------------------------------------------------------------");
                Console.WriteLine("뭘 하시겠습니까?:");
                Console.WriteLine("1. 공격");
                Console.WriteLine("2. 아이템 사용");
                Console.WriteLine("3. 도망가기");
                Console.WriteLine("--------------------------------------------------------------------");
                Console.WriteLine();
                string input = Console.ReadLine() ?? "";

                switch(input)
                {
                    case "1":
                        Attack(player, enemy);
                        break;
                    case "2":

                        Program.Inventory(player);

                        break;
                    case "3":

                        Flee(player, enemy);

                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }


                if (player.Health <= 0)
                {
                    Console.WriteLine("사망했습니다....");
                    Console.WriteLine();
                    isCombat = false;
                }

                if (enemy.Health <= 0)
                {
                    Console.WriteLine("적을 쓰러트렸습니다!");
                    Console.WriteLine();
                    isCombat = false;
                }

                if (isCombat)
                {
                    EnemyAttack(player, enemy);
                }

            }

            }
        }


    }

    


