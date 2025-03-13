namespace CLIMFinders.Domain.Entities
{
    public class BaseEntity
    { 
        public int AddedById
        {
            get;
            set; 
        }
        public DateTime AddedOn
        {
            get;
            set;
        } = DateTime.Now;
        public int ModifiedById
        {
            get;
            set;
        }
        public DateTime ModifiedOn
        {
            get;
            set;
        } = DateTime.Now; 
        public bool IsDeleted
        {
            get;
            set;
        } = false;
    }
     
}
