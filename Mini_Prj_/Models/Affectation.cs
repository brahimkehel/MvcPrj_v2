//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mini_Prj_.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Affectation
    {
        public int idAbonnement { get; set; }
        public int idClient { get; set; }
        public Nullable<System.DateTime> date_affectation { get; set; }
    
        public virtual Abonnement Abonnement { get; set; }
        public virtual Client Client { get; set; }
    }
}
