using System;
using System.Collections.Generic;
using System.Text;
using Game.Server.GameObjects;
using Game.Base.Packets;
using Bussiness;
using SqlDataProvider.Data;
using Game.Server.Managers;
using Game.Server.GameUtils;
using Game.Server.Statics;
using Bussiness.Managers;

namespace Game.Server.Packets.Client
{
    [PacketHandler((byte)ePackageType.ITEM_FUSION, "熔化")]

     public class ItemFusionHandler : IPacketHandler
    {
        public static List<int> FusionFormulID;

        static ItemFusionHandler()
        {
            List<int> list = new List<int> {
                0x2bc1,
                0x2bc2,
                0x2bc3,
                0x2bc4,
                0x2c25,
                0x2c26,
                0x2c27,
                0x2c28
            };
            FusionFormulID = list;
        }

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int num4;
            eBageType type;
            int num5;
            ItemInfo itemAt;
            PlayerInventory inventory;
            ItemInfo info3;
            string str;
            string str2;
            bool flag2;
            int num10;
            bool flag3;
            object[] objArray;
            StringBuilder builder = new StringBuilder();
            int num = packet.ReadByte();
            int num2 = packet.ReadInt();
            int minValid = 0x7fffffff;
            List<ItemInfo> items = new List<ItemInfo>();
            List<ItemInfo> appendItems = new List<ItemInfo>();
            List<eBageType> list3 = new List<eBageType>();
            if (!client.Player.PlayerCharacter.HasBagPassword || !client.Player.PlayerCharacter.IsLocked)
            {
                num4 = 0;
                while (true)
                {
                    flag3 = num4 < num2;
                    if (flag3)
                    {
                        type = (eBageType)packet.ReadByte();
                        num5 = packet.ReadInt();
                        itemAt = client.Player.GetItemAt(type, num5);
                        if (!ReferenceEquals(itemAt, null))
                        {
                            if (!items.Contains(itemAt))
                            {
                                objArray = new object[] { itemAt.ItemID, ":", itemAt.TemplateID, "," };
                                builder.Append(string.Concat(objArray));
                                items.Add(itemAt);
                                list3.Add(type);
                                if ((itemAt.ValidDate < minValid) && (itemAt.ValidDate != 0))
                                {
                                    minValid = itemAt.ValidDate;
                                }
                            }
                            else
                            {
                                client.Out.SendMessage(eMessageType.Normal, "Bad Input");
                                return 1;
                            }
                        }
                        num4++;
                        continue;
                    }
                    else
                    {
                        if (minValid == 0x7fffffff)
                        {
                            minValid = 0;
                            items.Clear();
                        }
                        inventory = client.Player.StoreBag2;
                        itemAt = inventory.GetItemAt(0);
                        info3 = null;
                        str = null;
                        str2 = "";
                        int slot = 1;
                        while (true)
                        {
                            flag3 = slot <= 4;
                            if (!flag3)
                            {
                                ItemRecordBussiness objA = new ItemRecordBussiness();
                                try
                                {
                                    using (List<ItemInfo>.Enumerator enumerator = items.GetEnumerator())
                                    {
                                        while (true)
                                        {
                                            flag3 = enumerator.MoveNext();
                                            if (!flag3)
                                            {
                                                break;
                                            }
                                            ItemInfo current = enumerator.Current;
                                            objA.FusionItem(current, ref str);
                                        }
                                    }
                                }
                                finally
                                {
                                    if (!ReferenceEquals(objA, null))
                                    {
                                        objA.Dispose();
                                    }
                                }
                                break;
                            }
                            items.Add(inventory.GetItemAt(slot));
                            slot++;
                        }
                    }
                    break;
                }
            }
            else
            {
                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.Locked", new object[0]));
                return 1;
            }
            int num7 = packet.ReadInt();
            List<eBageType> list4 = new List<eBageType>();
            num4 = 0;
            while (true)
            {
                bool flag;
                int current;
                ItemInfo info6;
                List<int>.Enumerator enumerator2;
                flag3 = num4 < num7;
                if (flag3)
                {
                    type = (eBageType)packet.ReadByte();
                    num5 = packet.ReadInt();
                    itemAt = client.Player.GetItemAt(type, num5);
                    if (!ReferenceEquals(itemAt, null))
                    {
                        if (items.Contains(itemAt) || appendItems.Contains(itemAt))
                        {
                            client.Out.SendMessage(eMessageType.Normal, "Bad Input");
                            num10 = 1;
                            break;
                        }
                        objArray = new object[] { itemAt.ItemID, ":", itemAt.TemplateID, "," };
                        builder.Append(string.Concat(objArray));
                        appendItems.Add(itemAt);
                        list4.Add(type);
                        objArray = new object[] { str2, itemAt.ItemID, ":", itemAt.Template.Name, "," };
                        objArray[5] = itemAt.IsBinds ? "1" : "0";
                        objArray[6] = "|";
                        str2 = string.Concat(objArray);
                    }
                    num4++;
                    continue;
                }
                flag3 = num == 0;
                if (flag3)
                {
                    flag = false;
                    Dictionary<int, double> previewItemList = null;
                    using (enumerator2 = FusionFormulID.GetEnumerator())
                    {
                        while (true)
                        {
                            flag3 = enumerator2.MoveNext();
                            if (flag3)
                            {
                                current = enumerator2.Current;
                                info6 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(current), 1, 0x69);
                                previewItemList = FusionMgr.FusionPreview(items, appendItems, info6, ref flag);
                                if ((previewItemList == null) || (previewItemList.Count <= 0))
                                {
                                    continue;
                                }
                            }
                            break;
                        }
                    }
                    if ((previewItemList == null) || (previewItemList.Count <= 0))
                    {
                        client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemFusionHandler.ItemNotEnough", new object[0]));
                    }
                    else if (previewItemList.Count != 0)
                    {
                        client.Out.SendFusionPreview(client.Player, previewItemList, flag, minValid);
                    }
                    num10 = 0;
                    break;
                }
                inventory.ClearBag();
                int num8 = (num2 + num7) * 400;
                flag3 = client.Player.PlayerCharacter.Gold >= num8;
                if (flag3)
                {
                    flag = false;
                    flag2 = false;
                    ItemTemplateInfo objA = null;
                    using (enumerator2 = FusionFormulID.GetEnumerator())
                    {
                        while (true)
                        {
                            flag3 = enumerator2.MoveNext();
                            if (flag3)
                            {
                                current = enumerator2.Current;
                                itemAt = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(current), 1, 0x69);
                                objA = FusionMgr.Fusion(items, appendItems, itemAt, ref flag, ref flag2);
                                flag3 = ReferenceEquals(objA, null);
                                if (flag3)
                                {
                                    continue;
                                }
                            }
                            break;
                        }
                    }
                    if (ReferenceEquals(objA, null))
                    {
                        client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemFusionHandler.NoCondition", new object[0]));
                        goto TR_0009;
                    }
                    else
                    {
                        client.Player.RemoveGold(num8);
                        num4 = 0;
                        while (true)
                        {
                            flag3 = num4 < items.Count;
                            if (flag3)
                            {
                                ItemInfo local1 = items[num4];
                                local1.Count--;
                                client.Player.UpdateItem(items[num4]);
                                num4++;
                                continue;
                            }
                            itemAt.Count--;
                            client.Player.UpdateItem(itemAt);
                            num4 = 0;
                            while (true)
                            {
                                flag3 = num4 < appendItems.Count;
                                if (flag3)
                                {
                                    ItemInfo local2 = appendItems[num4];
                                    local2.Count--;
                                    client.Player.UpdateItem(appendItems[num4]);
                                    num4++;
                                    continue;
                                }
                                if (!flag2)
                                {
                                    builder.Append("false");
                                    client.Out.SendFusionResult(client.Player, flag2);
                                    client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemFusionHandler.Failed", new object[0]));
                                    goto TR_000A;
                                }
                                else
                                {
                                    info6 = ItemInfo.CreateFromTemplate(objA, 1, 0x69);
                                    if (!ReferenceEquals(info6, null))
                                    {
                                        info3 = info6;
                                        info6.IsBinds = flag;
                                        info6.ValidDate = minValid;
                                        client.Player.OnItemFusion(info6.Template.FusionType);
                                        client.Out.SendFusionResult(client.Player, flag2);
                                        client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemFusionHandler.Succeed1", new object[0]) + info6.Template.Name);
                                        if (!((((info6.TemplateID < 0x206c) || (info6.TemplateID > 0x2327)) && (((info6.TemplateID < 0x2454) || (info6.TemplateID > 0x270f)) && (((info6.TemplateID < 0x37dc) || (info6.TemplateID > 0x3a97)) && ((info6.TemplateID < 0x1b70) || (info6.TemplateID > 0x1b74))))) && (((info6.TemplateID < 0x36b6) || (info6.TemplateID > 0x36ba)) ? ((info6.TemplateID < 0x4268) || (info6.TemplateID > 0x4272)) : false)))
                                        {
                                            objArray = new object[] { client.Player.PlayerCharacter.NickName, info6.Template.Name };
                                            GSPacketIn @in = new GSPacketIn(10);
                                            @in.WriteInt(1);
                                            @in.WriteString(LanguageMgr.GetTranslation("ItemFusionHandler.Notice", objArray));
                                            GameServer.Instance.LoginServer.SendPacket(@in);
                                            GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
                                            int index = 0;
                                            while (true)
                                            {
                                                flag3 = index < allPlayers.Length;
                                                if (!flag3)
                                                {
                                                    break;
                                                }
                                                GamePlayer player = allPlayers[index];
                                                player.Out.SendTCP(@in);
                                                index++;
                                            }
                                        }
                                        if (!client.Player.AddTemplate(info6, info6.Template.BagType, info6.Count))
                                        {
                                            builder.Append("NoPlace");
                                            client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation(info6.GetBagName(), new object[0]) + LanguageMgr.GetTranslation("ItemFusionHandler.NoPlace", new object[0]));
                                        }
                                        goto TR_000A;
                                    }
                                    else
                                    {
                                        num10 = 0;
                                    }
                                }
                                break;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    client.Out.SendMessage(eMessageType.ERROR, LanguageMgr.GetTranslation("ItemFusionHandler.NoMoney", new object[0]));
                    num10 = 0;
                }
                break;
            }
            return num10;
            TR_0009:
            return 0;
            TR_000A:
            LogMgr.LogItemAdd(client.Player.PlayerCharacter.ID, LogItemType.Fusion, str, info3, str2, Convert.ToInt32(flag2));
            client.Player.SaveIntoDatabase();
            goto TR_0009;
        }
    }
}