using Bussiness;
using Bussiness.Managers;
using Game.Server.GameObjects;
using Game.Server.Packets;
using SqlDataProvider.Data;
using System;


namespace Game.Server.GameUtils
{
    public class PlayerEquipInventory : PlayerInventory
    {
        private const int BAG_START = 0x1f;
        private static readonly int[] StyleIndex = new int[] { 1, 2, 3, 4, 5, 6, 11, 13, 14, 15, 0x10, 0x11, 0x12, 0x13, 20 };

        public PlayerEquipInventory(GamePlayer player) : base(player, true, 0x51, 0, 0x1f, true)
        {
        }

        public void AddBaseProperty(int templateid, ref int attack, ref int defence, ref int agility, ref int lucky)
        {
            ItemTemplateInfo info = ItemMgr.FindItemTemplate(templateid);
            if ((info != null) && (((info.CategoryID == 11) && (info.Property1 == 0x1f)) && (info.Property2 == 3)))
            {
                attack += info.Property3;
                defence += info.Property4;
                agility += info.Property5;
                lucky += info.Property6;
            }
        }

        public void AddProperty(SqlDataProvider.Data.ItemInfo item, ref int attack, ref int defence, ref int agility, ref int lucky)
        {
            if (item != null)
            {
                if (item.Hole1 > 0)
                {
                    this.AddBaseProperty(item.Hole1, ref attack, ref defence, ref agility, ref lucky);
                }
                if (item.Hole2 > 0)
                {
                    this.AddBaseProperty(item.Hole2, ref attack, ref defence, ref agility, ref lucky);
                }
                if (item.Hole3 > 0)
                {
                    this.AddBaseProperty(item.Hole3, ref attack, ref defence, ref agility, ref lucky);
                }
                if (item.Hole4 > 0)
                {
                    this.AddBaseProperty(item.Hole4, ref attack, ref defence, ref agility, ref lucky);
                }
                if (item.Hole5 > 0)
                {
                    this.AddBaseProperty(item.Hole5, ref attack, ref defence, ref agility, ref lucky);
                }
                if (item.Hole6 > 0)
                {
                    this.AddBaseProperty(item.Hole6, ref attack, ref defence, ref agility, ref lucky);
                }
            }
        }

        public bool CanEquipSlotContains(int slot, ItemTemplateInfo temp)
        {
            if (temp.CategoryID == 8)
            {
                return ((slot == 7) || (slot == 8));
            }
            if (temp.CategoryID == 9)
            {
                if (((((temp.TemplateID == 0x233e) || (temp.TemplateID == 0x23a2)) || ((temp.TemplateID == 0x2406) || (temp.TemplateID == 0x246a))) || (temp.TemplateID == 0x24ce)) || (temp.TemplateID == 0x2532))
                {
                    return (((slot == 9) || (slot == 10)) || (slot == 0x10));
                }
                return ((slot == 9) || (slot == 10));
            }
            if (temp.CategoryID == 13)
            {
                return (slot == 11);
            }
            if (temp.CategoryID == 14)
            {
                return (slot == 12);
            }
            if (temp.CategoryID == 15)
            {
                return (slot == 13);
            }
            if (temp.CategoryID == 0x10)
            {
                return (slot == 14);
            }
            if (temp.CategoryID == 0x11)
            {
                return (slot == 15);
            }
            return ((temp.CategoryID - 1) == slot);
        }

        public void EquipBuffer()
        {
            base.m_player.EquipEffect.Clear();
            for (int i = 0; i < 0x1f; i++)
            {
                SqlDataProvider.Data.ItemInfo itemAt = this.GetItemAt(i);
                if (itemAt != null)
                {
                    if (itemAt.Hole1 > 0)
                    {
                        base.m_player.EquipEffect.Add(itemAt.Hole1);
                    }
                    if (itemAt.Hole2 > 0)
                    {
                        base.m_player.EquipEffect.Add(itemAt.Hole2);
                    }
                    if (itemAt.Hole3 > 0)
                    {
                        base.m_player.EquipEffect.Add(itemAt.Hole3);
                    }
                    if (itemAt.Hole4 > 0)
                    {
                        base.m_player.EquipEffect.Add(itemAt.Hole4);
                    }
                    if (itemAt.Hole5 > 0)
                    {
                        base.m_player.EquipEffect.Add(itemAt.Hole5);
                    }
                    if (itemAt.Hole6 > 0)
                    {
                        base.m_player.EquipEffect.Add(itemAt.Hole6);
                    }
                }
            }
        }

