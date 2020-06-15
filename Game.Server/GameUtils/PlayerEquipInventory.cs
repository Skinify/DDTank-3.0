using System;
using SqlDataProvider.Data;
using Game.Server.GameObjects;
using Bussiness;
using Game.Server.Packets;
using Bussiness.Managers;

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
            if (((info != null) && (info.CategoryID == 11)) && ((info.Property1 == 0x1f) && (info.Property2 == 3)))
            {
                attack += info.Property3;
                defence += info.Property4;
                agility += info.Property5;
                lucky += info.Property6;
            }
        }

        public void AddProperty(SqlDataProvider.Data.ItemInfo item, ref int attack, ref int defence, ref int agility, ref int lucky)
        {
            if (!ReferenceEquals(item, null))
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

        public bool CanEquipSlotContains(int slot, ItemTemplateInfo temp) =>
            ((temp.CategoryID != 8) ? ((temp.CategoryID != 9) ? ((temp.CategoryID != 13) ? ((temp.CategoryID != 14) ? ((temp.CategoryID != 15) ? ((temp.CategoryID != 0x10) ? ((temp.CategoryID != 0x11) ? ((temp.CategoryID - 1) == slot) : (slot == 15)) : (slot == 14)) : (slot == 13)) : (slot == 12)) : (slot == 11)) : ((((temp.TemplateID != 0x233e) && ((temp.TemplateID != 0x23a2) && ((temp.TemplateID != 0x2406) && (temp.TemplateID != 0x246a)))) && ((temp.TemplateID != 0x24ce) && (temp.TemplateID != 0x2532))) ? ((slot == 9) || (slot == 10)) : (((slot == 9) || (slot == 10)) || (slot == 0x10)))) : ((slot == 7) || (slot == 8)));

        public void EquipBuffer()
        {
            base.m_player.EquipEffect.Clear();
            for (int i = 0; i < 0x1f; i++)
            {
                SqlDataProvider.Data.ItemInfo itemAt = this.GetItemAt(i);
                if (!ReferenceEquals(itemAt, null))
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
            int num;
            switch (item.CategoryID)
            {
                case 8:
                    num = (base.m_items[7] == null) ? 7 : 8;
                    break;

                case 9:
                    num = (base.m_items[9] == null) ? 9 : 10;
                    break;

                case 13:
                    num = 11;
                    break;

                case 14:
                    num = 12;
                    break;

                case 15:
                    num = 13;
                    break;

                case 0x10:
                    num = 14;
                    break;

                default:
                    num = item.CategoryID - 1;
                    break;
            }
            return num;
        }

        public void GetUserNimbus()
        {
            int num = 0;
            int num2 = 0;
            int slot = 0;
            while (true)
            {
                if (slot >= 0x1f)
                {
                    base.m_player.PlayerCharacter.Nimbus = (num * 100) + num2;
                    base.m_player.Out.SendUpdatePublicPlayer(base.m_player.PlayerCharacter);
                    return;
                }
                SqlDataProvider.Data.ItemInfo itemAt = this.GetItemAt(slot);
                if (!ReferenceEquals(itemAt, null))
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
                    if ((itemAt.StrengthenLevel >= 9) && (itemAt.StrengthenLevel <= 11))
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
                    if ((itemAt.StrengthenLevel >= 12) && (itemAt.StrengthenLevel <= 14))
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
                    if (itemAt.StrengthenLevel == 15)
                    {
                        if ((itemAt.Template.CategoryID == 1) || (itemAt.Template.CategoryID == 5))
                        {
                            num = (num > 1) ? num : 5;
                        }
                        if (itemAt.Template.CategoryID == 7)
                        {
                            num2 = (num2 > 1) ? num2 : 4;
                        }
                    }
                }
                slot++;
            }
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
                PlayerBussiness pb = new PlayerBussiness();
                try
                {
                    int index = 0;
                    while (true)
                    {
                        if (index >= 0x1f)
                        {
                            break;
                        }
                        SqlDataProvider.Data.ItemInfo item = base.m_items[index];
                        if (!((base.m_items[index] == null) || base.m_items[index].IsValidItem()))
                        {
                            int toSlot = base.FindFirstEmptySlot(0x1f);
                            if (toSlot >= 0)
                            {
                                this.MoveItem(item.Place, toSlot, item.Count);
                            }
                            else
                            {
                                flag = flag || base.SendItemToMail(item, pb, null);
                            }
                        }
                        index++;
                    }
                }
                finally
                {
                    if (!ReferenceEquals(pb, null))
                    {
                        pb.Dispose();
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
            bool flag;
            if (!ReferenceEquals(base.m_items[fromSlot], null))
            {
                if ((!this.IsEquipSlot(fromSlot) || this.IsEquipSlot(toSlot)) || ((base.m_items[toSlot] == null) || (base.m_items[toSlot].Template.CategoryID == base.m_items[fromSlot].Template.CategoryID)))
                {
                    if (this.IsEquipSlot(toSlot))
                    {
                        if (this.CanEquipSlotContains(toSlot, base.m_items[fromSlot].Template))
                        {
                            if (!(base.m_player.CanEquip(base.m_items[fromSlot].Template) && base.m_items[fromSlot].IsValidItem()))
                            {
                                this.UpdateItem(base.m_items[fromSlot]);
                                return false;
                            }
                        }
                        else
                        {
                            this.UpdateItem(base.m_items[fromSlot]);
                            return false;
                        }
                    }
                    if (!((!this.IsEquipSlot(fromSlot) || (base.m_items[toSlot] == null)) || this.CanEquipSlotContains(fromSlot, base.m_items[toSlot].Template)))
                    {
                        this.UpdateItem(base.m_items[toSlot]);
                        return false;
                    }
                }
                else if (!this.CanEquipSlotContains(fromSlot, base.m_items[toSlot].Template))
                {
                    toSlot = base.FindFirstEmptySlot(0x1f);
                }
                flag = base.MoveItem(fromSlot, toSlot, count);
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public override void UpdateChangedPlaces()
        {
            bool flag = false;
            int[] numArray2 = base.m_changedPlaces.ToArray();
            int index = 0;
            while (true)
            {
                if (index < numArray2.Length)
                {
                    int slot = numArray2[index];
                    if (!this.IsEquipSlot(slot))
                    {
                        index++;
                        continue;
                    }
                    SqlDataProvider.Data.ItemInfo itemAt = this.GetItemAt(slot);
                    if (!ReferenceEquals(itemAt, null))
                    {
                        base.m_player.OnUsingItem(this.GetItemAt(slot).TemplateID);
                        itemAt.IsBinds = true;
                        if (!itemAt.IsUsed)
                        {
                            itemAt.IsUsed = true;
                            itemAt.BeginDate = DateTime.Now;
                        }
                    }
                    flag = true;
                }
                base.UpdateChangedPlaces();
                if (flag)
                {
                    this.UpdatePlayerProperties();
                }
                return;
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
                lock (base.m_lock)
                {
                    style = (base.m_items[0] == null) ? "" : base.m_items[0].TemplateID.ToString();
                    colors = (base.m_items[0] == null) ? "" : base.m_items[0].Color;
                    skin = (base.m_items[5] == null) ? "" : base.m_items[5].Skin;
                    SqlDataProvider.Data.ItemInfo info = base.m_items[6];
                    int index = 0;
                    while (true)
                    {
                        bool flag = index < 0x1f;
                        if (!flag)
                        {
                            this.EquipBuffer();
                            int num7 = 0;
                            while (true)
                            {
                                flag = num7 < StyleIndex.Length;
                                if (!flag)
                                {
                                    break;
                                }
                                style = style + ",";
                                colors = colors + ",";
                                if (!ReferenceEquals(base.m_items[StyleIndex[num7]], null))
                                {
                                    style = style + base.m_items[StyleIndex[num7]].TemplateID;
                                    colors = colors + base.m_items[StyleIndex[num7]].Color;
                                }
                                num7++;
                            }
                            break;
                        }
                        SqlDataProvider.Data.ItemInfo objA = base.m_items[index];
                        if (!ReferenceEquals(objA, null))
                        {
                            attack += objA.Attack;
                            defence += objA.Defence;
                            agility += objA.Agility;
                            lucky += objA.Luck;
                            level = (level > objA.StrengthenLevel) ? level : objA.StrengthenLevel;
                            this.AddProperty(objA, ref attack, ref defence, ref agility, ref lucky);
                        }
                        index++;
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

    /*
    public class PlayerEquipInventory : PlayerInventory
    {
        private const int BAG_START = 31;

        public PlayerEquipInventory(GamePlayer player)
            : base(player, true, 81, 0, BAG_START, true)
        {
        }

        /// <summary>
        /// 从数据库中加载物品，到指定的格子。
        /// </summary>
        public override void LoadFromDatabase()
        {
            bool response = false;

            BeginChanges();
            try
            {
                base.LoadFromDatabase();

                using (PlayerBussiness pb = new PlayerBussiness())
                {
                    for (int i = 0; i < BAG_START; i++)
                    {
                        ItemInfo item = m_items[i];
                        if (m_items[i] != null && !m_items[i].IsValidItem())
                        {
                            int slot = FindFirstEmptySlot(BAG_START);

                            if (slot >= 0)
                            {
                                MoveItem(item.Place, slot, item.Count);
                            }
                            else
                            {
                                response = response || SendItemToMail(item, pb, null);
                            }
                        }
                    }
                }
            }
            finally
            {
                CommitChanges();
            }

            if (response)
            {
                m_player.Out.SendMailResponse(m_player.PlayerCharacter.ID, eMailRespose.Receiver);
            }
        }

        public override bool MoveItem(int fromSlot, int toSlot, int count)
        {
            if (m_items[fromSlot] == null) return false;


            if (IsEquipSlot(fromSlot) && !IsEquipSlot(toSlot) && m_items[toSlot] != null && m_items[toSlot].Template.CategoryID != m_items[fromSlot].Template.CategoryID)
            {
                if (!CanEquipSlotContains(fromSlot, m_items[toSlot].Template))
                    toSlot = FindFirstEmptySlot(BAG_START);
                //return false;

                //if (!m_player.CanEquip(m_items[toSlot].Template))
                //    return false;
                //return this.AddItemTo(m_items[fromSlot], toSlot);
            }
            else
            {
                if (IsEquipSlot(toSlot))
                {
                    if (!CanEquipSlotContains(toSlot, m_items[fromSlot].Template))
                    {
                        UpdateItem(m_items[fromSlot]);
                        return false;
                    }

                    if (!(m_player.CanEquip(m_items[fromSlot].Template) && m_items[fromSlot].IsValidItem()))
                    {
                        UpdateItem(m_items[fromSlot]);
                        return false;
                    }
                }
                if (IsEquipSlot(fromSlot))
                {
                    if (m_items[toSlot] != null && !CanEquipSlotContains(fromSlot, m_items[toSlot].Template))
                    {
                        UpdateItem(m_items[toSlot]);
                        return false;
                    }
                }
            }

            return base.MoveItem(fromSlot, toSlot, count);
        }

        public override void UpdateChangedPlaces()
        {
            int[] changedSlot = m_changedPlaces.ToArray();


            bool updateStyle = false;

            foreach (int i in changedSlot)
            {
                if (IsEquipSlot(i))
                {

                    ItemInfo item = GetItemAt(i);
                    if (item != null)
                    {
                        m_player.OnUsingItem(GetItemAt(i).TemplateID);//触发任务<使用道具>
                        item.IsBinds = true;
                        if (!item.IsUsed)
                        {
                            item.IsUsed = true;
                            item.BeginDate = DateTime.Now;
                        }
                    }
                    updateStyle = true;
                    break;
                }
            }
            base.UpdateChangedPlaces();
            if (updateStyle)
            {
                UpdatePlayerProperties();
            }
        }


        #region Build Player Style/Properties

        //private static readonly int[] StyleIndex = new int[] { 1, 2, 3, 4, 5, 6, 11, 13, 14 };
        //DDTAnk
        private static readonly int[] StyleIndex = new int[] { 1, 2, 3, 4, 5, 6, 11, 13, 14, 15, 16, 17, 18, 19, 20 };
        public void UpdatePlayerProperties()
        {
            m_player.BeginChanges();

            try
            {
                int attack = 0;
                int defence = 0;
                int agility = 0;
                int lucky = 0;
                int strengthenLevel = 0;
                string style = "";
                string color = "";
                string skin = "";
                ItemInfo weapon = null;

                lock (m_lock)
                {
                    style = m_items[0] == null ? "" : m_items[0].TemplateID.ToString();
                    color = m_items[0] == null ? "" : m_items[0].Color;
                    skin = m_items[5] == null ? "" : m_items[5].Skin;
                    weapon = m_items[6];

                    for (int i = 0; i < BAG_START; i++)
                    {
                        ItemInfo item = m_items[i];
                        if (item != null)
                        {
                            attack += item.Attack;
                            defence += item.Defence;
                            agility += item.Agility;
                            lucky += item.Luck;

                            //item.IsBinds=true;
                            strengthenLevel = strengthenLevel > item.StrengthenLevel ? strengthenLevel : item.StrengthenLevel;
                            AddProperty(item, ref  attack, ref  defence, ref  agility, ref   lucky);
                        }
                    }

                    EquipBuffer();
                    for (int i = 0; i < StyleIndex.Length; i++)
                    {
                        style += ",";
                        color += ",";
                        if (m_items[StyleIndex[i]] != null)
                        {
                            style += m_items[StyleIndex[i]].TemplateID;
                            color += m_items[StyleIndex[i]].Color;
                        }
                    }
                }

                m_player.UpdateBaseProperties(attack, defence, agility, lucky);
                m_player.UpdateStyle(style, color, skin);
                GetUserNimbus();
                m_player.ApertureEquip(strengthenLevel);
                m_player.UpdateWeapon(m_items[6]);
                m_player.UpdateSecondWeapon(m_items[15]);
                m_player.UpdateFightPower();
            }
            finally
            {
                m_player.CommitChanges();
            }
        }

        #endregion

        #region EquipSlot GetItemEpuipSlot/IsEquipSlot/CanEquipSlotContains

        /// <summary>
        /// 获取物品的装备位置
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int FindItemEpuipSlot(ItemTemplateInfo item)
        {
            switch (item.CategoryID)
            {
                case 8:
                    if (m_items[7] == null)
                    {
                        return 7;
                    }
                    else
                    {
                        return 8;
                    }
                case 9:
                    if (m_items[9] == null)
                    {
                        return 9;
                    }
                    else
                    {
                        return 10;
                    }
                case 13:
                    return 11;
                case 14:
                    return 12;
                case 15:
                    return 13;
                case 16:
                    return 14;
                default:
                    return item.CategoryID - 1;
            }
        }

        /// <summary>
        /// 装备的位置是否能装备此物品。
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="temp"></param>
        /// <returns></returns>
        public bool CanEquipSlotContains(int slot, ItemTemplateInfo temp)
        {
            if (temp.CategoryID == 8)
            {
                return slot == 7 || slot == 8;
            }
            else if (temp.CategoryID == 9)
            {
                if (temp.TemplateID == 9022 || temp.TemplateID == 9122 || temp.TemplateID == 9222 || temp.TemplateID == 9322 || temp.TemplateID == 9422 || temp.TemplateID == 9522)
                    return slot == 9 || slot == 10 || slot == 16;
                else
                    return slot == 9 || slot == 10;
            }
            else if (temp.CategoryID == 13)
            {
                return slot == 11;
            }
            else if (temp.CategoryID == 14)
            {
                return slot == 12;
            }
            else if (temp.CategoryID == 15)
            {
                return slot == 13;
            }
            else if (temp.CategoryID == 16)
            {
                return slot == 14;
            }
            else if (temp.CategoryID == 17)
            {
                return slot == 15;
            }
            else
            {
                return temp.CategoryID - 1 == slot;
            }
        }

        /// <summary>
        /// 是否为身上的插槽
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public bool IsEquipSlot(int slot)
        {
            return slot >= 0 && slot < BAG_START;
        }


        public void GetUserNimbus()
        {

            int i = 0;
            int j = 0;
            for (int m = 0; m < BAG_START; m++)
            {
                ItemInfo item = GetItemAt(m);
                if (item != null)
                {

                    if (item.StrengthenLevel >= 5 && item.StrengthenLevel <= 8)
                    {
                        if (item.Template.CategoryID == 1 || item.Template.CategoryID == 5)
                        { i = i > 01 ? i : 01; }
                        if (item.Template.CategoryID == 7)
                        { j = j > 01 ? j : 01; }

                    }
                    if (item.StrengthenLevel == 9)
                    {
                        if (item.Template.CategoryID == 1 || item.Template.CategoryID == 5)
                        { i = i > 01 ? i : 02; }
                        if (item.Template.CategoryID == 7)
                        { j = j > 01 ? j : 02; }

                    }
                    if (item.StrengthenLevel == 12)
                    {
                        if (item.Template.CategoryID == 1 || item.Template.CategoryID == 5)
                        { i = i > 01 ? i : 03; }
                        if (item.Template.CategoryID == 7)
                        { j = j > 01 ? j : 03; }
                    }
                    if (item.StrengthenLevel == 15)
                    {
                        if (item.Template.CategoryID == 1 || item.Template.CategoryID == 5)
                        { i = i > 01 ? i : 04; }
                        if (item.Template.CategoryID == 7)
                        { j = j > 01 ? j : 04; }
                    }

                }
                continue;

            }
            m_player.PlayerCharacter.Nimbus = i * 100 + j;
            //m_player.Out.SendUpdateAllSorce();//更新光环
            m_player.Out.SendUpdatePublicPlayer(m_player.PlayerCharacter);
        }


        /// <summary>
        /// 玩家装备Buffer
        /// </summary>
        public void EquipBuffer()
        {
            m_player.EquipEffect.Clear();//EquipBufferList.Clear();
            for (int m = 0; m < BAG_START; m++)
            {
                ItemInfo item = GetItemAt(m);
                if (item != null)
                {

                    if (item.Hole1 > 0)
                        m_player.EquipEffect.Add(item.Hole1);

                    if (item.Hole2 > 0)
                        m_player.EquipEffect.Add(item.Hole2);

                    if (item.Hole3 > 0)
                        m_player.EquipEffect.Add(item.Hole3);

                    if (item.Hole4 > 0)
                        m_player.EquipEffect.Add(item.Hole4);

                    if (item.Hole5 > 0)
                        m_player.EquipEffect.Add(item.Hole5);

                    if (item.Hole6 > 0)
                        m_player.EquipEffect.Add(item.Hole6);

                }
            }
        }

        public void AddProperty(ItemInfo item, ref int attack, ref int defence, ref int agility, ref  int lucky)
        {
            if (item != null)
            {

                if (item.Hole1 > 0)

                    AddBaseProperty(item.Hole1, ref attack, ref defence, ref agility, ref lucky);

                if (item.Hole2 > 0)
                    AddBaseProperty(item.Hole2, ref attack, ref defence, ref agility, ref lucky);

                if (item.Hole3 > 0)
                    AddBaseProperty(item.Hole3, ref attack, ref defence, ref agility, ref lucky);

                if (item.Hole4 > 0)
                    AddBaseProperty(item.Hole4, ref attack, ref defence, ref agility, ref lucky);

                if (item.Hole5 > 0)
                    AddBaseProperty(item.Hole5, ref attack, ref defence, ref agility, ref lucky);

                if (item.Hole6 > 0)
                    AddBaseProperty(item.Hole6, ref attack, ref defence, ref agility, ref lucky);

            }

        }

        public void AddBaseProperty(int templateid, ref int attack, ref int defence, ref int agility, ref  int lucky)
        {
            ItemTemplateInfo temp = ItemMgr.FindItemTemplate(templateid);
            if (temp != null)
            {
                if (temp.CategoryID == 11 && temp.Property1 == 31 && temp.Property2 == 3)
                {
                    attack += temp.Property3;
                    defence += temp.Property4;
                    agility += temp.Property5;
                    lucky += temp.Property6;
                }

            }
        }


        #endregion

    }*/
}
