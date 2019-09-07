using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace RGBToGrayScale
{
    class Program
    {
        static void Main(string[] args)
        {
            String path = "PATH a donde se encuentra el archivo que se desea convertir a escala de grises";
            FileInfo file = new FileInfo(path);
            if (ExistsFormat(file.Extension))
                ToGrayScaleIterativeMethod(path, file);
            else
                Console.WriteLine("El archivo no es compatible.");
            Console.WriteLine("Presione una tecla para salir...");
            Console.ReadKey();            
        }

        /// <summary>
        /// Metodo que convierte a escala de grises una imagen utilizando iteraciones
        /// </summary>
        /// <param name="path">Ruta del archivo que se desea convertir a escala de grises</param>
        /// <param name="file">Propiedades del archivo</param>
        public static void ToGrayScaleIterativeMethod(String path, FileInfo file)
        {
            Bitmap bmp = new Bitmap(path);
            Console.WriteLine("Iniciando procesamiento...");
            for(int x = 0; x < bmp.Width; x++)
            {
                for(int y = 0; y < bmp.Height; y++)
                {
                    Color pixel = bmp.GetPixel(x, y);
                    int gray = (int)((pixel.R * .3) + (pixel.G * .59) + (pixel.B * .11));
                    bmp.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }
            Console.WriteLine("Procesamiento terminado...");
            Console.WriteLine("Guardando archivo en el escritorio...");
            String name = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + file.Name;
            Console.WriteLine(name);
            bmp.Save(name, Format(file.Extension));
            Console.WriteLine("Archivo guardado en el escritorio...");
        }

        /// <summary>
        /// Metodo que convierte a escala de grises una imagen utilizando una matriz de color
        /// </summary>
        /// <param name="path">Ruta del archivo que se desea convertir a escala de grises</param>
        /// <param name="file">Propiedades del archivo</param>
        public static void ToGrayScaleColorMatrixMethod(String path, FileInfo file)
        {
            Bitmap bmp = new Bitmap(path);
            Console.WriteLine("Iniciando procesamiento...");

            Bitmap newBMP = new Bitmap(bmp.Width, bmp.Height);
            Graphics g = Graphics.FromImage(newBMP);

            ColorMatrix colorMatrix = new ColorMatrix(
                new float[][]
                {
                    new float[] {.3f, .3f, .3f, 0, 0},
                    new float[] {.59f, .59f, .59f, 0, 0},
                    new float[] {.11f, .11f, .11f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });

            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);

            g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
            g.Dispose();

            Console.WriteLine("Procesamiento terminado...");
            Console.WriteLine("Guardando archivo en el escritorio...");
            String name = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + file.Name;
            Console.WriteLine(name);
            newBMP.Save(name, Format(file.Extension));
            Console.WriteLine("Archivo guardado en el escritorio...");
        }

        /// <summary>
        /// Establece el formato en el que se guardará la imagen dependiendo de la extensión del archivo original
        /// </summary>
        /// <param name="format">Extensión del archivo original</param>
        /// <returns>Retorna el formato en el que se guardará la nueva imagen</returns>
        public static ImageFormat Format(String format)
        {
            if(String.IsNullOrEmpty(format) || String.IsNullOrWhiteSpace(format))
            {
                throw new ArgumentException("No se puede determinar la extensión del archivo", format);
            }
            switch (format.ToLower())
            {
                case (".jpg"):
                case (".jpeg"):
                    return ImageFormat.Jpeg;
                case (".png"):
                    return ImageFormat.Png;
                case (".bmp"):
                    return ImageFormat.Bmp;
                case (".ico"):
                    return ImageFormat.Icon;
                case (".tif"):
                case (".tiff"):
                    return ImageFormat.Tiff;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Metodo que verifica si la extensión del archivo es una imagen
        /// </summary>
        /// <param name="format">Extensión del archivo que se desea comprobar</param>
        /// <returns>Retorna verdadero o falso dependiendo si la extensión propocionada corresponde a una imagen</returns>
        public static bool ExistsFormat(String format)
        {
            string[] formats = { ".jpg", ".jpeg", ".png", ".ico", ".bmp", ".tiff", ".tif" };
            if (formats.Contains(format.ToLower()))
                return true;
            return false;
        }
    }
}
