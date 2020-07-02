﻿using System;

namespace SqlDataProvider.Data
{
    public class AchievementProcessInfo : DataObject
    {
        private int int_0;
        private int int_1;

        public AchievementProcessInfo()
        {
        }

        public AchievementProcessInfo(int type, int value)
        {
            this.CondictionType = type;
            this.Value = value;
        }

        public int CondictionType
        {
            get => 
                this.int_0;
            set
            {
                this.int_0 = value;
                base._isDirty = true;
            }
        }

        public int Value
        {
            get => 
                this.int_1;
            set
            {
                this.int_1 = value;
                base._isDirty = true;
            }
        }
    }
}
