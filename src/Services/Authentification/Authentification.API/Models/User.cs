using ApiLibrary.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentification.API.Models
{
    public class User : BaseModel<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage = "Le mail est obligatoire.")]
        [Index("MailIndex", IsUnique = true)]
        public string Mail { get; set; }
        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        public string Password { get; set; }
    }
}
