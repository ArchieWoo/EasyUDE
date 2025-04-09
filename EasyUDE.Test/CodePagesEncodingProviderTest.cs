using System.Text;

namespace EasyUDE.Test
{
    [TestClass]
    public class CodePagesEncodingProviderTest
    {
        [TestMethod]
        public void SimpleTest() 
        {
            // 注册 CodePagesEncodingProvider 以支持非默认编码
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // 测试编码名称列表
            string[] encodingNames = {
                "ASCII", "UTF-8", "UTF-16LE", "UTF-16BE", "UTF-32BE", "UTF-32LE",
                "X-ISO-10646-UCS-4-3412", "X-ISO-10646-UCS-4-2413", // 非标准 BOM 名称
                "windows-1251", "windows-1252", "windows-1253", "windows-1255",
                "Big5", "EUC-KR", "EUC-JP", "EUC-TW", "gb18030",
                "ISO-2022-JP", "ISO-2022-CN", "ISO-2022-KR", "HZ-GB-2312",
                "Shift-JIS", "x-mac-cyrillic", "KOI8-R", "IBM855", "IBM866",
                "ISO-8859-2", "ISO-8859-5", "ISO-8859-7", "ISO-8859-8", "TIS620"
            };

            foreach (var name in encodingNames)
            {
                try
                {
                    Encoding encoding = Encoding.GetEncoding(name);
                    Console.WriteLine($"编码名称 '{name}' 是有效的。");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine($"编码名称 '{name}' 不受支持。");
                }
            }
        }
        [TestMethod]
        public void SimpleTest1()
        {
            // 注册 CodePagesEncodingProvider 以支持非默认编码
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // 测试编码名称列表
            string[] encodingNames = {
                "GBK","GB2312","GB18030"
            };

            foreach (var name in encodingNames)
            {
                try
                {
                    Encoding encoding = Encoding.GetEncoding(name);
                    Console.WriteLine($"编码名称 '{name}' 是有效的。");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine($"编码名称 '{name}' 不受支持。");
                }
            }
        }
    }
}
