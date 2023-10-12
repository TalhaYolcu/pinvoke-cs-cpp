#include <iostream>
#include <comdef.h>
#include <string>
#include <comutil.h>
#include <vector>

extern "C" __declspec(dllexport) void HelloWorld();
void HelloWorld() {
	std::cout << "Hello World from C++ dll with changes"<<std::endl;
}

extern "C" __declspec(dllexport) int Add(int, int);
int Add(int a, int b) {
	return a + b;
}

extern "C" __declspec(dllexport) bool IsLengthGreaterThan5(const char*);
bool IsLengthGreaterThan5(const char* value) {
	return strlen(value) > 5;
}

extern "C" __declspec(dllexport) BSTR GetName();
BSTR GetName() {
	return SysAllocString(L"Noyan");
}


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


extern "C" __declspec(dllexport) void CreateShoe(Shoe*newShoe, int id,const char*color, double shoeSize,const char*brand) {
	newShoe->id = id;
	newShoe->size = shoeSize;
	newShoe->brand = SysAllocString(_com_util::ConvertStringToBSTR(color));
	newShoe->color = SysAllocString(_com_util::ConvertStringToBSTR(brand));
}

extern "C" __declspec(dllexport) void BuyShoe(Shoe shoe) {
	shoe.buy();
}

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

