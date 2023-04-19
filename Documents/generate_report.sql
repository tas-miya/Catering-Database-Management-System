
--within given time

--total orders placed
select count(*) as 'Total Orders Placed' from Orders where OrderDate >= '2020-11-20' and OrderDate <= '2020-12-07'

--total orders delivered
select count(*) as 'Total Orders Delivered' from Orders where OrderDate >= '2020-11-20' and OrderDate <= '2020-12-07' and OrderStatus = 'Delivered'

--most frequent customer
select Customers_CustomerID, count(*) as 'No of orders' from Orders where OrderDate >= '2020-11-20' and OrderDate <= '2020-12-07' group by Customers_CustomerID order by [No of orders]

select * from orders
select * from Region

--select count(*)/DATEDIFF(day,(select top 1 OrderDate from Orders),GETDATE()) from Orders where OrderDate >= '2020-11-20' and OrderDate <= '2020-12-07'
--select count(*)/DATEDIFF(day,(select top 1 ShippedDate from Orders),GETDATE()) from Orders where OrderStatus='Delivered'
select top(1) CustomerID from Customers full outer join Orders on Orders.Customers_CustomerID=Customers.CustomerID Group by CustomerID,CustomerFName,CustomerLName Order by count(Customers_CustomerID) desc
--select top 1 ItemName from FoodItem full outer join OrderbyItem on OrderbyItem.FoodItem_FoodItemID=FoodItem.FoodItemID Group by ItemName Order by count(FoodItemID) desc,count(Quantity) desc
--select top 1 RegionDescription from Region full outer join Orders on Orders.Region_RegionID=Region.RegionID Group by RegionDescription Order by count(RegionID) desc
select count(CustomerID) from Customers full outer join Orders on Orders.Customers_CustomerID=Customers.CustomerID Group by CustomerID,OrderDate having count(CustomerID)=1 and OrderDate=GETDATE()

-- avg no.of orders per day
-- avg. no of orders delivered per day
-- most frequent customer
create procedure MostFrequentCustomer @Datefrom date, @DateTo date
as
select CustomerID, CustomerFName + ' ' + CustomerLName from customers c inner join orders o on c.CustomerID =  o.Customers_CustomerID where CustomerID = (select top(1) CustomerID from Customers full outer join Orders on Orders.Customers_CustomerID=Customers.CustomerID Group by CustomerID,CustomerFName,CustomerLName Order by count(Customers_CustomerID) desc) and OrderDate >= @Datefrom and OrderDate <= @DateTo 

exec MostFrequentCustomer @datefrom = '2020-11-20', @dateto = '2020-12-07'

-- most popular food item
create procedure MostPopularItem @Datefrom date, @DateTo date
as
select FoodItemID, ItemName from FoodItem f inner join OrderbyItem oi on f.FoodItemID = oi.FoodItem_FoodItemID inner join Orders o on o.OrderID = oi.FoodItem_FoodItemID where FoodItemID = (select top(1) FoodItemID from FoodItem f inner join OrderbyItem oi on f.FoodItemID = oi.FoodItem_FoodItemID inner join Orders o on o.OrderID = oi.FoodItem_FoodItemID Group by FoodItemID Order by count(FoodItemID) desc) and OrderDate >= @Datefrom and OrderDate <= @DateTo 

exec MostPopularItem @datefrom = '2020-11-10', @dateto = '2020-12-07'

-- region with most orders 
create procedure RegionMostOrders @Datefrom date, @DateTo date
as
select regionID, regionDescription from Region r inner join orders o on r.RegionID =  o.Region_RegionID where RegionID = (select top(1) RegionID from Region r inner join orders o on r.RegionID =  o.Region_RegionID Group by RegionID Order by count(RegionID) desc) and OrderDate >= @Datefrom and OrderDate <= @DateTo 

exec RegionMostOrders @datefrom = '2020-11-20', @dateto = '2020-12-07'
-- total number of new customers

--select FoodItemID, ItemName, o.OrderID, OrderDate from FoodItem f inner join OrderbyItem oi on f.FoodItemID = oi.FoodItem_FoodItemID inner join Orders o on o.OrderID = oi.FoodItem_FoodItemID