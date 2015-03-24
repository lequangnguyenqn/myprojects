
using System;
using System.Collections.Generic;
using WeddingInvitation.Core.Models.Catalog;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Core.Models.Storages;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class ProductInStorageModel
    {
        public int ProductInStorageId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int StorageId { get; set; }
        public string StorageName { get; set; }
        public int Amount { get; set; }
        public string AmountDisplay { get { return String.Format("{0:0,0}", Amount); } }
        public string OfficeName { get; set; }
        public List<Product> Products { get; set; }
        public List<Storage> Storages { get; set; }
    }
    public class IndexProductInStorageModel
    {

        public List<Product> Products { get; set; }
        public List<Storage> Storages { get; set; }
        public List<ProductInStorageModel> ProductInStorageModels { get; set; }
        public bool GroupByProduct { get; set; }
        public bool GroupByStorage { get; set; }
        public string ProductsSelected { get; set; }
        public string StoragesSelected { get; set; }
    }

}