using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabApiCalls : MonoBehaviour
{

    //Login to playfab/game
    public static void PlayFabLogin(string username, string password)
    {
        var loginRequest = new LoginWithPlayFabRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            Username = username,
            Password = password
        };

        PlayFabClientAPI.LoginWithPlayFab(loginRequest, (result) =>
        {
            PlayFabDataStore.playFabId = result.PlayFabId;
            PlayFabDataStore.sessionTicket = result.SessionTicket;
            GetPhotonToken();
        }, (error) =>
        {
            PlayFabUserLogin.playfabUserLogin.Authentication(error.ErrorMessage.ToString().ToUpper(), 3);
        });
    }

    //Create an account onPlayFab
    public static void PlayFabRegister(string username, string password, string email)
    {
        var registerRequest = new RegisterPlayFabUserRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            Username = username,
            Password = password,
            Email = email
        };

        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, (result) =>
        {
            PlayFabDataStore.playFabId = result.PlayFabId;
            PlayFabDataStore.sessionTicket = result.SessionTicket;
            GetPhotonToken();
        }, (error) =>
        {
            PlayFabUserLogin.playfabUserLogin.Authentication(error.ErrorMessage.ToString().ToUpper(), 3);
        });
    }
    
    //Access the newest version of cloud script
    public static void PlayFabInitialize()
    {
        var cloudRequest = new GetCloudScriptUrlRequest()
        {
            Testing = false
        };

        PlayFabClientAPI.GetCloudScriptUrl(cloudRequest, (result) =>
        {
            Debug.Log("URL is set");
            
        },
        (error) =>
        {
            Debug.Log("Failed to retrieve Cloud Script URL");
        });
    }

    //Get Photon Token from playfab
    public static void GetPhotonToken()
    {
        var request = new GetPhotonAuthenticationTokenRequest();
        {
            request.PhotonApplicationId = "67a8e458-b05b-463b-9abe-ce766a75b832".Trim();
        }

        PlayFabClientAPI.GetPhotonAuthenticationToken(request, (result) =>
        {
            string photonToken = result.PhotonCustomAuthenticationToken;
            Debug.Log(string.Format("Yay, logged in in session token: {0}", photonToken));
            PhotonNetwork.AuthValues = new AuthenticationValues();
            PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Custom;
            PhotonNetwork.AuthValues.AddAuthParameter("username", PlayFabDataStore.playFabId);
            PhotonNetwork.AuthValues.AddAuthParameter("Token", result.PhotonCustomAuthenticationToken);
            PhotonNetwork.AuthValues.UserId = PlayFabDataStore.playFabId;
            PhotonNetwork.ConnectUsingSettings("1.0");
            
            GetCatalogRunes();
            GetCatalogQuests();
            GetCatalogItems();
        }, (error) =>
        {
            PlayFabUserLogin.playfabUserLogin.Authentication(error.ErrorMessage.ToString().ToUpper(), 3);
        });
    }

    //Calls each function that retrieves character data - Use this when you need to update data in game
    public static void GetAllPlayfabData()
    {
        GetCharacterStats();
    }


    //Receives all characters belong to the user
    public static void GetAllUsersCharacters(string playfabId, string target)
    {
        var request = new ListUsersCharactersRequest()
        {
            PlayFabId = playfabId
        };

        PlayFabClientAPI.GetAllUsersCharacters(request, (result) =>
        {
            if (target == "Player")
            {
                foreach (var character in result.Characters)
                {
                    if (!PlayFabDataStore.characters.ContainsKey(character.CharacterName))
                    {
                        PlayFabDataStore.characters.Add(character.CharacterName, character.CharacterId);
                    }

                }
            }
            /*else
            {
                foreach (var character in result.Characters)
                {
                    if(character.CharacterName == PlayFabDataStore.friendUsername)
                    {
                        PlayFabDataStore.friendCharacterId = character.CharacterId;
                    }
                }
            }*/
            
        }, (error) =>
        {
            Debug.Log("Can't retrieve character!");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    //Create new character
    public static void CreateNewCharacter(string name)
    {
        var request = new RunCloudScriptRequest()
        {
            ActionId = "newCharacter",
            Params = new { characterName = name, characterType = "Player" }//set to whatever default class is
        };
        PlayFabClientAPI.RunCloudScript(request, (result) =>
        {
            string[] splitResult = result.ResultsEncoded.Split('"');
            SetCharacterInitialData(splitResult[3]);
            
        }, (error) =>
        {
            PlayFabCreateCharacter.playFabCreateCharacter.errorText.text = error.ErrorMessage;
            Debug.Log("Character not created!");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    //Remove character
    public static void RemoveCharacter(string Id)
    {
        var request = new RunCloudScriptRequest()
        {
            ActionId = "removeCharacter",
            Params = new { characterId = Id, saveInventorycharacter = false }
        };
        PlayFabClientAPI.RunCloudScript(request, (result) =>
        {
            Debug.Log("Character Removed!");
            PlayFabMainMenu.playfabMainMenu.ListCharacters();

        }, (error) =>
        {
            Debug.Log("Character cannot Removed!");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    //Get custom data of the character and set them to their locals
    public static void GetCharacterStats()
    {
        var request = new GetCharacterDataRequest()
        {
            CharacterId = PlayFabDataStore.characterId
        };
        PlayFabClientAPI.GetCharacterData(request, (result) =>
        {    
            PlayFabDataStore.playerLevel = int.Parse(result.Data["Level"].Value);
            PlayFabDataStore.playerExperience = int.Parse(result.Data["Experience"].Value);
            PlayFabDataStore.statsBuilder["Vitality"] = int.Parse(result.Data["StatBuilderVitality"].Value);
            PlayFabDataStore.statsBuilder["Strength"] = int.Parse(result.Data["StatBuilderStrength"].Value);
            PlayFabDataStore.statsBuilder["Intellect"] = int.Parse(result.Data["StatBuilderIntellect"].Value);
            PlayFabDataStore.statsBuilder["Dexterity"] = int.Parse(result.Data["StatBuilderDexterity"].Value);
            PlayFabDataStore.statsBuilder["Spirit"] = int.Parse(result.Data["StatBuilderSpirit"].Value);
            PlayFabDataStore.statsBuilder["Critical Chance"] = int.Parse(result.Data["StatBuilderCriticalChance"].Value);
            PlayFabDataStore.statsBuilder["Nature Resistance"] = int.Parse(result.Data["StatBuilderNatureResistance"].Value);
            PlayFabDataStore.statsBuilder["Fire Resistance"] = int.Parse(result.Data["StatBuilderFireResistance"].Value);
            PlayFabDataStore.statsBuilder["Frost Resistance"] = int.Parse(result.Data["StatBuilderFrostResistance"].Value);
            PlayFabDataStore.statsBuilder["Holy Resistance"] = int.Parse(result.Data["StatBuilderHolyResistance"].Value);
            PlayFabDataStore.statsBuilder["Arcane Resistance"] = int.Parse(result.Data["StatBuilderArcaneResistance"].Value);
            Debug.Log("Data successfully retrieved!");
            Debug.Log("LEVEL " + PlayFabDataStore.playerLevel);

        }, (error) =>
        {
            Debug.Log("Character data request failed!");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    //Set character's custom data to playfab
    public static void SetCharacterInitialData(string characterId)
    {
        var request = new UpdateCharacterDataRequest()
        {
            CharacterId = characterId,
            Data = PlayFabDataStore.playerInitialData
        };
        PlayFabClientAPI.UpdateCharacterData(request, (result) =>
        {
            PlayFabCreateCharacter.playFabCreateCharacter.mainMenu.gameObject.SetActive(true);
            PlayFabCreateCharacter.playFabCreateCharacter.gameObject.SetActive(false);
            Debug.Log("Initial Data Set!");
        }, (error) =>
        {
            Debug.Log("Data Set Failed!");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    //Grant character the items in the array
    public static void GrantItemsToCharacter(string[] items, string customDataTitle, string itemClass)
    {
        var request = new RunCloudScriptRequest()
        {
            ActionId = "grantItemsToCharacter",
            Params = new { playFabId = PlayFabDataStore.playFabId, characterId = PlayFabDataStore.characterId, items = items }
        };
        PlayFabClientAPI.RunCloudScript(request, (result) =>
        {

            string[] splitResult = result.ResultsEncoded.Split('"'); //19th element is the itemInstanceId
            //Debug.Log("Split Result " + splitResult[59]); // 63th element is the itemId of the item granted from the drop table
            //Debug.Log("Split Result " + splitResult[63]); // 63th element is the itemInstanceId of the item granted from the drop table
            //Debug.Log("Split Result " + splitResult[67]); // 67st element is the item class 

            if (itemClass == "Quest")
            {
                foreach (var quest in items)
                {
                    if (PlayFabDataStore.catalogQuests[quest].currencies != null)
                    {
                        foreach (var currency in PlayFabDataStore.catalogQuests[quest].currencies)
                        {
                            AddUserCurrency(int.Parse(currency.Value.ToString()));
                        }
                    }
                }
                if (splitResult[67] == "Skill" || splitResult[67] == "Modifier")
                {
                    if (!PlayFabDataStore.playerAllRunes.ContainsKey(splitResult[59]))
                    {
                        Debug.Log("Quest 1");
                        PlayFabDataStore.playerAllRunes.Add(splitResult[59], new PlayerRune(splitResult[59], splitResult[63], splitResult[67], PlayFabDataStore.catalogRunes[splitResult[59]].displayName, "0"));
                        Debug.Log("Quest 2");
                        SetCustomDataOnItem("Active", "0", splitResult[63]);
                        Debug.Log("Quest 3");
                        RuneWindow.SortAllRunes();
                        Debug.Log("Quest 4");
                    }
                }
                else
                if(splitResult[67] == "Item")
                {
                    List<PlayerItemInfo> itemInfoList = new List<PlayerItemInfo>();
                    SetCustomDataOnItem("IsEquipped", "0", splitResult[63]);
                    PlayFabDataStore.playerInventory.Add(splitResult[59]);
                    PlayerItemInfo itemInfo = new PlayerItemInfo(splitResult[59], splitResult[63], "0");

                    if (PlayFabDataStore.playerInventoryInfo.ContainsKey(splitResult[59]))
                    {
                        PlayFabDataStore.playerInventoryInfo[splitResult[59]].Add(itemInfo);
                    }
                    else
                    {
                        PlayFabDataStore.playerInventoryInfo.Add(splitResult[59], itemInfoList);
                        PlayFabDataStore.playerInventoryInfo[splitResult[59]].Add(itemInfo);
                    }
                }

            }

            if (itemClass == "Skill" || itemClass == "Modifier")
            {
                SetCustomDataOnItem(customDataTitle, "0", splitResult[19]);

                foreach (var item in items)
                {

                    if (!PlayFabDataStore.playerAllRunes.ContainsKey(item))
                    {
                        PlayFabDataStore.playerAllRunes.Add(item, new PlayerRune(item, splitResult[19], itemClass, PlayFabDataStore.catalogRunes[item].displayName, "0"));
                    }
                }
                RuneWindow.SortAllRunes();

            }
            if (itemClass == "Item")
            {
                List<PlayerItemInfo> itemInfoList = new List<PlayerItemInfo>();
                SetCustomDataOnItem(customDataTitle, "0", splitResult[19]);
                Debug.Log("Item 1");

                foreach (var item in items)
                {
                    Debug.Log("Item 2");
                    Debug.Log("Item " + item);

                    PlayFabDataStore.playerInventory.Add(item);
                    PlayerItemInfo itemInfo = new PlayerItemInfo(item, splitResult[19], "0");
                    Debug.Log("Item 3");
                    if (PlayFabDataStore.playerInventoryInfo.ContainsKey(item))
                    {
                        PlayFabDataStore.playerInventoryInfo[item].Add(itemInfo);
                    }
                    else
                    {
                        PlayFabDataStore.playerInventoryInfo.Add(item, itemInfoList);
                        PlayFabDataStore.playerInventoryInfo[item].Add(itemInfo);
                    }
                    Debug.Log("Item 4");
                    foreach (var slot in UIItemSlot_Assign.inventorySlots)
                    {
                        if (slot.assignItem == PlayFabDataStore.playerInventory.Count)
                        {
                            slot.SetItemToSlotInstant();
                        }
                    }
                }
            }
            
        },
        (error) =>
        {
            Debug.Log("Item not Granted!");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    //Removes the specific item from the users inventory
    public static void RevokeInventoryItem(string itemId, string itemInstanceId)
    {
        var request = new RunCloudScriptRequest()
        {
            ActionId = "revokeInventoryItem",
            Params = new { characterId = PlayFabDataStore.characterId, itemId = itemInstanceId }
        };
        PlayFabClientAPI.RunCloudScript(request, (result) =>
        {
            Debug.Log(result.Results);
            if(itemId != null)
            {
                PlayFabDataStore.playerInventory.Remove(itemId);
            }
            
        },
        (error) =>
        {
            Debug.Log("Item not Revoked!");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    //Receives all items in characters inventory
    public static void GetCharacterInventory()
    {
        var request = new GetCharacterInventoryRequest()
        {
            CharacterId = PlayFabDataStore.characterId
        };

        PlayFabClientAPI.GetCharacterInventory(request, (result) =>
        {
            Debug.Log("Inventory Count: " + result.Inventory.Count);
            foreach (var item in result.Inventory)
            {
                Debug.Log(item.DisplayName);
                Debug.Log(item.ItemInstanceId);
            }
        }, (error) =>
        {
            Debug.Log("Listing Inventory Failed!");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    public static void GetAllCharacterRunes()
    {
        var request = new GetCharacterInventoryRequest()
        {
            CharacterId = PlayFabDataStore.characterId
        };
        PlayFabClientAPI.GetCharacterInventory(request, (result) =>
        {       
            foreach (var item in result.Inventory)
            {
                if (item.ItemClass == "Skill" || item.ItemClass == "Modifier")
                {
                    if (!PlayFabDataStore.playerAllRunes.ContainsKey(item.ItemId))
                    {

                        if (item.CustomData == null)
                        {
                            PlayFabDataStore.playerAllRunes.Add(item.ItemId, new PlayerRune(item.ItemId, item.ItemInstanceId, item.ItemClass, item.DisplayName, "0"));

                        }
                        else
                        {
                            PlayFabDataStore.playerAllRunes.Add(item.ItemId, new PlayerRune(item.ItemId, item.ItemInstanceId, item.ItemClass, item.DisplayName, item.CustomData["Active"]));
                        }
                    }
                }
            }
            Debug.Log("Runes are retrieved");
            RuneWindow.SortAllRunes();
        },
        (error) =>
        {
            Debug.Log("Catalog can't retrieved!");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });

    }



    public static void SetCustomDataOnItem(string key, string value, string itemInstanceId)
    {
        Dictionary<string, string> customData = new Dictionary<string, string>();
        customData.Add(key, value);
        var request = new RunCloudScriptRequest()
        {
            ActionId = "setCustomDataToGrantedItem",
            Params = new { characterId = PlayFabDataStore.characterId, itemInstanceId = itemInstanceId, data = customData }
        };

        PlayFabClientAPI.RunCloudScript(request, (result) =>
        {
            Debug.Log("Custom data set!");
        },
        (error) =>
        {
            Debug.Log("Cant set custom data");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    public static void AddFriend(string email)
    {
        var request = new AddFriendRequest()
        {
            FriendEmail = email
        };
        PlayFabClientAPI.AddFriend(request, (result) =>
        {
            Debug.Log("Friend added");
            GetFriendsList();
        },
        (error) =>
        {
            Debug.Log("Cant add to friends list");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });

    }

    public static void GetFriendsList()
    {
        var request = new GetFriendsListRequest()
        {
        };
        PlayFabClientAPI.GetFriendsList(request, (result) =>
        {
            foreach(var friend in result.Friends)
            {
                PlayFabDataStore.friendsList.Add(friend.Username, friend.FriendPlayFabId);
            }
            FriendsList.friendsList.LoadFriendsList();
           
        },
        (error) =>
        {
            Debug.Log("Cant get friends list");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    //Updates User Data - Used to set room ID for now
    public static void UpdateUserData(Dictionary<string, string> data)
    {
        var request = new UpdateUserDataRequest()
        {
            Data = data,
            Permission = UserDataPermission.Public
        };
        PlayFabClientAPI.UpdateUserData(request, (result) =>
        {
            Debug.Log("User Data Updated");
        },
        (error) =>
        {
            Debug.Log("User Data Can't Updated");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    //Gets specific user's room name
    public static void GetUserRoomName(string playFabId)
    {
        var request = new GetUserDataRequest()
        {
            PlayFabId = playFabId   
        };
        PlayFabClientAPI.GetUserData(request, (result) =>
        {
            PlayFabDataStore.friendsCurrentRoomName = result.Data["RoomName"].Value;
            Debug.Log(result.Data["RoomName"].Value);
        },
        (error) =>
        {
            Debug.Log("Can't get Room Name");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    //Creates or Updates a data on a character
    public static void UpdateCharacterData(Dictionary<string, string> data)
    {
        var request = new UpdateCharacterDataRequest()
        {
            CharacterId = PlayFabDataStore.characterId,
            Data = data
        };
        PlayFabClientAPI.UpdateCharacterData(request, (result) =>
        {
            Debug.Log("Character Data Set");
        }, (error) =>
        {
            Debug.Log("Data Set Failed!");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    //Gets a specific character data
    public static void GetQuestLog()
    {
        var request = new GetCharacterDataRequest()
        {
            CharacterId = PlayFabDataStore.characterId
        };
        PlayFabClientAPI.GetCharacterData(request, (result) =>
        {
            if (result.Data.ContainsKey("QuestLog"))
            {
                
                Debug.Log(result.Data["QuestLog"].Value);
                string[] customData = result.Data["QuestLog"].Value.Split('#');
                Debug.Log(customData.Length);

                foreach (var quest in customData)
                {
                    if (!PlayFabDataStore.playerQuestLog.Contains(quest))
                    {
                        Debug.Log(quest);
                        PlayFabDataStore.playerQuestLog.Add(quest);
                    }
                }
                //PlayFabDataStore.playerQuestLog.Add("Quest_Initial"); Testing
            } 
            

            QuestTracker.questTracker.LoadTrackerQuests();
            Debug.Log("Quest Log retrieved and set");

        }, (error) =>
        {
            Debug.Log("Quest Log Cannot Retrieved!");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }




    //Gets all the runes in the catalog and stores them
    public static void GetCatalogRunes()
    {
        var request = new GetCatalogItemsRequest()
        {
        };
        PlayFabClientAPI.GetCatalogItems(request, (result) =>
        {
            foreach (var item in result.Catalog)
            {
                if (item.ItemClass == "Skill" || item.ItemClass == "Modifier")
                {
                    
                    string[] customData = item.CustomData.Split('"');
                    PlayFabDataStore.catalogRunes.Add(item.ItemId, new CatalogRune(item.ItemId, item.ItemClass, item.DisplayName, item.Description, customData[3], int.Parse(customData[7]), 
                        int.Parse(customData[11]), int.Parse(customData[15]), int.Parse(customData[19]), int.Parse(customData[23]), int.Parse(customData[27]), int.Parse(customData[31]), 
                        int.Parse(customData[35]), float.Parse(customData[39]), float.Parse(customData[43]), customData[47].ToString()));
                }
            }
            Debug.Log("Catalog Retrieved");
        },
        (error) =>
        {
            Debug.Log("Can't get Catalog Runes");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    //Gets all the items in the game and stores them
    public static void GetCatalogItems()
    {
        var request = new GetCatalogItemsRequest()
        {
        };
        PlayFabClientAPI.GetCatalogItems(request, (result) =>
        {
            foreach (var item in result.Catalog)
            {
                if (item.ItemClass == "Item")
                {
                    string[] customData = item.CustomData.Split('"');

                    PlayFabDataStore.catalogItems.Add(item.ItemId, new UIItemInfo(item.ItemId, item.DisplayName, customData[3], int.Parse(customData[7]),
                        customData[11], int.Parse(customData[15]), int.Parse(customData[19]), int.Parse(customData[23]), int.Parse(customData[27]), int.Parse(customData[31]),
                        int.Parse(customData[35]), int.Parse(customData[39]), int.Parse(customData[43])));
                }
            }
            Debug.Log("Items are retrieved");
            PlayFabUserLogin.playfabUserLogin.Authentication("AUTHENTICATING...", 1);
        },
        (error) =>
        {
            Debug.Log("Items can't retrieved!");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });

    }

    //Gets all the quests in the game and stores them
    public static void GetCatalogQuests()
    {
        var request = new GetCatalogItemsRequest()
        {
        };
        PlayFabClientAPI.GetCatalogItems(request, (result) =>
        {
            foreach (var item in result.Catalog)
            {
                if (item.ItemClass == "Quest")
                {
                    string[] customData = item.CustomData.Split('"');

                    PlayFabDataStore.catalogQuests.Add(item.ItemId, new CatalogQuest(item.ItemId, item.ItemClass, item.DisplayName, item.Description, customData[3], item.Bundle.BundledItems, item.Bundle.BundledVirtualCurrencies));
                }
            }
            Debug.Log("Quests Retrieved");
        },
        (error) =>
        {
            Debug.Log("Can't get Quests");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    //Gets all the quests that player completed
    public static void GetCharacterCompletedQuests()
    {
        var request = new GetCharacterInventoryRequest()
        {
            CharacterId = PlayFabDataStore.characterId
        };
        PlayFabClientAPI.GetCharacterInventory(request, (result) =>
        {
            foreach (var item in result.Inventory)
            {
                if(item.ItemClass == "Quest")
                {
                    if (!PlayFabDataStore.playerCompletedQuests.Contains(item.ItemId))
                    {
                        PlayFabDataStore.playerCompletedQuests.Add(item.ItemId);
                    }
                }
                
            }
            Debug.Log("Character Quests are retrieved");
            //RuneWindow.SortAllRunes();
        },
        (error) =>
        {
            Debug.Log("Character Quests can't retrieved!");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });

    }

    //Gets all the items that player completed
    public static void GetAllCharacterItems()
    {
        var request = new GetCharacterInventoryRequest()
        {
            CharacterId = PlayFabDataStore.characterId
        };
        PlayFabClientAPI.GetCharacterInventory(request, (result) =>
        {
            foreach (var item in result.Inventory)
            {
                if (item.ItemClass == "Item")
                {
                    List<PlayerItemInfo> itemInfoList = new List<PlayerItemInfo>();
                    
                    if (item.CustomData == null)
                    {
                        PlayerItemInfo itemInfo = new PlayerItemInfo(item.ItemId, item.ItemInstanceId, "0");
                        PlayFabDataStore.playerInventory.Add(item.ItemId);
                        if(PlayFabDataStore.playerInventoryInfo.ContainsKey(item.ItemId))
                        {
                            PlayFabDataStore.playerInventoryInfo[item.ItemId].Add(itemInfo);
                        }
                        else
                        {
                            PlayFabDataStore.playerInventoryInfo.Add(item.ItemId, itemInfoList);
                            PlayFabDataStore.playerInventoryInfo[item.ItemId].Add(itemInfo);
                        }
                    }
                    else
                    {
                        PlayerItemInfo itemInfo = new PlayerItemInfo(item.ItemId, item.ItemInstanceId, item.CustomData["IsEquipped"].ToString());
                        if (PlayFabDataStore.playerInventoryInfo.ContainsKey(item.ItemId))
                        {
                            PlayFabDataStore.playerInventoryInfo[item.ItemId].Add(itemInfo);
                        }
                        else
                        {
                            PlayFabDataStore.playerInventoryInfo.Add(item.ItemId, itemInfoList);
                            PlayFabDataStore.playerInventoryInfo[item.ItemId].Add(itemInfo);
                        }
                        if (item.CustomData["IsEquipped"] == "1")
                        {
                            Debug.Log("Equipped item added to the equipped dictionary");
                            PlayFabDataStore.playerEquippedItems.Add(PlayFabDataStore.catalogItems[item.ItemId].itemType, itemInfo);
                        }
                        else
                        {
                            PlayFabDataStore.playerInventory.Add(item.ItemId);
                        }
                    }
                }

            }
            Debug.Log("Character Items are retrieved");
        },
        (error) =>
        {
            Debug.Log("Character Items can't retrieved!");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });

    }

    //Get Loot Drop
    public static void GetLoot(string[] items, GameObject enemy)
    {
        var request = new RunCloudScriptRequest()
        {
            ActionId = "grantItemsToCharacter",
            Params = new { playFabId = PlayFabDataStore.playFabId, characterId = PlayFabDataStore.characterId, items = items }
        };
        PlayFabClientAPI.RunCloudScript(request, (result) =>
        {
            Debug.Log(result.ResultsEncoded.ToString());
            string[] splitResult = result.ResultsEncoded.Split('"'); //19th element is the itemInstanceId
            Debug.Log("Split Result " + splitResult[61]); // 61st element is the itemId of the item granted from the drop table
            Debug.Log("Split Result " + splitResult[65]); // 65th element is the itemInstanceId of the item granted from the drop table
            RevokeInventoryItem(null, splitResult[65]); // Remove the item granted from the loot table
            enemy.GetComponent<DropItem>().dropItemId = splitResult[61];
            enemy.GetComponent<DropItem>().isItemReceived = true;


        },
        (error) =>
        {
            Debug.Log("Item not Granted!");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    //Add Currency to the user
    public static void AddUserCurrency(int amount)
    {
        var request = new AddUserVirtualCurrencyRequest()
        {
            VirtualCurrency = "GC",
            Amount = amount
        };
        PlayFabClientAPI.AddUserVirtualCurrency(request, (result) =>
        {
            PlayFabDataStore.playerCurrency = result.Balance;
            CharacterStats.characterStats.SetGoldText();
            PlayFabDataStore.playerUnupdatedCurrency = 0;
            Debug.Log("Currency Updated");
        },
        (error) =>
        {
            Debug.Log("Currency Can't added!");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    //Subtract Currency from the user
    public static void SubtractUserCurrency(int amount)
    {
        var request = new SubtractUserVirtualCurrencyRequest()
        {
            VirtualCurrency = "GC",
            Amount = amount
        };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request, (result) =>
        {
            PlayFabDataStore.playerCurrency = result.Balance;
            CharacterStats.characterStats.SetStatsText();
        },
        (error) =>
        {
            Debug.Log("Currency Can't subtracted!");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }

    //Get Currency of the user
    public static void GetUserVirtualCurrency()
    {
        var request = new GetUserInventoryRequest()
        {
            
        };
        PlayFabClientAPI.GetUserInventory(request, (result) =>
        {
            PlayFabDataStore.playerCurrency = result.VirtualCurrency["GC"];
        },
        (error) =>
        {
            Debug.Log("Currency Can't retrieved!");
            Debug.Log(error.ErrorMessage);
            Debug.Log(error.ErrorDetails);
        });
    }
}
