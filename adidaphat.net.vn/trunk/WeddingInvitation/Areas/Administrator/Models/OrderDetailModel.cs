using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WeddingInvitation.Core.Models.Catalog;
using WeddingInvitation.Core.Models.Storages;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class OrderDetailModel
    {
        public int OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int GroupId { get; set; }
        public int[] ProductIds { get; set; }
        public List<Category> Categories { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string FilePath { get; set; }
        public string FilePathBia { get; set; }
        public string FilePathRuot { get; set; }
        public string Note { get; set; }
        public bool SaleAllProductInCategory { get; set; }
        public bool PrintIncludeImage { get; set; }
        public string PrintIncludeImageDisplay
        {
            get
            {
                if (PrintIncludeImage)
                {
                    return "Content/Images/checkbox_checked.png";
                }
                return "Content/Images/checkbox_unchecked.png";
            }
        }
        public bool PrintWithoutImage { get; set; }
        public string PrintWithoutImageDisplay
        {
            get
            {
                if (PrintWithoutImage)
                {
                    return "Content/Images/checkbox_checked.png";
                }
                return "Content/Images/checkbox_unchecked.png";
            }
        }
        public decimal Price { get; set; }
        public string PriceDisplay { get { return String.Format("{0:0,0}", PriceByCategory > 0 ? PriceByCategory : Price); } }
        public decimal PriceByCategory { get; set; }
        public decimal ExtraFee { get; set; }
        public string ExtraFeeDisplay { get { return String.Format("{0:0,0}", ExtraFee); } }
        public decimal TotalCost { get; set; }
        public decimal TotalCostByCategory { get; set; }
        public string TotalCostDisplay { get { return String.Format("{0:0,0}", TotalCostByCategory > 0 ? TotalCostByCategory : TotalCost); } }
        [Range(1, Int32.MaxValue, ErrorMessage = "Nhập số lượng lớn hơn 0")]
        public int Amount { get; set; }
        public string AmountDisplay { get { return String.Format("{0:0,0}", Amount); } }
        public string AmountDisplayToCustomer
        {
            get
            {
                var amount = 0;
                if (Amount > 503)
                {
                    amount = Amount - 5;
                }
                else
                {
                    amount = Amount - 3;
                }
                return String.Format("{0:0,0}", amount);
            }
        }
        public int OrderId { get; set; }
        public int StorageId { get; set; }
        public List<Storage> Storages { get; set; }
        public string DownloadBiaActionLink { get; set; }
        public string DownloadRuotActionLink { get; set; }
        public string DownloadActionLink { get; set; }
        public string DownloadImagePath { get; set; }
        public bool Printable { get; set; }
        public string DownloadLink
        {
            get
            {
                if (string.IsNullOrEmpty(FilePath) || string.IsNullOrEmpty(DownloadImagePath))
                {
                    return string.Empty;
                }
                else
                {
                    return "<a href=\"" + DownloadActionLink + "/" + OrderDetailId + "\"> <img src=\"" + DownloadImagePath +
                           "\" /></a>";
                }
            }
        }
        public string DownloadBiaLink
        {
            get
            {
                if (string.IsNullOrEmpty(FilePathBia) || string.IsNullOrEmpty(DownloadImagePath))
                {
                    return string.Empty;
                }
                else
                {
                    return "<a href=\"" + DownloadBiaActionLink + "/" + OrderDetailId + "\"> <img src=\"" + DownloadImagePath +
                           "\" /></a>";
                }
            }
        }
        public string DownloadRuotLink
        {
            get
            {
                if (string.IsNullOrEmpty(FilePathRuot) || string.IsNullOrEmpty(DownloadImagePath))
                {
                    return string.Empty;
                }
                else
                {
                    return "<a href=\"" + DownloadRuotActionLink + "/" + OrderDetailId + "\"> <img src=\"" + DownloadImagePath +
                           "\" /></a>";
                }
            }
        }
        public string UploadImagePath { get; set; }
        public string UploadLink
        {
            get
            {
                if (string.IsNullOrEmpty(UploadImagePath))
                {
                    return string.Empty;
                }
                else
                {
                    return "<a onclick='onMessageBoxUploadImage("+ OrderDetailId +",\""+ Note +"\")'><img src=\"" + UploadImagePath + "\" /></a>";
                }
            }
        }
    }
}