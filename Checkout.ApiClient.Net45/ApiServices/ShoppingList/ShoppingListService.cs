using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkout.ApiServices.SharedModels;

namespace Checkout.ApiServices.ShoppingList
{
    public class ShoppingListService
    {
        public HttpResponse<Models.Item> CreateShoppingList(Models.Item requestModel)
        {
            return new ApiHttpClient().PostRequest<Models.Item>(ApiUrls.CreateShoppingList, AppSettings.SecretKey, requestModel);
        }

        public HttpResponse<List<Models.Item>> GetItemsFromCustomer(string customerId)
        {
            var getShoppingListUri = string.Format(ApiUrls.GetShoppingListsForCustomer, customerId);
            return new ApiHttpClient().GetRequest<List<Models.Item>>(getShoppingListUri, AppSettings.SecretKey);
        }


        public HttpResponse<Models.Item> GetItemById(int itemId)
        {
            var getShoppingListUri = string.Format(ApiUrls.GetOrDeleteShoppingLists, itemId);
            return new ApiHttpClient().GetRequest<Models.Item>(getShoppingListUri, AppSettings.SecretKey);
        }

        public HttpResponse<List<Models.Item>> GetShoppingLists()
        {
            var getShoppingListUri = string.Format(ApiUrls.GetOrDeleteShoppingLists, "");
            return new ApiHttpClient().GetRequest<List<Models.Item>>(getShoppingListUri, AppSettings.SecretKey);
        }

        public HttpResponse<bool> UpdateShoppingList(Models.Item requestModel)
        {
            return new ApiHttpClient().PutRequest<bool>(ApiUrls.UpdateShoppingList, AppSettings.SecretKey, requestModel);
        }

        public HttpResponse<bool> DeleteShoppingList(int shoppingListId)
        {
            var deleteShoppingListUri = string.Format(ApiUrls.GetOrDeleteShoppingLists, shoppingListId);
            return new ApiHttpClient().DeleteRequest<bool>(deleteShoppingListUri, AppSettings.SecretKey);
        }

        public HttpResponse<List<Models.Item>> GetItemsByProduct(int productId, int page = 0, int size = 10)
        {
            var getShoppingListUri = string.Format(ApiUrls.GetShoppingListsByProduct, productId);
            if(page > 0)
            {
                getShoppingListUri += string.Format("&skip={0}&take={1}", page, size);
            }
            return new ApiHttpClient().GetRequest<List<Models.Item>>(getShoppingListUri, AppSettings.SecretKey);
        }

        public HttpResponse<Models.Product> GetProductById(int productId)
        {
            var getProductUri = string.Format(ApiUrls.GetProducts, productId);
            return new ApiHttpClient().GetRequest<Models.Product>(getProductUri, AppSettings.SecretKey);
        }

        public HttpResponse<List<Models.Product>> GetProducts()
        {
            var getProductUri = string.Format(ApiUrls.GetProducts, "");
            return new ApiHttpClient().GetRequest<List<Models.Product>>(getProductUri, AppSettings.SecretKey);
        }
    }
}
