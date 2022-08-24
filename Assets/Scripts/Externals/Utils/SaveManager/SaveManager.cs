using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Externals.Utils.SaveManager
{
    public static class SaveManager
    {
        public static event System.Action<BinaryWriter> OnSave;
        public static event System.Action<BinaryReader> OnLoad;


        /// <param name="path">full path with filename
        /// in the end</param>
        /// <returns>true if file exists</returns>
        public static bool CheckFile(string path)
        {
            return File.Exists(path);
        }


        /// <param name="path">full path with filename
        /// in the end</param>
        /// <returns>true if file created/updated
        /// successfully</returns>
        public static async Task<bool> SaveToAsync(string path)
        {
            bool failed = false;

            MemoryStream ms = null;
            BinaryWriter bw = null;
            FileStream fs = null;

            try
            {
                ms = new();
                bw = new(ms, Encoding.UTF8);
                OnSave?.Invoke(bw);

                //сначала создаю поток в памяти, закидываю в него
                //данные для сохранения, а потом создаю файловый
                //поток и переношу данные из потока памяти, а не
                //сразу записываю данные для сохранения в файловый
                //поток для того, чтобы записать данный в быстрый
                //поток синхронно, а потом, асинхронно, записать
                //его в файл

                fs = File.OpenWrite(path);
                await fs.WriteAsync(ms.GetBuffer(), 0, (int)ms.Length);
                await ms.CopyToAsync(fs);
            }
            catch (Exception ex)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError(ex);
#endif
                failed = true;
            }
            finally
            {
                if (bw != null)
                    bw.Dispose();
                else if (ms != null)
                    ms.Dispose();

                if (fs != null)
                    fs.Dispose();
            }

            return !failed;
        }

        public static async Task<bool> LoadFromAsync(string path)
        {
            if (!File.Exists(path))
                return false;

            bool failed = false;

            MemoryStream ms = null;
            BinaryReader br = null;

            try
            {
                var data = await File.ReadAllBytesAsync(path);
                ms = new(data);
                br = new(ms);
                OnLoad?.Invoke(br);
            }
            catch (Exception ex)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError(ex);
#endif
                failed = true;
            }
            finally
            {
                if (br != null)
                    br.Dispose();
                else if (ms != null)
                    ms.Dispose();
            }

            return !failed;
        }
    }
}
