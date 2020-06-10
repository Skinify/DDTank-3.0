namespace GameServerScript.AI.Messions
{
    public class NpcCreateParam
    {
        private int _remoteCount;
        private int _livingCount;
        private int _turnCreateRemoteNum;
        private int _turnCreateLivingNum;
        private int _samePingMaxRemoteNum;
        private int _samePingMaxLivingNum;

        public NpcCreateParam(int remoteCount, int livingCount, int turnCreateRemoteNum, int turnCreateLivingNum, int samePingMaxRemoteNum, int samePingMaxLivingNum)
        {
            this.RemoteCount = remoteCount;
            this.LivingCount = livingCount;
            this.TurnCreateRemoteNum = turnCreateRemoteNum;
            this.TurnCreateLivingNum = turnCreateLivingNum;
            this.SamePingMaxRemoteNum = samePingMaxRemoteNum;
            this.SamePingMaxLivingNum = samePingMaxLivingNum;
        }

        public int RemoteCount
        {
            get { 
                return this._remoteCount;
            }
                
            set {
                this._remoteCount = value;
            }
        }

        public int LivingCount
        {
            get{
               return this._livingCount;
            }
            set
            {
                this._livingCount = value;
            }
                
        }

        public int TurnCreateRemoteNum
        {
            get
            {
                return this._turnCreateRemoteNum;
            }
            set
            {
                this._turnCreateRemoteNum = value;
            }
                
        }

        public int TurnCreateLivingNum
        {
            get
            {
                return this._turnCreateLivingNum;
            } 
            set
            {
                this._turnCreateLivingNum = value;
            }
                
        }

        public int SamePingMaxRemoteNum
        {
            get {
                return this._samePingMaxRemoteNum;
            }    
            set {
                this._samePingMaxRemoteNum = value;
            }   
        }

        public int SamePingMaxLivingNum
        {
            get
            {
                return this._samePingMaxLivingNum;
            }
            set
            {
                this._samePingMaxLivingNum = value;
            }
                
        }
    }
}

