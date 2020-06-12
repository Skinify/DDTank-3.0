using Bussiness;
using Game.Server.GameObjects;
using Game.Server.Packets;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Text;


namespace Game.Server.GameUtilsNOVO
{
    public class PlayerInventory : AbstractInventory
    {
        protected GamePlayer m_player;
        private List<SqlDataProvider.Data.ItemInfo> m_removedList;
        private bool m_saveToDb;

        public PlayerInventory(GamePlayer player, bool saveTodb, int capibility, int type, int beginSlot, bool autoStack) : base(capibility, type, beginSlot, autoStack)
        {
            this.m_removedList = new List<SqlDataProvider.Data.ItemInfo>();
            this.m_player = player;
            this.m_saveToDb = saveTodb;
        }

        public override bool AddItemTo(SqlDataProvider.Data.ItemInfo item, int place)
        {
            if (base.AddItemTo(item, place))
            {
                item.UserID = this.m_player.PlayerCharacter.ID;
                item.IsExist = true;
                return true;
            }
            return false;
        }

        public virtual void LoadFromDatabase()
        {
            if (this.m_saveToDb)
            {
                using (PlayerBussiness bussiness = new PlayerBussiness())
                {
                    SqlDataProvider.Data.ItemInfo[] userBagByType = bussiness.GetUserBagByType(this.m_player.PlayerCharacter.ID, base.BagType);
                    base.BeginChanges();
                    try
                    {
                        foreach (SqlDataProvider.Data.ItemInfo info in userBagByType)
                        {
                            this.AddItemTo(info, info.Place);
                        }
                    }
                    finally
                    {
                        base.CommitChanges();
                    }
                }
            }
        }

        public void MoveToStore(PlayerInventory bag, int fromSolt, int toSolt, PlayerInventory tobag, int maxCount)
        {
            SqlDataProvider.Data.ItemInfo itemAt = bag.GetItemAt(fromSolt);
            SqlDataProvider.Data.ItemInfo item = tobag.GetItemAt(toSolt);
            if ((itemAt != null) || (item != null))
            {
                if (((item == null) && ((toSolt < 0) || (toSolt > 80))) || ((tobag.BagType == 11) && (toSolt >= maxCount)))
                {
                    if ((tobag.BagType == 11) || (tobag.BagType == 1))
                    {
                        toSolt = tobag.FindFirstEmptySlot(0);
                    }
                    else
                    {
                        toSolt = tobag.FindFirstEmptySlot(0x1f);
                    }
                }
                if (((((itemAt.Template.CategoryID == 10) || (itemAt.Template.CategoryID == 11)) || (itemAt.Template.CategoryID == 12)) && (tobag.BagType != 0)) || ((((itemAt.Template.CategoryID != 10) && (itemAt.Template.CategoryID != 11)) && (itemAt.Template.CategoryID != 12)) && (tobag.BagType != 1)))
                {
                    if ((item != null) && (itemAt != null))
                    {
                        if (((((item.Template.CategoryID != 10) && (item.Template.CategoryID != 11)) && (item.Template.CategoryID != 12)) || (bag.BagType != 0)) && ((((item.Template.CategoryID == 10) || (item.Template.CategoryID == 11)) || (item.Template.CategoryID == 12)) || (bag.BagType != 1)))
                        {
                            int place = itemAt.Place;
                            int bagType = itemAt.BagType;
                            int num3 = item.Place;
                            int num4 = item.BagType;
                            bag.RemoveItem(itemAt);
                            tobag.RemoveItem(item);
                            itemAt.IsExist = true;
                            itemAt.BagType = num4;
                            tobag.AddItemTo(itemAt, num3);
                            item.IsExist = true;
                            item.BagType = bagType;
                            if ((place == -1) && (bag.BagType == 0))
                            {
                                place = bag.FindFirstEmptySlot(0x1f);
                            }
                            if (place < 0x1f)
                            {
                                place = bag.FindFirstEmptySlot(0x1f);
                            }
                            if ((tobag.BagType != 1) || (((item.Template.CategoryID != 10) && (item.Template.CategoryID != 11)) && (item.Template.CategoryID != 12)))
                            {
                                bag.AddItemTo(item, place);
                            }
                        }
                    }
                    else
                    {
                        if (item != null)
                        {
                            tobag.RemoveItem(itemAt);
                            bag.AddItemTo(itemAt, toSolt);
                        }
                        if (itemAt != null)
                        {
                            if ((tobag.BagType == 11) && (toSolt >= maxCount))
                            {
                                this.UpdateItem(itemAt);
                            }
                            else
                            {
                                bag.RemoveItem(itemAt);
                                tobag.AddItemTo(itemAt, toSolt);
                            }
                        }
                    }
                }
                else
                {
                    if (item != null)
                    {
                        this.UpdateItem(item);
                    }
                    if (itemAt != null)
                    {
                        this.UpdateItem(itemAt);
                    }
                }
            }
        }

