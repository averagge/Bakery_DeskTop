//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bakery
{
    using System;
    using System.Collections.Generic;
    
    public partial class good
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public good()
        {
            this.compositions = new HashSet<composition>();
            this.order_composition = new HashSet<order_composition>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public string image { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<composition> compositions { get; set; }
        public virtual goods_in_stock goods_in_stock { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_composition> order_composition { get; set; }
    }
}
