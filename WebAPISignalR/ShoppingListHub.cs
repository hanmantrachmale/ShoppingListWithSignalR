using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace WebAPISignalR
{
    public class ShoppingListHub : Hub
    {
        private static readonly ConcurrentDictionary<string, List<string>> _groupIdShoppingLists = new();

        public async Task AddItem(string shoppingListId, string itemName)
        {
            if (_groupIdShoppingLists.TryGetValue(shoppingListId, out var shoppingList)) { 
                shoppingList.Add(itemName);
                await Clients.Group(shoppingListId).SendAsync("ReceiveShoppingList", shoppingList);
            }

        }

        public async Task RemoveItem(string shoppingListId, string itemName)
        {
            if (_groupIdShoppingLists.TryGetValue(shoppingListId, out var shoppingList))
            {
                shoppingList.Remove(itemName);
                await Clients.Group(shoppingListId).SendAsync("ReceiveShoppingList", shoppingList);
            }
        }

        public async Task JoinShoppingList(string shoppingListId)
        {
            if(_groupIdShoppingLists.TryGetValue(shoppingListId, out var shoppingList))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, shoppingListId);
                await Clients.Caller.SendAsync("JoinShoppingList", shoppingListId, shoppingList);
            }
        }

        public async Task CreateShoppingList()
        {
            string shoppingListId = Guid.NewGuid().ToString();
            _groupIdShoppingLists.TryAdd(shoppingListId, []);
            await Groups.AddToGroupAsync(Context.ConnectionId, shoppingListId);
            await Clients.Caller.SendAsync("ShoppingListCreated", shoppingListId);
        }
        public override async Task OnConnectedAsync()
        {
            await Clients.Others.SendAsync(method: "Method1", $"{Context.ConnectionId} has joined");
            await Clients.All.SendAsync(method: "Method1", $"{Context.ConnectionId} has joined");
            await Clients.Caller.SendAsync(method: "Method1", $"{Context.ConnectionId} has joined");
            await Clients.Groups("XYZ").SendAsync(method: "Method1", $"{Context.ConnectionId} has joined");
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
