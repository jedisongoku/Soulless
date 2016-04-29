handlers.helloWorld = function (args)
{
  var message = "Hello " + currentPlayerId + "!";
  log.info(message);
  return { messageValue: message };
}

handlers.newCharacter = function (args)
{
  var characterID = server.GrantCharacterToUser({
    PlayFabId: currentPlayerId,
    CharacterName: args.characterName,
    CharacterType: args.characterType
  });
  return characterID;
}

handlers.removeCharacter = function (args)
{
  var characterID = server.DeleteCharacterFromUser({
    PlayFabId: currentPlayerId,
    CharacterId: args.characterId,
    SaveCharacterInventory: args.saveInventory
  });
  return characterID;
}

handlers.setCustomDataToGrantedItem = function(args)
{
  var request = server.UpdateUserInventoryItemCustomData({
    PlayFabId: currentPlayerId,
    CharacterId: args.characterId,
    ItemInstanceId: args.itemInstanceId,
    Data: args.data
  });
}

handlers.grantItemsToCharacter = function (args)
{
  var request = server.GrantItemsToCharacter({
    PlayFabId: currentPlayerId,
    CharacterId: args.characterId,
    ItemIds: args.items
  });
  return request;		
}

handlers.revokeInventoryItem = function (args)
{
  var request = server.RevokeInventoryItem({
    PlayFabId: currentPlayerId,
    CharacterId: args.characterId,
    ItemInstanceId: args.itemId
  });
  return request;		
}


handlers.grantItemsToUser = function (args)
{
  var request = server.GrantItemsToUser({
    PlayFabId: currentPlayerId,
    ItemIds: [args.itemId]
  });
  return request;		
}

handlers.addUserVirtualCurrency = function (args)
{
  var request = server.AddUserVirtualCurrency({
    PlayFabId: currentPlayerId,
    VirtualCurrency: [args.currency],
    Amount: [args.amount]
  });
  return request;		
}

handlers.subtractUserVirtualCurrency = function (args)
{
  var request = server.SubtractUserVirtualCurrency({
    PlayFabId: currentPlayerId,
    VirtualCurrency: [args.currency],
    Amount: [args.amount]
  });
  return request;		
}

handlers.moveItemFromUserToCharacter = function (args)
{
  var request = server.GrantItemsToCharacter({
    PlayFabId: currentPlayerId,
    CharacterId: args.characterId,
    ItemInstanceId: args.itemInstanceId
  });
  return request;		
}



handlers.addFriend = function (args)
{
  var request = server.AddFriend({
    PlayFabId: currentPlayerId,
    FriendUsername: args.userName
  });
  return request;		
}
handlers.getFriendsList = function (args)
{
  var request = server.GetFriendsList({
    PlayFabId: currentPlayerId
    
  });
  return request;		
}