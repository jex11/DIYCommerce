using ECWebApp.WebUI.Areas.Admin.Models;
using ECWebApp.WebUI.Areas.CustomProduct.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECWebApp.WebUI.Models
{
    public class ProductInfo
    {

        public Guid ProductID { get; set; }
        public Guid CartID { get; set; }
        public Guid CartListID { get; set; }
        public Guid CustomerID { get; set; }
        public Guid FolderID { get; set; }
        public Guid CustomProductID { get; set; }

        [Required(ErrorMessage = "Product Code cannot be empty.")]
        [DataType(DataType.Text)]
        [Display(Name = "Code:")]
        public string ProductCode { get; set; }

        [Required(ErrorMessage = "Product Name cannot be empty.")]
        [DataType(DataType.Text)]
        [Display(Name = "Name:")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        [Display(Name = "Category:")]
        public int ProductCategory { get; set; }

        [Required(ErrorMessage = "Please enter the quantity of the product.")]
        [DataType(DataType.Text)]
        [Display(Name = "Quantity:")]
        public int ProductQuantity { get; set; }

        [DataType(DataType.ImageUrl)]
        [Required(ErrorMessage = "Please upload a primary image for the product.")]
        [Display(Name = "Cover Photo")]
        public HttpPostedFileBase ProductImage { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Status:")]
        public int ProductStatus { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description:")]
        public string ProductDescription { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Width:")]
        public Nullable<decimal> ProductWidth { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Height:")]
        public Nullable<decimal> ProductHeight { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Length:")]
        public Nullable<decimal> ProductLength { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Weight:")]
        public Nullable<int> ProductWeight { get; set; }

        [Required(ErrorMessage = "Please enter the original price.")]
        [DataType(DataType.Text)]
        [Display(Name = "Original Price:")]
        public Nullable<decimal> ProductOriginalPrice { get; set; }

        [Required(ErrorMessage = "Please enter the desired retail price.")]
        [DataType(DataType.Text)]
        [Display(Name = "Retail Price:")]
        public Nullable<decimal> ProductRetailPrice { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Out of Stock:")]
        public Nullable<int> IsOutOfStock { get; set; }

        public string ProductScale { get; set; }
       
        public SelectList CategoryList { get; set; }

        public SelectList ScaleList { get; set; }

        public Guid FolderLocation { get; set; }

        #region Custom Product
        public Guid TemplateID { get; set; }

        public string Measurements { get; set; }

        public string Color { get; set; }

        public int? PurchasedCount { get; set; }

        public Guid TextureID { get; set; }

        public Guid CustomProductAuthorID { get; set; }

        public String CustomProductAuthorName { get; set; }

        public string CustomProductCreatedOn { get; set; }

        public string CustomProductSoldOn { get; set; }

        public string CustomProductSoldOnMonth { get; set; }

        public int CustomProductSoldOnMonthNo { get; set; }
        #endregion

        #region Image
        public byte[] ProductImageByte { get; set; }

        public string ProductImageBase64 { get; set; }

        public string ProductImageType { get; set; }

        public string ProductImageToBase64
        {
            get
            {
                if (ProductImageByte != null)
                {
                    var base64Image = "data:image/" + ProductImageType + ";base64," + Convert.ToBase64String(ProductImageByte);
                    return base64Image;
                }

                return null;
            }
        }

        public byte[] ProductImageToByte
        {
            get
            {
                if (ProductImageBase64 != null)
                {
                    return Convert.FromBase64String(ProductImageBase64);
                }
                return null;
            }
        }

        public byte[] ProductImageResized
        {
            get
            {
                if (ProductImageType != "svg+xml" && ProductImageBitmap != null)
                {
                    Bitmap image = Resize(ProductImageBitmap, 150, 112);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, ImageFormat.Jpeg);
                        return ms.ToArray();
                    }
                }
                return ProductImageByte;
            }
        }


        public Bitmap ProductImageBitmap
        {
            get
            {
                if (ProductImageByte != null)
                {
                    if (ProductImageByte.Length != 0 && ProductImageType != "svg+xml")
                    {
                        try
                        {
                            Bitmap bmp;
                            using (var ms = new MemoryStream(ProductImageByte))
                            {
                                bmp = new Bitmap(ms);
                            }
                            return bmp;
                        }
                        catch (ArgumentException e)
                        {
                            throw new Exception("The file received from the Map Server is not a valid jpeg image", e);
                        }
                       
                    }
                }
                

                return null;
            }
        }

        public List<AccessoryInfo> Accessories { get; set; }
        public Bitmap Resize(Bitmap _currentBitmap, int newWidth, int newHeight)
        {
            if (newWidth != 0 && newHeight != 0 && _currentBitmap != null)
            {
                Bitmap temp = _currentBitmap;
                Bitmap bmap = new Bitmap(newWidth, newHeight, temp.PixelFormat);

                double nWidthFactor = (double)temp.Width / (double)newWidth;
                double nHeightFactor = (double)temp.Height / (double)newHeight;

                double fx, fy, nx, ny;
                int cx, cy, fr_x, fr_y;
                Color color1 = new Color();
                Color color2 = new Color();
                Color color3 = new Color();
                Color color4 = new Color();
                byte nRed, nGreen, nBlue;

                byte bp1, bp2;

                for (int x = 0; x < bmap.Width; ++x)
                {
                    for (int y = 0; y < bmap.Height; ++y)
                    {

                        fr_x = (int)Math.Floor(x * nWidthFactor);
                        fr_y = (int)Math.Floor(y * nHeightFactor);
                        cx = fr_x + 1;
                        if (cx >= temp.Width) cx = fr_x;
                        cy = fr_y + 1;
                        if (cy >= temp.Height) cy = fr_y;
                        fx = x * nWidthFactor - fr_x;
                        fy = y * nHeightFactor - fr_y;
                        nx = 1.0 - fx;
                        ny = 1.0 - fy;

                        color1 = temp.GetPixel(fr_x, fr_y);
                        color2 = temp.GetPixel(cx, fr_y);
                        color3 = temp.GetPixel(fr_x, cy);
                        color4 = temp.GetPixel(cx, cy);

                        // Blue
                        bp1 = (byte)(nx * color1.B + fx * color2.B);

                        bp2 = (byte)(nx * color3.B + fx * color4.B);

                        nBlue = (byte)(ny * (double)(bp1) + fy * (double)(bp2));

                        // Green
                        bp1 = (byte)(nx * color1.G + fx * color2.G);

                        bp2 = (byte)(nx * color3.G + fx * color4.G);

                        nGreen = (byte)(ny * (double)(bp1) + fy * (double)(bp2));

                        // Red
                        bp1 = (byte)(nx * color1.R + fx * color2.R);

                        bp2 = (byte)(nx * color3.R + fx * color4.R);

                        nRed = (byte)(ny * (double)(bp1) + fy * (double)(bp2));

                        bmap.SetPixel(x, y, System.Drawing.Color.FromArgb
                (255, nRed, nGreen, nBlue));
                    }
                }
                return (Bitmap)bmap.Clone();
            }
            return null;
        }
        #endregion

    }
}