        public int FindItemEpuipSlot(ItemTemplateInfo item)
        {
            switch (item.CategoryID)
            {
                case 8:
                    if (base.m_items[7] == null)
                    {
                        return 7;
                    }
                    return 8;

                case 9:
                    if (base.m_items[9] == null)
                    {
                        return 9;
                    }
                    return 10;

                case 13:
                    return 11;

                case 14:
                    return 12;

                case 15:
                    return 13;

                case 0x10:
                    return 14;
            }
            return (item.CategoryID - 1);
        }

        public void GetUserNimbus()
        {
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < 0x1f; i++)
            {
                SqlDataProvider.Data.ItemInfo itemAt = this.GetItemAt(i);
                if (itemAt != null)
                {
                    if ((itemAt.StrengthenLevel >= 5) && (itemAt.StrengthenLevel <= 8))
                    {
                        if ((itemAt.Template.CategoryID == 1) || (itemAt.Template.CategoryID == 5))
                        {
                            num = (num > 1) ? num : 1;
                        }
                        if (itemAt.Template.CategoryID == 7)
                        {
                            num2 = (num2 > 1) ? num2 : 1;
                        }
                    }
                    if (itemAt.StrengthenLevel == 9)
                    {
                        if ((itemAt.Template.CategoryID == 1) || (itemAt.Template.CategoryID == 5))
                        {
                            num = (num > 1) ? num : 2;
                        }
                        if (itemAt.Template.CategoryID == 7)
                        {
                            num2 = (num2 > 1) ? num2 : 2;
                        }
                    }
                    if (itemAt.StrengthenLevel == 12)
                    {
                        if ((itemAt.Template.CategoryID == 1) || (itemAt.Template.CategoryID == 5))
                        {
                            num = (num > 1) ? num : 3;
                        }
                        if (itemAt.Template.CategoryID == 7)
                        {
                            num2 = (num2 > 1) ? num2 : 3;
                        }
                    }
                }
            }
            base.m_player.PlayerCharacter.Nimbus = (num * 100) + num2;
            base.m_player.Out.SendUpdatePublicPlayer(base.m_player.PlayerCharacter);
        }

        public bool IsEquipSlot(int slot) =>
            ((slot >= 0) && (slot < 0x1f));

        public override void LoadFromDatabase()
        {
            bool flag = false;
            base.BeginChanges();
            try
            {
                base.LoadFromDatabase();
                using (PlayerBussiness bussiness = new PlayerBussiness())
                {
                    for (int i = 0; i < 0x1f; i++)
                    {
                        SqlDataProvider.Data.ItemInfo item = base.m_items[i];
                        if ((base.m_items[i] != null) && !base.m_items[i].IsValidItem())
                        {
                            int toSlot = base.FindFirstEmptySlot(0x1f);
                            if (toSlot >= 0)
                            {
                                this.MoveItem(item.Place, toSlot, item.Count);
                            }
                            else
                            {
                                flag = flag || base.SendItemToMail(item, bussiness, null);
                            }
                        }
                    }
                }
            }
            finally
            {
                base.CommitChanges();
            }
            if (flag)
            {
                base.m_player.Out.SendMailResponse(base.m_player.PlayerCharacter.ID, eMailRespose.Receiver);
            }
        }

