/*


using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic;
using Game.Logic.Phy.Object;
using Game.Server;
using Game.Server.Buffer;
using Game.Server.GameUtils;
using Game.Server.HotSpringRooms;
using Game.Server.Managers;
using Game.Server.Packets;
using Game.Server.Quests;
using Game.Server.Rooms;
using Game.Server.SceneMarryRooms;
using Game.Server.Statics;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;


namespace Game.Server.GameObjects
{
    public class GamePlayer : IGamePlayer
    {
        private MarryRoom _currentMarryRoom;
        private HotSpringRoom _currentHotRoom;
        private Dictionary<int, int> _friends;
        public int CurrentRoomIndex;
        public int CurrentRoomTeam;
        public int FightPower;
        public double GPAddPlus;
        public double GuildRichAddPlus = 1.0;
        public bool KickProtect;
        public DateTime LastChatTime;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string m_account;
        private Game.Server.Buffer.BufferList m_bufferList;
        private PlayerInventory m_caddyBag;
        private PlayerInventory m_cardBag;
        private int m_changed;
        private PlayerInfo m_character;
        protected GameClient m_client;
        private UTF8Encoding m_converter;
        private BaseRoom m_currentRoom;
        private SqlDataProvider.Data.ItemInfo m_currentSecondWeapon;
        private List<int> m_equipEffect;
        private PlayerInventory m_fightBag;
        private int m_immunity = 0xff;
        private bool m_isAASInfo;
        private bool m_isMinor;
        private PlayerEquipInventory m_mainBag;
        private SqlDataProvider.Data.ItemInfo m_MainWeapon;
        private long m_pingTime;
        private int m_playerId;
        private PlayerInventory m_propBag;
        private byte[] m_pvepermissions;
        private Game.Server.Quests.QuestInventory m_questInventory;
        private PlayerInventory m_storeBag;
        private PlayerInventory m_storeBag2;
        private PlayerInventory m_tempBag;
        public int MarryMap;
        public double OfferAddPlus = 1.0;
        public long PingStart;
        public int X;
        public int Y;

        public event PlayerGameKillEventHandel AfterKillingLiving;

        public event PlayerItemPropertyEventHandle AfterUsingItem;

        public event GameKillDropEventHandel GameKillDrop;

        public event PlayerGameOverEventHandle GameOver;

        public event PlayerOwnConsortiaEventHandle GuildChanged;

        public event PlayerItemComposeEventHandle ItemCompose;

        public event PlayerItemFusionEventHandle ItemFusion;

        public event PlayerItemMeltEventHandle ItemMelt;

        public event PlayerItemStrengthenEventHandle ItemStrengthen;

        public event PlayerEventHandle LevelUp;

        public event PlayerMissionOverEventHandle MissionOver;

        public event PlayerMissionTurnOverEventHandle MissionTurnOver;

        public event PlayerShopEventHandle Paid;

        public event PlayerEventHandle UseBuffer;

        public GamePlayer(int playerId, string account, GameClient client, PlayerInfo info)
        {
            this.m_playerId = playerId;
            this.m_account = account;
            this.m_client = client;
            this.m_character = info;
            this.LastChatTime = DateTime.Today;
            this.m_mainBag = new PlayerEquipInventory(this);
            this.m_propBag = new PlayerInventory(this, true, 0x31, 1, 0, true);
            this.m_storeBag = new PlayerInventory(this, true, 100, 11, 0, true);
            this.m_storeBag2 = new PlayerInventory(this, true, 20, 12, 0, true);
            this.m_fightBag = new PlayerInventory(this, false, 3, 3, 0, false);
            this.m_tempBag = new PlayerInventory(this, false, 60, 4, 0, true);
            this.m_caddyBag = new PlayerInventory(this, false, 20, 5, 0, true);
            this.m_cardBag = new PlayerInventory(this, true, 20, 15, 0, true);
            this.m_questInventory = new Game.Server.Quests.QuestInventory(this);
            this.m_bufferList = new Game.Server.Buffer.BufferList(this);
            this.m_equipEffect = new List<int>();
            this.GPAddPlus = 1.0;
            this.X = 0x286;
            this.Y = 0x4d9;
            this.MarryMap = 0;
            this.m_converter = new UTF8Encoding();
        }

        public int AddGiftToken(int value)
        {
            if (value > 0)
            {
                this.m_character.GiftToken += value;
                this.OnPropertiesChanged();
                return value;
            }
            return 0;
        }

        public int AddGold(int value)
        {
            if (value > 0)
            {
                this.m_character.Gold += value;
                this.OnPropertiesChanged();
                return value;
            }
            return 0;
        }

        public int AddGP(int gp)
        {
            if (gp >= 0)
            {
                if (AntiAddictionMgr.ISASSon)
                {
                    gp = (int)(gp * AntiAddictionMgr.GetAntiAddictionCoefficient(this.PlayerCharacter.AntiAddiction));
                }
                gp = (int)(gp * RateMgr.GetRate(eRateType.Experience_Rate));
                if (this.GPAddPlus > 0.0)
                {
                    gp = (int)(gp * this.GPAddPlus);
                }
                this.m_character.GP += gp;
                if (this.m_character.GP < 1)
                {
                    this.m_character.GP = 1;
                }
                this.Level = LevelMgr.GetLevel(this.m_character.GP);
                this.UpdateFightPower();
                this.OnPropertiesChanged();
                return gp;
            }
            return 0;
        }

        public bool AddItem(SqlDataProvider.Data.ItemInfo item)
        {
            AbstractInventory itemInventory = this.GetItemInventory(item.Template);
            return itemInventory.AddItem(item, itemInventory.BeginSlot);
        }

        public int AddMoney(int value)
        {
            if (value > 0)
            {
                this.m_character.Money += value;
                this.OnPropertiesChanged();
                return value;
            }
            return 0;
        }

        public int AddOffer(int value) =>
            this.AddOffer(value, true);

        public int AddOffer(int value, bool IsRate)
        {
            if (value > 0)
            {
                if (AntiAddictionMgr.ISASSon)
                {
                    value = (int)(value * AntiAddictionMgr.GetAntiAddictionCoefficient(this.PlayerCharacter.AntiAddiction));
                }
                if (IsRate)
                {
                    value *= (((int)this.OfferAddPlus) == 0) ? 1 : ((int)this.OfferAddPlus);
                }
                this.m_character.Offer += value;
                this.OnPropertiesChanged();
                return value;
            }
            return 0;
        }

        public void AddProperty(SqlDataProvider.Data.ItemInfo item, ref int defence)
        {
            if (item.Hole1 > 0)
            {
                this.BaseDefence(item.Hole1, ref defence);
            }
            if (item.Hole2 > 0)
            {
                this.BaseDefence(item.Hole2, ref defence);
            }
            if (item.Hole3 > 0)
            {
                this.BaseDefence(item.Hole3, ref defence);
            }
            if (item.Hole4 > 0)
            {
                this.BaseDefence(item.Hole4, ref defence);
            }
            if (item.Hole5 > 0)
            {
                this.BaseDefence(item.Hole5, ref defence);
            }
            if (item.Hole6 > 0)
            {
                this.BaseDefence(item.Hole6, ref defence);
            }
        }

        public int AddRichesOffer(int value)
        {
            if (value > 0)
            {
                this.m_character.RichesOffer += value;
                this.OnPropertiesChanged();
                return value;
            }
            return 0;
        }

        public int AddRobRiches(int value)
        {
            if (value > 0)
            {
                if (AntiAddictionMgr.ISASSon)
                {
                    value = (int)(value * AntiAddictionMgr.GetAntiAddictionCoefficient(this.PlayerCharacter.AntiAddiction));
                }
                this.m_character.RichesRob += value;
                this.OnPropertiesChanged();
                return value;
            }
            return 0;
        }

        public bool AddTemplate(SqlDataProvider.Data.ItemInfo cloneItem, eBageType bagType, int count)
        {
            PlayerInventory inventory = this.GetInventory(bagType);
            if ((inventory != null) && inventory.AddTemplate(cloneItem, count))
            {
                if ((this.CurrentRoom != null) && this.CurrentRoom.IsPlaying)
                {
                    this.SendItemNotice(cloneItem);
                }
                return true;
            }
            return false;
        }

        public void ApertureEquip(int level)
        {
            this.EquipShowImp(0, (level < 5) ? 1 : ((level < 7) ? 2 : 3));
        }

        public void BaseAttack(int template, ref int baseattack)
        {
            ItemTemplateInfo info = ItemMgr.FindItemTemplate(template);
            if ((info != null) && (((info.CategoryID == 11) && (info.Property1 == 0x1f)) && (info.Property2 == 3)))
            {
                baseattack += info.Property7;
            }
        }

        public void BaseDefence(int template, ref int defence)
        {
            ItemTemplateInfo info = ItemMgr.FindItemTemplate(template);
            if ((info != null) && (((info.CategoryID == 11) && (info.Property1 == 0x1f)) && (info.Property2 == 3)))
            {
                defence += info.Property8;
            }
        }

        public void BeginAllChanges()
        {
            this.BeginChanges();
            this.m_bufferList.BeginChanges();
            this.m_mainBag.BeginChanges();
            this.m_propBag.BeginChanges();
        }

        public void BeginChanges()
        {
            Interlocked.Increment(ref this.m_changed);
        }

        public bool CanEquip(ItemTemplateInfo item)
        {
            bool flag = true;
            string message = "";
            if (!item.CanEquip)
            {
                flag = false;
                message = LanguageMgr.GetTranslation("Game.Server.GameObjects.NoEquip", new object[0]);
            }
            else if ((item.NeedSex != 0) && (item.NeedSex != (this.m_character.Sex ? 1 : 2)))
            {
                flag = false;
                message = LanguageMgr.GetTranslation("Game.Server.GameObjects.CanEquip", new object[0]);
            }
            else if (this.m_character.Grade < item.NeedLevel)
            {
                flag = false;
                message = LanguageMgr.GetTranslation("Game.Server.GameObjects.CanLevel", new object[0]);
            }
            if (!flag)
            {
                this.Out.SendMessage(eMessageType.ERROR, message);
            }
            return flag;
        }

        public void ChargeToUser()
        {
            using (PlayerBussiness bussiness = new PlayerBussiness())
            {
                int money = 0;
                bussiness.ChargeToUser(this.m_character.UserName, ref money, this.m_character.NickName);
                this.AddMoney(money);
                LogMgr.LogMoneyAdd(LogMoneyType.Charge, LogMoneyType.Charge_RMB, this.m_character.ID, money, this.m_character.Money, 0, 0, 0, "", "", "");
            }
        }

        public void ClearConsortia()
        {
            this.PlayerCharacter.ClearConsortia();
            this.OnPropertiesChanged();
            this.QuestInventory.ClearConsortiaQuest();
            string translation = LanguageMgr.GetTranslation("Game.Server.GameUtils.CommonBag.Sender", new object[0]);
            string title = LanguageMgr.GetTranslation("Game.Server.GameUtils.Title", new object[0]);
            this.StoreBag.SendAllItemsToMail(translation, title, eMailType.StoreCanel);
        }

        public bool ClearFightBag()
        {
            this.FightBag.ClearBag();
            return true;
        }

        public bool ClearTempBag()
        {
            this.TempBag.ClearBag();
            return true;
        }

        public void CommitAllChanges()
        {
            this.CommitChanges();
            this.m_bufferList.CommitChanges();
            this.m_mainBag.CommitChanges();
            this.m_propBag.CommitChanges();
        }

        public void CommitChanges()
        {
            Interlocked.Decrement(ref this.m_changed);
            this.OnPropertiesChanged();
        }

        public int ConsortiaFight(int consortiaWin, int consortiaLose, Dictionary<int, Player> players, eRoomType roomType, eGameType gameClass, int totalKillHealth, int count) =>
            ConsortiaMgr.ConsortiaFight(consortiaWin, consortiaLose, players, roomType, gameClass, totalKillHealth, count);

        public void Disconnect()
        {
            this.m_client.Disconnect();
        }

        private void EquipShowImp(int categoryID, int para)
        {
            this.UpdateHide(this.m_character.Hide + ((int)(Math.Pow(10.0, (double)categoryID) * (para - ((this.m_character.Hide / ((int)Math.Pow(10.0, (double)categoryID))) % 10)))));
        }

        public void FriendsAdd(int playerID, int relation)
        {
            if (!this._friends.ContainsKey(playerID))
            {
                this._friends.Add(playerID, relation);
            }
            else
            {
                this._friends[playerID] = relation;
            }
        }

        public void FriendsRemove(int playerID)
        {
            if (this._friends.ContainsKey(playerID))
            {
                this._friends.Remove(playerID);
            }
        }

        public double GetBaseAgility() =>
            (1.0 - (this.m_character.Agility * 0.001));

        public double GetBaseAttack()
        {
            int baseattack = 0;
            SqlDataProvider.Data.ItemInfo itemAt = this.m_mainBag.GetItemAt(6);
            if (itemAt != null)
            {
                if (itemAt.Hole1 > 0)
                {
                    this.BaseAttack(itemAt.Hole1, ref baseattack);
                }
                if (itemAt.Hole2 > 0)
                {
                    this.BaseAttack(itemAt.Hole2, ref baseattack);
                }
                if (itemAt.Hole3 > 0)
                {
                    this.BaseAttack(itemAt.Hole3, ref baseattack);
                }
                if (itemAt.Hole4 > 0)
                {
                    this.BaseAttack(itemAt.Hole4, ref baseattack);
                }
                if (itemAt.Hole5 > 0)
                {
                    this.BaseAttack(itemAt.Hole5, ref baseattack);
                }
                if (itemAt.Hole6 > 0)
                {
                    this.BaseAttack(itemAt.Hole6, ref baseattack);
                }
                return ((itemAt.Template.Property7 * Math.Pow(1.1, (double)itemAt.StrengthenLevel)) + baseattack);
            }
            return 50.0;
        }

        public double GetBaseBlood()
        {
            SqlDataProvider.Data.ItemInfo itemAt = this.MainBag.GetItemAt(12);
            return ((itemAt == null) ? 1.0 : ((100.0 + itemAt.Template.Property1) / 100.0));
        }

        public double GetBaseDefence()
        {
            int defence = 0;
            SqlDataProvider.Data.ItemInfo itemAt = this.m_mainBag.GetItemAt(0);
            SqlDataProvider.Data.ItemInfo item = this.m_mainBag.GetItemAt(4);
            if (itemAt != null)
            {
                this.AddProperty(itemAt, ref defence);
                defence = itemAt.Template.Property7 * ((int)Math.Pow(1.1, (double)itemAt.StrengthenLevel));
            }
            if (item != null)
            {
                this.AddProperty(item, ref defence);
                defence += item.Template.Property7 * ((int)Math.Pow(1.1, (double)item.StrengthenLevel));
            }
            return (double)defence;
        }

        public PlayerInventory GetInventory(eBageType bageType)
        {
            switch ((int)bageType)
            {
                case 0:
                    return this.m_mainBag;

                case 1:
                    return this.m_propBag;

                case 3:
                    return this.m_fightBag;

                case 4:
                    return this.m_tempBag;

                case 5:
                    return this.m_caddyBag;

                case 11:
                    return this.m_storeBag;

                case 12:
                    return this.m_storeBag2;
            }
            throw new NotSupportedException($"Did not support this type bag: {bageType}");
        }

        public string GetInventoryName(eBageType bageType)
        {
            switch ((int)bageType)
            {
                case 0:
                    return LanguageMgr.GetTranslation("Game.Server.GameObjects.Equip", new object[0]);

                case 1:
                    return LanguageMgr.GetTranslation("Game.Server.GameObjects.Prop", new object[0]);
            }
            return bageType.ToString();
        }

        public SqlDataProvider.Data.ItemInfo GetItemAt(eBageType bagType, int place)
        {
            PlayerInventory inventory = this.GetInventory(bagType);
            if (inventory != null)
            {
                return inventory.GetItemAt(place);
            }
            return null;
        }

        public int GetItemCount(int templateId) =>
            ((this.m_propBag.GetItemCount(templateId) + this.m_mainBag.GetItemCount(templateId)) + this.m_storeBag.GetItemCount(templateId));

        public PlayerInventory GetItemInventory(ItemTemplateInfo template) =>
            this.GetInventory(template.BagType);

        public void HideEquip(int categoryID, bool hide)
        {
            if ((categoryID >= 0) && (categoryID < 10))
            {
                this.EquipShowImp(categoryID, hide ? 2 : 1);
            }
        }

        public byte[] InitPvePermission()
        {
            byte[] buffer = new byte[50];
            for (int i = 0; i < 50; i++)
            {
                buffer[i] = 0x11;
            }
            return buffer;
        }

        public bool IsBlackFriend(int playerID) =>
            ((this._friends == null) || (this._friends.ContainsKey(playerID) && (this._friends[playerID] == 1)));

        public bool IsPvePermission(int missionId, eHardLevel hardLevel)
        {
            string str2;
            if (hardLevel == eHardLevel.Simple)
            {
                return true;
            }
            string str = this.m_converter.GetString(this.m_pvepermissions).Substring(missionId - 1, 1);
            if (hardLevel == eHardLevel.Normal)
            {
                str2 = str;
                if ((str2 != null) && (((str2 == "3") || (str2 == "7")) || (str2 == "F")))
                {
                    return true;
                }
            }
            else if (hardLevel == eHardLevel.Hard)
            {
                str2 = str;
                if ((str2 != null) && ((str2 == "7") || (str2 == "F")))
                {
                    return true;
                }
            }
            else if ((hardLevel == eHardLevel.Terror) && (str == "F"))
            {
                return true;
            }
            return false;
        }

        public bool LoadFromDatabase()
        {
            using (PlayerBussiness bussiness = new PlayerBussiness())
            {
                PlayerInfo userSingleByUserID = bussiness.GetUserSingleByUserID(this.m_character.ID);
                if (userSingleByUserID == null)
                {
                    this.Out.SendKitoff(LanguageMgr.GetTranslation("UserLoginHandler.Forbid", new object[0]));
                    this.Client.Disconnect();
                    return false;
                }
                this.m_character = userSingleByUserID;
                this.ChargeToUser();
                int[] numArray2 = new int[3];
                numArray2[1] = 1;
                numArray2[2] = 2;
                int[] updatedSlots = numArray2;
                this.Out.SendUpdateInventorySlot(this.FightBag, updatedSlots);
                this.AddGP(0);
                this.UpdateWeapon(this.m_mainBag.GetItemAt(6));
                this.UpdateSecondWeapon(this.m_mainBag.GetItemAt(15));
                this.m_pvepermissions = string.IsNullOrEmpty(userSingleByUserID.PvePermission) ? this.InitPvePermission() : this.m_converter.GetBytes(userSingleByUserID.PvePermission);
                this._friends = new Dictionary<int, int>();
                this._friends = bussiness.GetFriendsIDAll(this.m_character.ID);
                this.m_character.State = 1;
                bussiness.UpdatePlayer(this.m_character);
                return true;
            }
        }

        public void LoadMarryMessage()
        {
            using (PlayerBussiness bussiness = new PlayerBussiness())
            {
                MarryApplyInfo[] playerMarryApply = bussiness.GetPlayerMarryApply(this.PlayerCharacter.ID);
                if (playerMarryApply != null)
                {
                    foreach (MarryApplyInfo info in playerMarryApply)
                    {
                        switch (info.ApplyType)
                        {
                            case 1:
                                this.Out.SendPlayerMarryApply(this, info.ApplyUserID, info.ApplyUserName, info.LoveProclamation, info.ID);
                                break;

                            case 2:
                                this.Out.SendMarryApplyReply(this, info.ApplyUserID, info.ApplyUserName, info.ApplyResult, true, info.ID);
                                if (!info.ApplyResult)
                                {
                                    this.Out.SendMailResponse(this.PlayerCharacter.ID, eMailRespose.Receiver);
                                }
                                break;

                            case 3:
                                this.Out.SendPlayerDivorceApply(this, true, false);
                                break;
                        }
                    }
                }
            }
        }

        public void LoadMarryProp()
        {
            using (PlayerBussiness bussiness = new PlayerBussiness())
            {
                MarryProp marryProp = bussiness.GetMarryProp(this.PlayerCharacter.ID);
                this.PlayerCharacter.IsMarried = marryProp.IsMarried;
                this.PlayerCharacter.SpouseID = marryProp.SpouseID;
                this.PlayerCharacter.SpouseName = marryProp.SpouseName;
                this.PlayerCharacter.IsCreatedMarryRoom = marryProp.IsCreatedMarryRoom;
                this.PlayerCharacter.SelfMarryRoomID = marryProp.SelfMarryRoomID;
                this.PlayerCharacter.IsGotRing = marryProp.IsGotRing;
                this.Out.SendMarryProp(this, marryProp);
            }
        }

        public void LogAddMoney(AddMoneyType masterType, AddMoneyType sonType, int userId, int moneys, int SpareMoney)
        {
            LogMgr.LogMoneyAdd((LogMoneyType)masterType, (LogMoneyType)sonType, userId, moneys, SpareMoney, 0, 0, 0, "", "", "");
        }

        public bool Login()
        {
            if (WorldMgr.AddPlayer(this.m_character.ID, this))
            {
                try
                {
                    if (this.LoadFromDatabase())
                    {
                        this.Out.SendLoginSuccess();
                        this.Out.SendDateTime();
                        this.UpdateItemForUser(1);
                        this.Out.SendCheckCode();
                        AntiAddictionMgr.AASStateGet(this);
                        this.Out.SendDailyAward(this);
                        this.LoadMarryMessage();
                        return true;
                    }
                    WorldMgr.RemovePlayer(this.m_character.ID);
                }
                catch (Exception exception)
                {
                    log.Error("Error Login!", exception);
                }
            }
            return false;
        }

        public void OnGameOver(AbstractGame game, bool isWin, int gainXp)
        {
            if (game.RoomType == eRoomType.Match)
            {
                if (isWin)
                {
                    this.m_character.Win++;
                }
                this.m_character.Total++;
            }
            if (this.GameOver != null)
            {
                this.GameOver(game, isWin, gainXp);
            }
        }

        public void OnGuildChanged()
        {
            if (this.GuildChanged != null)
            {
                this.GuildChanged();
            }
        }

        public void OnItemCompose(int composeType)
        {
            if (this.ItemCompose != null)
            {
                this.ItemCompose(composeType);
            }
        }

        public void OnItemFusion(int fusionType)
        {
            if (this.ItemFusion != null)
            {
                this.ItemFusion(fusionType);
            }
        }

        public void OnItemMelt(int categoryID)
        {
            if (this.ItemMelt != null)
            {
                this.ItemMelt(categoryID);
            }
        }

        public void OnItemStrengthen(int categoryID, int level)
        {
            if (this.ItemStrengthen != null)
            {
                this.ItemStrengthen(categoryID, level);
            }
        }

        public void OnKillingLiving(AbstractGame game, int type, int id, bool isLiving, int damage)
        {
            if (this.AfterKillingLiving != null)
            {
                this.AfterKillingLiving(game, type, id, isLiving, damage);
            }
            if (!((this.GameKillDrop == null) || isLiving))
            {
                this.GameKillDrop(game, type, id, isLiving);
            }
        }

        public void OnLevelUp(int grade)
        {
            if (this.LevelUp != null)
            {
                this.LevelUp(this);
            }
        }

        public void OnMissionOver(AbstractGame game, bool isWin, int missionId, int turnNum)
        {
            if (this.MissionOver != null)
            {
                this.MissionOver(game, missionId, isWin);
            }
            if (this.MissionTurnOver != null)
            {
                this.MissionTurnOver(game, missionId, turnNum);
            }
        }

        public void OnPaid(int money, int gold, int offer, int gifttoken, string payGoods)
        {
            if (this.Paid != null)
            {
                this.Paid(money, gold, offer, gifttoken, payGoods);
            }
        }

        protected void OnPropertiesChanged()
        {
            if (this.m_changed <= 0)
            {
                if (this.m_changed < 0)
                {
                    log.Error("Player changed count < 0");
                    Thread.VolatileWrite(ref this.m_changed, 0);
                }
                this.UpdateProperties();
            }
        }

        public void OnUseBuffer()
        {
            if (this.UseBuffer != null)
            {
                this.UseBuffer(this);
            }
        }

        public void OnUsingItem(int templateID)
        {
            if (this.AfterUsingItem != null)
            {
                this.AfterUsingItem(templateID);
            }
        }

        public virtual bool Quit()
        {
            Exception exception;
            try
            {
                try
                {
                    if (this.CurrentRoom != null)
                    {
                        this.CurrentRoom.RemovePlayerUnsafe(this);
                        this.CurrentRoom = null;
                    }
                    else
                    {
                        RoomMgr.WaitingRoom.RemovePlayer(this);
                    }
                    if (this._currentMarryRoom != null)
                    {
                        this._currentMarryRoom.RemovePlayer(this);
                        this._currentMarryRoom = null;
                    }
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                    log.Error("Player exit Game Error!", exception);
                }
                this.m_character.State = 0;
                this.SaveIntoDatabase();
            }
            catch (Exception exception3)
            {
                exception = exception3;
                log.Error("Player exit Error!!!", exception);
            }
            finally
            {
                WorldMgr.RemovePlayer(this.m_character.ID);
            }
            return true;
        }

        public bool RemoveAt(eBageType bagType, int place)
        {
            PlayerInventory inventory = this.GetInventory(bagType);
            return ((inventory != null) && inventory.RemoveItemAt(place));
        }

        public int RemoveGiftToken(int value)
        {
            if ((value > 0) && (value <= this.m_character.GiftToken))
            {
                this.m_character.GiftToken -= value;
                this.OnPropertiesChanged();
                return value;
            }
            return 0;
        }

        public int RemoveGold(int value)
        {
            if ((value > 0) && (value <= this.m_character.Gold))
            {
                this.m_character.Gold -= value;
                this.OnPropertiesChanged();
                return value;
            }
            return 0;
        }

        public int RemoveGP(int gp)
        {
            if (gp > 0)
            {
                this.m_character.GP -= gp;
                if (this.m_character.GP < 1)
                {
                    this.m_character.GP = 1;
                }
                this.Level = LevelMgr.GetLevel(this.m_character.GP);
                this.OnPropertiesChanged();
                return gp;
            }
            return 0;
        }

        public bool RemoveItem(SqlDataProvider.Data.ItemInfo item)
        {
            if (item.BagType == this.m_propBag.BagType)
            {
                return this.m_propBag.RemoveItem(item);
            }
            if (item.BagType == this.m_fightBag.BagType)
            {
                return this.m_fightBag.RemoveItem(item);
            }
            return this.m_mainBag.RemoveItem(item);
        }

        public int RemoveMoney(int value)
        {
            if ((value > 0) && (value <= this.m_character.Money))
            {
                this.m_character.Money -= value;
                this.OnPropertiesChanged();
                return value;
            }
            return 0;
        }

        public int RemoveOffer(int value)
        {
            if (value > 0)
            {
                if (value >= this.m_character.Offer)
                {
                    value = this.m_character.Offer;
                }
                this.m_character.Offer -= value;
                this.OnPropertiesChanged();
                return value;
            }
            return 0;
        }

        public bool RemoveTempate(eBageType bagType, ItemTemplateInfo template, int count)
        {
            PlayerInventory inventory = this.GetInventory(bagType);
            return ((inventory != null) && inventory.RemoveTemplate(template.TemplateID, count));
        }

        public bool RemoveTemplate(ItemTemplateInfo template, int count)
        {
            PlayerInventory itemInventory = this.GetItemInventory(template);
            return ((itemInventory != null) && itemInventory.RemoveTemplate(template.TemplateID, count));
        }

        public bool RemoveTemplate(int templateId, int count)
        {
            int itemCount = this.m_mainBag.GetItemCount(templateId);
            int num2 = this.m_propBag.GetItemCount(templateId);
            int num3 = this.m_storeBag.GetItemCount(templateId);
            int num4 = (itemCount + num2) + num3;
            ItemTemplateInfo template = ItemMgr.FindItemTemplate(templateId);
            if ((template != null) && (num4 >= count))
            {
                if (((itemCount > 0) && (count > 0)) && this.RemoveTempate(eBageType.MainBag, template, (itemCount > count) ? count : itemCount))
                {
                    count = (count < itemCount) ? 0 : (count - itemCount);
                }
                if (((num2 > 0) && (count > 0)) && this.RemoveTempate(eBageType.PropBag, template, (num2 > count) ? count : num2))
                {
                    count = (count < num2) ? 0 : (count - num2);
                }
                if (((num3 > 0) && (count > 0)) && this.RemoveTempate(eBageType.Bank, template, (num3 > count) ? count : num3))
                {
                    count = (count < num3) ? 0 : (count - num3);
                }
                if (count == 0)
                {
                    return true;
                }
                if (log.IsErrorEnabled)
                {
                    log.Error($"Item Remover Error：PlayerId {this.m_playerId} Remover TemplateId{templateId} Is Not Zero!");
                }
            }
            return false;
        }

        public bool SaveIntoDatabase()
        {
            try
            {
                if (this.m_character.IsDirty)
                {
                    using (PlayerBussiness bussiness = new PlayerBussiness())
                    {
                        bussiness.UpdatePlayer(this.m_character);
                    }
                }
                this.MainBag.SaveToDatabase();
                this.PropBag.SaveToDatabase();
                this.StoreBag.SaveToDatabase();
                this.QuestInventory.SaveToDatabase();
                this.BufferList.SaveToDatabase();
                return true;
            }
            catch (Exception exception)
            {
                log.Error("Error saving player " + this.m_character.NickName + "!", exception);
                return false;
            }
        }

        public void SendConsortiaFight(int consortiaID, int riches, string msg)
        {
            GSPacketIn packet = new GSPacketIn(0x9e);
            packet.WriteInt(consortiaID);
            packet.WriteInt(riches);
            packet.WriteString(msg);
            GameServer.Instance.LoginServer.SendPacket(packet);
        }

        public void SendInsufficientMoney(int type)
        {
            GSPacketIn pkg = new GSPacketIn(0x58, this.PlayerId);
            pkg.WriteByte((byte)type);
            pkg.WriteBoolean(false);
            this.SendTCP(pkg);
        }

        private void SendItemNotice(SqlDataProvider.Data.ItemInfo info)
        {
            GSPacketIn pkg = new GSPacketIn(14);
            pkg.WriteString(this.PlayerCharacter.NickName);
            pkg.WriteInt(1);
            pkg.WriteInt(info.TemplateID);
            pkg.WriteBoolean(info.IsBinds);
            pkg.WriteInt(1);
            if ((info.Template.Quality >= 3) && (info.Template.Quality < 5))
            {
                if (this.CurrentRoom != null)
                {
                    this.CurrentRoom.SendToTeam(pkg, this.CurrentRoomTeam, this);
                    Console.WriteLine(">=3 && <5 " + info.Template.Name);
                }
            }
            else if (info.Template.Quality >= 5)
            {
                GameServer.Instance.LoginServer.SendPacket(pkg);
                GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
                foreach (GamePlayer player in allPlayers)
                {
                    if (player != this)
                    {
                        player.Out.SendTCP(pkg);
                        Console.WriteLine(">=5 " + info.Template.Name);
                    }
                }
            }
        }

        public void SendMessage(string msg)
        {
            GSPacketIn pkg = new GSPacketIn(3);
            pkg.WriteInt(0);
            pkg.WriteString(msg);
            this.SendTCP(pkg);
        }

        public void SendTCP(GSPacketIn pkg)
        {
            if (this.m_client.IsConnected)
            {
                this.m_client.SendTCP(pkg);
            }
        }

        public bool SetPvePermission(int missionId, eHardLevel hardLevel)
        {
            string str;
            string str2;
            if (missionId <= (this.m_pvepermissions.Length * 2))
            {
                if (hardLevel == eHardLevel.Terror)
                {
                    return true;
                }
                if (!this.IsPvePermission(missionId, hardLevel))
                {
                    return false;
                }
                str = string.Empty;
                string str3 = this.m_converter.GetString(this.m_pvepermissions).Substring(missionId - 1, 1);
                if ((hardLevel == eHardLevel.Simple) && (str3 == "1"))
                {
                    str = "3";
                    goto Label_00D7;
                }
                if ((hardLevel == eHardLevel.Normal) && (str3 == "3"))
                {
                    str = "7";
                    goto Label_00D7;
                }
                if ((hardLevel == eHardLevel.Hard) && (str3 == "7"))
                {
                    str = "F";
                    goto Label_00D7;
                }
            }
            return false;
            Label_00D7:
            str2 = this.m_converter.GetString(this.m_pvepermissions);
            int length = str2.Length;
            str2 = str2.Substring(0, missionId - 1) + str + str2.Substring(missionId, length - missionId);
            this.m_character.PvePermission = str2;
            this.OnPropertiesChanged();
            return true;
        }

        public override string ToString() =>
            $"Id:{this.PlayerId} nickname:{this.PlayerCharacter.NickName} room:{this.CurrentRoom} ";

        public void UpdateBaseProperties(int attack, int defence, int agility, int lucky)
        {
            if ((((attack != this.m_character.Attack) || (defence != this.m_character.Defence)) || (agility != this.m_character.Agility)) || (lucky != this.m_character.Luck))
            {
                this.m_character.Attack = attack;
                this.m_character.Defence = defence;
                this.m_character.Agility = agility;
                this.m_character.Luck = lucky;
                this.OnPropertiesChanged();
            }
        }

        public void UpdateFightPower()
        {
            int num = 0;
            this.FightPower = 0;
            int num2 = 0;
            num2 = (int)((((950 + (this.PlayerCharacter.Grade * 50)) + this.LevelPlusBlood) + (this.PlayerCharacter.Defence / 10)) * this.GetBaseBlood());
            num += this.PlayerCharacter.Attack;
            num += this.PlayerCharacter.Defence;
            num += this.PlayerCharacter.Agility;
            num += this.PlayerCharacter.Luck;
            this.FightPower += (int)(((((num + 0x3e8) * (((this.GetBaseAttack() * this.GetBaseAttack()) * this.GetBaseAttack()) + (((3.5 * this.GetBaseDefence()) * this.GetBaseDefence()) * this.GetBaseDefence()))) / 100000000.0) + (num2 * 0.95)) - 950.0);
            this.PlayerCharacter.FightPower = this.FightPower;
        }

        public void UpdateHide(int hide)
        {
            if (hide != this.m_character.Hide)
            {
                this.m_character.Hide = hide;
                this.OnPropertiesChanged();
            }
        }

        public void UpdateItem(SqlDataProvider.Data.ItemInfo item)
        {
            this.m_mainBag.UpdateItem(item);
            this.m_propBag.UpdateItem(item);
        }

        public void UpdateItemForUser(object state)
        {
            this.m_mainBag.LoadFromDatabase();
            this.m_propBag.LoadFromDatabase();
            this.m_storeBag.LoadFromDatabase();
            this.m_cardBag.LoadFromDatabase();
            this.m_questInventory.LoadFromDatabase(this.m_character.ID);
            this.m_bufferList.LoadFromDatabase(this.m_character.ID);
        }

        public void UpdateProperties()
        {
            this.Out.SendUpdatePrivateInfo(this.m_character);
            GSPacketIn pkg = this.Out.SendUpdatePublicPlayer(this.m_character);
            if (this.m_currentRoom != null)
            {
                this.m_currentRoom.SendToAll(pkg, this);
            }
        }

        public void UpdateProperty()
        {
            this.OnPropertiesChanged();
        }

        public void UpdateSecondWeapon(SqlDataProvider.Data.ItemInfo item)
        {
            if (item != this.m_currentSecondWeapon)
            {
                this.m_currentSecondWeapon = item;
                this.OnPropertiesChanged();
            }
        }

        public void UpdateStyle(string style, string colors, string skin)
        {
            if (((style != this.m_character.Style) || (colors != this.m_character.Colors)) || (skin != this.m_character.Skin))
            {
                this.m_character.Style = style;
                this.m_character.Colors = colors;
                this.m_character.Skin = skin;
                this.OnPropertiesChanged();
            }
        }

        public void UpdateWeapon(SqlDataProvider.Data.ItemInfo item)
        {
            if (item != this.m_MainWeapon)
            {
                this.m_MainWeapon = item;
                this.OnPropertiesChanged();
            }
        }

        public bool UsePropItem(AbstractGame game, int bag, int place, int templateId, bool isLiving)
        {
            SqlDataProvider.Data.ItemInfo itemAt;
            if (bag == 1)
            {
                ItemTemplateInfo info2 = PropItemMgr.FindFightingProp(templateId);
                if (isLiving && (info2 != null))
                {
                    this.OnUsingItem(info2.TemplateID);
                    if ((place == -1) && this.CanUseProp)
                    {
                        return true;
                    }
                    itemAt = this.GetItemAt(eBageType.PropBag, place);
                    if (((itemAt != null) && itemAt.IsValidItem()) && (itemAt.Count >= 0))
                    {
                        itemAt.Count--;
                        this.UpdateItem(itemAt);
                        return true;
                    }
                }
            }
            else
            {
                itemAt = this.GetItemAt(eBageType.FightBag, place);
                if (itemAt.TemplateID == templateId)
                {
                    this.OnUsingItem(itemAt.TemplateID);
                    return this.RemoveAt(eBageType.FightBag, place);
                }
            }
            return false;
        }

        public HotSpringRoom CurrentHotSpringRoom
        {
            get =>
                this._currentHotRoom;
            set =>
                this._currentHotRoom = value;
        }

        public string Account =>
            this.m_account;

        public Game.Server.Buffer.BufferList BufferList =>
            this.m_bufferList;

        public PlayerInventory CaddyBag =>
            this.m_caddyBag;

        public bool CanUseProp { get; set; }

        public PlayerInventory CardBag =>
            this.m_cardBag;

        public GameClient Client =>
            this.m_client;

        public MarryRoom CurrentMarryRoom
        {
            get =>
                this._currentMarryRoom;
            set =>
                this._currentMarryRoom = value;
        }

        public BaseRoom CurrentRoom
        {
            get =>
                this.m_currentRoom;
            set
            {
                BaseRoom room = Interlocked.Exchange<BaseRoom>(ref this.m_currentRoom, value);
                if (room != null)
                {
                    RoomMgr.ExitRoom(room, this);
                }
            }
        }

        public List<int> EquipEffect
        {
            get =>
                this.m_equipEffect;
            set =>
                this.m_equipEffect = value;
        }

        public PlayerInventory FightBag =>
            this.m_fightBag;

        public Dictionary<int, int> Friends =>
            this._friends;

        public int GamePlayerId { get; set; }

        public int Immunity
        {
            get =>
                this.m_immunity;
            set =>
                this.m_immunity = value;
        }

        public bool IsAASInfo
        {
            get =>
                this.m_isAASInfo;
            set =>
                this.m_isAASInfo = value;
        }

        public bool IsActive =>
            this.m_client.IsConnected;

        public bool IsInMarryRoom =>
            (this._currentMarryRoom != null);

        public bool IsMinor
        {
            get =>
                this.m_isMinor;
            set =>
                this.m_isMinor = value;
        }

        public int Level
        {
            get =>
                this.m_character.Grade;
            set
            {
                if (value != this.m_character.Grade)
                {
                    this.m_character.Grade = value;
                    this.OnLevelUp(value);
                    this.OnPropertiesChanged();
                }
            }
        }

        public int LevelPlusBlood
        {
            get
            {
                int num = 0;
                for (int i = 10; i <= 60; i += 10)
                {
                    if ((this.PlayerCharacter.Grade - i) > 0)
                    {
                        num += (this.PlayerCharacter.Grade - i) * (i + 20);
                    }
                }
                return num;
            }
        }

        public PlayerEquipInventory MainBag =>
            this.m_mainBag;

        public ItemTemplateInfo MainWeapon =>
            this.m_MainWeapon?.Template;

        public IPacketLib Out =>
            this.m_client.Out;

        public long PingTime
        {
            get =>
                this.m_pingTime;
            set
            {
                this.m_pingTime = value;
                GSPacketIn pkg = this.Out.SendNetWork(this.m_character.ID, this.m_pingTime);
                if (this.m_currentRoom != null)
                {
                    this.m_currentRoom.SendToAll(pkg, this);
                }
            }
        }

        public PlayerInfo PlayerCharacter =>
            this.m_character;

        public int PlayerId =>
            this.m_playerId;

        public PlayerInventory PropBag =>
            this.m_propBag;

        public Game.Server.Quests.QuestInventory QuestInventory =>
            this.m_questInventory;

        public SqlDataProvider.Data.ItemInfo SecondWeapon
        {
            get
            {
                if (this.m_currentSecondWeapon == null)
                {
                    return null;
                }
                return this.m_currentSecondWeapon;
            }
        }

        public int ServerID { get; set; }

        public PlayerInventory StoreBag =>
            this.m_storeBag;

        public PlayerInventory StoreBag2 =>
            this.m_storeBag2;

        public PlayerInventory TempBag =>
            this.m_tempBag;

        public bool IsInHotSpringRoom { get; set; }

        public delegate void GameKillDropEventHandel(AbstractGame game, int type, int npcId, bool playResult);

        public delegate void PlayerEventHandle(GamePlayer player);

        public delegate void PlayerGameKillEventHandel(AbstractGame game, int type, int id, bool isLiving, int demage);

        public delegate void PlayerGameOverEventHandle(AbstractGame game, bool isWin, int gainXp);

        public delegate void PlayerItemComposeEventHandle(int composeType);

        public delegate void PlayerItemFusionEventHandle(int fusionType);

        public delegate void PlayerItemMeltEventHandle(int categoryID);

        public delegate void PlayerItemPropertyEventHandle(int templateID);

        public delegate void PlayerItemStrengthenEventHandle(int categoryID, int level);

        public delegate void PlayerMissionOverEventHandle(AbstractGame game, int missionId, bool isWin);

        public delegate void PlayerMissionTurnOverEventHandle(AbstractGame game, int missionId, int turnNum);

        public delegate void PlayerOwnConsortiaEventHandle();

        public delegate void PlayerShopEventHandle(int money, int gold, int offer, int gifttoken, string payGoods);
    }
}

*/