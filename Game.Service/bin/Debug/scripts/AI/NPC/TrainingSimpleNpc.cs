using Game.Logic.Phy.Object;

namespace GameServerScript.AI.NPC
{
    public class TrainingSimpleNpc : SimpleNpcAi
    {
        private void Beat()
        {
            int demageAmount = base.m_targer.Blood / 10;
            if ((base.m_targer != null) && !base.Body.Beat(base.m_targer, "beat", demageAmount, 0, 0))
            {
                int num3 = base.Game.Random.Next(80, 150);
                if ((base.Body.X - base.m_targer.X) > num3)
                {
                    base.Body.MoveTo(base.Body.X - num3, base.m_targer.Y, "walk", 0x4b0, new LivingCallBack(this.BeatCallBack));
                }
                else
                {
                    base.Body.MoveTo(base.Body.X + num3, base.m_targer.Y, "walk", 0x4b0, new LivingCallBack(this.BeatCallBack));
                }
            }
        }

        public void BeatCallBack()
        {
            int demageAmount = base.m_targer.Blood / 10;
            base.Body.Beat(base.m_targer, "beat", demageAmount, 0, 0);
        }

        public override void OnStartAttacking()
        {
            base.m_body.CurrentDamagePlus = 1f;
            base.m_body.CurrentShootMinus = 1f;
            base.m_targer = base.Game.FindNearestPlayer(base.Body.X, base.Body.Y);
            if (base.m_targer != null)
            {
                if (base.m_targer.Blood > 200)
                {
                    base.Beating();
                }
                else
                {
                    this.Beat();
                }
            }
        }
    }
}

