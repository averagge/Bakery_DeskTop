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
    
    public partial class composition
    {
        public int id { get; set; }
        public Nullable<int> good_id { get; set; }
        public Nullable<int> component_id { get; set; }
    
        public virtual component component { get; set; }
        public virtual good good { get; set; }
    }
}