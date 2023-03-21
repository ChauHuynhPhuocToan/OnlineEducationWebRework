using System.ComponentModel.DataAnnotations;

namespace OnlineEducation.Models
{
    public class BaseClasses
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? ModificationTime { get; set; }
        public DateTime DeleteTime { get; set; }
        public Guid CreatorId { get; set; }
        public Guid? ModifierId { get; set; }
        public Boolean IsDelete { get; set; }

        public BaseClasses()
        {
           
        }

        public void Create(BaseClasses entities, Guid creatorId)
        {
            entities.Id = Guid.NewGuid();
            entities.CreationTime = DateTime.Now;
            entities.ModificationTime = null;
            entities.CreatorId = creatorId;
            this.ModifierId = null;
            this.IsDelete = false;
        }

        public void Update(BaseClasses entities, Guid? modifierId = null)
        {
            entities.ModificationTime = DateTime.Now;
            entities.ModifierId = modifierId;
        }

        public void Delete(BaseClasses entities, Guid? modifierId = null) {
            this.ModificationTime = DateTime.Now;
            this.ModifierId = modifierId;
            this.IsDelete = true;
        }
    }
}
