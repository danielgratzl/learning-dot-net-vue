using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;
using System;

namespace WebApi.Models
{
    [Table("AuthToken")]
    public class AuthToken
    {
        [ExplicitKey]
	    public string Id { get; set; }

        public string Pin { get; set; }

        public string Email { get; set; }

        public DateTime? DateVerified { get; set; } 
    }
}