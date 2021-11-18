using System;
namespace DesignPatterns2.Prototype
{
    public class InvitationCard
    {
        public String To;
        public String Title;
        public String Content;
        public String SendBy;
        public DateTime Date;

        public String p_To
        {
            get { return To; }
            set { To = value; }
        }
        public String p_Title
        {
            get { return Title; }
            set { Title = value; }
        }
        public String p_content
        {
            get { return Content; }
            set { Content = value; }
        }
        public String p_SendBy
        {
            get { return SendBy; }
            set { SendBy = value; }
        }
        public DateTime p_Date
        {
            get { return Date; }
            set { Date = value; }
        }

        public InvitationCard CloneMe()
        {
            return (InvitationCard)this.MemberwiseClone();
        }
    }
}
