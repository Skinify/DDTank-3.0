using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Server.HotSpringRooms.TankHandle
{
    public class HotSpringCommandAttribute:Attribute
    {
        public HotSpringCommandAttribute(byte code)
        {
            Code = code;
        }
        public byte Code
        {
            get;
            private set;
        }

    }
}
