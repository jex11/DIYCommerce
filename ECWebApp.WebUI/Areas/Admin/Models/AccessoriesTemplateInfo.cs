using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECWebApp.WebUI.Areas.Admin.Models
{
    public class AccessoriesTemplateInfo
    {
        public Guid AccessoriesTemplateId { get; set; }
        public System.Guid AccessoriesID { get; set; }
        public System.Guid ProductID { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Accessories Code cannot be blank.")]
        [Display(Name = "Code:")]
        public string AccessoriesTemplateCode { get; set; }

        [Required(ErrorMessage = "Accesory Name cannot be empty.")]
        [DataType(DataType.Text)]
        [Display(Name = "Name:")]
        public string AccessoriesTemplateName { get; set; }
        public byte[] AccessoriesTemplateSource { get; set; }
        public string AccessoriesTemplateType { get; set; }

        [Required(ErrorMessage = "Accesory Description cannot be empty.")]
        [DataType(DataType.Text)]
        [Display(Name = "Description:")]
        public string AccessoriesTemplateDescription { get; set; }

        [Required(ErrorMessage = "Accesory Price cannot be empty.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Price:")]
        public decimal? AccessoriesTemplatePrice { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        [Display(Name = "Category:")]
        public Guid AccessoriesTemplateCategory { get; set; }


        [Required(ErrorMessage = "Please select a category.")]
        [Display(Name = "Category:")]
        public string AccessoriesTemplateCategoryName { get; set; }

        public SelectList CategoryList { get; set; }

        
        public decimal? AccessoriesX { get; set; }

        public decimal? AccessoriesY { get; set; }

        [DataType(DataType.ImageUrl)]
        [Required(ErrorMessage = "Please upload a primary image for the product.")]
        [Display(Name = "Accessory Image:")]
        public HttpPostedFileBase AccessoriesTemplateImage { get; set; }

        public string AccessoriesThumbnailBase64
        {
            get
            {
                if (AccessoriesTemplateSource != null)
                {
                    var base64Image = "data:image/" + AccessoriesTemplateType + ";base64," + Convert.ToBase64String(AccessoriesTemplateSource);
                    return base64Image;
                }

                return null;
            }
        }

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
    }
}