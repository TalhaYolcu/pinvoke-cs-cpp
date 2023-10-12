using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    public class WishList
    {
        [DllImport("HelloWorld.dll")]
        private static extern IntPtr CreateWishList(string name);

        [DllImport("HelloWorld.dll")]
        private static extern void DeleteWishList(IntPtr wishListPointer);

        [DllImport("HelloWorld.dll")]
        [return: MarshalAs(UnmanagedType.BStr)]
        private static extern string GetWishListName(IntPtr wishListPointer);

        [DllImport("HelloWorld.dll")]
        private static extern void SetWishListName(IntPtr wishListPointer, string name);

        [DllImport("HelloWorld.dll")]
        private static extern void AddWishListItem(IntPtr wishListPointer, string name);

        [DllImport("HelloWorld.dll")]
        private static extern void RemoveWishListItem(IntPtr wishListPointer, string name);

        [DllImport("HelloWorld.dll")]
        private static extern int CountWishListItems(IntPtr wishListPointer);

        [DllImport("HelloWorld.dll")]
        private static extern void PrintWishList(IntPtr wishListPointer);

        private readonly IntPtr _wishListPointer;

        public string Name
        {
            get
            {
                return GetWishListName(_wishListPointer);
            }
            set
            {
                SetWishListName(_wishListPointer, value);
            }
        }

        public int Count => CountWishListItems(_wishListPointer);

        public WishList(string name)
        {
            _wishListPointer = CreateWishList(name);
        }

        ~WishList()
        {
            DeleteWishList(_wishListPointer);
        }

        public void AddItem(string name)
        {
            AddWishListItem(_wishListPointer, name);
        }

        public void RemoveItem(string name)
        {
            RemoveWishListItem(_wishListPointer, name);
        }

        public void Print()
        {
            PrintWishList(_wishListPointer);
        }
    }
}
