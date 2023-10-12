# P-Invoke on C#

# Simple Hello World example

1. First create Empty C++ project on VS
2. Delete Header - Source - Resource files
3. Go to the properties, select output type as dll
4. add source.cpp file to the project
5. Write hello World code

```cpp
#include <iostream>

extern "C" __declspec(dllexport) void HelloWorld();
void HelloWorld() {
	std::cout << "Hello World from C++ dll";
}
```

1. Then create a c# console app (could be .net framework or .net core it does not matter)
2. Set it as a startup project.
3. Add project dependency → HelloWorld cpp project

C#code : 

```csharp
[DllImport("HelloWorld.dll")]
public static extern void HelloWorld();
static void Main(string[] args)
{
	HelloWorld();
	Console.ReadLine();
}
```

Be careful having the same output path for both projects. HelloWorld.dll should be in the same directory with the ConsoleApp1.exe .

# Pass Parameter and Return Result Example

## Primitive Types

Add this code to the C++

```cpp
extern "C" __declspec(dllexport) int Add(int, int);
int Add(int a, int b) {
	return a + b;
}
```

Add this line to the C#

```csharp
[DllImport("HelloWorld.dll")]
public static extern int Add(int a, int b);
```

Function names does not have to be the same , this example also works : 

Implement it in the C#

```csharp
[DllImport("HelloWorld.dll", EntryPoint="Add")]
public static extern int AddNumbers(int num1, int num2);
...
Console.WriteLine("Add result of 4 and 4 : " + AddNumbers(4, 4).ToString());
```

## Strings

Add this to cpp file 

```cpp
extern "C" __declspec(dllexport) bool IsLengthGreaterThan5(const char*);
bool isLengthGreaterThan5(const char* value) {
	return strlen(value) > 5;
}
```

Add this to c# file

```csharp
[DllImport("HelloWorld.dll")]
public static extern bool IsLengthGreaterThan5(string str);
...
...
Console.WriteLine("Is length of -crew- is greater than 5 ? : " + IsLengthGreaterThan5("crew"));
Console.WriteLine("Is length of -crewmate- is greater than 5 ? : " + IsLengthGreaterThan5("crewmate"));
```

Returning a string : 

cpp code : 

```cpp
#include <comdef.h>
...
extern "C" __declspec(dllexport) BSTR GetName();
BSTR GetName() {
	return SysAllocString(L"Noyan");
}
```

We are using SysAllocString because otherwise genetated string is deallocated when function returns. We need to Allocate the string as system wide

c# code : 

```cpp
[DllImport("HelloWorld.dll")]
[return: MarshalAs(UnmanagedType.BStr)]
public static extern string GetName();
...
...
Console.WriteLine("Who are you : "+GetName());
```

Don’t worry about memory leaks, it is handled as Bstring in c# side

## Structs

Lets say that we have Shoe struct in C++

```cpp
struct Shoe {
	int id;
	BSTR color;
	double size;
	BSTR brand;

	void buy() {
		std::cout << "Successfully purchased the ";
	    std::wcout << color;
		std::wcout << " " << brand;
		std::cout << " shoe.";
	}
};

extern "C" __declspec(dllexport) void BuyShoe(Shoe shoe) {
	shoe.buy();
}
```

In c# side we have shoe struct

```csharp
//ensures order is matched
    [StructLayout(LayoutKind.Sequential)]
    public struct Shoe
    {
        public int id;

        [MarshalAs(UnmanagedType.BStr)]
        public string color;

        public double size;

        [MarshalAs(UnmanagedType.BStr)]
        public string brand;
    }

...
[DllImport("HelloWorld.dll")]
public static extern void BuyShoe(Shoe shoe);
...
//create shoe
Shoe newShoe = new Shoe()
{
    id = 1,
    brand = "Nike",
    color = "Blue",
    size = 40
};
BuyShoe(newShoe);
```

creating a shoe, but we have to use call by reference

in c++

```cpp
extern "C" __declspec(dllexport) void CreateShoe(Shoe*newShoe, int id,const char*color, double shoeSize,const char*brand) {
	newShoe->id = id;
	newShoe->size = shoeSize;
	newShoe->brand = SysAllocString(_com_util::ConvertStringToBSTR(color));
	newShoe->color = SysAllocString(_com_util::ConvertStringToBSTR(brand));
}
```

(have to include comutil.h in the cpp side)

in c# : 

```csharp
[DllImport("HelloWorld.dll")]
public static extern void CreateShoe(out Shoe shoe, double shoeSize);
...
Shoe myShoe;
CreateShoe(out myShoe,1,"Blue",11.0,"Adidas");
BuyShoe(myShoe);
```

## Class

Class case is a little bit of different.

We have a WishList class and its wrapper 

```cpp
class WishList {
public : 
	WishList(std::string name) : _name(name), _items() {}
	std::string getName() {
		return _name;
	}
	void setName(std::string name) {
		_name = name;
	}
	int countItems() {
		return _items.size();
	}
	void addItem(std::string item) {
		_items.push_back(item);
	}
	void removeItem(std::string item) {
		for (int i = 0; i < _items.size(); ++i) {
			if (_items[i] == item) {
				_items.erase(_items.begin() + i);
			}
		}
	}
	void print() {
		for (std::string item : _items) {
			std::cout << item << std::endl;
		}
	}

private : 
	std::string _name;
	std::vector<std::string> _items;
};

//wishlist wrapper
extern "C" __declspec(dllexport) WishList * CreateWishList(const char* name) {
	return new WishList(name);
}

extern "C" __declspec(dllexport) void DeleteWishList(WishList * wishList) {
	delete wishList;
}

extern "C" __declspec(dllexport) BSTR GetWishListName(WishList * wishList) {
	return _com_util::ConvertStringToBSTR(wishList->getName().c_str());
}

extern "C" __declspec(dllexport) void SetWishListName(WishList * wishList, const char* name) {
	wishList->setName(name);
}

extern "C" __declspec(dllexport) void AddWishListItem(WishList * wishList, const char* name) {
	wishList->addItem(name);
}

extern "C" __declspec(dllexport) void RemoveWishListItem(WishList * wishList, const char* name) {
	wishList->removeItem(name);
}

extern "C" __declspec(dllexport) int CountWishListItems(WishList * wishList) {
	return wishList->countItems();
}

extern "C" __declspec(dllexport) void PrintWishList(WishList * wishList) {
	wishList->print();
}
```

In c# side, we also should have the WishList class

```csharp
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
...
WishList wishList = new WishList("Life");

            wishList.Name = "Birthday";

            wishList.AddItem("Lamp");
            wishList.AddItem("Graphics Card");
            wishList.AddItem("Chipotle Burrito Bowl");
            wishList.RemoveItem("Lamp");

            Console.WriteLine($"{wishList.Name}: {wishList.Count} items");

            wishList.Print();
```