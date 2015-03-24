using System;
using System.Collections.Generic;
using WeddingInvitation.Core.Models.Catalog;
using WeddingInvitation.Core.Models.Customers;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Core.Models.Storages;
using WeddingInvitation.Core.Models.Users;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class ExportTrackModel
    {
        public int ExportTrackId { get; set; }
        public int Amount { get; set; }
        public string AmountDisplay { get { return String.Format("{0:0,0}", Amount); } }
        public DateTime CreateDate { get; set; }
        public string Note { get; set; }
        public int ReceivedUserId { get; set; }
        public string ReceivedUserName { get; set; }
        public string MyOfficeName { get; set; }
        public int MyOfficeId { get; set; }
        public List<MyOffice> MyOffices { get; set; }
        public int ProductId { get; set; }
        public List<Product> Products { get; set; }
        public string ProductName { get; set; }
        public List<User> Users { get; set; }
        public int StorageId { get; set; }
        public List<Storage> Storages { get; set; }
        public string ExportUserName { get; set; }
        public List<Customer> Customers { get; set; }
        public string CustomerName { get; set; }
    }
}