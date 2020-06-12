using Game.Base.Packets;
using Game.Server.GameUtils;
using System.Collections.Generic;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.MOVE_GOODS_BAG, "物品比较")]
    public class MoveGoodsAllHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            bool flag = packet.ReadBoolean();
            int num = packet.ReadInt();
            int num2 = packet.ReadInt();
            PlayerInventory inventory = client.Player.GetInventory((eBageType)num2);
            int capalility = inventory.Capalility;
            List<ItemInfo> items = inventory.GetItems(inventory.BeginSlot, capalility);
            if (num == items.Count)
            {
                inventory.BeginChanges();
                try
                {
                    if (inventory.FindFirstEmptySlot(inventory.BeginSlot, capalility) != -1)
                    {
                        for (int i = 1; inventory.FindFirstEmptySlot(inventory.BeginSlot, capalility) < items[items.Count - i].Place; i++)
                        {
                            inventory.MoveItem(items[items.Count - i].Place, inventory.FindFirstEmptySlot(inventory.BeginSlot, capalility), items[items.Count - i].Count);
                        }
                    }
                }
                finally
                {
                    if (flag)
                    {
                        try
                        {
                            items = inventory.GetItems(inventory.BeginSlot, capalility);
                            List<int> list2 = new List<int>();
                            for (int j = 0; j < items.Count; j++)
                            {
                                if (!list2.Contains(j))
                                {
                                    for (int k = items.Count - 1; k > j; k--)
                                    {
                                        if ((!list2.Contains(k) && (items[j].TemplateID == items[k].TemplateID)) && items[j].CanStackedTo(items[k]))
                                        {
                                            inventory.MoveItem(items[k].Place, items[j].Place, items[k].Count);
                                            list2.Add(k);
                                        }
                                    }
                                }
                            }
                        }
                        finally
                        {
                            items = inventory.GetItems(inventory.BeginSlot, capalility);
                            if (inventory.FindFirstEmptySlot(inventory.BeginSlot, capalility) != -1)
                            {
                                for (int m = 1; inventory.FindFirstEmptySlot(inventory.BeginSlot, capalility) < items[items.Count - m].Place; m++)
                                {
                                    inventory.MoveItem(items[items.Count - m].Place, inventory.FindFirstEmptySlot(inventory.BeginSlot, capalility), items[items.Count - m].Count);
                                }
                            }
                        }
                    }
                    inventory.CommitChanges();
                }
            }
            return 0;
        }
    }
}