        public override bool RemoveItem(SqlDataProvider.Data.ItemInfo item)
        {
            if (base.RemoveItem(item))
            {
                item.IsExist = false;
                if (this.m_saveToDb)
                {
                    lock (this.m_removedList)
                    {
                        this.m_removedList.Add(item);
                    }
                }
                return true;
            }
            return false;
        }

        public virtual void SaveToDatabase()
        {
            if (this.m_saveToDb)
            {
                using (PlayerBussiness bussiness = new PlayerBussiness())
                {
                    lock (base.m_lock)
                    {
                        for (int i = 0; i < base.m_items.Length; i++)
                        {
                            SqlDataProvider.Data.ItemInfo item = base.m_items[i];
                            if ((item != null) && item.IsDirty)
                            {
                                if (item.ItemID > 0)
                                {
                                    bussiness.UpdateGoods(item);
                                }
                                else
                                {
                                    bussiness.AddGoods(item);
                                }
                            }
                        }
                    }
                    lock (this.m_removedList)
                    {
                        foreach (SqlDataProvider.Data.ItemInfo info in this.m_removedList)
                        {
                            if (info.ItemID > 0)
                            {
                                bussiness.UpdateGoods(info);
                            }
                        }
                        this.m_removedList.Clear();
                    }
                }
            }
        }

