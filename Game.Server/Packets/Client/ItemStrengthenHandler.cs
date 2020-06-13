using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server;
using Game.Server.GameObjects;
using Game.Server.GameUtils;
using Game.Server.Managers;
using Game.Server.Packets;
using Game.Server.Statics;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Server.Packets.Client
{
    [PacketHandler(0x3b, "物品强化")]
    public class ItemStrengthenHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            GSPacketIn @in = packet.Clone();
            @in.ClearContext();
            StringBuilder builder = new StringBuilder();
            bool flag = false;
            int num = GameProperties.PRICE_STRENGHTN_GOLD;
            if (client.Player.PlayerCharacter.Gold < num)
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemStrengthenHandler.NoMoney", new object[0]));
                return 0;
            }
            bool flag2 = packet.ReadBoolean();
            List<ItemInfo> list = new List<ItemInfo>();
            ItemInfo itemAt = client.Player.StoreBag2.GetItemAt(5);
            ItemInfo item = null;
            ItemInfo info3 = null;
            string property = null;
            string addItem = "";
            using (ItemRecordBussiness bussiness = new ItemRecordBussiness())
            {
                bussiness.PropertyString(itemAt, ref property);
            }
            if ((((itemAt != null) && itemAt.Template.CanStrengthen) && (itemAt.Template.CategoryID < 0x12)) && (itemAt.Count == 1))
            {
                StrengthenInfo info4;
                flag = flag || itemAt.IsBinds;
                builder.Append(string.Concat(new object[] { itemAt.ItemID, ":", itemAt.TemplateID, "," }));
                ThreadSafeRandom random = new ThreadSafeRandom();
                int num2 = 1;
                double num3 = 0.0;
                bool flag3 = false;
                StrengthenGoodsInfo info5 = null;
                if (((itemAt.StrengthenLevel != 9) && (itemAt.StrengthenLevel != 11)) && (itemAt.StrengthenLevel != 14))
                {
                    info4 = StrengthenMgr.FindRefineryStrengthenInfo(itemAt.StrengthenLevel + 1);
                }
                else
                {
                    if(itemAt.Template.CategoryID == 7)
                    {
                        info5 = StrengthenMgr.FindStrengthenGoodsInfo(itemAt.StrengthenLevel + 1, itemAt.TemplateID);
                    }
                    else
                    {
                        info5 = StrengthenMgr.FindStrengthenGoodsInfo(itemAt.StrengthenLevel, itemAt.TemplateID);
                    }
                    if (info5 == null)
                        Console.WriteLine("Game.Server.Packets.Client.ItemStrengthenHandler.Arma sem proxima evolução: " + itemAt.TemplateID + " " + itemAt.StrengthenLevel + 1);
                    info4 = StrengthenMgr.FindStrengthenInfo(itemAt.StrengthenLevel + 1);
                }
                if (info4 == null)
                {
                    client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemStrengthenHandler.NoStrength", new object[0]));
                    return 0;
                }
                if (client.Player.StoreBag2.GetItemAt(3) != null)
                {
                    info3 = client.Player.StoreBag2.GetItemAt(3);
                    addItem = addItem + "," + info3.ItemID.ToString() + ":" + info3.Template.Name;
                    if (((info3 != null) && (info3.Template.CategoryID == 11)) && (info3.Template.Property1 == 7))
                    {
                        flag = flag || info3.IsBinds;
                        builder.Append(string.Concat(new object[] { info3.ItemID, ":", info3.TemplateID, "," }));
                        flag3 = true;
                    }
                    else
                    {
                        info3 = null;
                    }
                }
                ItemInfo info6 = client.Player.StoreBag2.GetItemAt(0);
                if (!((((info6 == null) || (info6.Template.CategoryID != 11)) || ((info6.Template.Property1 != 2) && (info6.Template.Property1 != 0x23))) || list.Contains(info6)))
                {
                    flag = flag || info6.IsBinds;
                    addItem = addItem + "," + info6.ItemID.ToString() + ":" + info6.Template.Name;
                    list.Add(info6);
                    num3 += info6.Template.Property2;
                }
                ItemInfo info7 = client.Player.StoreBag2.GetItemAt(1);
                if (!((((info7 == null) || (info7.Template.CategoryID != 11)) || ((info7.Template.Property1 != 2) && (info7.Template.Property1 != 0x23))) || list.Contains(info7)))
                {
                    flag = flag || info7.IsBinds;
                    addItem = addItem + "," + info7.ItemID.ToString() + ":" + info7.Template.Name;
                    list.Add(info7);
                    num3 += info7.Template.Property2;
                }
                ItemInfo info8 = client.Player.StoreBag2.GetItemAt(2);
                if (!((((info8 == null) || (info8.Template.CategoryID != 11)) || ((info8.Template.Property1 != 2) && (info8.Template.Property1 != 0x23))) || list.Contains(info8)))
                {
                    flag = flag || info8.IsBinds;
                    addItem = string.Concat(new object[] { addItem, ",", info8.ItemID, ":", info8.Template.Name });
                    list.Add(info8);
                    num3 += info8.Template.Property2;
                }
                if (client.Player.StoreBag2.GetItemAt(4) != null)
                {
                    item = client.Player.StoreBag2.GetItemAt(4);
                    addItem = addItem + "," + item.ItemID.ToString() + ":" + item.Template.Name;
                    if (((item != null) && (item.Template.CategoryID == 11)) && (item.Template.Property1 == 3))
                    {
                        flag = flag || item.IsBinds;
                        builder.Append(string.Concat(new object[] { item.ItemID, ":", item.TemplateID, "," }));
                        num3 *= item.Template.Property2 + 100;
                    }
                    else
                    {
                        num3 *= 100.0;
                        item = null;
                    }
                }
                else
                {
                    num3 *= 100.0;
                }
                bool flag4 = false;
                ConsortiaInfo info9 = ConsortiaMgr.FindConsortiaInfo(client.Player.PlayerCharacter.ConsortiaID);
                if (flag2)
                {
                    ConsortiaEquipControlInfo info10 = new ConsortiaBussiness().GetConsortiaEuqipRiches(client.Player.PlayerCharacter.ConsortiaID, 0, 2);
                    if (info9 == null)
                    {
                        client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemStrengthenHandler.Fail", new object[0]));
                    }
                    else
                    {
                        if (client.Player.PlayerCharacter.Riches < info10.Riches)
                        {
                            client.Out.SendMessage(eMessageType.ERROR, LanguageMgr.GetTranslation("ItemStrengthenHandler.FailbyPermission", new object[0]));
                            return 1;
                        }
                        flag4 = true;
                    }
                }
                if (list.Count >= 1)
                {
                    num3 /= (double)info4.Rock;
                    for (int i = 0; i < list.Count; i++)
                    {
                        builder.Append(string.Concat(new object[] { list[i].ItemID, ":", list[i].TemplateID, "," }));
                        AbstractInventory itemInventory = client.Player.GetItemInventory(list[i].Template);
                        ItemInfo info11 = list[i];
                        info11.Count--;
                        itemInventory.UpdateItem(list[i]);
                    }
                    if (item != null)
                    {
                        client.Player.GetItemInventory(item.Template).RemoveItem(item);
                    }
                    if (info3 != null)
                    {
                        client.Player.GetItemInventory(info3.Template).RemoveItem(info3);
                    }
                    if (flag4)
                    {
                        num3 *= 1.0 + (0.1 * info9.SmithLevel);
                    }
                    itemAt.IsBinds = flag;
                    client.Player.StoreBag2.ClearBag();
                    if (num3 > random.Next(0x2710))
                    {
                        builder.Append("true");
                        @in.WriteByte(0);
                        if (info5 != null)
                        {
                            ItemTemplateInfo goods = ItemMgr.FindItemTemplate(info5.GainEquip);
                            if (goods != null)
                            {
                                ItemInfo info13 = ItemInfo.CreateFromTemplate(goods, 1, 0x74);
                                info13.StrengthenLevel = itemAt.StrengthenLevel + 1;
                                ItemInfo.OpenHole(ref info13);
                                StrengthenMgr.InheritProperty(itemAt, ref info13);
                                client.Player.StoreBag2.AddItemTo(info13, 5);
                                itemAt = info13;
                                if (((itemAt.StrengthenLevel == 3) || (itemAt.StrengthenLevel == 6)) || (itemAt.StrengthenLevel == 9))
                                {
                                    @in.WriteBoolean(true);
                                }
                                else
                                {
                                    @in.WriteBoolean(false);
                                }
                            }
                        }
                        else
                        {
                            itemAt.StrengthenLevel++;
                            ItemInfo.OpenHole(ref itemAt);
                            client.Player.StoreBag2.AddItemTo(itemAt, 5);
                            if (((itemAt.StrengthenLevel == 3) || (itemAt.StrengthenLevel == 6)) || (itemAt.StrengthenLevel == 9))
                            {
                                @in.WriteBoolean(true);
                            }
                            else
                            {
                                @in.WriteBoolean(false);
                            }
                        }
                        client.Player.OnItemStrengthen(itemAt.Template.CategoryID, itemAt.StrengthenLevel);
                        LogMgr.LogItemAdd(client.Player.PlayerCharacter.ID, LogItemType.Strengthen, property, itemAt, addItem, 1);
                        client.Player.SaveIntoDatabase();
                        if (itemAt.StrengthenLevel >= 10)
                        {
                            string translation = LanguageMgr.GetTranslation("ItemStrengthenHandler.congratulation", new object[] { client.Player.PlayerCharacter.NickName, itemAt.Template.Name, itemAt.StrengthenLevel });
                            GSPacketIn in2 = new GSPacketIn(10);
                            in2.WriteInt(1);
                            in2.WriteString(translation);
                            GameServer.Instance.LoginServer.SendPacket(in2);
                            GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
                            foreach (GamePlayer player in allPlayers)
                            {
                                player.Out.SendTCP(in2);
                            }
                        }
                    }
                    else
                    {
                        builder.Append("false");
                        @in.WriteByte(1);
                        @in.WriteBoolean(false);
                        if (!flag3)
                        {
                            if (itemAt.Template.Level == 3)
                            {
                                itemAt.StrengthenLevel = (itemAt.StrengthenLevel == 0) ? 0 : (itemAt.StrengthenLevel - 1);
                                client.Player.StoreBag2.AddItemTo(itemAt, 5);
                            }
                            else
                            {
                                itemAt.Count--;
                                client.Player.StoreBag2.AddItemTo(itemAt, 5);
                            }
                        }
                        else
                        {
                            client.Player.StoreBag2.AddItemTo(itemAt, 5);
                        }
                        LogMgr.LogItemAdd(client.Player.PlayerCharacter.ID, LogItemType.Strengthen, property, itemAt, addItem, 0);
                        client.Player.SaveIntoDatabase();
                    }
                    client.Out.SendTCP(@in);
                    builder.Append(itemAt.StrengthenLevel);
                    client.Player.BeginChanges();
                    client.Player.RemoveGold(num);
                    client.Player.CommitChanges();
                }
                else
                {
                    client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemStrengthenHandler.Content1", new object[0]) + num2 + LanguageMgr.GetTranslation("ItemStrengthenHandler.Content2", new object[0]));
                }
                if (itemAt.Place < 0x1f)
                {
                    client.Player.MainBag.UpdatePlayerProperties();
                }
            }
            else
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemStrengthenHandler.Success", new object[0]));
            }
            return 0;
        }
    }
}

