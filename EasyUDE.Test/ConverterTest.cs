using System.Text;

namespace EasyUDE.Test
{
    [TestClass]
    public class ConverterTest
    {
        [TestMethod]
        public void TestUTF8_2_GB2312()
        {
            // 定义输入和输出文件路径
            string inputFilePath = @"..\..\..\Data\TestFile_UTF8.txt";
            string outputFilePath = @"..\..\..\Data\OutputFile_UTF8_2_GB2312.txt";
            ICharsetHandler handler = new CharsetHandler();
            handler.ConvertFileEncoding(inputFilePath,outputFilePath,TargetEncoding.GB18030);
            Assert.IsTrue(File.Exists(outputFilePath), "输出文件未生成！");
            // 验证输出文件内容是否正确
            string expectedContent = File.ReadAllText(inputFilePath, Encoding.GetEncoding("UTF-8"));
            string actualContent = File.ReadAllText(outputFilePath, Encoding.GetEncoding("GB18030"));
            Assert.AreEqual(expectedContent, actualContent, "转换后的文件内容与原始内容不一致！");
        }
    }
}
