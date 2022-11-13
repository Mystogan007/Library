using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Utils
{
    public class ImageHelper
    {
        private static string _pathPictures = Path.Combine(new string [] { Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "images" }) ;
    
        public static void CreatureImageDirectory()
        {
            if (!Directory.Exists(_pathPictures))
            {
                Directory.CreateDirectory(_pathPictures);
            }
        }

        public static async Task SavePictureAsync(string name, byte [] data, CancellationToken token)
        {
            await File.WriteAllBytesAsync(Path.Combine(new string[] { _pathPictures, name }), data, token);
        }
               

        public static void DeletePicture(string name)
        {
            File.Delete(Path.Combine(new string[] { _pathPictures, name }));
        }

        public static async Task<byte[]> GetPicture(string name)
        {
            string path = Path.Combine(new string[] { _pathPictures, name });
            if (File.Exists(Path.Combine(new string[] { _pathPictures, name })))
            {
                return await File.ReadAllBytesAsync(Path.Combine(new string[] { _pathPictures, name }));
            }
           return null;
        }

    }
}
