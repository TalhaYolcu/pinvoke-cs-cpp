using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ConsoleApp1.Models;

namespace ConsoleApp1
{
    class Program
    {
        [DllImport("HelloWorld.dll")]
        public static extern void HelloWorld();

        /*[DllImport("HelloWorld.dll")]
        public static extern int Add(int a, int b);
        */

        [DllImport("HelloWorld.dll", EntryPoint="Add")]
        public static extern int AddNumbers(int num1, int num2);

        [DllImport("HelloWorld.dll")]
        public static extern bool IsLengthGreaterThan5(string str);

        [DllImport("HelloWorld.dll")]
        [return: MarshalAs(UnmanagedType.BStr)]
        public static extern string GetName();

        [DllImport("HelloWorld.dll")]
        public static extern void CreateShoe(out Shoe shoe, int id, string color, double shoeSize,string brand);

        [DllImport("HelloWorld.dll")]
        public static extern void BuyShoe(Shoe shoe);



        static void Main(string[] args)
        {
            HelloWorld();
            //Console.WriteLine("Add result of 4 and 4 : " + Add(4, 4).ToString());
            Console.WriteLine("Add result of 4 and 4 : " + AddNumbers(4, 4).ToString());

            Console.WriteLine("Is length of -crew- is greater than 5 ? : " + IsLengthGreaterThan5("crew"));
            Console.WriteLine("Is length of -crewmate- is greater than 5 ? : " + IsLengthGreaterThan5("crewmate"));

            Console.WriteLine("Who are you : "+GetName());

            //create shoe
            Shoe newShoe = new Shoe()
            {
                id = 1,
                brand = "Nike",
                color = "Blue",
                size = 40
            };
            BuyShoe(newShoe);
            Console.WriteLine();



            Shoe myShoe;
            CreateShoe(out myShoe,1,"Blue",11.0,"Adidas");
            BuyShoe(myShoe);

            WishList wishList = new WishList("Life");

            wishList.Name = "Birthday";

            wishList.AddItem("Lamp");
            wishList.AddItem("Graphics Card");
            wishList.AddItem("Chipotle Burrito Bowl");
            wishList.RemoveItem("Lamp");

            Console.WriteLine($"{wishList.Name}: {wishList.Count} items");

            wishList.Print();

            Console.ReadLine();
        }
    }
}
