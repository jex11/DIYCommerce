using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace ECWebApp.WebUI.Areas.Admin.Models
{
    public class TextureInfo
    {
        public Guid TextureId { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Texture Code cannot be blank.")]
        [Display(Name = "Code:")]
        public string TextureCode { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Texture Name cannot be blank.")]
        [Display(Name = "Name:")]
        public string TextureName { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Texture Description cannot be blank.")]
        [Display(Name = "Description:")]
        public string TextureDescription { get; set; }

        [Required(ErrorMessage = "Texture Price cannot be empty.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Price:")]
        public decimal? TexturePrice { get; set; }

        [DataType(DataType.ImageUrl)]
        [Required(ErrorMessage = "Please upload an image for texture.")]
        [Display(Name = "Texture Image:")]
        public HttpPostedFileBase TextureImage { get; set; }

        public byte[] TextureImageByte { get; set; }

        public byte[] TextureThumbnail { 
            get {
                if (TextureType != "svg+xml" && TextureImageBitmap != null)
                {
                    Bitmap image = Resize(TextureImageBitmap, 150, 112);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, ImageFormat.Jpeg);
                        return ms.ToArray();
                    }
                }
                return TextureImageByte;
            } 
        }

        public Bitmap TextureImageBitmap
        {
            get
            {
                if (TextureImageByte != null)
                {
                    if (TextureImageByte.Length != 0)
                    {
                        Bitmap bmp;
                        using (var ms = new MemoryStream(TextureImageByte))
                        {
                            bmp = new Bitmap(ms);
                        }
                        return bmp;
                    }
                }


                return null;
            }
        }

        

        [Display(Name = "Texture Image:")]
        public string TextureThumbnailBase64 { 
            get {
                if (TextureThumbnail != null)
                {
                    var base64Image = "data:image/" + TextureType + ";base64," + Convert.ToBase64String(TextureThumbnail);
                    return base64Image;
                }

                return null;
            } 
        }

        public string TextureType { get; set; }

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