# Catering-Database-Management-System
A Windows management application developed with C# and TSQL for Catering businesses.

This is a course project for CS355 - Database Systems; a Windows form application developed using C# and TSQL, best to be used with small scale catering businesses. It provides interfaces for 3 types of users; Admin, Customer, and Delivery Person(Rider).

![ERD Final](https://user-images.githubusercontent.com/76451627/233139994-421afb24-2262-4813-ae6b-57509250d84a.png)

The log in screen prompts the user to specify their role, which then leads them to authenticate access for the user type they chose.
![image](https://user-images.githubusercontent.com/76451627/233134987-a3b8cbbf-d1e1-489c-a54b-c4488cce9ad8.png)

Assuming that the system is only used from the vendor's end, thus the Customer end will be used by the employee taking the order from the customer. The screen is divided into 3 sections. First section asks for the Customer's details, the region has to be chosen from the dropdown to aid the ease of delivery. The second specifies all the available options for ordering, and the third proceeds to the checkout.
![image](https://user-images.githubusercontent.com/76451627/233138512-318e974a-e018-4cd6-9768-bc2ab15a85d6.png)

The screen for delivery person gives them the choice to take as many orders they wish from the regions they see fit for their route. They select the region they want to take the orders to, and the system returns the query for all the orders placed from that region. After delivering, they have to update the order which is mostly the payment details.
![image](https://user-images.githubusercontent.com/76451627/233139223-64349a07-0ddb-4bd8-be71-4b63dda60660.png)

The last, and the user with most access is the Admin. They have multiple screens to manage their inventory, deals, meal plans, monitor their orders and delivery people, etc. The user must authenticate to enter the Admin section, which keeps the system and its data secure.
![image](https://user-images.githubusercontent.com/76451627/233139782-d4dad3da-3ccf-441a-a262-fc24bb9209de.png)
![image](https://user-images.githubusercontent.com/76451627/233139846-e73d76d4-7286-4084-9ac7-5ad54e69b39e.png)
![image](https://user-images.githubusercontent.com/76451627/233139910-8cecc673-a1b6-45d0-b8e6-aca268f711b9.png)

*It is a group project done in collaboration with Hana Ali Rashid and IFrah Ilyas.
