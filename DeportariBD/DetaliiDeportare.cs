//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DeportariBD
{
    using System;
    using System.Collections.Generic;
    
    public partial class DetaliiDeportare
    {
        public Nullable<System.DateTime> DATA_REABILITARE { get; set; }
        public Nullable<int> ID_LOCALITATE_REABILITARE { get; set; }
        public System.DateTime DATA_DEPORTARE { get; set; }
        public int ID_LOCALITATE_DEPORTARE { get; set; }
        public int ID_DEPORTAT { get; set; }
    
        public virtual Deportati Deportati { get; set; }
        public virtual Localitati Localitati { get; set; }
        public virtual Localitati Localitati1 { get; set; }
    }
}
