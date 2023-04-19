

insert into Customers (CustomerID, CustomerName, CustomerContactNo, CustomerAddress, AlternatePhone, Email) 
values (1, 'Artemis Fowl', 88744822,'Defense, Lahore',NULL,'artemisfowl@gmail.com')
--update Customers set CustomerName = 'Magnus Chase' where CustomerID = 1
select * from Customers

insert into Categories (idCategories, CategoryName) values (4, 'Frozen')
select * from Categories

insert into FoodItem (FoodItemID, ItemName, Categories_idCategories) values (5, 'Chicken Tikka Roll', 3)
select * from FoodItem

insert into deal (DealID, DealPrice, ValidTill) values (3, 500, '2020-12-10')
select * from Deal

insert into Deal_has_FoodItem(Deal_DealID, FoodItem_FoodItemID) values (3, 5)
select * from Deal_has_FoodItem

insert into orders (orderid,Payment_PaymentID, Region_RegionID,Rider_RiderID,Customers_CustomerID, OrderDate, RequiredDate, OrderStatus) values (2,2,1,1,1, '2020-11-12','2020-11-12','In Process')
select * from orders

update FoodItem set UnitQuantity = 0.5 where FoodItemID = 2
select * from FoodItem

insert into region (regionID, regionDescription) values (1, 'Malir')
select * from region

insert into rider (riderID, riderName, riderPhoneNo, RiderCNIC, RiderCompany, RiderEmail, RiderPassword) values (1, 'Ali Ahmed', '0303032992', '35202-939393-2', 'XYZ', 'aliahmed@gmail.com', 'kajhdkaa')
select * from Rider

if not exists (select * from Customers where (CustomerName = 'Nancy Drew' and CustomerContactNo = '0974828373' and CustomerAddress = 'Defense, Karachi' and Email = 'nancydrew@gmail.com' and CreditCardNo is null)) 
begin 
insert into Customers (CustomerID, CustomerName, CustomerContactNo, CustomerAddress, alternatePhone, Email, creditCardNo) 
values ((select max(CustomerID) from Customers)+1, 'Nancy Drew', '0974828373', 'Defense, Karachi', null, 'nancydrew@gmail.com', null) 
end
 
 update customers set CreditCardNo = '' where CreditCardNo is null
 select * from Customers

insert into payment (paymentID, paymentType) values (1, 'COD')
select * from Payment
delete from Region where RegionID = 2

--delete from Payment where PaymentID =8 
--delete from Customers where CustomerID >= 5
--delete from Orders where OrderID >= 2
--delete from OrderbyItem

update orders set Payment_PaymentID = 1 where OrderID = 1
exec deleteCustomerAndOrder @customer = 8

select * from Customers
select * from Payment
select * from Orders
select * from OrderbyItem
select * from Ingredients

/*
ALTER TABLE OrderbyItem
ADD CONSTRAINT FoodItem_FoodItemID
FOREIGN KEY (FoodItem_FoodItemID) REFERENCES fooditem(fooditemid);

ALTER TABLE OrderbyItem
ADD FoodItem_FoodItemID int 

ALTER TABLE OrderbyItem
DROP CONSTRAINT Orders_OrderID
ALTER TABLE OrderbyItem
ADD CONSTRAINT PK_OrderbyItem PRIMARY KEY (Orders_OrderID,FoodItem_FoodItemID) */

--get details of rider that appeared most times in orders table
select * from rider where riderID = (select top(1) Rider_RiderID, count(*) from Orders group by Rider_RiderID)
select top(1) RiderID, RiderName, RiderPhoneNo, RiderCNIC, RiderCompany, RiderEmail, RiderPassword, count(OrderID) as 'No of Orders Delivered' from rider r inner join orders o on r.riderID = o.Rider_RiderID group by riderID, RiderName, RiderPhoneNo, RiderCNIC, RiderCompany, RiderEmail, RiderPassword


select * from Ingredients_for_FoodItem
select * from FoodItem


if not exists (select ingredients_ingredientsID from ingredients_for_fooditem where ingredients_ingredientsID = (select ingredientsid from ingredients where ingredientname = 'rice'))
begin 
insert into ingredients_for_fooditem (fooditem_fooditemID, ingredients_ingredientsID, quantity_grams) values (1,(select ingredientsid from ingredients where ingredientname = 'rice'),500)
end
select * from Ingredients_for_FoodItem where FoodItem_FoodItemID = 1
select * from Ingredients

--CREATING VIEWS

create view showFoodItemIngredients as select itemname 'Food Item', ingredientname 'Ingredients', Quantity_grams 'Quantity (grams)' from fooditem f inner join ingredients_for_fooditem fi on f.fooditemid = fi.fooditem_fooditemID inner join ingredients i on i.ingredientsid = fi.ingredients_ingredientsid 
--select * from showFoodItemIngredients where [Item ID] = 1
--drop view showFoodItems

create view showAllFoodItems as select fooditemID as 'ID', itemname as 'Item', categoryname as 'Category', unitquantity as 'Unit Quantity', measuredin as 'Measured In', unitprice as 'Unit Price (Rs.)' from fooditem f inner join categories c on f.Categories_CategoriesID = c.CategoriesID

--select * from showAllFoodItems

create view ItemsWithIngrnt as select ingredientsID as 'ID', ingredientName as 'Ingredient', itemName as 'Food Item', quantity_grams as 'Quantity Required (grams)' from ingredients i inner join ingredients_for_fooditem fi on i.ingredientsID = fi.Ingredients_IngredientsID inner join fooditem f on fi.FoodItem_FoodItemID = f.FoodItemID

--select [Food Item], Ingredient, [Quantity Required (grams)] from ItemsWithIngrnt where ID = 6

