using System.ComponentModel.DataAnnotations;

namespace QLNhaTro.Commons
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }
    }
    public class BaseEntityIsDelete : BaseEntity
    {
        public bool IsDeleted { get; set; }
    }
    public class BaseMasterData : BaseEntityIsDelete
    {
        public string Name { get; set; }
    }
    public class BaseInfoPeoPle : BaseEntityIsDelete 
    {
        public string FullName { get; set; }
        public DateTime DoB { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string CCCD { get; set; }
        public string Address { get; set; }
        public string? SDTZalo { get; set; }
    }
}
