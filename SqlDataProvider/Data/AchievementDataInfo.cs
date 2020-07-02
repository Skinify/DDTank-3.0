using System;

namespace SqlDataProvider.Data
{
    public class AchievementDataInfo : DataObject
    {
        private int _achievementID;
        private DateTime _completedDate;
        private bool _isComplete;
        private int _userID;

        public int AchievementID
        {
            get => 
                this._achievementID;
            set
            {
                this._achievementID = value;
                base._isDirty = true;
            }
        }

        public DateTime CompletedDate
        {
            get => 
                this._completedDate;
            set
            {
                this._completedDate = value;
                base._isDirty = true;
            }
        }

        public bool IsComplete
        {
            get => 
                this._isComplete;
            set
            {
                this._isComplete = value;
                base._isDirty = true;
            }
        }

        public int UserID
        {
            get => 
                this._userID;
            set
            {
                this._userID = value;
                base._isDirty = true;
            }
        }
    }
}

