using System.Linq;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Packets;
using SqlDataProvider.Data;
using Bussiness.Managers;
using Game.Server.Statics;
using Bussiness;

namespace Game.Server.HotSpringRooms.TankHandle
{
    [HotSpringCommand((byte)HotSpringCmdType.HOTSPRING_ROOM_TIME_UPDATE)]
    public class TimeUpdateCommand : IHotSpringCommandHandler
    {
        public bool HandleCommand(TankHotSpringLogicProcessor process, GamePlayer player, GSPacketIn packet)
        {
            if (player.CurrentHotSpringRoom != null/* && player.CurrentHotSpringRoom.RoomState == eRoomState.FREE*/)
            {
                int userID = packet.ReadInt();
                int templateID = packet.ReadInt();

                //ItemTemplateInfo template = ItemMgr.FindItemTemplate(templateID);
                ShopItemInfo temp = ShopMgr.FindShopbyTemplatID(templateID).FirstOrDefault();
                if (temp != null)
                {
                    
                    if(temp.APrice1 == -2)
                    {
                        if(player.PlayerCharacter.Gold >= temp.AValue1)
                        {
                            player.RemoveGold(temp.AValue1);
                            player.CurrentHotSpringRoom.ReturnPacketForScene(player, packet);
                            player.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("UserFirecrackersCommand.Successed1", temp.AValue1));
                            return true;
                        }
                        else
                        {
                            player.Out.SendMessage(eMessageType.ERROR, LanguageMgr.GetTranslation("UserFirecrackersCommand.GoldNotEnough"));
                        }
                    }
                    if (temp.APrice1 == -1)
                    {
                        if (player.PlayerCharacter.Money >= temp.AValue1)
                        {
                            player.RemoveMoney(temp.AValue1);
                            LogMgr.LogMoneyAdd(LogMoneyType.Marry, LogMoneyType.Marry_Flower, player.PlayerCharacter.ID, temp.AValue1, player.PlayerCharacter.Money, 0, 0, 0, "", "", "");
                            player.CurrentHotSpringRoom.ReturnPacketForScene(player, packet);
                            player.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("UserFirecrackersCommand.Successed2", temp.AValue1));
                            return true;
                        }
                        else
                        {
                            player.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("UserFirecrackersCommand.MoneyNotEnough"));
                        }
                    }
                }

            }
            return false;
        }
        
    }
}
