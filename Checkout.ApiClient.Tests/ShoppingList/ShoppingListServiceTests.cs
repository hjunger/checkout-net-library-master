using FluentAssertions;
using NUnit.Framework;
using Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Tests.ShoppingList
{
    [TestFixture(Category = "CardsApi")]
    public class ShoppingListServiceTests : BaseServiceTests
    {

        [Test]
        public void CreateShoppingList()
        {
            // GET PRODUCT ID=1
            var productResponse = CheckoutClient.ShoppingListService.GetProducts();
            productResponse.Should().NotBeNull();
            productResponse.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            productResponse.Model.Count.Should().BeGreaterThan(0);

            // CREATE A NEW LIST
            var item = TestHelper.GetShoppingListItem();
            item.ProductID = productResponse.Model.First().ProductID;
            var response = CheckoutClient.ShoppingListService.CreateShoppingList(item);

            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.Id.Should().NotBe(0);
            response.Model.CustomerID.Should().BeEquivalentTo(item.CustomerID);
            response.Model.Name.Should().BeEquivalentTo(item.Name);
            response.Model.Quantity.Should().Be(1);
        }

        [Test]
        public void UpdateShoppingList()
        {
            // GET PRODUCT ID=2
            var productResponse = CheckoutClient.ShoppingListService.GetProducts();
            productResponse.Should().NotBeNull();
            productResponse.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            productResponse.Model.Count.Should().BeGreaterThan(0);

            // CREATE A NEW LIST
            var item = TestHelper.GetShoppingListItem();
            item.Product = productResponse.Model[productResponse.Model.Count-1];
            item.ProductID = productResponse.Model[productResponse.Model.Count - 1].ProductID;
            var response = CheckoutClient.ShoppingListService.CreateShoppingList(item);

            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.Id.Should().NotBe(0);
            response.Model.CustomerID.Should().BeEquivalentTo(item.CustomerID);
            response.Model.Name.Should().BeEquivalentTo(item.Name);

            // UPDATE THE LIST
            var updatedShoppingList = response.Model;
            updatedShoppingList.Name = new RandomData().FullName;
            updatedShoppingList.Quantity += 1;
            
            var responseUpd = CheckoutClient.ShoppingListService.UpdateShoppingList(updatedShoppingList);
            responseUpd.Should().NotBeNull();
            responseUpd.HttpStatusCode.Should().Be(HttpStatusCode.OK);

            // FIND BY ID
            var responseFind = CheckoutClient.ShoppingListService.GetItemById(updatedShoppingList.Id);
            responseFind.Should().NotBeNull();
            responseFind.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            responseFind.Model.Id.Should().Be(updatedShoppingList.Id);
            responseFind.Model.CustomerID.Should().BeEquivalentTo(updatedShoppingList.CustomerID);
            responseFind.Model.Quantity.Should().Be(updatedShoppingList.Quantity);
        }

        [Test]
        public void CreateUpdateGetCustomer()
        {
            var item1 = TestHelper.GetShoppingListItem();
            item1.ProductID = CheckoutClient.ShoppingListService.GetProductById(2).Model.ProductID;
            item1 = CheckoutClient.ShoppingListService.CreateShoppingList(item1).Model;

            var item2 = TestHelper.GetShoppingListItem();
            item2.CustomerID = item1.CustomerID;
            item2.ProductID = CheckoutClient.ShoppingListService.GetProductById(3).Model.ProductID;
            item2 = CheckoutClient.ShoppingListService.CreateShoppingList(item2).Model;

            var items = CheckoutClient.ShoppingListService.GetItemsFromCustomer(item1.CustomerID);
            items.Should().NotBeNull();
            items.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            items.Model.Count.Should().Be(2);
            items.Model.Any(item => item.ProductID == item1.ProductID 
                && item1.CustomerID == item.CustomerID
                && item1.Quantity == item.Quantity).Should().Be(true);
            items.Model.Any(item => item.ProductID == item2.ProductID
                && item2.CustomerID == item.CustomerID
                && item2.Quantity == item.Quantity).Should().Be(true);

            var oldQtd = item1.Quantity;
            item1.Quantity = 2;
            var response = CheckoutClient.ShoppingListService.CreateShoppingList(item1);
            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.Quantity.Should().Be(item1.Quantity + oldQtd);

            item2.Quantity += 1;
            CheckoutClient.ShoppingListService.UpdateShoppingList(item2);


            var updatedItems = CheckoutClient.ShoppingListService.GetItemsFromCustomer(item1.CustomerID);
            updatedItems.Should().NotBeNull();
            updatedItems.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            updatedItems.Model.Count.Should().Be(2);
            updatedItems.Model.Any(item => item.ProductID == item1.ProductID
                && item1.CustomerID == item.CustomerID
                && item1.Quantity + oldQtd == item.Quantity).Should().Be(true);
            updatedItems.Model.Any(item => item.ProductID == item2.ProductID
                && item2.CustomerID == item.CustomerID
                && item2.Quantity == item.Quantity).Should().Be(true);
        }

        [Test]
        public void DeleteItems()
        {
            var item1 = TestHelper.GetShoppingListItem();
            item1.ProductID = CheckoutClient.ShoppingListService.GetProductById(2).Model.ProductID;
            item1 = CheckoutClient.ShoppingListService.CreateShoppingList(item1).Model;

            var item2 = TestHelper.GetShoppingListItem();
            item2.CustomerID = item1.CustomerID;
            item2.ProductID = CheckoutClient.ShoppingListService.GetProductById(3).Model.ProductID;
            item2 = CheckoutClient.ShoppingListService.CreateShoppingList(item2).Model;

            var items = CheckoutClient.ShoppingListService.GetItemsFromCustomer(item1.CustomerID);
            items.Should().NotBeNull();
            items.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            items.Model.Count.Should().Be(2);
            items.Model.Any(item => item.ProductID == item1.ProductID
                && item1.CustomerID == item.CustomerID
                && item1.Quantity == item.Quantity).Should().Be(true);
            items.Model.Any(item => item.ProductID == item2.ProductID
                && item2.CustomerID == item.CustomerID
                && item2.Quantity == item.Quantity).Should().Be(true);

            var response = CheckoutClient.ShoppingListService.DeleteShoppingList(item1.ItemID);
            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.Should().Be(true);

            item2.Quantity = 0;
            CheckoutClient.ShoppingListService.UpdateShoppingList(item2);


            var updatedItems = CheckoutClient.ShoppingListService.GetItemsFromCustomer(item1.CustomerID);
            updatedItems.Should().NotBeNull();
            updatedItems.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            updatedItems.Model.Count.Should().Be(0);
        }

        [Test]
        public void GetByProduct()
        {
            var item = TestHelper.GetShoppingListItem();
            var productId = 2;
            item.ProductID = CheckoutClient.ShoppingListService.GetProductById(productId).Model.ProductID;
            item = CheckoutClient.ShoppingListService.CreateShoppingList(item).Model;
            
            int page = 1;
            int size = 5;
            var response = CheckoutClient.ShoppingListService.GetItemsByProduct(productId, page, size);
            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);

            var firstItemId = 0;
            var found = false;
            do
            {
                if (response.Model.Any(i => i.ItemID == item.ItemID))
                {
                    found = true;
                    break;
                }
                page++;
                firstItemId = response.Model.First().ItemID;
                response = CheckoutClient.ShoppingListService.GetItemsByProduct(productId, page, size);
                response.Should().NotBeNull();
                response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
                if (response.Model.Count == 0)
                    break;

            } while (response.Model.First().ItemID != firstItemId);

            found.Should().Be(true);
        }
    }
}
