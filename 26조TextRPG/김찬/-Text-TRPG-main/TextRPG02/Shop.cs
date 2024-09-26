using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG02.Character;
using TextRPG02.Item.Interface;
using TextRPG02.Item;

namespace TextRPG02
{
    public enum eShop
    {
        Armor = 100, Consum = 200 , Weapon = 300, 
        Startshop = 1
    }
    public class Shop
    {
        public List<IItem> ItemsForSale { get; set; }

        public Shop(eShop shoptype)
        {
            
            ItemsForSale = new List<IItem>();
            if ((int)shoptype == 1)
            {
                ItemsForSale = new List<IItem>
                {
                ItemRepository.GetItemByID(101),
                ItemRepository.GetItemByID(102),
                ItemRepository.GetItemByID(103),
                ItemRepository.GetItemByID(201),
                ItemRepository.GetItemByID(202),
                ItemRepository.GetItemByID(203),
                ItemRepository.GetItemByID(301),
                ItemRepository.GetItemByID(302),
                ItemRepository.GetItemByID(303),
                };
            }
            for (int i = 0; i < 10; i++)
            {
                ItemsForSale.Add(ItemRepository.GetItemByID((int)shoptype + i));
                //ItemRepository.GetItemByID((int)shoptype + 1)
                //ItemRepository.GetItemByID((int)shoptype + 1)
            }



        }
        //public Shop(List<IItem>Itemlist)
        //{
        //    ItemsForSale = Itemlist;
        //    ItemsForSale = new List<IItem>
        //{
        //    ItemRepository.GetItemByID(01),
        //    ItemRepository.GetItemByID(02),
        //    ItemRepository.GetItemByID(03),
        //    ItemRepository.GetItemByID(11),
        //    ItemRepository.GetItemByID(12),
        //    ItemRepository.GetItemByID(13),
        //    ItemRepository.GetItemByID(21),
        //    ItemRepository.GetItemByID(22),
        //    ItemRepository.GetItemByID(23),
        //};
        //}

        public void DisplayItems()
        {
            Console.WriteLine("상점 아이템 목록:");
            Console.WriteLine("--------------------------------------------------------------------");
            for (int i = 0; i < ItemsForSale.Count; i++)
            {
                if (ItemsForSale[i] != null)
                Console.WriteLine($"{i + 1}. {ItemsForSale[i].Name}({ItemsForSale[i].Value}Gold): {ItemsForSale[i].Description}   - {ItemsForSale[i].GetType().Name}");
            }
            Console.WriteLine("--------------------------------------------------------------------");
        }

        public void BuyItem(Player player)
        {
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------------------");
            Console.WriteLine($"현재 골드: {player.Gold}골드");
            DisplayItems();
            Console.WriteLine("구매할 아이템의 번호를 입력하세요 (취소하려면 0 입력):");


            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (choice == 0)
                {
                    Console.WriteLine("구매를 취소했습니다.");
                    return;
                }

                if (choice > 0 && choice <= ItemsForSale.Count)
                {
                    IItem selectedItem = ItemsForSale[choice - 1];

                    if (player.Gold >= selectedItem.Value)
                    {
                        player.Gold -= selectedItem.Value;
                        player.Inventory.Add(selectedItem);
                        Console.WriteLine($"{selectedItem.Name}을(를) 구매하여 인벤토리에 추가했습니다.");
                        Console.WriteLine($"남은 골드: {player.Gold}골드");
                    }
                    else
                    {
                        Console.WriteLine("골드가 부족하여 아이템을 구매할 수 없습니다.");
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 선택입니다.");
                }
            }
            else
            {
                Console.WriteLine("숫자를 입력해주세요.");
            }
        }

    }
}