        public override bool MoveItem(int fromSlot, int toSlot, int count)
        {
            if (base.m_items[fromSlot] == null)
            {
                return false;
            }
            if (((this.IsEquipSlot(fromSlot) && !this.IsEquipSlot(toSlot)) && (base.m_items[toSlot] != null)) && (base.m_items[toSlot].Template.CategoryID != base.m_items[fromSlot].Template.CategoryID))
            {
                if (!this.CanEquipSlotContains(fromSlot, base.m_items[toSlot].Template))
                {
                    toSlot = base.FindFirstEmptySlot(0x1f);
                }
            }
            else
            {
                if (this.IsEquipSlot(toSlot))
                {
                    if (!this.CanEquipSlotContains(toSlot, base.m_items[fromSlot].Template))
                    {
                        this.UpdateItem(base.m_items[fromSlot]);
                        return false;
                    }
                    if (!(base.m_player.CanEquip(base.m_items[fromSlot].Template) && base.m_items[fromSlot].IsValidItem()))
                    {
                        this.UpdateItem(base.m_items[fromSlot]);
                        return false;
                    }
                }
                if (this.IsEquipSlot(fromSlot) && ((base.m_items[toSlot] != null) && !this.CanEquipSlotContains(fromSlot, base.m_items[toSlot].Template)))
                {
                    this.UpdateItem(base.m_items[toSlot]);
                    return false;
                }
            }
            return base.MoveItem(fromSlot, toSlot, count);
        }

        public override void UpdateChangedPlaces()
        {
            int[] numArray = base.m_changedPlaces.ToArray();
            bool flag = false;
            foreach (int num in numArray)
            {
                if (this.IsEquipSlot(num))
                {
                    SqlDataProvider.Data.ItemInfo itemAt = this.GetItemAt(num);
                    if (itemAt != null)
                    {
                        base.m_player.OnUsingItem(this.GetItemAt(num).TemplateID);
                        itemAt.IsBinds = true;
                        if (!itemAt.IsUsed)
                        {
                            itemAt.IsUsed = true;
                            itemAt.BeginDate = DateTime.Now;
                        }
                    }
                    flag = true;
                    break;
                }
            }
            base.UpdateChangedPlaces();
            if (flag)
            {
                this.UpdatePlayerProperties();
            }
        }

        public void UpdatePlayerProperties()
        {
            base.m_player.BeginChanges();
            try
            {
                int attack = 0;
                int defence = 0;
                int agility = 0;
                int lucky = 0;
                int level = 0;
                string style = "";
                string colors = "";
                string skin = "";
                SqlDataProvider.Data.ItemInfo info = null;
                lock (base.m_lock)
                {
                    int num6;
                    style = (base.m_items[0] == null) ? "" : base.m_items[0].TemplateID.ToString();
                    colors = (base.m_items[0] == null) ? "" : base.m_items[0].Color;
                    skin = (base.m_items[5] == null) ? "" : base.m_items[5].Skin;
                    info = base.m_items[6];
                    for (num6 = 0; num6 < 0x1f; num6++)
                    {
                        SqlDataProvider.Data.ItemInfo item = base.m_items[num6];
                        if (item != null)
                        {
                            attack += item.Attack;
                            defence += item.Defence;
                            agility += item.Agility;
                            lucky += item.Luck;
                            level = (level > item.StrengthenLevel) ? level : item.StrengthenLevel;
                            this.AddProperty(item, ref attack, ref defence, ref agility, ref lucky);
                        }
                    }
                    this.EquipBuffer();
                    for (num6 = 0; num6 < StyleIndex.Length; num6++)
                    {
                        style = style + ",";
                        colors = colors + ",";
                        if (base.m_items[StyleIndex[num6]] != null)
                        {
                            style = style + base.m_items[StyleIndex[num6]].TemplateID;
                            colors = colors + base.m_items[StyleIndex[num6]].Color;
                        }
                    }
                }
                base.m_player.UpdateBaseProperties(attack, defence, agility, lucky);
                base.m_player.UpdateStyle(style, colors, skin);
                this.GetUserNimbus();
                base.m_player.ApertureEquip(level);
                base.m_player.UpdateWeapon(base.m_items[6]);
                base.m_player.UpdateSecondWeapon(base.m_items[15]);
                base.m_player.UpdateFightPower();
            }
            finally
            {
                base.m_player.CommitChanges();
            }
        }
    }
}