        public bool SendAllItemsToMail(string sender, string title, eMailType type)
        {
            if (this.m_saveToDb)
            {
                base.BeginChanges();
                try
                {
                    using (PlayerBussiness bussiness = new PlayerBussiness())
                    {
                        lock (base.m_lock)
                        {
                            List<SqlDataProvider.Data.ItemInfo> items = this.GetItems();
                            int count = items.Count;
                            for (int i = 0; i < count; i += 5)
                            {
                                MailInfo mail = new MailInfo
                                {
                                    SenderID = 0,
                                    Sender = sender,
                                    ReceiverID = this.m_player.PlayerCharacter.ID,
                                    Receiver = this.m_player.PlayerCharacter.NickName,
                                    Title = title,
                                    Type = (int)type,
                                    Content = ""
                                };
                                List<SqlDataProvider.Data.ItemInfo> list2 = new List<SqlDataProvider.Data.ItemInfo>();
                                for (int j = 0; j < 5; j++)
                                {
                                    int num4 = (i * 5) + j;
                                    if (num4 < items.Count)
                                    {
                                        list2.Add(items[num4]);
                                    }
                                }
                                if (!this.SendItemsToMail(list2, mail, bussiness))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Send Items Mail Error:" + exception);
                }
                finally
                {
                    this.SaveToDatabase();
                    base.CommitChanges();
                }
                this.m_player.Out.SendMailResponse(this.m_player.PlayerCharacter.ID, eMailRespose.Receiver);
            }
            return true;
        }

        public bool SendItemsToMail(List<SqlDataProvider.Data.ItemInfo> items, MailInfo mail, PlayerBussiness pb)
        {
            if (mail != null)
            {
                if (items.Count > 5)
                {
                    return false;
                }
                if (this.m_saveToDb)
                {
                    SqlDataProvider.Data.ItemInfo info;
                    List<SqlDataProvider.Data.ItemInfo> list = new List<SqlDataProvider.Data.ItemInfo>();
                    StringBuilder builder = new StringBuilder();
                    builder.Append(LanguageMgr.GetTranslation("Game.Server.GameUtils.CommonBag.AnnexRemark", new object[0]));
                    if ((items.Count > 0) && this.TakeOutItem(items[0]))
                    {
                        info = items[0];
                        mail.Annex1 = info.ItemID.ToString();
                        mail.Annex1Name = info.Template.Name;
                        builder.Append(string.Concat(new object[] { "1、", mail.Annex1Name, "x", info.Count, ";" }));
                        list.Add(info);
                    }
                    if ((items.Count > 1) && this.TakeOutItem(items[1]))
                    {
                        info = items[1];
                        mail.Annex2 = info.ItemID.ToString();
                        mail.Annex2Name = info.Template.Name;
                        builder.Append(string.Concat(new object[] { "2、", mail.Annex2Name, "x", info.Count, ";" }));
                        list.Add(info);
                    }
                    if ((items.Count > 2) && this.TakeOutItem(items[2]))
                    {
                        info = items[2];
                        mail.Annex3 = info.ItemID.ToString();
                        mail.Annex3Name = info.Template.Name;
                        builder.Append(string.Concat(new object[] { "3、", mail.Annex3Name, "x", info.Count, ";" }));
                        list.Add(info);
                    }
                    if ((items.Count > 3) && this.TakeOutItem(items[3]))
                    {
                        info = items[3];
                        mail.Annex4 = info.ItemID.ToString();
                        mail.Annex4Name = info.Template.Name;
                        builder.Append(string.Concat(new object[] { "4、", mail.Annex4Name, "x", info.Count, ";" }));
                        list.Add(info);
                    }
                    if ((items.Count > 4) && this.TakeOutItem(items[4]))
                    {
                        info = items[4];
                        mail.Annex5 = info.ItemID.ToString();
                        mail.Annex5Name = info.Template.Name;
                        builder.Append(string.Concat(new object[] { "5、", mail.Annex5Name, "x", info.Count, ";" }));
                        list.Add(info);
                    }
                    mail.AnnexRemark = builder.ToString();
                    if (pb.SendMail(mail))
                    {
                        return true;
                    }
                    foreach (SqlDataProvider.Data.ItemInfo info2 in list)
                    {
                        base.AddItem(info2);
                    }
                }
            }
            return false;
        }

        public bool SendItemToMail(SqlDataProvider.Data.ItemInfo item)
        {
            if (this.m_saveToDb)
            {
                using (PlayerBussiness bussiness = new PlayerBussiness())
                {
                    return this.SendItemToMail(item, bussiness, null);
                }
            }
            return false;
        }

        public bool SendItemToMail(SqlDataProvider.Data.ItemInfo item, PlayerBussiness pb, MailInfo mail)
        {
            if (this.m_saveToDb && (item.BagType == base.BagType))
            {
                if (mail == null)
                {
                    mail = new MailInfo();
                    mail.Annex1 = item.ItemID.ToString();
                    mail.Content = LanguageMgr.GetTranslation("Game.Server.GameUtils.Title", new object[0]);
                    mail.Gold = 0;
                    mail.IsExist = true;
                    mail.Money = 0;
                    mail.Receiver = this.m_player.PlayerCharacter.NickName;
                    mail.ReceiverID = item.UserID;
                    mail.Sender = this.m_player.PlayerCharacter.NickName;
                    mail.SenderID = item.UserID;
                    mail.Title = LanguageMgr.GetTranslation("Game.Server.GameUtils.Title", new object[0]);
                    mail.Type = 9;
                }
                if (pb.SendMail(mail))
                {
                    this.RemoveItem(item);
                    item.IsExist = true;
                    return true;
                }
                return false;
            }
            return false;
        }

        public override bool TakeOutItem(SqlDataProvider.Data.ItemInfo item)
        {
            if (base.TakeOutItem(item))
            {
                if (this.m_saveToDb)
                {
                    lock (this.m_removedList)
                    {
                        this.m_removedList.Add(item);
                    }
                }
                return true;
            }
            return false;
        }

        public override void UpdateChangedPlaces()
        {
            int[] updatedSlots = base.m_changedPlaces.ToArray();
            //this.m_player.Out.SendUpdateInventorySlot(this, updatedSlots);
            base.UpdateChangedPlaces();
        }
    }
}

