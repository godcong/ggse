using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace guigubahuang
{
    public static class Config
    {
        private static string _iniPath = Application.StartupPath + "\\config.ini";

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
        [DllImport("kernel32")]
        private static extern int WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);


        public static string Read(string section, string key)
        {
            StringBuilder sb = new StringBuilder(1024);
            GetPrivateProfileString(section, key, string.Empty, sb, 1024, _iniPath);
            return sb.ToString();
        }

        public static int Write(string section, string key, string value)
        {
            return WritePrivateProfileString(section, key, value, _iniPath);
        }

        public static string ReadIni()
        {
            try
            {
                return Read("FilePath", "Path");
            }
            catch { return string.Empty; }
        }
        public static int WriteIni(string value)
        {
            return Write("FilePath", "Path", value);
        }
    }
}
