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
    
    public partial class Demande_Trajet
    {
        public int id { get; set; }
        public string depart { get; set; }
        public string arriver { get; set; }
        public Nullable<System.DateTime> date_depart { get; set; }
        public Nullable<System.DateTime> date_arriver { get; set; }
        public Nullable<int> idUtilisateur { get; set; }
    
        public virtual Client Client { get; set; }
    }
}