create view ViewOrder as select orderID as 'ID',customers_customerID as 'Customer ID', paymentType as 'Payment Type', region_regionID as 'Region ID', rider_riderID as 'Rider ID', orderDate as 'Order Date', requiredDate as 'Required Date', shippedDate as 'Shipped Date', OrderStatus as 'Order Status', totalPrice as 'Total Price/Rs.', CashReceived as 'Cash Received', CashReturned as 'Cash Returned'  from payment p inner join orders o on p.paymentid = o.payment_paymentid

select * from ViewOrder 
select * from ViewOrder where [Order Status] = 'In Process' 

select * from showAllFoodItems 

create view OrdersDeliveredByRider as select rider_riderID as 'Rider ID', orderID as 'Order ID',customers_customerID as 'Customer ID', paymentType as 'Payment Type', region_regionID as 'Region ID', orderDate as 'Order Date', requiredDate as 'Required Date', shippedDate as 'Shipped Date', OrderStatus as 'Order Status', totalPrice as 'Total Price/Rs.'  from payment p inner join orders o on p.paymentid = o.payment_paymentid

select * from OrdersDeliveredByRider where [Rider ID]

drop view ViewOrder

create view RiderDeliveredMostOrders as select top(1) RiderID, RiderName, RiderPhoneNo, RiderCNIC, RiderCompany, RiderEmail, RiderPassword, count(OrderID) as 'No of Orders Delivered' from rider r inner join orders o on r.riderID = o.Rider_RiderID group by riderID, RiderName, RiderPhoneNo, RiderCNIC, RiderCompany, RiderEmail, RiderPassword

--create view ItemsInCategory as Select itemname as 'Item', unitprice as 'Price/unit' ,unitquantity 'Serving Size',measuredin as 'Measured In' from fooditem f inner join categories c on f.categories_categoriesid = c.categoriesid

--drop view ItemsInCategory

create procedure GetIngrQtyInItem @ItemName varchar(30)
as
select ItemName, UnitQuantity, IngredientsID, IngredientName, Quantity_grams, QtyInStock_kg from Ingredients i inner join Ingredients_for_FoodItem fi on i.IngredientsID = fi.Ingredients_IngredientsID inner join FoodItem f on f.FoodItemID = fi.FoodItem_FoodItemID where ItemName = @ItemName
go
--drop procedure GetIngrQtyInItem 
exec GetIngrQtyInItem @ItemName = 'chicken tikka roll'


create procedure displayItems @CategoryName varchar(30)
as 
Select itemname as 'Item', unitprice as 'Price/unit' ,unitquantity 'Serving Size',measuredin as 'Measured In' from fooditem f inner join categories c on f.categories_categoriesid = c.categoriesid where categoryname = @CategoryName
go

exec displayItems @categoryName = 'Rice'

create procedure displayTodaysSpecial @WeekDay varchar(25), @today date as
Select itemname as 'Item', unitprice as 'Price/unit' ,unitquantity 'Serving Size',measuredin as 'Measured In' from fooditem f inner join categories c on f.categories_categoriesid = c.categoriesid where FoodItemID = (select FoodItem_FoodItemID from WeeklyMenu w inner join WeeklyMenuItems wi on w.WeeklyMenuID = wi.WeeklyMenuID where [WeekDay] = @WeekDay and ValidFrom <= @today and ValidTill >= @today) 
Go

exec displayTodaysSpecial @weekday = 'Saturday', @today = '2020-12-06'

select * from Ingredients
update Ingredients set QtyInStock_kg = 20


insert into weeklyMenu (weeklyMenuID, ValidFrom, ValidTill) values (1, '2020-11-12', '2020-11-15')
select * from WeeklyMenu
select * from WeeklyMenuItems order by WeeklyMenuID
delete from WeeklyMenu
delete from WeeklyMenuItems

create view viewAllMenus as
select w.WeeklyMenuID as 'ID', [WeekDay] as 'Day', FoodItem_FoodItemID as 'Food Item ID', ItemName as 'Food Item', ValidFrom, ValidTill from WeeklyMenu w inner join WeeklyMenuItems wi on w.WeeklyMenuID = wi.WeeklyMenuID inner join FoodItem f on f.FoodItemID = wi.FoodItem_FoodItemID

select * from viewAllMenus

create procedure viewOrdersByItem @ItemName varchar(30) as
select o.orderID as 'ID',customers_customerID as 'Customer ID', paymentType as 'Payment Type', region_regionID as 'Region ID', rider_riderID as 'Rider ID', orderDate as 'Order Date', requiredDate as 'Required Date', shippedDate as 'Shipped Date', OrderStatus as 'Order Status', totalPrice as 'Total Price/Rs.', CashReceived as 'Cash Received', CashReturned as 'Cash Returned'  from payment p inner join orders o on p.paymentid = o.payment_paymentid inner join orderbyitem oi on o.orderid = oi.orderid where fooditem_fooditemid = (select fooditemid from fooditem where itemname = @ItemName)

exec viewOrdersByItem @itemname = 'Pulao'

drop procedure viewOrdersByItem

select * from [admin]
select * from Rider

create procedure OrderRiderView as 

select * from orders o inner join payment p on o.payment_paymentID = p.paymentID

create procedure deleteCustomerAndOrder @customer int as
delete from Payment where PaymentID = (select payment_paymentID from orders where orderID = (select orderID from orders where Customers_CustomerID = @customer and (OrderStatus = 'In Process' or OrderStatus = 'Dispatched') ))
delete from OrderbyItem where OrderID = (select orderID from orders where Customers_CustomerID = @customer and (OrderStatus = 'In Process' or OrderStatus = 'Dispatched'))
delete from Orders where Customers_CustomerID = @customer
delete from Customers where CustomerID = @customer