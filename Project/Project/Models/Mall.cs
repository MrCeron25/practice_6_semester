//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Mall
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Mall()
        {
            this.Pavilion = new HashSet<Pavilion>();
        }
    
        public long mall_id { get; set; }
        public string mall_name { get; set; }
        public long status_id { get; set; }
        public decimal cost { get; set; }
        public int number_of_pavilion { get; set; }
        public string city { get; set; }
        public double value_added_factor { get; set; }
        public int number_of_storeys { get; set; }
        public byte[] photo { get; set; }
    
        public virtual Mall_statuses Mall_statuses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pavilion> Pavilion { get; set; }
    }
}
