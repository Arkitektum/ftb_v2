using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Ftb_DbModels
{
    public class PostDistributionMetaData
    {
        /// <summary>
        /// Reference to DistributionForms.Id
        /// </summary>
        public Guid Id { get; set; }
        public string ReferenceId { get; set; }
        public DateTime DateCreated
        {
            get
            {
                return this.dateCreated.HasValue
                   ? this.dateCreated.Value
                   : DateTime.Now;
            }

            set { this.dateCreated = value; }
        }

        private DateTime? dateCreated = null;

        public DateTime DateChanged { get; set; }
        public string NotificationStatus { get; set; }
        public string NotificationChannel { get; set; }
    }
}